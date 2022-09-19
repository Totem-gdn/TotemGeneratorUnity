using System;
using System.Collections.Generic;
using System.Linq;
using TotemEnums;
using JetBrains.Annotations;
using UnityEngine;

namespace TotemEntities
{
    [Serializable]
    public class TotemSpear : ITotemAsset
    {
        public string Id { get; set; }
        public List<TotemUser> Owners { get; set; }

        public string shaftColor;
        public TipMaterialEnum tipMaterial;
        public ElementEnum element;
        public Color shaftColorRGB;
        public float range;
        public float damage;

        public TotemSpear()
        {
            Owners = new List<TotemUser>();
        }

        public TotemSpear(TipMaterialEnum aTip, ElementEnum aElement, Color aShaftColor, float aRange, float aDamage)
        {
            Owners = new List<TotemUser>();
            tipMaterial = aTip;
            element = aElement;
            shaftColorRGB = aShaftColor;
            range = aRange;
            damage = aDamage;
        }
    
        [CanBeNull]
        public TotemUser GetCurrentOwner()
        {
            return Owners.Count == 0 ? null : Owners.Last();
        }

        public void SetOwner(TotemUser owner)
        {
            Owners.Add(owner);
        }

        public override string ToString() {
            return $"Id:{Id},Tip:{tipMaterial},Element:{element},ShaftColor:{shaftColorRGB},Range:{range},Damage:{damage}";
        }
    }
}