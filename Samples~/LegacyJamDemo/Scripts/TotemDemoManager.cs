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
    public class TotemDemoManager : MonoBehaviour
    {
        public static TotemDemoManager Instance;
        private TotemDB totemDB;

        [Header("Demo")]
        public string _gameId = "TotemDemo"; // Id of your game, used for legacy records identification

        [SerializeField] private GameObject loginButton;

        [Header("Login UI")]
        [SerializeField] private GameObject googleLoginObject;
        [SerializeField] private GameObject profileNameObject;
        [SerializeField] private TextMeshProUGUI profileNameText;

        [Header("Legacy UI")]
        [SerializeField] private TMP_InputField legacyGameIdInput;
        [SerializeField] private TMP_InputField dataToCompoareInput;
        [SerializeField] private UIAssetsList assetList;
        [SerializeField] private UIAssetLegacyRecordsList legacyRecordsList;
        [SerializeField] private Animator popupAnimator;

        //Meta Data
        private string _accessToken;
        private string _publicKey;
        private List<TotemAvatar> _userAvatars;

        //Default Avatar reference - use for your game
        private TotemAvatar firstAvatar;

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
            legacyGameIdInput.onEndEdit.AddListener(OnGameIdInputEndEdit);
        }

        #region USER AUTHENTICATION
        public void OnLoginButtonClick()
        {
            UILoadingScreen.Instance.Show();
            totemDB.AuthenticateCurrentUser();
        }

        private void OnTotemUserLoggedIn(TotemAccountGateway.SocialLoginResponse loginResult)
        {
            googleLoginObject.SetActive(false);
            profileNameObject.SetActive(true);
            profileNameText.SetText(loginResult.profile.username);

            _accessToken = loginResult.accessToken;
            totemDB.GetUserProfile(_accessToken);

            assetList.ClearList();
            legacyRecordsList.ClearList();
        }

        private void OnUserProfileLoaded(string publicKey)
        {
            _publicKey = publicKey;
            totemDB.GetUserAvatars(_publicKey);
        }

        private void OnAvatarsLoaded(List<TotemAvatar> avatars)
        {
            _userAvatars = avatars;

            //Reference the first Avatar in the list
            firstAvatar = avatars[0];

            //UI Example Methods
            BuildAvatarList();
            ShowAvatarRecords();
        }
        public void ShowAvatarRecords()
        {
            GetLegacyRecords(firstAvatar, (records) =>
            {
                UIAssetLegacyRecordsList.Instance.BuildList(firstAvatar, records);
                UILoadingScreen.Instance.Hide();
            });
        }
        #endregion

        #region LEGACY RECORDS
        /// <summary>
        /// Add a new Legacy Record to a specific Totem Asset.
        /// </summary>
        public void AddLegacyRecord(ITotemAsset asset, int data)
        {
            UILoadingScreen.Instance.Show();
            totemDB.AddLegacyRecord(asset, data.ToString(), (record) =>
            {
                legacyRecordsList.AddRecordToList(record, true);
                UILoadingScreen.Instance.Hide();
                popupAnimator.Play("Write Legacy");
            });
        }

        /// <summary>
        /// Add a new Legacy Record to the first Totem Avatar.
        /// </summary>
        public void AddLegacyToFirstAvatar(int data)
        {
            AddLegacyRecord(firstAvatar, data);
        }

        public void GetLegacyRecords(ITotemAsset asset, UnityAction<List<TotemLegacyRecord>> onSuccess)
        {
            totemDB.GetLegacyRecords(asset, onSuccess, legacyGameIdInput.text);
        }

        public void GetLastLegacyRecord(UnityAction<TotemLegacyRecord> onSuccess)
        {
            GetLegacyRecords(firstAvatar, (records) => { onSuccess.Invoke(records[records.Count - 1]); });
        }

        public void CompareLastLegacyRecord()
        {
            GetLastLegacyRecord((record) =>
            {
                string valueToCheckText = dataToCompoareInput.text;
                if (valueToCheckText.Equals(record.data))
                {
                    popupAnimator.Play("Read Legacy");
                }
            }
            );
        }
        #endregion

        #region UI EXAMPLE METHOD

        private void BuildAvatarList()
        {
            assetList.BuildList(_userAvatars.Cast<ITotemAsset>().ToList());
        }

        private void OnGameIdInputEndEdit(string text)
        {
            ShowAvatarRecords();
        }

        #endregion
    }
}