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
            public string gameAddress;
            public string data;
        }

        #endregion

        private int requestLimit = 20; 

        #region Requests


        public void GetAssetLegacy(string assetId, string assetType, string gameAddress, string publicKey,
            UnityAction<List<TotemLegacyRecord>> onSuccess, UnityAction<string> onFailure = null)
        {
            var key = new EthECKey(TotemUtils.Convert.HexToByteArray(publicKey), false);
            string address = key.GetPublicAddress();

            StartCoroutine(GetAssetLegacyCoroutine(assetId, assetType, gameAddress, address, 0, new List<TotemLegacyRecord>(), onSuccess, onFailure));
        }

        private IEnumerator GetAssetLegacyCoroutine(string assetId, string assetType, string gameAddress, string playerAddress, int offset, 
            List<TotemLegacyRecord> data,
            UnityAction<List<TotemLegacyRecord>> onSuccess, 
            UnityAction<string> onFailure)
        {
            string url = ServicesEnv.AssetLegacyServicesUrl +
                $"/{assetType}?playerAddress={playerAddress}&assetId={assetId}&gameAddress={gameAddress}&limit={requestLimit}&offset={offset}";

            UnityWebRequest www = UnityWebRequest.Get(url);
            yield return www.SendWebRequest();

            if (www.result == UnityWebRequest.Result.ConnectionError || www.result == UnityWebRequest.Result.ProtocolError)
            {
                Debug.LogError("TotemLegacyService- Failed to get legecy records: " + www.downloadHandler.text);
                onFailure?.Invoke(www.error);
            }
            else
            {
                LegacyRepsonse response = JsonUtility.FromJson<LegacyRepsonse>(www.downloadHandler.text);
                foreach (var record in response.results)
                {
                    string decodedData = record.data;
                    try
                    {
                        decodedData = System.Text.Encoding.UTF8.GetString(TotemUtils.Convert.DecodeBase64(record.data));
                    }
                    catch (Exception e)
                    {
                        Debug.LogError($"TotemLegacyService- Failed to decode data (assetId: {record.assetId}): " + e.Message);
                    }
                    TotemLegacyRecord legacy = new TotemLegacyRecord(TotemEnums.LegacyRecordTypeEnum.Achievement, record.assetId, record.gameId, 
                        decodedData, record.timestamp);
                    data.Add(legacy);
                }

                if (response.total > data.Count)
                {
                    StartCoroutine(GetAssetLegacyCoroutine(assetId, assetType, gameAddress, playerAddress, data.Count, data, onSuccess, onFailure));
                }
                else
                {
                    onSuccess.Invoke(data);
                }

            }

            www.Dispose();
        }



        public void AddAssetLegacy(TotemLegacyRecord legacy, string assetType, string publicKey,
            UnityAction onSuccess = null, UnityAction<string> onFailure = null)
        {
            var key = new EthECKey(TotemUtils.Convert.HexToByteArray(publicKey), false);
            string address = key.GetPublicAddress();

            StartCoroutine(AddAssetLegacyCoroutine(legacy, assetType, address, onSuccess, onFailure));
        }

        private IEnumerator AddAssetLegacyCoroutine(TotemLegacyRecord legacy, string assetType, string playerAddress,
            UnityAction onSuccess = null, UnityAction<string> onFailure = null)
        {
            string url = ServicesEnv.AssetLegacyServicesUrl + $"/{assetType}";

            string base64Data = System.Convert.ToBase64String(System.Text.Encoding.UTF8.GetBytes(legacy.data));

            LegacyRequest request = new LegacyRequest
            {
                playerAddress = playerAddress,
                assetId = legacy.assetId,
                gameAddress = legacy.gameAddress,
                data = base64Data
            };

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
