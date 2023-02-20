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

        private string currentGameId;
        private UnityAction<TotemUser> onLoginCallback;
        private HttpListener httpListener;

        private void Start()
        {
            Application.deepLinkActivated += OnDeepLinkActivated;
        }

        /// <summary>
        /// Open a web-page in a browser for user to login
        /// </summary>
        /// <param name="onSucces"></param>
        public void LoginUser(UnityAction<TotemUser> onSucces, string gameId)
        {
            onLoginCallback = onSucces;
            currentGameId = gameId;
#if UNITY_STANDALONE || UNITY_EDITOR
            ListenHttpResponse();
#endif

            string query = $"?{redirectUrlQueryName}={LoadRedirectUrl()}";
#if !UNITY_EDITOR
            if (!string.IsNullOrEmpty(gameId))
            {
                query += $"&{gameIdQueryName}={gameId}";
            }
#endif
            Application.OpenURL(ServicesEnv.AuthServiceUrl + query);
        }

        /// <summary>
        /// Logins the user using token from the command line or player prefs
        /// </summary>
        /// <param name="onComplete"></param>
        /// <param name="onFailure"></param>
        public void LoginUserFromToken(string gameId, UnityAction<TotemUser> onComplete, UnityAction<string> onFailure)
        {
#if UNITY_STANDALONE || UNITY_EDITOR
            List<string> args = Environment.GetCommandLineArgs().ToList();
            int tokenArgIndex = args.IndexOf(ServicesEnv.TokenComandLineArgName);

            if (tokenArgIndex != -1)
            {
                string argsToken = args[tokenArgIndex + 1];
                TotemUser user = HandleToken(argsToken);
                onComplete.Invoke(user);
                return;
            }
#endif
            string prefsToken = PlayerPrefs.GetString(ServicesEnv.TokenPlayerPrefsName + "_" + gameId, null);
            if (!string.IsNullOrEmpty(prefsToken))
            {
                TotemUser user = HandleToken(prefsToken);
                onComplete.Invoke(user);
                return;
            }

            onFailure?.Invoke("No previous login found or token not provided");
        }

        /// <summary>
        /// A Http listener for geting a result json from web browser
        /// </summary>
        /// <param name="onSuccess"></param>
        /// <param name="onFailure"></param>
        private async void ListenHttpResponse()
        {
            httpListener = new HttpListener();
            httpListener.Prefixes.Add(ServicesEnv.HttpListenerUrl);
            httpListener.Start();
            HttpListenerContext context = await httpListener.GetContextAsync();
            HttpListenerRequest req = context.Request;

            string token = req.QueryString.Get(ServicesEnv.HttpResultParameterName);

            TotemUser user = HandleToken(token);
            PlayerPrefs.SetString(ServicesEnv.TokenPlayerPrefsName + "_" + currentGameId, token);

            onLoginCallback.Invoke(user);

            HttpListenerResponse response = context.Response;
            string responseText = Resources.Load<TextAsset>(ServicesEnv.AuthHttpResponseFileName).text;
            byte[] responseBuffer = System.Text.Encoding.UTF8.GetBytes(responseText);
            response.ContentLength64 = responseBuffer.Length;
            var output = response.OutputStream;
            output.Write(responseBuffer, 0, responseBuffer.Length);

            httpListener.Stop();
            httpListener = null;
        }

        public void CancelLogin()
        {
#if UNITY_STANDALONE || UNITY_EDITOR
            if (httpListener != null)
            {
                httpListener.Stop();
                httpListener = null;
                onLoginCallback = null;
            }
#endif
        }

        private TotemUser HandleToken(string token)
        {
            string decode = token.Split('.')[1];
            byte[] bytes = TotemUtils.Convert.DecodeBase64(decode);
            IdToken idToken = JsonConvert.DeserializeObject<IdToken>(System.Text.Encoding.UTF8.GetString(bytes));
            string publicKey = idToken.wallets[0].public_key;

            TotemUser user = new TotemUser(idToken.name, idToken.email, idToken.profileImage, publicKey);
            return user;
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
            TotemUser user = HandleToken(token);
            PlayerPrefs.SetString(ServicesEnv.TokenPlayerPrefsName + "_" + currentGameId, token);
            onLoginCallback.Invoke(user);
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
#else
            return "";
#endif
        }
    }
}
