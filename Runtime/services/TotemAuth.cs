using System.Collections;
using System;
using System.Net;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using Newtonsoft.Json;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Networking;
using TotemEntities;
using TotemConsts;
using System.Linq;
using NativeWebSocket;


namespace TotemServices
{

    public class TotemAuth : MonoBehaviour
    {

        #region Models
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

        [Serializable]
        private class SocketMessage
        {
            [JsonProperty("event")]
            public string Event { get; set; }

            public SocketRoomData data;
        }

        [Serializable]
        private class SocketRoomData
        {
            public string room;
        }
        #endregion

        private const string redirectUrlQueryName = "success_url";
        private const string gameIdQueryName = "game_id";
        private const string socketEnabledQueryName = "ws_enabled";
        private const string socketRoomIdQueryName = "roomId";

        private const string socketEventTokenName = "token";
        private const string socketEventDisconnectedType = "user:disconnected";

        [DllImport("__Internal")]
        private static extern void OpenURLPopup(string url);
        [DllImport("__Internal")]
        private static extern void ClosePopup();


        private string currentGameId;
        private UnityAction<TotemUser> onLoginCallback;
        private HttpListener httpListener;
        private WebSocket socket;

        private bool loginComplete;

        private void Start()
        {
            Application.deepLinkActivated += OnDeepLinkActivated;
        }

        void Update()
        {
#if !UNITY_WEBGL || UNITY_EDITOR
            if (socket != null)
            {
                socket.DispatchMessageQueue();
            }
#endif
        }

        /// <summary>
        /// Open a web-page in a browser for user to login
        /// </summary>
        /// <param name="onSuccess"></param>
        public async void LoginUser(UnityAction<TotemUser> onSuccess, string gameId)
        {
            onLoginCallback = onSuccess;
            currentGameId = gameId;
            loginComplete = false;


            string socketRoomId = GenerateSocketRoomId();
            string query = $"?{socketEnabledQueryName}=true&{socketRoomIdQueryName}={socketRoomId}";
#if !UNITY_EDITOR
            if (!string.IsNullOrEmpty(gameId))
            {
                query += $"&{gameIdQueryName}={gameId}";
            }
#endif
#if UNITY_ANDROID || UNITY_IOS
             query += $"&{redirectUrlQueryName}={LoadRedirectUrl()}";
#endif

            SetupIOSocket(socketRoomId, query);

            await socket.Connect();

        }

        /// <summary>
        /// Logins the user using token from the command line or player prefs
        /// </summary>
        /// <param name="onComplete"></param>
        /// <param name="onFailure"></param>
        public void LoginUserFromToken(string gameId, UnityAction<TotemUser> onComplete, UnityAction<string> onFailure)
        {
            loginComplete = false;

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
                Debug.Log(prefsToken);
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
            httpListener.Prefixes.Add(ServicesEnv.HttpListenerUrl + "auth/");
            httpListener.Start();
            HttpListenerContext context = await httpListener.GetContextAsync();
            HttpListenerRequest req = context.Request;

            string token = req.QueryString.Get(ServicesEnv.HttpResultParameterName);

            CompleteLogin(token);

            HttpListenerResponse response = context.Response;
            string responseText = Resources.Load<TextAsset>(ServicesEnv.AuthHttpResponseFileName).text;
            byte[] responseBuffer = System.Text.Encoding.UTF8.GetBytes(responseText);
            response.ContentLength64 = responseBuffer.Length;
            var output = response.OutputStream;
            output.Write(responseBuffer, 0, responseBuffer.Length);

            httpListener.Stop();
            httpListener = null;
        }

        /// <summary>
        /// Cancels the login process and stops all related processes
        /// </summary>
        public async void CancelLogin()
        {
            httpListener?.Stop();

            await socket.Close();

            onLoginCallback = null;
            httpListener = null;
            socket = null;
            loginComplete = false;
        }

        private void CompleteLogin(string token)
        {
            if (!loginComplete) //Login complete check for possible case with redirect and socket login interfering
            {
                TotemUser user = null;
                if (!string.IsNullOrEmpty(token))
                {
                    user = HandleToken(token);
                    PlayerPrefs.SetString(ServicesEnv.TokenPlayerPrefsName + "_" + currentGameId, token);
#if UNITY_WEBGL && !UNITY_EDITOR
                    ClosePopup();
#endif
                }

                onLoginCallback.Invoke(user);

                loginComplete = true;
            }

            httpListener?.Stop();
            socket?.Close();

            httpListener = null;
            socket = null;
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
            CompleteLogin(token);
        }

        private string LoadRedirectUrl()
        {
#if UNITY_EDITOR || UNITY_STANDALONE
            return ServicesEnv.HttpListenerUrl + "auth/";
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

        private void SetupIOSocket(string roomId, string authQuery)
        {
            socket = new WebSocket(ServicesEnv.SocketServerURL);
   
            socket.OnOpen += () =>
            {
                var socketMessage = new SocketMessage()
                {
                    Event = "connect:room",
                    data = new SocketRoomData() { room = roomId }
                };
                socket.SendText(JsonConvert.SerializeObject(socketMessage));
#if UNITY_WEBGL && !UNITY_EDITOR
                OpenURLPopup(ServicesEnv.AuthServiceUrl + authQuery);
#else
                Application.OpenURL(ServicesEnv.AuthServiceUrl + authQuery);
#endif
            };

            socket.OnClose += (e) =>
            {
            };

            socket.OnMessage += (bytes) =>
            {
                var message = System.Text.Encoding.UTF8.GetString(bytes);
                var resultAttributes = JsonConvert.DeserializeObject< Dictionary<string, string>>(message);
                if (resultAttributes.ContainsKey(socketEventTokenName))
                {
                    CompleteLogin(resultAttributes[socketEventTokenName]);

                }
                else if (resultAttributes["type"].Equals(socketEventDisconnectedType))
                {
                    CompleteLogin("");
                }
            };

        }



        private string GenerateSocketRoomId()
        {
            return Guid.NewGuid().ToString();
        }
    }
}
