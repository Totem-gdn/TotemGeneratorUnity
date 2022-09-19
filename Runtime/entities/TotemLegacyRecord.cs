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
        public string itemId;
        public string gameId;
        public string data;

        public TotemLegacyRecord(LegacyRecordTypeEnum legacyType, string itemId, string gameId, string data)
        {
            this.legacyRecordType = legacyType;
            this.itemId = itemId;
            this.gameId = gameId;
            this.data = data;
        }
    }
}
