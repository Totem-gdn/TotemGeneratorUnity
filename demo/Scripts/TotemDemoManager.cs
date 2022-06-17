using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TotemEntities;
using TMPro;

namespace TotemDemo
{
    public class TotemDemoManager : MonoBehaviour
    {
        [SerializeField] private UIUserItemsList itemList;
        [SerializeField] private UIItemLegaciesList legacyItemsList;
        [SerializeField] private GameObject loginButton;
        [SerializeField] private GameObject addLegacyRecordButon;
        [SerializeField] private TextMeshProUGUI profileNameText;

        private TotemDB totemDB;

        void Start()
        {
            totemDB = new TotemDB();
        }

        public void OnLoginButtonClick()
        {
            UILoadingScreen.Instance.Show();
            totemDB.AuthenticateCurentUser((OnTotemUserLogedIn));
        }

        private void OnTotemUserLogedIn(TotemUser user)
        {
            List<TotemSpear> spears = user.GetOwnedSpears();

            itemList.BuildList(spears);

            loginButton.SetActive(false);
            profileNameText.gameObject.SetActive(true);
            profileNameText.SetText("User: " + user.GetUserName());

            addLegacyRecordButon.SetActive(true);

            UILoadingScreen.Instance.Hide();

        }

        public void AddLegacyRecord(TotemSpear spear, int data)
        {
            UILoadingScreen.Instance.Show();
            totemDB.AddAchievementToSpear(spear, data, () =>
            {
                legacyItemsList.RebuildList();
                UILoadingScreen.Instance.Hide();
            });
        }

    }
}
