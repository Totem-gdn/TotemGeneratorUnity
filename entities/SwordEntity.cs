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
    public class TotemSword : ITotemAsset
    {
        public string Id { get; set; }
        public List<TotemUser> Owners { get; set; }

        public string shaftColor;
        public TipMaterialEnum tipMaterial;
        public ElementEnum element;
        public Color shaftColorRGB;
        public float damage;

        public TotemSword(TipMaterialEnum aTip, ElementEnum aElement, Color aShaftColor, float aDamage)
        {
            Owners = new List<TotemUser>();
            tipMaterial = aTip;
            element = aElement;
            shaftColorRGB = aShaftColor;
            damage = aDamage;
        }

        public List<TotemUser> GetOwners()
        {
            return Owners;
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

        public override string ToString()
        {
            return $"Id:{Id}, Tip:{tipMaterial},Element:{element},ShaftColor:{shaftColorRGB},Damage:{damage}";
        }
    }

}
