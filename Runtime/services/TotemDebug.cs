using System;
using System.Collections;
using System.IO;
using System.Collections.Generic;
using UnityEngine;
using TotemEnums;
using TotemConsts;
using TotemEntities;
using TotemServices.DNA;
using Newtonsoft.Json;
using UnityEngine.Events;

namespace TotemServices
{
    public class AssetOverride
    {
        public ForcedAssets forced_assets;
        public Dictionary<string, object> first_avatar_property_override;
        public Dictionary<string, object> first_item_property_override;
    }

    public class ForcedAssets
    {
        public int[] avatars;
        public int[] items;
    }


    public class TotemDebug : MonoBehaviour
    {
        public AssetOverride assetOverride { get; private set; }
        void Awake()
        {
            string pathToAssetOverride = Application.dataPath + "/" + ServicesEnv.AssetsOverrideFilePath;
#if UNITY_ANDROID || UNITY_IOS
            pathToAssetOverride = Application.persistentDataPath + "/" + ServicesEnv.AssetsOverrideFilePath;
#endif
            if (File.Exists(pathToAssetOverride))
            {
                string overrideJson = File.ReadAllText(pathToAssetOverride);
                assetOverride = JsonConvert.DeserializeObject<AssetOverride>(overrideJson);
            }
        }

        public void OverrideAvatars<T>(List<T> avatars, TotemUser user,
            TotemDNAFilter filter, TotemSmartContractManager smartContract, UnityAction onComplete) where T: new()
        {
            if (assetOverride == null)
            {
                Debug.LogWarning("TotemDebug: Avatars override failed. Asset override file not loaded");
                onComplete.Invoke();
                return;
            }


            if (assetOverride.forced_assets != null && assetOverride.forced_assets.avatars != null)
            {
                int avatarsDownloadCount = assetOverride.forced_assets.avatars.Length;
                List<T> forceLoadedAvatars = new List<T>();
                foreach (var assetId in assetOverride.forced_assets.avatars)
                {
                    smartContract.GetAvatar<T>(user, filter, assetId, (avatar) =>
                    {
                        forceLoadedAvatars.Add(avatar);
                        if (--avatarsDownloadCount == 0)
                        {
                            for (int i = 0; i < assetOverride.forced_assets.avatars.Length; i++)
                            {
                                T forcedAvatar = forceLoadedAvatars.Find((x) => smartContract.GetAssetId(x) == assetOverride.forced_assets.avatars[i]);
                                avatars.Insert(i, forcedAvatar);
                            }

                            OverrideAvatarProperties(avatars);
                            onComplete.Invoke();
                        }
                    });
                }
            }
            else
            {

                OverrideAvatarProperties(avatars);
                onComplete.Invoke();
            }
        }

        public void OverrideItems<T>(List<T> items, TotemUser user,
            TotemDNAFilter filter, TotemSmartContractManager smartContract, UnityAction onComplete) where T : new()
        {

            if (assetOverride == null)
            {
                Debug.LogWarning("TotemDebug: Items override failed. Asset override file not loaded");
                onComplete.Invoke();
                return;
            }

            if (assetOverride.forced_assets != null && assetOverride.forced_assets.items != null)
            {

                int itemsDownloadCount = assetOverride.forced_assets.items.Length;
                List<T> forceLoadedItems = new List<T>();
                foreach (var assetId in assetOverride.forced_assets.items)
                {
                    smartContract.GetItem<T>(user, filter, assetId, (item) =>
                    {
                        forceLoadedItems.Add(item);
                        if (--itemsDownloadCount == 0)
                        {
                            for (int i = 0; i < assetOverride.forced_assets.items.Length; i++)
                            {
                                T forcedItem = forceLoadedItems.Find((x) => smartContract.GetAssetId(x) == assetOverride.forced_assets.items[i]);
                                items.Insert(i, forcedItem);
                            }

                            OverrideItemProperties(items);
                            onComplete.Invoke();
                        }
                    });
                }
            }
            else
            {
                OverrideItemProperties(items);
                onComplete.Invoke();
            }
        }


        private void OverrideItemProperties<T>(List<T> items)
        {
            if (assetOverride.first_item_property_override != null && items.Count > 0)
            {
                Type itemType = items[0].GetType();
                foreach (var fieldNameValue in assetOverride.first_item_property_override)
                {
                    var field = itemType.GetField(fieldNameValue.Key);
                    if (field != null)
                    {
                        if (field.FieldType == typeof(Color))
                        {
                            Color color = Color.black;
                            if (ColorUtility.TryParseHtmlString((string)fieldNameValue.Value, out color))
                            {
                                field.SetValue(items[0], color);
                            }

                        }
                        else
                        {
                            field.SetValue(items[0], fieldNameValue.Value);
                        }
                    }
                }
            }

        }

        private void OverrideAvatarProperties<T>(List<T> avatars)
        {
            if (assetOverride.first_avatar_property_override != null && avatars.Count > 0)
            {
                Type avatarType = avatars[0].GetType();
                foreach (var fieldNameValue in assetOverride.first_avatar_property_override)
                {
                    var field = avatarType.GetField(fieldNameValue.Key);
                    if (field != null)
                    {
                        if (field.FieldType == typeof(Color))
                        {
                            Color color = Color.black;
                            if (ColorUtility.TryParseHtmlString((string)fieldNameValue.Value, out color))
                            {
                                field.SetValue(avatars[0], color);
                            }

                        }
                        else
                        {
                            field.SetValue(avatars[0], fieldNameValue.Value);
                        }
                    }
                }
            }

        }

    }

}
