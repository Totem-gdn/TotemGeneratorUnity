using System.Collections;
using System;
using System.Collections.Generic;
using Newtonsoft.Json;
using UnityEngine;
using UnityEngine.Events;
using TotemEntities;
using TotemConsts;

namespace TotemServices
{
    public class TotemWeb3Auth : MonoBehaviour
    {
        private Web3Auth _web3Auth;
        private UnityAction<TotemUser> _onLoginCallback;


        /// <summary>
        /// 
        /// </summary>
        /// <param name="loginProvider">Which provider will be used for login (e.g. Google, Facebook, Discord)</param>
        /// <param name="onLogin">Will be called after login</param>
        public void LoginUser(Provider loginProvider, UnityAction<TotemUser> onLogin)
        {
            _onLoginCallback = onLogin;
            _web3Auth.login(new LoginParams() { loginProvider = loginProvider });
        }


        private void Awake()
        {
            _web3Auth = gameObject.AddComponent<Web3Auth>();
            _web3Auth.setOptions(new Web3AuthOptions()
            {
                whiteLabel = new WhiteLabelData()
                {
                    name = ServicesEnv.Web3AuthWhiteLabelName,
                    logoLight = null,
                    logoDark = null,
                    defaultLanguage = "en",
                    dark = true,
                    theme = new Dictionary<string, string>
                {
                    { "primary", ServicesEnv.Web3AuthWhiteLabeColor }
                }
                },
                network = ServicesEnv.Web3AuthNetwork,
                redirectUrl = new Uri(LoadRedirectUrl()),
                clientId = ServicesEnv.Web3AuthClientId
            });

            _web3Auth.onLogin += OnWeb3Login;

        }


        private void OnWeb3Login(Web3AuthResponse response)
        {
            string idToken = response.userInfo.idToken;
            var decode = idToken.Split('.')[1];
            decode = decode.Replace('-', '+'); // 62nd char of encoding
            decode = decode.Replace('_', '/'); // 63rd char of encoding
            var padLength = decode.Length % 4;
            if (padLength == 2)
            {
                decode += "==";
            }
            else if (padLength == 3)
            {
                decode += "=";
            }
            byte[] bytes = Convert.FromBase64String(decode);
            Web3AuthIdToken web3AuthIdToken = JsonConvert.DeserializeObject<Web3AuthIdToken>(System.Text.Encoding.ASCII.GetString(bytes));

            string publicKey = web3AuthIdToken.wallets[0].public_key;

            TotemUser user = new TotemUser(response.userInfo.name, response.userInfo.email, response.userInfo.profileImage, 
                publicKey, response.privKey);

            _onLoginCallback.Invoke(user);
        }

        private string LoadRedirectUrl()
        {
#if UNITY_EDITOR || UNITY_STANDALONE
            return "http://localhost";
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
