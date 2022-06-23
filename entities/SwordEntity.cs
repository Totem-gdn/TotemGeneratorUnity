using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using JetBrains.Annotations;
using enums;
using UnityEngine;

namespace TotemEntities
{
    [Serializable]
    public class TotemSword
    {
        private List<TotemUser> _owners;
        public TipMaterialEnum tipMaterial;
        public ElementEnum element;
        public Color shaftColor;
        public float damage;

        public TotemSword(TipMaterialEnum aTip, ElementEnum aElement, Color aShaftColor, float aDamage)
        {
            _owners = new List<TotemUser>();
            tipMaterial = aTip;
            element = aElement;
            shaftColor = aShaftColor;
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

        public override string ToString()
        {
            return $"Tip:{tipMaterial},Element:{element},ShaftColor:{shaftColor},Damage:{damage}";
        }
    }

}
