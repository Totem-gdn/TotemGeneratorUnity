using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TotemEntities;


namespace TotemDemo
{
    public class UIItemLegacyRecordsList : MonoBehaviour
    {
        public static UIItemLegacyRecordsList Instance;

        [SerializeField] private Transform itemsParent;
        [SerializeField] private GameObject itemPrefab;
        [SerializeField] private GameObject empyListPrefab;
        [SerializeField] private TotemDemoManager totemDemo;

        private TotemSpear selectedSpear;
        private TotemAvatar selectedAvatar;

        private void Awake()
        {
            Instance = this;
        }

        public void BuildList(TotemSpear spear)
        {
            CreateListItems(spear.GetLegacyRecords());

            selectedSpear = spear;
            selectedAvatar = null;

        }

        public void BuildList(TotemAvatar avatar)
        {
            CreateListItems(avatar.GetLegacyRecords());

            selectedAvatar = avatar;
            selectedSpear = null;

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
                GameObject item = Instantiate(itemPrefab);
                item.transform.parent = itemsParent;
                item.GetComponent<UIItemLegaciesListItem>().Setup(record);
            }
        }

        public void RebuildList()
        {
            if (selectedSpear == null)
            {
                BuildList(selectedAvatar);
            }
            else
            {
                BuildList(selectedSpear);
            }
        }

        public void OnAddLegacyButtonClick()
        {
            if (selectedSpear == null)
            {
                totemDemo.AddLegacyRecord(selectedAvatar, IterateLastLegacyRecordData(selectedAvatar.GetLegacyRecords()));

            }
            else
            {
                totemDemo.AddLegacyRecord(selectedSpear, IterateLastLegacyRecordData(selectedSpear.GetLegacyRecords()));
            }

        }

        private string IterateLastLegacyRecordData(List<TotemLegacyRecord> records)
        {
            if (records.Count == 0)
            {
                return "1";
            }

            var lastRecord = records[records.Count - 1];
            int parsedData;
            if (int.TryParse(lastRecord.data, out parsedData))
            {
                return (++parsedData).ToString();
            }
            return lastRecord.data + "1";
        }
    }
}
