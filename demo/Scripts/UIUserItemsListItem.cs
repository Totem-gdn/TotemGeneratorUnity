using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TotemEntities;
using TMPro;

namespace TotemDemo
{
    public class UIUserItemsListItem : MonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI text;

        private TotemSpear spear;

        public void Setup(TotemSpear spear)
        {
            text.SetText(spear.ToString());
            this.spear = spear;
        }

        public void OnSelectButtonClick()
        {
            UIItemLegaciesList.Instance.BuildList(spear);
        }
    }
}
