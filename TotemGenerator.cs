using System.Collections.Generic;
using System.IO;
using System.Net;
using UnityEngine;
using UnityEngine.Serialization;

public class TotemGenerator: MonoBehaviour
{
    public TotemSpear generateItem(TipMaterialEnum Tip=null, ElementEnum Element=null, ColorEntity ShaftColor=null, float Range=null, float Damage=null) {
        if (Tip == null) {
            var values = Enum.GetValues(typeof(TipMaterialEnum));
            Tip = (TipMaterialEnum)values[Random.Range(0, values.Length)];
        }
        if (ElementEnum == null) {
            var values = Enum.GetValues(typeof(ElementEnum));
            Tip = (ElementEnum)values[Random.Range(0, values.Length)];
        }
        if (ColorEntity == null) {
            ColorEntity = ColorEntity(red=(float)Random.Range(0, 255), green=(float)Random.Range(0, 255), blue=(float)Random.Range(0, 255), alpha=1.0);
        }
        if (Range == null) {
            Range = (float) 100 * (1 - Mathf.sqrt(Random.Range(0, 101)));
        } 
        if (Damage == null) {
            Damage = (float) 100 * (1 - Mathf.sqrt(Random.Range(0, 101)));
        }
        return TotemSpear(Tip, Element, ShaftColor, Range, Damage);
    }
}