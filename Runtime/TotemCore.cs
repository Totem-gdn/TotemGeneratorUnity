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
using TotemEnums;

public class TotemCore
{
    /// <summary>
    /// Invoked after successful login
    /// Holds user's profile information and a publicKey for item retrieval 
    /// </summary>
    public UnityEvent<TotemUser> OnUserProfileLoaded;

    /// <summary>
    /// Currently logged in user info
    /// </summary>
    public TotemUser CurrentUser { get; private set; }

    private TotemLegacyService _legacyService;
    private TotemAuth _auth;
    private TotemSmartContractManager _smartContract;
    private TotemAnalytics _analytics;

    private GameObject _servicesGameObject;

    private string _gameId;
    private string _userPublicKey;
    private string _userEmail;

    /// <summary>
    /// Initialize DB and services
    /// </summary>
    /// <param name="gameId">Id of your game. Used for legacy records identification</param>
    public TotemCore(string gameId)
    {
        _gameId = gameId;
        CreateServicesGameObject();

        OnUserProfileLoaded = new UnityEvent<TotemUser>();
    }

    /// <summary>
    /// Opens social login web-page
    /// Invokes OnUserProfileLoaded event on completion
    /// </summary>
    /// <param name="onComplete">If set, will be invoked instead of OnUserProfileLoaded event</param>
    public void AuthenticateCurrentUser(UnityAction<TotemUser> onComplete = null)
    {
        _auth.LoginUser((user) =>
        {
            CurrentUser = user;
            _userPublicKey = user.PublicKey;
            _userEmail = user.Email;

            if (onComplete != null)
            {
                onComplete.Invoke(user);
            }
            else
            {
                OnUserProfileLoaded.Invoke(user);
            }

        });
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
        _smartContract.GetAvatars(user, filter, onComplete);

        _analytics.RecordAction(TotemServicesAction.avatars_requested, _gameId, CurrentUser.PublicKey, _userEmail);
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
        _smartContract.GetItems(user, filter, onComplete);

        _analytics.RecordAction(TotemServicesAction.items_requested, _gameId, CurrentUser.PublicKey, _userEmail);
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

            _analytics.RecordAction(TotemServicesAction.legacy_requested, _gameId, _userPublicKey, _userEmail);
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

            _analytics.RecordAction(TotemServicesAction.legacy_requested, _gameId, _userPublicKey, _userEmail);
        });
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


        MonoBehaviour.DontDestroyOnLoad(_servicesGameObject);
    }



}
