using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TotemEntities.DNA;

namespace TotemDemo
{
    public class UIAssetsList : MonoBehaviour
    {
        [SerializeField] private Transform itemsParent;
        [SerializeField] private GameObject itemPrefab;

        public void BuildList(List<TotemDNADefaultAvatar> assets)
        {
            foreach (Transform item in itemsParent)
            {
                Destroy(item.gameObject);
            }

            foreach (var asset in assets)
            {
                GameObject item = Instantiate(itemPrefab);
                item.transform.SetParent(itemsParent);
                item.GetComponent<UIAssetsListItem>().Setup(asset);
            }
        }
        public void ClearList()
        {
            foreach (Transform item in itemsParent)
            {
                Destroy(item.gameObject);
            }
        }
    }
}
