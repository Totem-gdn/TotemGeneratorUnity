using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions;
using UnityEngine.Events;
using System.Net.Http;
using TotemServices;
using TotemEntities;
using enums;

public class TotemDB
{
    /// <summary>
    /// Invoked after user logs in through socials
    /// TotemAccountGateway.SocialLoginResponse holds accessToken which is needed for profile retrieval
    /// </summary>
    public UnityEvent<TotemAccountGateway.SocialLoginResponse> OnSocialLoginCompleted;

    /// <summary>
    /// Invoked after user profile retrievel
    /// Holds user's publicKey which is needed for assets retrieval
    /// </summary>
    public UnityEvent<string> OnUserProfileLoaded;

    /// <summary>
    /// Invoked after users spears retrievel
    /// Hold list of owned spears
    /// </summary>
    public UnityEvent<List<TotemSpear>> OnSpearsLoaded;

    /// <summary>
    /// Invoked after users avatars retrievel
    /// Hold list of owned avatars
    /// </summary>
    public UnityEvent<List<TotemAvatar>> OnAvatarsLoaded;

    /// <summary>
    /// Invoked after users swords retrievel
    /// Hold list of owned swords
    /// </summary>
    public UnityEvent<List<TotemSword>> OnSwordsLoaded;

    private TotemSimpleAPI _simpleAPI;
    private TotemLegacyService _legacyService;
    private TotemAccountGateway _accountGateway;
    private GameObject _servicesGameObject;

    private string _gameId;

    /// <summary>
    /// Initialize DB and services
    /// </summary>
    /// <param name="gameId">Id of your game. Used for legacy records identification</param>
    public TotemDB(string gameId)
    {
        _gameId = gameId;
        CreateServicesGameObject();

        OnSocialLoginCompleted = new UnityEvent<TotemAccountGateway.SocialLoginResponse>();
        OnUserProfileLoaded = new UnityEvent<string>();
        OnSpearsLoaded = new UnityEvent<List<TotemSpear>>();
        OnAvatarsLoaded = new UnityEvent<List<TotemAvatar>>();
        OnSwordsLoaded = new UnityEvent<List<TotemSword>>();
    }

    /// <summary>
    /// Opens social login web-page
    /// Invokes OnSocialLoginCompleted event on completion
    /// </summary>
    public void AuthenticateCurrentUser()
    {
        _accountGateway.LoginGoogle((loginResult) =>
        {
            OnSocialLoginCompleted.Invoke(loginResult);
        });

    }

    /// <summary>
    /// Retrieves publicKey for user
    /// Invokes OnUserProfileLoaded event on completion
    /// </summary>
    /// <param name="accessToken">Token from user socail login</param>
    public void GetUserProfile(string accessToken)
    {
        _accountGateway.GetUserProfile(accessToken, (publicKey) =>
        {
            OnUserProfileLoaded.Invoke(publicKey);
        });
    }

    /// <summary>
    /// Retrieves user's spears
    /// Invokes OnSpearsLoaded event on success
    /// </summary>
    /// <param name="publicKey">User's publicKey</param>
    public void GetUserSpears(string publicKey)
    {
        _simpleAPI.GetItems(publicKey, (spears) =>
        {
            OnSpearsLoaded.Invoke(spears);
        });
    }


    /// <summary>
    /// Retrieves user's avatars
    /// Invokes OnAvatarsLoaded event on success
    /// </summary>
    /// <param name="publicKey">User's publicKey</param>
    public void GetUserAvatars(string publicKey)
    {
        _simpleAPI.GetAvatas(publicKey, (avatars) =>
        {
            OnAvatarsLoaded.Invoke(avatars);
        });

    }

    /// <summary>
    /// Retrieves user's swords
    /// Invokes OnSwordsLoaded event on success
    /// </summary>
    /// <param name="publicKey">User's publicKey</param>
    public void GetUserSwords(string publicKey)
    {
        _simpleAPI.GetSwords(publicKey, (swords) =>
        {
            OnSwordsLoaded.Invoke(swords);
        });

    }

    /// <summary>
    /// Retrieves legacy records for the provided asset
    /// </summary>
    /// <param name="asset">Spear or avatar asset to get records for</param>
    /// <param name="onSuccess">Callback contains list of legacy records</param>
    /// <param name="gameId">From which game to retrieve legacy records. Can be left emtpy to retrieve from all</param>
    public void GetLegacyRecords(ITotemAsset asset, UnityAction<List<TotemLegacyRecord>> onSuccess, string gameId = "")
    {
        _legacyService.GetAchivements(asset.Id, gameId, (records) =>
        {
            onSuccess.Invoke(records);
        });
    }

    /// <summary>
    /// Adds a legacy record to provided asset
    /// </summary>
    /// <param name="asset">Spear or avatar asset</param>
    /// <param name="data">An UTF-8 encoded string</param>
    /// <param name="onSuccess">Callback contains newly created legacy record</param>
    public void AddLegacyRecord(ITotemAsset asset, string data, UnityAction<TotemLegacyRecord> onSuccess = null)
    {
        Assert.IsTrue(asset != null, "Asset object is null");

        TotemLegacyRecord legacy = new TotemLegacyRecord(LegacyRecordTypeEnum.Achievement, asset.Id, _gameId, data);
        _legacyService.AddAchievement(legacy, () =>
        {
            Debug.Log($"Legacy record for {asset.Id} created");
            onSuccess?.Invoke(legacy);
        });
    }


    /// <summary>
    /// Creates a GameObjet for handling coroutines in services scripts
    /// </summary>
    private void CreateServicesGameObject()
    {
        _servicesGameObject = new GameObject("TotemServices");
        _simpleAPI = _servicesGameObject.AddComponent<TotemSimpleAPI>();
        _legacyService = _servicesGameObject.AddComponent<TotemLegacyService>();
        _accountGateway = _servicesGameObject.AddComponent<TotemAccountGateway>();
        MonoBehaviour.DontDestroyOnLoad(_servicesGameObject);
    }
}
