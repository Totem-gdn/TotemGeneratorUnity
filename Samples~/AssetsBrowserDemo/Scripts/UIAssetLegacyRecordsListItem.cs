using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TotemEntities;
using TMPro;

namespace TotemDemo
{
    public class UIAssetLegacyRecordsListItem : MonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI legacyInfoText;

        public void Setup(TotemLegacyRecord legacy)
        {
            legacyInfoText.SetText($"Type:{legacy.legacyRecordType.ToString()}, ItemId:{legacy.itemId}, GameId:{legacy.gameId}, Data:{legacy.data}");
        }
    }
}
