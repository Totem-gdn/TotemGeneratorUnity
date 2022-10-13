using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TotemEntities.DNA;
using TMPro;

namespace TotemDemo
{
    public class UIAssetsListItem : MonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI text;

        private TotemDNADefaultAvatar asset;

        public void Setup(TotemDNADefaultAvatar asset)
        {
            text.SetText(asset.ToString());
            this.asset = asset;
        }

        public void OnShowRecordsButtonClick()
        {
            UILoadingScreen.Instance.Show();
            TotemDemoManager.Instance.GetLegacyRecords(asset, (records) =>
            {
                UIAssetLegacyRecordsList.Instance.BuildList(asset, records);
                UILoadingScreen.Instance.Hide();
            });
        }
    }
}
