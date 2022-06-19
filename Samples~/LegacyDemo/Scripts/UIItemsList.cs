using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TotemEntities;

namespace TotemDemo
{
    public class UIItemsList : MonoBehaviour
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
                item.GetComponent<UIItemsListItem>().Setup(spear);
            }
        }

        public void BuildList(List<TotemAvatar> avatars)
        {
            foreach (Transform item in itemsParent)
            {
                Destroy(item.gameObject);
            }

            foreach (var avatar in avatars)
            {
                GameObject item = Instantiate(itemPrefab);
                item.transform.parent = itemsParent;
                item.GetComponent<UIItemsListItem>().Setup(avatar);
            }
        }
    }
}
