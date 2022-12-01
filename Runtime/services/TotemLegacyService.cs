using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Networking;
using TotemEntities;
using TotemConsts;


namespace TotemServices
{
    public class TotemLegacyService : MonoBehaviour
    {

        #region Resoponse Models

        [Serializable]
        private class LegacyRepsonse
        {
            public LegacyAchievement[] achievements;
        }

        [Serializable]
        private class LegacyAchievement
        {
            public string _id;
            public string itemId;
            public string gameId;
            public string timestamp;
            public string data;
        }

        #endregion

        #region Requests

        public void GetAchivements(string itemId, UnityAction<List<TotemLegacyRecord>> onSuccess, UnityAction<string> onFailure = null)
        {
            StartCoroutine(GetAchiementsCoroutine(itemId, "", onSuccess, onFailure));
        }

        public void GetAchivements(string itemId, string gameId, UnityAction<List<TotemLegacyRecord>> onSuccess, UnityAction<string> onFailure = null)
        {
            StartCoroutine(GetAchiementsCoroutine(itemId, gameId, onSuccess, onFailure));
        }

        private IEnumerator GetAchiementsCoroutine(string itemId, string gameId, UnityAction<List<TotemLegacyRecord>> onSuccess, UnityAction<string> onFailure)
        {
            string url = ServicesEnv.LegacyServicesUrl + itemId + (string.IsNullOrEmpty(gameId) ? "" : $"/{gameId}");
            UnityWebRequest www = UnityWebRequest.Get(url);
            yield return www.SendWebRequest();
            if (www.result == UnityWebRequest.Result.ConnectionError || www.result == UnityWebRequest.Result.ProtocolError)
            {
                Debug.LogError("TotemLegacyService- Failed to get achievements: " + www.error);
                onFailure?.Invoke(www.error);
            }
            else
            {
                LegacyRepsonse response = JsonUtility.FromJson<LegacyRepsonse>(www.downloadHandler.text);
                List<TotemLegacyRecord> legacies = new List<TotemLegacyRecord>();
                foreach (var ach in response.achievements)
                {
                    var base64EncodedBytes = Convert.FromBase64String(ach.data);
                    string utfData = System.Text.Encoding.UTF8.GetString(base64EncodedBytes);

                    TotemLegacyRecord legacy = new TotemLegacyRecord(TotemEnums.LegacyRecordTypeEnum.Achievement, ach.itemId, ach.gameId, utfData);
                    legacies.Add(legacy);
                }

                onSuccess.Invoke(legacies);
            }

            www.Dispose();
        }



        public void AddAchievement(TotemLegacyRecord legacy, UnityAction onSuccess = null, UnityAction<string> onFailure = null)
        {
            StartCoroutine(AddAchievementCoroutine(legacy, onSuccess, onFailure));
        }

        private IEnumerator AddAchievementCoroutine(TotemLegacyRecord legacy, UnityAction onSuccess = null, UnityAction<string> onFailure = null)
        {
            string url = ServicesEnv.LegacyServicesUrl + legacy.itemId + "/" + legacy.gameId;

            UnityWebRequest www = UnityWebRequest.Post(url, legacy.data);
            yield return www.SendWebRequest();
            if (www.result == UnityWebRequest.Result.ConnectionError || www.result == UnityWebRequest.Result.ProtocolError)
            {
                Debug.LogError("TotemLegacyService- Failed to add legacy: " + www.error);
                onFailure?.Invoke(www.error);
            }
            else
            {
                onSuccess?.Invoke();
            }

            www.Dispose();

        }

        #endregion
    }
}
