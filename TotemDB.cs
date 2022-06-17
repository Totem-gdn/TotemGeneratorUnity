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

    private int _legaciesToLoad;
    private bool _avatarsLoaded;
    private UnityAction<TotemUser> _loginCallback;
    

    /// <summary>
    /// Initialize DB and services
    /// </summary>
    public TotemDB()
    {
        CreateServicesGameObject();
    }

    /// <summary>
    /// Opens social login web-page.
    /// After successful login stores publicKey and retrievs all owned items/avatars.
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
            GetCurrentUserItems(_publicKey);
            GetCurrentUserAvatars();
        });

    }

    public void AddAchievementToSpear(TotemSpear spear, int data, UnityAction onSuccess = null)
    {
        Assert.IsTrue(spear != null, "Spear object is null");

        //TODO: A proper way to get gameID
        string gameId = string.IsNullOrEmpty(Application.identifier) ? "gameID-000000" : Application.identifier;

        var dataUTF = data.ToString();
        TotemLegacyRecord legacy = new TotemLegacyRecord(LegacyRecordTypeEnum.Achievement, spear.id, gameId, dataUTF);
        _legacyService.AddAchievement(legacy, () =>
        {
            Debug.Log($"Legacy record for {spear.id} created");
            spear.AddLegacyRecord(legacy);
            onSuccess?.Invoke();
        });
    }


    private void GetCurrentUserItems(string publicKey)
    {
        _legaciesToLoad = 1;
        _simpleAPI.GetItems(publicKey, (spears) =>
        {
            _legaciesToLoad = spears.Count;
            foreach (var spear in spears)
            {
                CurrentUser.AddSpear(spear);
                GetItemLegacyRecords(spear);
            }

        });
    }

    private void GetItemLegacyRecords(TotemSpear item)
    {
        _legacyService.GetAchivements(item.id, (legacies) =>
        {
            foreach (var legacy in legacies)
            {
                item.AddLegacyRecord(legacy);
            }

            if (_avatarsLoaded && --_legaciesToLoad == 0)
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
            if (_legaciesToLoad == 0)
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
