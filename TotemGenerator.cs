using System.Collections.Generic;
using System.IO;
using System.Net;
using UnityEngine;
using UnityEngine.Serialization;

public class TotemGenerator: MonoBehaviour
{
    static public TotemSpear generateItem(TipMaterialEnum? Tip=null, ElementEnum? Element=null, ColorEntity ShaftColor = null, float? Range=null, float? Damage=null) {
        if (Tip == null) {
            Tip = TotemGenerator.GetRandomEnum<TipMaterialEnum>();
        }
        if (Element == null) {
            Element = TotemGenerator.GetRandomEnum<ElementEnum>();
        }
        if (ShaftColor == null) {
            ShaftColor = new ColorEntity((float)Random.Range(0, 255), (float)Random.Range(0, 255), (float)Random.Range(0, 255));
        }
        if (Range == null) {
            float RandomNumber = Random.Range(0f, 100f);
            Range = RandomNumber;
        } 
        if (Damage == null) {
            float RandomNumber = Random.Range(0f, 100f);
            Damage = RandomNumber;
        }
        return new TotemSpear((TipMaterialEnum)Tip, (ElementEnum)Element, ShaftColor, (float)Range, (float)Damage);
    }

    static T GetRandomEnum<T>()
    {
        System.Array A = System.Enum.GetValues(typeof(T));
        T V = (T)A.GetValue(UnityEngine.Random.Range(0,A.Length));
        return V;
    }
}