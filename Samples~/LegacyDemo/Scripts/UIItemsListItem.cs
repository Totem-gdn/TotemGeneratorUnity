using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TotemEntities;
using TMPro;

namespace TotemDemo
{
    public class UIItemsListItem : MonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI text;

        private TotemSpear spear;
        private TotemAvatar avatar;

        public void Setup(TotemSpear spear)
        {
            text.SetText(spear.ToString());
            this.spear = spear;
            avatar = null;
        }

        public void Setup(TotemAvatar avatar)
        {
            text.SetText(avatar.ToString());
            this.avatar = avatar;
            spear = null;
        }

        public void OnShowRecordsButtonClick()
        {
            UILoadingScreen.Instance.Show();
            if (spear == null)
            {
                TotemDemoManager.Instance.GetLegacyRecords(avatar, () =>
                {
                    UIItemLegacyRecordsList.Instance.BuildList(avatar);
                    UILoadingScreen.Instance.Hide();
                });
            }
            else
            {
                TotemDemoManager.Instance.GetLegacyRecords(spear, () =>
                {
                    UIItemLegacyRecordsList.Instance.BuildList(spear);
                    UILoadingScreen.Instance.Hide();
                });
            }
        }
    }
}
