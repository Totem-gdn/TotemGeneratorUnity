using System;
using System.Collections;
using System.Collections.Generic;
using TotemConsts;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Networking;
using TotemEntities;

namespace TotemServices {
    public class TotemSimpleAPI : MonoBehaviour
    {

        #region Response Models

        [Serializable]
        private class ItemsResponse
        {
            public int status;
            public string message;
            public ItemInfo[] data;
        }

        [Serializable]
        private class ItemInfo
        {
            public string _id;
            public string seed;
            public string owner;
            public string[] owners;
            public string itemType;

            public TotemSpear item;

            public string createdAt;
            public string updatedAt;
        }

        [Serializable]
        private class AvatarsResponse
        {
            public int status;
            public string message;
            public AvatarInfo[] data;
        }
        [Serializable]
        private class AvatarInfo
        {
            public string _id;
            public string seed;
            public string owner;
            public string[] owners;

            public TotemAvatar avatar;

            public string createdAt;
            public string updatedAt;
        }

        #endregion


        #region Requests

        public void GetItems(string publicKey, UnityAction<List<TotemSpear>> onSuccess, UnityAction<string> onFailure = null)
        {
            StartCoroutine(GetItemsCoroutine(publicKey, onSuccess, onFailure));
        }

        private IEnumerator GetItemsCoroutine(string publicKey, UnityAction<List<TotemSpear>> onSuccess, UnityAction<string> onFailure = null)
        {
            UnityWebRequest www = UnityWebRequest.Get(ServicesEnv.SimpleAPIItemsUrl + publicKey);
            yield return www.SendWebRequest();
            if (www.result == UnityWebRequest.Result.ConnectionError || www.result == UnityWebRequest.Result.ProtocolError)
            {
                Debug.LogError("TotemSimpleAPI- Failed to get items: " + www.error);
                onFailure?.Invoke(www.error);
            }
            else
            {
                ItemsResponse response = JsonUtility.FromJson<ItemsResponse>(www.downloadHandler.text);

                List<TotemSpear> spears = new List<TotemSpear>();
                foreach (var itemInfo in response.data)
                {
                    itemInfo.item.Id = itemInfo._id;
                    ColorUtility.TryParseHtmlString(itemInfo.item.shaftColor, out itemInfo.item.shaftColorRGB);
                    spears.Add(itemInfo.item);
                }

                onSuccess.Invoke(spears);
            }
        }



        public void GetAvatas(string publicKey, UnityAction<List<TotemAvatar>> onSuccess, UnityAction<string> onFailure = null)
        {
            StartCoroutine(GetAvatasCoroutine(publicKey, onSuccess, onFailure));
        }

        private IEnumerator GetAvatasCoroutine(string publicKey, UnityAction<List<TotemAvatar>> onSuccess, UnityAction<string> onFailure = null)
        {
            UnityWebRequest www = UnityWebRequest.Get(ServicesEnv.SimpleAPIAvatarsUrl + publicKey);
            yield return www.SendWebRequest();
            if (www.result == UnityWebRequest.Result.ConnectionError || www.result == UnityWebRequest.Result.ProtocolError)
            {
                Debug.LogError("TotemSimpleAPI- Failed to get avatars: " + www.error);
                onFailure?.Invoke(www.error);
            }
            else
            {
                AvatarsResponse response = JsonUtility.FromJson<AvatarsResponse>(www.downloadHandler.text);

                List<TotemAvatar> avatars = new List<TotemAvatar>();
                foreach (var avatarInfo in response.data)
                {
                    avatarInfo.avatar.Id = avatarInfo._id;
                    ColorUtility.TryParseHtmlString(avatarInfo.avatar.eyeColor, out avatarInfo.avatar.eyeColorRGB);
                    ColorUtility.TryParseHtmlString(avatarInfo.avatar.skinColor, out avatarInfo.avatar.skinColorRGB);
                    ColorUtility.TryParseHtmlString(avatarInfo.avatar.hairColor, out avatarInfo.avatar.hairColorRGB);
                    ColorUtility.TryParseHtmlString(avatarInfo.avatar.clothingColor, out avatarInfo.avatar.clothingColorRGB);
                    avatars.Add(avatarInfo.avatar);
                }

                onSuccess.Invoke(avatars);
            }

        }

        #endregion
    }
}
