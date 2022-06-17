using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TotemEntities;


namespace TotemDemo
{
    public class UIItemLegaciesList : MonoBehaviour
    {
        public static UIItemLegaciesList Instance;

        [SerializeField] private Transform itemsParent;
        [SerializeField] private GameObject itemPrefab;
        [SerializeField] private GameObject empyListPrefab;
        [SerializeField] private TotemDemoManager totemDemo;

        private TotemSpear selectedSpear;

        private int legacyDataIterrator = 0;

        private void Awake()
        {
            Instance = this;
        }

        public void BuildList(TotemSpear spear)
        {
            foreach (Transform item in itemsParent)
            {
                Destroy(item.gameObject);
            }

            var legacies = spear.GetLegacyRecords();

            if (legacies.Count == 0)
            {
                GameObject item = Instantiate(empyListPrefab);
                item.transform.parent = itemsParent;
            }

            foreach (var legacy in legacies)
            {
                GameObject item = Instantiate(itemPrefab);
                item.transform.parent = itemsParent;
                item.GetComponent<UIItemLegaciesListItem>().Setup(legacy);
            }

            selectedSpear = spear;

        }

        public void RebuildList()
        {
            BuildList(selectedSpear);
        }

        public void OnAddLegacyButtonClick()
        {
            totemDemo.AddLegacyRecord(selectedSpear, ++legacyDataIterrator);
        }
    }
}
