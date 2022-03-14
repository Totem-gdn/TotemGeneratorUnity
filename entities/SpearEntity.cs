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
        public TipMaterialEnum tipMaterial;
        public ElementEnum element;
        public Color shaftColor;
        public float range;
        public float damage;

        public TotemSpear(TipMaterialEnum aTip, ElementEnum aElement, Color aShaftColor, float aRange, float aDamage)
        {
            _owners = new List<TotemUser>();
            tipMaterial = aTip;
            element = aElement;
            shaftColor = aShaftColor;
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

        public override string ToString() {
            return $"Tip:{tipMaterial},Element:{element},ShaftColor:{shaftColor},Range:{range},Damage:{damage}";
        }
    }
}