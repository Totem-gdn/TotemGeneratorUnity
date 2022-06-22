using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TotemEntities;


namespace TotemDemo
{
    public class UIAssetLegacyRecordsList : MonoBehaviour
    {
        public static UIAssetLegacyRecordsList Instance;

        [SerializeField] private Transform itemsParent;
        [SerializeField] private GameObject itemPrefab;
        [SerializeField] private GameObject empyListPrefab;
        [SerializeField] private TotemDemoManager totemDemo;

        private ITotemAsset selectedAsset;
        private List<TotemLegacyRecord> assetLegacyRecords;

        private void Awake()
        {
            Instance = this;
        }

        public void BuildList(ITotemAsset asset, List<TotemLegacyRecord> records)
        {
            assetLegacyRecords = records;
            selectedAsset = asset;
            CreateListItems(assetLegacyRecords);
        }

        private void CreateListItems(List<TotemLegacyRecord> records)
        {
            foreach (Transform item in itemsParent)
            {
                Destroy(item.gameObject);
            }

            if (records.Count == 0)
            {
                GameObject item = Instantiate(empyListPrefab);
                item.transform.parent = itemsParent;
            }

            foreach (var record in records)
            {
                AddRecordToList(record);
            }
        }

        public void AddRecordToList(TotemLegacyRecord record)
        {
            GameObject item = Instantiate(itemPrefab);
            item.transform.parent = itemsParent;
            item.GetComponent<UIAssetLegacyRecordsListItem>().Setup(record);
        }

        public void OnAddLegacyButtonClick()
        {
            totemDemo.AddLegacyRecord(selectedAsset, IterateLastLegacyRecordData());
        }

        private string IterateLastLegacyRecordData()
        {
            if (assetLegacyRecords.Count == 0)
            {
                return "1";
            }

            var lastRecord = assetLegacyRecords[assetLegacyRecords.Count - 1];
            int parsedData;
            if (int.TryParse(lastRecord.data, out parsedData))
            {
                return (++parsedData).ToString();
            }
            return lastRecord.data + "1";
        }
    }
}
