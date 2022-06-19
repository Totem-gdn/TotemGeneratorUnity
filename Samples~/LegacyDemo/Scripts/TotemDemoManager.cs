using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using TotemEntities;
using TMPro;

namespace TotemDemo
{
    public class TotemDemoManager : MonoBehaviour
    {
        public static TotemDemoManager Instance;

        [SerializeField] private UIItemsList itemList;
        [SerializeField] private UIItemLegacyRecordsList legacyItemsList;
        [SerializeField] private UIItemsListTypeManager listTypeManager;
        [SerializeField] private GameObject loginButton;
        [SerializeField] private GameObject addLegacyRecordButon;
        [SerializeField] private TextMeshProUGUI profileNameText;
        [SerializeField] private TMP_InputField legacyGameIdInput;

        private bool _avatarsTabSelected;
        private bool _userLoggedIn;

        private TotemDB totemDB;


        private string _gameId = "TotemDemo";

        private void Awake()
        {
            if (Instance == null)
            {
                Instance = this;
            }
            else if (Instance != this)
            {
                Destroy(gameObject);
            }
        }

        void Start()
        {
            totemDB = new TotemDB(_gameId);
        }


        public void AddLegacyRecord(TotemSpear spear, string data)
        {
            UILoadingScreen.Instance.Show();
            totemDB.AddLegacyRecord(spear, data, () =>
            {
                legacyItemsList.RebuildList();
                UILoadingScreen.Instance.Hide();
            });
        }

        public void AddLegacyRecord(TotemAvatar avatar, string data)
        {
            UILoadingScreen.Instance.Show();
            totemDB.AddLegacyRecord(avatar, data, () =>
            {
                legacyItemsList.RebuildList();
                UILoadingScreen.Instance.Hide();
            });
        }

        public void GetLegacyRecords(TotemSpear spear, UnityAction onSuccess)
        {
            totemDB.GetLegacyRecords(spear, onSuccess, legacyGameIdInput.text);
        }

        public void GetLegacyRecords(TotemAvatar avatar, UnityAction onSuccess)
        {
            totemDB.GetLegacyRecords(avatar, onSuccess, legacyGameIdInput.text);
        }



        public void SwitchItemListTab(bool avatarTab)
        {
            _avatarsTabSelected = avatarTab;
            if (_userLoggedIn)
            {
                BuildItemList(totemDB.CurrentUser);
            }
        }

        public void OnLoginButtonClick()
        {
            UILoadingScreen.Instance.Show();
            totemDB.AuthenticateCurentUser((OnTotemUserLoggedIn));
        }


        private void OnTotemUserLoggedIn(TotemUser user)
        {
            BuildItemList(user);

            loginButton.SetActive(false);
            profileNameText.gameObject.SetActive(true);
            profileNameText.SetText("User: " + user.GetUserName());

            addLegacyRecordButon.SetActive(true);

            _userLoggedIn = true;

            UILoadingScreen.Instance.Hide();

        }

        private void BuildItemList(TotemUser user)
        {
            if (_avatarsTabSelected)
            {
                List<TotemAvatar> avatars = user.GetOwnedAvatars();
                itemList.BuildList(avatars);
            }
            else
            {
                List<TotemSpear> spears = user.GetOwnedSpears();
                itemList.BuildList(spears);
            }


        }

    }
}
