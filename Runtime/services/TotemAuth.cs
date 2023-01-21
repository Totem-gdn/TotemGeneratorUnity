using System.Collections;
using System;
using System.Net;
using System.Collections.Generic;
using Newtonsoft.Json;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Networking;
using TotemEntities;
using TotemConsts;
using System.Linq;


namespace TotemServices
{

    public class TotemAuth : MonoBehaviour
    {
        private class IdToken
        {
            public int iat;
            public string aud;
            public string nonce;
            public string iss;
            public List<Web3AuthWallet> wallets;
            public string email;
            public string name;
            public string profileImage;
        }


        private const string redirectUrlQueryName = "success_url";
        private const string gameIdQueryName = "game_id";

        private UnityAction<TotemUser> onLoginCallback;

        private void Start()
        {
            Application.deepLinkActivated += OnDeepLinkActivated;
        }

        /// <summary>
        /// Open a web-page in a browser for user to login
        /// </summary>
        /// <param name="onSucces"></param>
        public void LoginUser(UnityAction<TotemUser> onSucces, string gameId = "")
        {
            onLoginCallback = onSucces;
#if UNITY_STANDALONE || UNITY_EDITOR
            ListenHttpResponse();
#endif

            string query = $"?{redirectUrlQueryName}={LoadRedirectUrl()}";
#if UNITY_EDITOR
            if (!string.IsNullOrEmpty(gameId))
            {
                query += $"&{gameIdQueryName}={gameId}";
            }
#endif
            Application.OpenURL(ServicesEnv.AuthServiceUrl + query);
        }

        /// <summary>
        /// A Http listener for geting a result json from web browser
        /// </summary>
        /// <param name="onSuccess"></param>
        /// <param name="onFailure"></param>
        private async void ListenHttpResponse()
        {
            HttpListener listener = new HttpListener();
            listener.Prefixes.Add(ServicesEnv.HttpListenerUrl);
            listener.Start();
            HttpListenerContext context = await listener.GetContextAsync();
            HttpListenerRequest req = context.Request;

            string token = req.QueryString.Get(ServicesEnv.HttpResultParameterName);
            HandleToken(token);

            HttpListenerResponse response = context.Response;
            string responseText = Resources.Load<TextAsset>(ServicesEnv.AuthHttpResponseFileName).text;
            byte[] responseBuffer = System.Text.Encoding.UTF8.GetBytes(responseText);
            response.ContentLength64 = responseBuffer.Length;
            var output = response.OutputStream;
            output.Write(responseBuffer, 0, responseBuffer.Length);

            listener.Stop();
        }

        private void HandleToken(string token)
        {
            string decode = token.Split('.')[1];
            byte[] bytes = TotemUtils.Convert.DecodeBase64(decode);
            IdToken idToken = JsonConvert.DeserializeObject<IdToken>(System.Text.Encoding.UTF8.GetString(bytes));
            string publicKey = idToken.wallets[0].public_key;

            TotemUser user = new TotemUser(idToken.name, idToken.email, idToken.profileImage, publicKey);

            onLoginCallback.Invoke(user);

        }

        private void OnDeepLinkActivated(string url)
        {
            Uri resUri = new Uri(url);
            var arguments = resUri.Query
              .Substring(1) // Remove '?'
              .Split('&')
              .Select(q => q.Split('='))
              .ToDictionary(q => q.FirstOrDefault(), q => q.Skip(1).FirstOrDefault());

            string token = arguments[ServicesEnv.HttpResultParameterName];
            HandleToken(token);
        }

        private string LoadRedirectUrl()
        {
#if UNITY_EDITOR || UNITY_STANDALONE
            return ServicesEnv.HttpListenerUrl;
#elif UNITY_ANDROID || UNITY_IOS
            try
            {
                return Resources.Load<TextAsset>("webauth").text;
            }
            catch
            {
                throw new Exception("Deep Link uri is invalid or does not exist. Please generate from \"Window > Totem Generator > Generate Deep Link\" Menu");
            }
#endif
        }
    }
}
