using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace TotemDemo {
    public class UIItemsListTypeManager : MonoBehaviour
    {
        [SerializeField] private Button itemsTabButton;
        [SerializeField] private Button avatarsTabButton;


        public void OnItemsTabButtonClick()
        {
            itemsTabButton.interactable = false;
            avatarsTabButton.interactable = true;

            TotemDemoManager.Instance.SwitchItemListTab(false);
        }

        public void OnAvatarsTabButtonClick()
        {
            itemsTabButton.interactable = true;
            avatarsTabButton.interactable = false;

            TotemDemoManager.Instance.SwitchItemListTab(true);
        }

    }
}
