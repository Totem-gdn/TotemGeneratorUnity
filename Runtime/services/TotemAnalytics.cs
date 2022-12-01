using System;
using System.Collections;
using UnityEngine;
using UnityEngine.Networking;
using TotemEnums;
using TotemConsts;

namespace TotemServices
{
    public class TotemAnalytics : MonoBehaviour
    {
        public void RecordAction(TotemServicesAction action, string distinctId, string userId, string userEmail)
        {
            StartCoroutine(RecordActionCoroutine(action, distinctId, userId, userEmail));
        }

        private IEnumerator RecordActionCoroutine(TotemServicesAction action, string distinctId, string userId, string userEmail)
        {
            string trackJson = GenerateTrackJson(action, distinctId, userId, userEmail);

            var www = new UnityWebRequest(ServicesEnv.AnalyticsUrl, "POST");
            byte[] bodyRaw = System.Text.Encoding.UTF8.GetBytes(trackJson);
            www.uploadHandler = new UploadHandlerRaw(bodyRaw);
            www.downloadHandler = new DownloadHandlerBuffer();
            www.SetRequestHeader("Content-Type", "application/json");

            yield return www.SendWebRequest();
            if (www.result == UnityWebRequest.Result.ConnectionError || www.result == UnityWebRequest.Result.ProtocolError)
            {
                Debug.LogError("TotemAnalytics- Failed to record an action: " + www.error);
            }

            www.Dispose();
        }


        private string GenerateTrackJson(TotemServicesAction action, string distinctId, string userId, string userEmail)
        {
            long unixTimestamp = DateTimeOffset.Now.ToUnixTimeSeconds();

            var sBuilder = new System.Text.StringBuilder();
            sBuilder.Append("[{\"event\": \"");
            sBuilder.Append(action.ToString());
            sBuilder.Append("\", \"properties\": {\"time\": ");
            sBuilder.Append(unixTimestamp);
            sBuilder.Append(",\"distinct_id\": \"");
            sBuilder.Append(distinctId);
            sBuilder.Append("\",\"$insert_id\": \"");
            sBuilder.Append(unixTimestamp);
            sBuilder.Append("\",\"token\": \"");
            sBuilder.Append(ServicesEnv.AnalyticsToken);
            sBuilder.Append("\",\"user_id\": \"");
            sBuilder.Append(userId);
            sBuilder.Append("\",\"public_key\": \"");
            sBuilder.Append(userId.Substring(0, ServicesEnv.AnalyticsPublicKeyLength));
            sBuilder.Append("\",\"username\": \"");
            sBuilder.Append(userEmail);
            sBuilder.Append("\",\"source\": \"");
            sBuilder.Append("unity_plugin");
            sBuilder.Append("\"}}]");

            return sBuilder.ToString();
        }
    }
}
