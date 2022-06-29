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

    private TotemSimpleAPI _simpleAPI;
    private TotemLegacyService _legacyService;
    private TotemAccountGateway _accountGateway;
    private TotemAnalytics _analytics;
    private GameObject _servicesGameObject;

    private string _gameId;
    private string _userPublicKey;
    private string _userEmail;

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

            _userEmail = loginResult.profile.username;
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
            _userPublicKey = publicKey;
            OnUserProfileLoaded.Invoke(publicKey);

            _analytics.RecordAction(TotemServicesAction.user_login, _gameId, publicKey, _userEmail);
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

            _analytics.RecordAction(TotemServicesAction.items_requested, _gameId, publicKey, _userEmail);
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

            _analytics.RecordAction(TotemServicesAction.avatars_requested, _gameId, publicKey, _userEmail);
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

            _analytics.RecordAction(TotemServicesAction.legacy_requested, _gameId, _userPublicKey, _userEmail);
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

            _analytics.RecordAction(TotemServicesAction.legacy_requested, _gameId, _userPublicKey, _userEmail);
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
        _analytics = _servicesGameObject.AddComponent<TotemAnalytics>();
        MonoBehaviour.DontDestroyOnLoad(_servicesGameObject);
    }
}
