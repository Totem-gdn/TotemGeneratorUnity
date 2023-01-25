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
        public string gameAddress;
        public string timestamp;
        public string data;

        public TotemLegacyRecord(LegacyRecordTypeEnum legacyType, string itemId, string gameAddress, string data, string timestamp = "")
        {
            this.legacyRecordType = legacyType;
            this.assetId = itemId;
            this.gameAddress = gameAddress;
            this.data = data;
            this.timestamp = timestamp;
        }
    }
}
