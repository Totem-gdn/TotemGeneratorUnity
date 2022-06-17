using System;
using System.Collections.Generic;
using System.Linq;
using enums;
using JetBrains.Annotations;
using UnityEngine;

namespace TotemEntities
{
    [Serializable]
    public class TotemSpear
    {
        private List<TotemUser> _owners;
        private List<TotemLegacyRecord> _legacyRecords;

        public string id;
        public string shaftColor;

        public TipMaterialEnum tipMaterial;
        public ElementEnum element;
        public Color shaftColorRGB;
        public float range;
        public float damage;

        public TotemSpear()
        {
            _owners = new List<TotemUser>();
            _legacyRecords = new List<TotemLegacyRecord>();
        }

        public TotemSpear(TipMaterialEnum aTip, ElementEnum aElement, Color aShaftColor, float aRange, float aDamage)
        {
            _owners = new List<TotemUser>();
            _legacyRecords = new List<TotemLegacyRecord>();
            tipMaterial = aTip;
            element = aElement;
            shaftColorRGB = aShaftColor;
            range = aRange;
            damage = aDamage;
        }

        public List<TotemUser> GetOwners()
        {
            return _owners;
        }
    
        [CanBeNull]
        public TotemUser GetCurrentOwner()
        {
            return _owners.Count == 0 ? null : _owners.Last();
        }

        public void SetOwner(TotemUser owner)
        {
            _owners.Add(owner);
        }

        public void AddLegacyRecord(TotemLegacyRecord legacy)
        {
            _legacyRecords.Add(legacy);
        }

        public List<TotemLegacyRecord> GetLegacyRecords()
        {
            return _legacyRecords;
        }

        public override string ToString() {
            return $"Id:{id},Tip:{tipMaterial},Element:{element},ShaftColor:{shaftColorRGB},Range:{range},Damage:{damage}";
        }
    }
}