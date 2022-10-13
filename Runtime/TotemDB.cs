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

public class TotemDB
{
    /// <summary>
    /// Invoked after successful login
    /// Holds user's profile information and a publicKey for item retrieval 
    /// </summary>
    public UnityEvent<TotemUser> OnUserProfileLoaded;

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

    /// <summary>
    /// Currently logged in user info
    /// </summary>
    public TotemUser CurrentUser { get; private set; }


    private TotemSimpleAPI _simpleAPI;
    private TotemLegacyService _legacyService;
    private TotemWeb3Auth _web3Auth;
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
    public TotemDB(string gameId)
    {
        _gameId = gameId;
        CreateServicesGameObject();

        OnUserProfileLoaded = new UnityEvent<TotemUser>();
        OnSpearsLoaded = new UnityEvent<List<TotemSpear>>();
        OnAvatarsLoaded = new UnityEvent<List<TotemAvatar>>();
        OnSwordsLoaded = new UnityEvent<List<TotemSword>>();
    }

    /// <summary>
    /// Opens social login web-page
    /// Invokes OnUserProfileLoaded event on completion
    /// </summary>
    /// <param name="provider">Which provider will be used for login (e.g. Google, Facebook, Discord)</param>
    public void AuthenticateCurrentUser(Provider provider = Provider.GOOGLE)
    {
        _web3Auth.LoginUser(provider, (user) =>
        {
            CurrentUser = user;
            _userPublicKey = user.PublicKey;
            _userEmail = user.Email;

            OnUserProfileLoaded.Invoke(user);
        });
    }

    /// <summary>
    /// Opens social login web-page
    /// Loads all user assets after successful login
    /// Invokes OnUserProfileLoaded event on completion
    /// </summary>
    /// <param name="provider">Which provider will be used for login (e.g. Google, Facebook, Discord)</param>
    /// <param name="onLogin">If set, will be invoked instead of OnUserProfileLoaded event</param>
    public void AuthenticateUserWithAssets(Provider provider = Provider.GOOGLE, UnityAction<TotemUser> onLogin = null)
    {
        _web3Auth.LoginUser(provider, (user) =>
        {
            _userPublicKey = user.PublicKey;
            _userEmail = user.Email;


           //_smartContract.GetAvatars<TotemDNAAvatar>(user, new TotemDNAFilter(Resources.Load<TextAsset>("avatar-filter").text));

            bool avatarsLoaded = false;
            bool spearsLoaded = false;

            _simpleAPI.GetItems(user.PublicKey, (spears) =>
            {
                user.AddSpears(spears);

                spearsLoaded = true;
                if (avatarsLoaded)
                {
                    if (onLogin != null)
                    {
                        onLogin.Invoke(user);
                    }
                    else
                    {
                        OnUserProfileLoaded.Invoke(user);
                    }
                }

                _analytics.RecordAction(TotemServicesAction.items_requested, _gameId, _userPublicKey, _userEmail);
            });

            _simpleAPI.GetAvatas(user.PublicKey, (avatars) =>
            {
                user.AddAvatars(avatars);

                avatarsLoaded = true;
                if (spearsLoaded)
                {
                    if (onLogin != null)
                    {
                        onLogin.Invoke(user);
                    }
                    else
                    {
                        OnUserProfileLoaded.Invoke(user);
                    }
                }

                _analytics.RecordAction(TotemServicesAction.avatars_requested, _gameId, user.PublicKey, _userEmail);
            });


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
        _analytics = _servicesGameObject.AddComponent<TotemAnalytics>();
        _web3Auth = _servicesGameObject.AddComponent<TotemWeb3Auth>();
        _smartContract = _servicesGameObject.AddComponent<TotemSmartContractManager>();


        MonoBehaviour.DontDestroyOnLoad(_servicesGameObject);
    }



}
