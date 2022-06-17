using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TotemEntities;

namespace TotemDemo
{
    public class UIUserItemsList : MonoBehaviour
    {
        [SerializeField] private Transform itemsParent;
        [SerializeField] private GameObject itemPrefab;

        public void BuildList(List<TotemSpear> spears)
        {
            foreach (Transform item in itemsParent)
            {
                Destroy(item.gameObject);
            }

            foreach (var spear in spears)
            {
                GameObject item = Instantiate(itemPrefab);
                item.transform.parent = itemsParent;
                item.GetComponent<UIUserItemsListItem>().Setup(spear);
            }
        }
    }
}
