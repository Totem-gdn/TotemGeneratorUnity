using System;
using System.Collections.Generic;
using TotemEntities;
using enums;
using UnityEngine;
using UnityEngine.Serialization;

[Serializable]
public class TotemSpear
{
    public TipMaterialEnum tipMaterial;
    public ElementEnum element;
    public Color shaftColor;
    public float range;
    public float damage;

    public TotemSpear(TipMaterialEnum aTip, ElementEnum aElement, Color aShaftColor, float aRange, float aDamage) {
        tipMaterial = aTip;
        element = aElement;
        shaftColor = aShaftColor;
        range = aRange;
        damage = aDamage;
    }

    public override string ToString() {
        return $"Tip:{tipMaterial},Element:{element},ShaftColor:{shaftColor},Range:{range},Damage:{damage}";
    }
}