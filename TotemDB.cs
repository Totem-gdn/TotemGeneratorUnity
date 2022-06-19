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
    public TotemEntitiesDB EntitiesDB = new TotemEntitiesDB();
    public TotemUsersDB UsersDB = new TotemUsersDB();

    public TotemUser CurrentUser { get; private set; }

    private TotemSimpleAPI _simpleAPI;
    private TotemLegacyService _legacyService;
    private TotemAccountGateway _accountGateway;
    private GameObject _servicesGameObject;

    private string _publicKey;
    private string _gameId;

    private bool _avatarsLoaded;
    private bool _itemsLoaded;
    private UnityAction<TotemUser> _loginCallback;
    

    /// <summary>
    /// Initialize DB and services
    /// </summary>
    /// <param name="gameId">Id of your game. Used for legacy records identification</param>
    public TotemDB(string gameId)
    {
        _gameId = gameId;
        CreateServicesGameObject();
    }

    /// <summary>
    /// Opens social login web-page
    /// After successful login stores publicKey and retrievs all owned items and avatars
    /// NOTE: Legacy records have to be retrieved manualy
    /// </summary>
    /// <param name="onSuccess"></param>
    public void AuthenticateCurentUser(UnityAction<TotemUser> onSuccess)
    {
        _loginCallback = onSuccess;

        _accountGateway.LoginGoogle((publicKey, userProfile) =>
        {
            CurrentUser = new TotemUser(userProfile.username, "", publicKey);
            UsersDB.AddNewUser(CurrentUser);
            _publicKey = publicKey;
            GetCurrentUserItems();
            GetCurrentUserAvatars();
        });

    }

    /// <summary>
    /// Adds a legacy record to provided spear
    /// </summary>
    /// <param name="spear"></param>
    /// <param name="data">An UTF-8 encoded string</param>
    /// <param name="onSuccess"></param>
    public void AddLegacyRecord(TotemSpear spear, string data, UnityAction onSuccess = null)
    {
        Assert.IsTrue(spear != null, "Spear object is null");

        TotemLegacyRecord legacy = new TotemLegacyRecord(LegacyRecordTypeEnum.Achievement, spear.id, _gameId, data);
        _legacyService.AddAchievement(legacy, () =>
        {
            Debug.Log($"Legacy record for {spear.id} created");
            spear.AddLegacyRecord(legacy);
            onSuccess?.Invoke();
        });
    }

    /// <summary>
    /// Adds a legacy record to provided avatar
    /// </summary>
    /// <param name="spear"></param>
    /// <param name="data">An UTF-8 encoded string</param>
    /// <param name="onSuccess"></param>
    public void AddLegacyRecord(TotemAvatar avatar, string data, UnityAction onSuccess = null)
    {
        Assert.IsTrue(avatar != null, "Avatar object is null");

        TotemLegacyRecord legacy = new TotemLegacyRecord(LegacyRecordTypeEnum.Achievement, avatar.id, _gameId, data);
        _legacyService.AddAchievement(legacy, () =>
        {
            Debug.Log($"Legacy record for {avatar.id} created");
            avatar.AddLegacyRecord(legacy);
            onSuccess?.Invoke();
        });

    }

    /// <summary>
    /// Retrieves legacy records for the provided spear
    /// </summary>
    /// <param name="spear">Will have populated legacy records list</param>
    /// <param name="onSuccess">Callback holds spear with populated list of legacy records</param>
    /// <param name="gameId">From which game to retrieve legacy records. Can be left emtpy to retrieve from all</param>
    public void GetLegacyRecords(TotemSpear spear, UnityAction onSuccess, string gameId = "")
    {
        _legacyService.GetAchivements(spear.id, gameId, (records) =>
        {
            spear.ClearLegacyRecords();
            foreach (var record in records)
            {
                spear.AddLegacyRecord(record);
            }

            onSuccess.Invoke();
        });
    }

    /// <summary>
    /// Retrieves legacy records for the provided avatar
    /// </summary>
    /// <param name="avatar">Will have populated legacy records list</param>
    /// <param name="onSuccess"></param>
    /// <param name="gameId">From which game to retrieve legacy records. Can be left emtpy to retrieve from all</param>
    public void GetLegacyRecords(TotemAvatar avatar, UnityAction onSuccess, string gameId = "")
    {
        _legacyService.GetAchivements(avatar.id, gameId, (records) =>
        {
            avatar.ClearLegacyRecords();
            foreach (var record in records)
            {
                avatar.AddLegacyRecord(record);
            }

            onSuccess.Invoke();
        });
    }


    private void GetCurrentUserItems()
    {
        _itemsLoaded = false;
        _simpleAPI.GetItems(_publicKey, (spears) =>
        {
            _itemsLoaded = true;
            foreach (var spear in spears)
            {
                CurrentUser.AddSpear(spear);
            }

            _itemsLoaded = true;
            if (_avatarsLoaded)
            {
                _loginCallback.Invoke(CurrentUser);
            }

        });
    }


    private void GetCurrentUserAvatars()
    {
        _avatarsLoaded = false;
        _simpleAPI.GetAvatas(_publicKey, (avatars) =>
        {
            foreach (var avatar in avatars)
            {
                CurrentUser.AddAvatar(avatar);
            }

            _avatarsLoaded = true;
            if (_itemsLoaded)
            {
                _loginCallback.Invoke(CurrentUser);
            }
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
