using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions;
using UnityEngine.Events;
using System.Net.Http;
using TotemServices;
using TotemServices.DNA;
using TotemEntities;
using TotemEntities.DNA;
using TotemConsts;
using TotemEnums;
using TotemUtils;

public class TotemCore
{
    /// <summary>
    /// Currently logged in user info
    /// </summary>
    public TotemUser CurrentUser { get; set; }

    private TotemLegacyService _legacyService;
    private TotemAuth _auth;
    private TotemSmartContractManager _smartContract;
    private TotemPayment _payment;
    private TotemAnalytics _analytics;
    private TotemDebug _debug;

    private GameObject _servicesGameObject;

    private string _gameId;
    private bool _debugEnabled;

    /// <summary>
    /// Initialize DB and services
    /// </summary>
    /// <param name="gameId">Id of your game. Used for legacy records identification</param>
    public TotemCore(string gameId, bool enableDebug = true)
    {
        _gameId = gameId;
        _debugEnabled = enableDebug;
        CreateServicesGameObject();
    }

    /// <summary>
    /// Opens social login web-page
    /// Invokes OnUserProfileLoaded event on completion
    /// </summary>
    /// <param name="onComplete">Callback for login completion. TotemUser will be null if login was canceled</param>
    public void AuthenticateCurrentUser(UnityAction<TotemUser> onComplete)
    {
        _auth.LoginUser((user) =>
        {
            if (user != null)
            {
                CurrentUser = user;
                _analytics.RecordAction(TotemServicesAction.user_login, _gameId, CurrentUser, CurrentUser.Email);
            }

            onComplete.Invoke(user);

        }, _gameId);

    }

    /// <summary>
    /// Login previous user 
    /// Can be use in a Remember-me implementation
    /// Invokes OnUserProfileLoaded event on completion
    /// </summary>
    /// <param name="onComplete"></param>
    /// <param name="onFailure"></param>
    public void AuthenticateLastUser(UnityAction<TotemUser> onComplete, UnityAction<string> onFailure = null)
    {
        _auth.LoginUserFromToken(_gameId, (user) =>
        {
            CurrentUser = user;

            _analytics.RecordAction(TotemServicesAction.user_login, _gameId, CurrentUser, CurrentUser.Email);

            onComplete.Invoke(user);

        },
        (error) =>
        {
            onFailure?.Invoke("Failed to authenticate last user: " + error);
        });
    }

    /// <summary>
    /// Should be called to stop the login process
    /// </summary>
    public void CancelAuthentication()
    {
        _auth.CancelLogin();
    }

    /// <summary>
    /// Retrieves user avatars from a smart contract
    /// </summary>
    /// <typeparam name="T">Type of model implementing filter fields</typeparam>
    /// <param name="user"></param>
    /// <param name="filter">Filter to use for converting DNA to object fields</param>
    /// <param name="onComplete">Holds the list with filtered avatars</param>
    public void GetUserAvatars<T>(TotemUser user, TotemDNAFilter filter, UnityAction<List<T>> onComplete) where T : new()
    {
        _smartContract.GetAvatars<T>(user, filter, (avatars) =>
        {
            if (_debugEnabled)
            {
                _debug.OverrideAvatars(avatars, user, filter, _smartContract, () =>
                {
                    onComplete.Invoke(avatars);
                });
            }
            else
            {
                onComplete.Invoke(avatars);
            }
        });

        _analytics.RecordAction(TotemServicesAction.avatars_requested, _gameId, CurrentUser, CurrentUser.Email);
    }

    /// <summary>
    /// Retrieves user items from a smart contract
    /// </summary>
    /// <typeparam name="T">Type of model implementing filter fields</typeparam>
    /// <param name="user"></param>
    /// <param name="filter">Filter to use for converting DNA to object fields</param>
    /// <param name="onComplete">Holds the list with filtered items</param>
    public void GetUserItems<T>(TotemUser user, TotemDNAFilter filter, UnityAction<List<T>> onComplete) where T : new()
    {
        _smartContract.GetItems<T>(user, filter, (items) =>
        {
            if (_debugEnabled)
            {
                _debug.OverrideItems(items, user, filter, _smartContract, () =>
                {
                    onComplete.Invoke(items);
                });
            }
            else
            {
                onComplete.Invoke(items);
            }
        });

        _analytics.RecordAction(TotemServicesAction.items_requested, _gameId, CurrentUser, CurrentUser.Email);
    }

