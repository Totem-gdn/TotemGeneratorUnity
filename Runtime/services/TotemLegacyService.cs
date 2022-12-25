using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Networking;
using TotemEntities;
using TotemConsts;
using Nethereum.Signer;
using TotemUtils;



namespace TotemServices
{
    public class TotemLegacyService : MonoBehaviour
    {

        #region Resoponse Models

        [Serializable]
        private class LegacyRepsonse
        {
            public int total;
            public int limit;
            public int offset;
            public LegacyRecord[] results;
        }

        [Serializable]
        private class LegacyRecord
        {
            public string assetId;
            public string gameId;
            public string timestamp;
            public string data;
        }

        [Serializable]
        private class LegacyRequest
        {
            public string playerAddress;
            public string assetId;
            public string gameId;
            public string data;
        }

        #endregion

        private int requestLimit = 20; 

        #region Requests


        public void GetAssetLegacy(string assetId, string assetType, string gameId, string privateKey,
            UnityAction<List<TotemLegacyRecord>> onSuccess, UnityAction<string> onFailure = null)
        {
            var key = new EthECKey(privateKey);
            string address = key.GetPublicAddress();

            StartCoroutine(GetAssetLegacyCoroutine(assetId, assetType, gameId, address, 0, new List<TotemLegacyRecord>(), onSuccess, onFailure));
        }

        private IEnumerator GetAssetLegacyCoroutine(string assetId, string assetType, string gameId, string playerAddress, int offset, 
            List<TotemLegacyRecord> data,
            UnityAction<List<TotemLegacyRecord>> onSuccess, 
            UnityAction<string> onFailure)
        {
            string url = ServicesEnv.AssetLegacyServicesUrl +
                $"/{assetType}?playerAddress={playerAddress}&assetId={assetId}&gameId={gameId}&limit={requestLimit}&offset={offset}";
            UnityWebRequest www = UnityWebRequest.Get(url);
            yield return www.SendWebRequest();
            if (www.result == UnityWebRequest.Result.ConnectionError || www.result == UnityWebRequest.Result.ProtocolError)
            {
                Debug.LogError("TotemLegacyService- Failed to get legecy records: " + www.error);
                onFailure?.Invoke(www.error);
            }
            else
            {
                LegacyRepsonse response = JsonUtility.FromJson<LegacyRepsonse>(www.downloadHandler.text);
                foreach (var record in response.results)
                {
                    TotemLegacyRecord legacy = new TotemLegacyRecord(TotemEnums.LegacyRecordTypeEnum.Achievement, record.assetId, record.gameId, record.data);
                    data.Add(legacy);
                }

                if (response.total > data.Count)
                {
                    StartCoroutine(GetAssetLegacyCoroutine(assetId, assetType, gameId, playerAddress, data.Count, data, onSuccess, onFailure));
                }
                else
                {
                    onSuccess.Invoke(data);
                }

            }

            www.Dispose();
        }



        public void AddAssetLegacy(TotemLegacyRecord legacy, string assetType, string privateKey,
            UnityAction onSuccess = null, UnityAction<string> onFailure = null)
        {
            var key = new EthECKey(privateKey);
            string address = key.GetPublicAddress();

            StartCoroutine(AddAssetLegacyCoroutine(legacy, assetType, address, onSuccess, onFailure));
        }

        private IEnumerator AddAssetLegacyCoroutine(TotemLegacyRecord legacy, string assetType, string playerAddress,
            UnityAction onSuccess = null, UnityAction<string> onFailure = null)
        {
            string url = ServicesEnv.AssetLegacyServicesUrl + $"/{assetType}";

            LegacyRequest request = new LegacyRequest
            {
                playerAddress = playerAddress,
                assetId = legacy.assetId,
                gameId = legacy.gameId,
                data = legacy.data
            };

            Debug.Log(JsonUtility.ToJson(request));
            UnityWebRequest www = WebUtils.CreateRequestJson(url, JsonUtility.ToJson(request));
            yield return www.SendWebRequest();
            if (www.result == UnityWebRequest.Result.ConnectionError || www.result == UnityWebRequest.Result.ProtocolError)
            {
                Debug.LogError("TotemLegacyService- Failed to add legacy: " + www.error);
                Debug.Log(www.downloadHandler.text);
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
