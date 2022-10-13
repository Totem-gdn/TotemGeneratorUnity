using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TotemEntities;
using TotemEntities.DNA;
using TMPro;
using System.Linq;


namespace TotemDemo
{
    public class UIAssetLegacyRecordsList : MonoBehaviour
    {
        public static UIAssetLegacyRecordsList Instance;

        [SerializeField] private Transform itemsParent;
        [SerializeField] private GameObject itemPrefab;
        [SerializeField] private GameObject empyListPrefab;
        [SerializeField] private TotemDemoManager totemDemo;
        [SerializeField] private TMP_InputField inputLegacyNumber;

        private TotemDNADefaultAvatar selectedAsset;
        private List<TotemLegacyRecord> assetLegacyRecords;

        private void Awake()
        {
            Instance = this;
        }

        public void BuildList(TotemDNADefaultAvatar asset, List<TotemLegacyRecord> records)
        {
            assetLegacyRecords = records;
            selectedAsset = asset;
            CreateListItems(assetLegacyRecords);
        }

        public void ClearList()
        {
            foreach (Transform item in itemsParent)
            {
                Destroy(item.gameObject);
            }
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

        public void AddRecordToList(TotemLegacyRecord record, bool addToCollection = false)
        {
            GameObject item = Instantiate(itemPrefab);
            item.transform.SetParent(itemsParent);
            item.GetComponent<UIAssetLegacyRecordsListItem>().Setup(record);
            item.transform.SetSiblingIndex(0);
            if (addToCollection)
            {
                assetLegacyRecords.Add(record);
            }
        }

        public void OnIncrementLegacyButtonClick()
        {
            int nextValue = -1;
            nextValue = IncrementLastLegacyRecordData();
            if (nextValue != -1)
            {
                totemDemo.AddLegacyRecord(selectedAsset, nextValue);
            }
        }

        private int IncrementLastLegacyRecordData()
        {
            if (assetLegacyRecords.Count == 0)
            {
                return 1;
            }

            var lastRecord = assetLegacyRecords.FindLast((x) => x.gameId.Equals(totemDemo._gameId));

            if (lastRecord == null)
            {
                return 1;
            }

            int parsedData;
            if (int.TryParse(lastRecord.data, out parsedData))
            {
                return (++parsedData);
            }
            return -1;
        }
        public void OnSetLegacyButtonClick()
        {
            int customInt = 0;
            if (int.TryParse(inputLegacyNumber.text, out customInt))
            {
                totemDemo.AddLegacyRecord(selectedAsset, customInt);
            }
        }
        public void OnZeroLegacyButtonClick()
        {
            totemDemo.AddLegacyRecord(selectedAsset, 0);
        }
    }
}
