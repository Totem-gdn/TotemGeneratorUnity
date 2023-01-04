using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using TotemEnums;

namespace TotemEntities
{
    [Serializable]
    public class TotemLegacyRecord
    {
        public LegacyRecordTypeEnum legacyRecordType;
        public string assetId;
        public string gameId;
        public string timestamp;
        public string data;

        public TotemLegacyRecord(LegacyRecordTypeEnum legacyType, string itemId, string gameId, string data, string timestamp = "")
        {
            this.legacyRecordType = legacyType;
            this.assetId = itemId;
            this.gameId = gameId;
            this.data = data;
            this.timestamp = timestamp;
        }
    }
}
