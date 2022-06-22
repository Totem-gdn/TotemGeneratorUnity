using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Events;
using TotemEntities;
using TotemServices;
using TMPro;

namespace TotemDemo
{
    public class TotemExampleManager : MonoBehaviour
    {
        public static TotemExampleManager Instance;

        [SerializeField] private UIAssetsList itemList;
        [SerializeField] private UIAssetLegacyRecordsList legacyItemsList;
        [SerializeField] private GameObject loginButton;
        [SerializeField] private GameObject addLegacyRecordButon;
        [SerializeField] private TextMeshProUGUI profileNameText;
        [SerializeField] private TMP_InputField legacyGameIdInput;

        private bool _avatarsTabSelected;
        private bool _userLoggedIn;

        private bool _avatarsLoaded;

        private TotemDB totemDB;

        private string _accessToken;
        private string _publicKey;
        private List<TotemAvatar> _userAvatars;

        /// <summary>
        /// Id of your game
        /// Used for legacy records identification
        /// </summary>
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

        /// <summary>
        /// Initializing TotemDB and subscribing to events
        /// </summary>
        void Start()
        {
            totemDB = new TotemDB(_gameId);

            totemDB.OnSocialLoginCompleted.AddListener(OnTotemUserLoggedIn);
            totemDB.OnUserProfileLoaded.AddListener(OnUserProfileLoaded);
            totemDB.OnAvatarsLoaded.AddListener(OnAvatarsLoaded);

            ///If we saved the accessToken from the previous login
            ///We can retrive it here and skip the social login step
            //if (PlayerPrefs.HasKey("accessToken"))
            //{
            //    UILoadingScreen.Instance.Show();
            //    _accessToken = PlayerPrefs.GetString("accessToken", "");
            //    totemDB.GetUserProfile(_accessToken);
            //}
        }


        public void OnLoginButtonClick()
        {
            UILoadingScreen.Instance.Show();
            totemDB.AuthenticateCurentUser();
        }


        private void OnTotemUserLoggedIn(TotemAccountGateway.SocialLoginResponse loginResult)
        {
            profileNameText.gameObject.SetActive(true);
            profileNameText.SetText("User: " + loginResult.profile.username);

            _accessToken = loginResult.accessToken;

            ///At this point we can save accessToken to the PlayerPrefs
            ///and retrive it on the subsequent game starts to elimate the need for user to login each time
            //PlayerPrefs.SetString("accessToken", loginResult.accessToken);

            totemDB.GetUserProfile(_accessToken);
        }

        private void OnUserProfileLoaded(string publicKey)
        {
            _publicKey = publicKey;

            _userLoggedIn = true;
            _avatarsLoaded = false;

            totemDB.GetUserAvatars(_publicKey);
        }

        private void OnAvatarsLoaded(List<TotemAvatar> avatars)
        {
            _userAvatars = avatars;
            _avatarsLoaded = true;
            BuildItemList();
            addLegacyRecordButon.SetActive(true);
            UILoadingScreen.Instance.Hide();
        }

        public void AddLegacyRecord(ITotemAsset asset, string data)
        {
            UILoadingScreen.Instance.Show();
            totemDB.AddLegacyRecord(asset, data, (record) =>
            {
                legacyItemsList.AddRecordToList(record);
                UILoadingScreen.Instance.Hide();
            });
        }

        public void GetLegacyRecords(ITotemAsset asset, UnityAction<List<TotemLegacyRecord>> onSuccess)
        {
            totemDB.GetLegacyRecords(asset, onSuccess, legacyGameIdInput.text);
        }



        public void SwitchItemListTab(bool avatarTab)
        {
            _avatarsTabSelected = avatarTab;
            if (_avatarsLoaded)
            {
                BuildItemList();
            }
        }

        private void BuildItemList()
        {
            itemList.BuildList(_userAvatars.Cast<ITotemAsset>().ToList());
        }
    }
}