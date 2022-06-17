using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.Events;
using System.Net;
using utilities;
using consts;


namespace TotemServices
{
    public class TotemAccountGateway : MonoBehaviour
    {

        #region Response models

        [Serializable]
        private class SocialLoginResponse
        {
            public UserSocialProfile profile;
            public string accessToken;
        }

        [Serializable]
        public class UserSocialProfile
        {
            public string id;
            public string provider;
            public string username;
        }

        private class PublicKeyResponse
        {
            public string publicKey;
        }

        #endregion


        /// <summary>
        /// Opens browser tab with Google login
        /// </summary>
        /// <param name="onSuccess">Success callback with publicKey</param>
        /// <param name="onFailure">Failure callback</param>
        public void LoginGoogle(UnityAction<string, UserSocialProfile> onSuccess, UnityAction<string> onFailure = null) 
        {
            ListenHttpResponse(onSuccess, onFailure);
            Application.OpenURL(ServicesEnv.AccountGatewayUrl + $"auth/google?redirectTo={ServicesEnv.HttpListenerUrl}");
        }

        /// <summary>
        /// Opens browser tab with Facebook login
        /// </summary>
        /// <param name="onSuccess">Success callback with publicKey</param>
        /// <param name="onFailure">Failure callback</param>
        public void LoginFacebook(UnityAction<string, UserSocialProfile> onSuccess, UnityAction<string> onFailure = null) 
        {
            ListenHttpResponse(onSuccess, onFailure);
            Application.OpenURL(ServicesEnv.AccountGatewayUrl + $"auth/facebook?redirectTo={ServicesEnv.HttpListenerUrl}");
        }


        /// <summary>
        /// A Http listener for geting a result json from web browser
        /// </summary>
        /// <param name="onSuccess"></param>
        /// <param name="onFailure"></param>
        private async void ListenHttpResponse(UnityAction<string, UserSocialProfile> onSuccess, UnityAction<string> onFailure)
        {
            HttpListener listener = new HttpListener();
            listener.Prefixes.Add(ServicesEnv.HttpListenerUrl);
            listener.Start();
            HttpListenerContext context = await listener.GetContextAsync();
            HttpListenerRequest req = context.Request;
            string resultBase64 = req.QueryString.Get(ServicesEnv.HttpResultParametrName);
            string utfResult = Base64UrlDecoder.Base64UrlToUTF8(resultBase64);
            SocialLoginResponse loginResult = JsonUtility.FromJson<SocialLoginResponse>(utfResult);

            HttpListenerResponse response = context.Response;
            string responseText = Resources.Load<TextAsset>(ServicesEnv.HttpResponseFileName).text;
            byte[] responseBuffer = System.Text.Encoding.UTF8.GetBytes(responseText);
            response.ContentLength64 = responseBuffer.Length;
            var output = response.OutputStream;
            output.Write(responseBuffer, 0, responseBuffer.Length);

            listener.Stop();

            StartCoroutine(GetPublicKeyCoroutine(loginResult, onSuccess, onFailure));
        }

        private IEnumerator GetPublicKeyCoroutine(SocialLoginResponse loginInfo, UnityAction<string, UserSocialProfile> onSuccess, UnityAction<string> onFailure)
        {
            UnityWebRequest www = UnityWebRequest.Get(ServicesEnv.AccountGatewayUrl + "me");
            www.SetRequestHeader("Authorization", "Bearer " + loginInfo.accessToken);
            yield return www.SendWebRequest();
            if (www.result == UnityWebRequest.Result.ConnectionError || www.result == UnityWebRequest.Result.ProtocolError)
            {
                Debug.LogError("TotemAccountGateway- Failed to get public key: " + www.error);
                onFailure?.Invoke(www.error);
            }
            else
            {
                PublicKeyResponse response = JsonUtility.FromJson<PublicKeyResponse>(www.downloadHandler.text);
                onSuccess.Invoke(response.publicKey, loginInfo.profile);
            }


        }

    }
}