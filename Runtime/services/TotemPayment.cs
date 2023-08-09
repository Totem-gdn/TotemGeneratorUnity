using System;
using System.Net;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Networking;
using Newtonsoft.Json;
using TotemEntities;
using TotemConsts;
using Nethereum.Signer;
using TotemUtils;
using NativeWebSocket;


namespace TotemServices
{
    public class TotemPayment : MonoBehaviour
    {
        #region Models
        [Serializable]
        private class PaymentLinkRequest
        {
            public string successUrl;
            public string ownerAddress;
            public string imageUrl;
            public bool redirect;
            public bool redirectAfterPayment;
        }

        [Serializable]
        private class PaymentLinkResponse
        {
            public string url;
            public string order_id;
        }
        #endregion

        private WebSocket socket;
        private const string redirectUrlQueryName = "appUrl";
        private const string socketEnabledQueryName = "ws_enabled";
        private const string socketRoomIdQueryName = "roomId";

        private const string socketEventPaymentName = "payment_result";
        private const string socketEventDisconnectedType = "user:disconnected";


        private UnityAction<bool> onPurchaseCallback;
        private bool paymentComplete;

        void Update()
        {
#if !UNITY_WEBGL || UNITY_EDITOR
            if (socket != null)
            {
                socket.DispatchMessageQueue();
            }
#endif
        }

        public void PurchaseAsset(string assetType, string publicKey, UnityAction<bool> onSuccess)
        {
            var key = new EthECKey(TotemUtils.Convert.HexToByteArray(publicKey), false);
            string address = key.GetPublicAddress();

            StartCoroutine(PurchaseAssetCoroutine(assetType, address, onSuccess));
        }

        private IEnumerator PurchaseAssetCoroutine(string assetType, string ownerAddress, UnityAction<bool> onSuccess)
        {
            onPurchaseCallback = onSuccess;
            paymentComplete = false;

            string socketRoomId = WebUtils.GenerateSocketRoomId();
            string redirectUrl = $"{ServicesEnv.PaymentServiceUrl}?{socketEnabledQueryName}=true&{socketRoomIdQueryName}={socketRoomId}";

#if UNITY_ANDROID || UNITY_IOS
            redirectUrl += $"&{redirectUrlQueryName}={LoadRedirectUrl()}";
#elif UNITY_WEBGL && !UNITY_EDITOR
            redirectUrl += $"&{autoCloseQueryName}=true";
#endif

            PaymentLinkRequest linkRequest = new PaymentLinkRequest()
            {
                ownerAddress = ownerAddress,
                redirect = true,
                successUrl = redirectUrl,
                imageUrl = "https://totem-explorer.com/assets/images/avatar-placeholder.webp"
            };

            string paymentLinkUrl = $"{ServicesEnv.PaymentAPIUrl}/{ServicesEnv.PaymentSystem}/{assetType}/link";

            var www = WebUtils.CreateRequestJson(paymentLinkUrl, JsonUtility.ToJson(linkRequest));
            yield return www.SendWebRequest();

            if (www.result == UnityWebRequest.Result.ConnectionError || www.result == UnityWebRequest.Result.ProtocolError)
            {
                Debug.LogError("TotemPayment- Failed to get payment link: " + www.downloadHandler.text);
            }
            else
            {
                PaymentLinkResponse response = JsonUtility.FromJson<PaymentLinkResponse>(www.downloadHandler.text);
                OpenPaymentLink(socketRoomId, response.url);
            }

            www.Dispose();


        }

        private async void OpenPaymentLink(string socketRoomId, string linkUrl)
        {
            SetupWebSocket(socketRoomId, linkUrl);

            await socket.Connect();
        }


        private void SetupWebSocket(string roomId, string linkUrl)
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
                OpenURLPopup(linkUrl);
#else
                Application.OpenURL(linkUrl);
#endif
            };

            socket.OnMessage += (bytes) =>
            {
                var message = System.Text.Encoding.UTF8.GetString(bytes);
                var resultAttributes = JsonConvert.DeserializeObject<Dictionary<string, string>>(message);
                if (resultAttributes.ContainsKey(socketEventPaymentName))
                {
                    UnityThread.executeInUpdate(() =>
                    {
                        CompletePurchase();
                        Debug.Log("TotemPayment- Purchase complete");
                    });
                }
                else if (resultAttributes["type"].Equals(socketEventDisconnectedType))
                {
                    UnityThread.executeInUpdate(() =>
                    {
                        CompletePurchase();
                        onPurchaseCallback(false);
                        Debug.Log("TotemPayment- Purchase canceled");
                    });
                }
            };

        }

        private async void ListenHttpResponse()
        {
            HttpListener httpListener = new HttpListener();
            httpListener.Prefixes.Add(ServicesEnv.HttpListenerUrl + "payment/");
            httpListener.Start();
            HttpListenerContext context = await httpListener.GetContextAsync();
            HttpListenerRequest req = context.Request;

            Debug.Log("Res payment: " + req.Url);

            onPurchaseCallback.Invoke(true);

            HttpListenerResponse response = context.Response;
            string responseText = Resources.Load<TextAsset>(ServicesEnv.AuthHttpResponseFileName).text;
            byte[] responseBuffer = System.Text.Encoding.UTF8.GetBytes(responseText);
            response.ContentLength64 = responseBuffer.Length;
            var output = response.OutputStream;
            output.Write(responseBuffer, 0, responseBuffer.Length);

            httpListener.Stop();
        }

        private void CompletePurchase()
        {
            if (!paymentComplete) //Payment complete check for possible case with redirect and socket login interfering
            {
                onPurchaseCallback(true);

                paymentComplete = true;
            }

            socket?.Close();

            socket = null;
        }

        private string LoadRedirectUrl()
        {
#if UNITY_EDITOR || UNITY_STANDALONE
            return ServicesEnv.HttpListenerUrl + "payment/";
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