    /// <summary>
    /// Retrieves legacy records for the provided asset
    /// </summary>
    /// <param name="asset">Asset to get records for</param>
    /// <param name="onSuccess">Callback contains list of legacy records</param>
    /// <param name="gameId">From which game to retrieve legacy records. Leave empty to get from current game</param>
    public void GetLegacyRecords(object asset, TotemAssetType assetType, UnityAction<List<TotemLegacyRecord>> onSuccess, string gameId = "")
    {
        var assetId = _smartContract.GetAssetId(asset);
        if (assetId < 0)
        {
            Debug.LogError("Asset ID was not found!");
            return;
        }

        _legacyService.GetAssetLegacy(assetId.ToString(), assetType.ToString(), string.IsNullOrEmpty(gameId) ? _gameId : gameId, CurrentUser.PublicKey, (records) =>
        {
            onSuccess.Invoke(records);

            _analytics.RecordAction(TotemServicesAction.legacy_requested, _gameId, CurrentUser, CurrentUser.Email);
        });
    }

    /// <summary>
    /// Adds a legacy record to provided asset
    /// </summary>
    /// <param name="data">An UTF-8 encoded string</param>
    /// <param name="onSuccess">Callback contains newly created legacy record</param>
    public void AddLegacyRecord(object asset, TotemAssetType assetType, string data, UnityAction<TotemLegacyRecord> onSuccess = null)
    {
        var assetId = _smartContract.GetAssetId(asset);
        if (assetId < 0)
        {
            Debug.LogError("Asset ID was not found!");
            return;
        }

        TotemLegacyRecord legacy = new TotemLegacyRecord(LegacyRecordTypeEnum.Achievement, assetId.ToString(), _gameId, data);
        _legacyService.AddAssetLegacy(legacy, assetType.ToString(), CurrentUser.PublicKey, () =>
        {
            Debug.Log($"Legacy record created");
            onSuccess?.Invoke(legacy);

            _analytics.RecordAction(TotemServicesAction.legacy_saved, _gameId, CurrentUser, CurrentUser.Email);
        });
    }

    /// <summary>
    /// Opens external web browser to proceed with asset purchase
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="assetType"></param>
    /// <param name="onSuccess"></param>
    /// <param name="onFailure"></param>
    public void PurchaseAsset<T>(TotemAssetType assetType, TotemDNAFilter filter, UnityAction<T> onSuccess, UnityAction onFailure) where T : new()
    {
        _payment.PurchaseAsset(assetType.ToString(), CurrentUser.PublicKey, (success) =>
        {
            if (!success)
            {
                onFailure();
                return;
            }

            switch(assetType)
            {
                case TotemAssetType.avatar:
                    _smartContract.GetNewAvatar(CurrentUser, filter, onSuccess);
                    break;

                case TotemAssetType.item:
                    _smartContract.GetNewItem(CurrentUser, filter, onSuccess);
                    break;
            }

        });
    }

    /// <summary>
    /// 
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="asset"></param>
    /// <param name="genIndex"></param>
    /// <param name="register"></param>
    /// <returns></returns>
    public uint GetAssetExponentialValue<T>(T asset, int genIndex, TotemDNARegister register)
    {
        string assetDna = _smartContract.GetAssetBinaryDNA(asset);

        int startBitIndex = genIndex * 32;

        Debug.Log("DNA length:" + assetDna.Length);
        Debug.Log("Start bit: " + startBitIndex);
        string expBin = assetDna.Substring(startBitIndex, 32);

        string highExpVal = expBin.Substring(0, 16);
        string lowExpVal = expBin.Substring(16);

        switch(register)
        {
            case TotemDNARegister.LOW:
                return System.Convert.ToUInt32(lowExpVal, 2);

            case TotemDNARegister.HIGH:
                return System.Convert.ToUInt32(highExpVal, 2);

            default:
                return 0;
        }
    }

    public float GetAssetExponentialValueProbability(uint value)
    {
        return 1f - Mathf.Exp(-value / (float)ServicesEnv.AssetExponentialConstant);
    }

    /// <summary>
    /// Returns Id of the asset
    /// </summary>
    /// <param name="asset">Previously retrieved asset</param>
    /// <returns></returns>
    public string GetAssetId(object asset)
    {
        return _smartContract.GetAssetId(asset).ToString();
    }


    /// <summary>
    /// Creates a GameObjet for handling coroutines in services scripts
    /// </summary>
    private void CreateServicesGameObject()
    {
        _servicesGameObject = new GameObject("TotemServices");
        _legacyService = _servicesGameObject.AddComponent<TotemLegacyService>();
        _analytics = _servicesGameObject.AddComponent<TotemAnalytics>();
        _auth = _servicesGameObject.AddComponent<TotemAuth>();
        _smartContract = _servicesGameObject.AddComponent<TotemSmartContractManager>();
        _payment = _servicesGameObject.AddComponent<TotemPayment>();
        _debug = _servicesGameObject.AddComponent<TotemDebug>();

        UnityThread.initUnityThread();

        MonoBehaviour.DontDestroyOnLoad(_servicesGameObject);
    }
}

