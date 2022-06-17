using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.Events;
using System.Net;
using utilities;


namespace TotemServices
{
    public class TotemAccountGateway : MonoBehaviour
    {
        private readonly string _urlRoot = "https://account.totem.gdn/";
        private readonly string _httpListenerUrl = "http://localhost:6700/auth/";

        private readonly string _succesResponse = "<html><head><meta charset='utf8'></head><body>Login successful. You can close this page and return to the app</body></html>";
        private readonly string _resultQueryParametr = "result";


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
            Application.OpenURL(_urlRoot + $"auth/google?redirectTo={_httpListenerUrl}");
        }

        /// <summary>
        /// Opens browser tab with Facebook login
        /// </summary>
        /// <param name="onSuccess">Success callback with publicKey</param>
        /// <param name="onFailure">Failure callback</param>
        public void LoginFacebook(UnityAction<string, UserSocialProfile> onSuccess, UnityAction<string> onFailure = null) 
        {
            ListenHttpResponse(onSuccess, onFailure);
            Application.OpenURL(_urlRoot + $"auth/facebook?redirectTo={_httpListenerUrl}");
        }


        /// <summary>
        /// A Http listener for geting a result json from web browser
        /// </summary>
        /// <param name="onSuccess"></param>
        /// <param name="onFailure"></param>
        private async void ListenHttpResponse(UnityAction<string, UserSocialProfile> onSuccess, UnityAction<string> onFailure)
        {
            HttpListener listener = new HttpListener();
            listener.Prefixes.Add(_httpListenerUrl);
            listener.Start();
            HttpListenerContext context = await listener.GetContextAsync();
            HttpListenerRequest req = context.Request;
            string resultBase64 = req.QueryString.Get(_resultQueryParametr);
            string utfResult = Base64UrlDecoder.Base64UrlToUTF8(resultBase64);
            SocialLoginResponse loginResult = JsonUtility.FromJson<SocialLoginResponse>(utfResult);

            HttpListenerResponse response = context.Response;
            byte[] responseBuffer = System.Text.Encoding.UTF8.GetBytes(_succesResponse);
            response.ContentLength64 = responseBuffer.Length;
            var output = response.OutputStream;
            output.Write(responseBuffer, 0, responseBuffer.Length);

            listener.Stop();

            StartCoroutine(GetPublicKeyCoroutine(loginResult, onSuccess, onFailure));
        }

        private IEnumerator GetPublicKeyCoroutine(SocialLoginResponse loginInfo, UnityAction<string, UserSocialProfile> onSuccess, UnityAction<string> onFailure)
        {
            UnityWebRequest www = UnityWebRequest.Get(_urlRoot + "me");
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