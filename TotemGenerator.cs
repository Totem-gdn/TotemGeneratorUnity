using System.Collections.Generic;
using System.IO;
using System.Net;
using UnityEngine;
using UnityEngine.Serialization;

public class TotemGenerator: MonoBehaviour
{
    static public TotemSpear generateSpear(TipMaterialEnum? Tip=null, ElementEnum? Element=null, ColorEntity ShaftColor = null, float? Range=null, float? Damage=null) {
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
            float RandomNumber = TotemGenerator.NormalizedRandom(0f, 100f);
            Range = RandomNumber;
        } 
        if (Damage == null) {
            float RandomNumber = TotemGenerator.NormalizedRandom(0f, 100f);
            Damage = RandomNumber;
        }
        return new TotemSpear((TipMaterialEnum)Tip, (ElementEnum)Element, ShaftColor, (float)Range, (float)Damage);
    }

    static public TotemAvatar generateAvatar(SexEnum? Sex=null, ColorEntity SkinColor=null, ColorEntity HairColor=null, HairStyleEnum? HairStyle=null, ColorEntity EyeColor=null, bool? BodyFat=null, bool? BodyMuscles=null) {
        if (Sex == null) {
            Sex = TotemGenerator.GetRandomEnum<SexEnum>();
        }
        if (SkinColor == null) {
            SkinColor = new ColorEntity((float)Random.Range(0, 255), (float)Random.Range(0, 255), (float)Random.Range(0, 255));
        }
        if (HairColor == null) {
            HairColor = new ColorEntity((float)Random.Range(0, 255), (float)Random.Range(0, 255), (float)Random.Range(0, 255));
        }
        if (HairStyle == null) {
            HairStyle = TotemGenerator.GetRandomEnum<HairStyleEnum>();
        } 
        if (EyeColor == null) {
            EyeColor = new ColorEntity((float)Random.Range(0, 255), (float)Random.Range(0, 255), (float)Random.Range(0, 255));
        }
        if (BodyFat == null) {
            BodyFat = Random.Range(0, 1) == 1 ? true: false;
        }
        if (BodyMuscles == null) {
            BodyMuscles = Random.Range(0, 1) == 1 ? true: false;
        }
        return new TotemAvatar((SexEnum)Sex, (ColorEntity)SkinColor, (ColorEntity)HairColor, (HairStyleEnum)HairStyle, (ColorEntity)EyeColor, (bool)BodyFat, (bool)BodyMuscles);
    }

    static T GetRandomEnum<T>()
    {
        System.Array A = System.Enum.GetValues(typeof(T));
        T V = (T)A.GetValue(UnityEngine.Random.Range(0,A.Length));
        return V;
    }

    static float NormalizedRandom(float min, float max) 
    {
        float u, v, S;
 
        do
        {
            u = 2.0f * UnityEngine.Random.value - 1.0f;
            v = 2.0f * UnityEngine.Random.value - 1.0f;
            S = u * u + v * v;
        }
        while (S >= 1.0f);
    
        // Standard Normal Distribution
        float std = u * Mathf.Sqrt(-2.0f * Mathf.Log(S) / S);
    
        // Normal Distribution centered between the min and max value
        // and clamped following the "three-sigma rule"
        float mean = (min + max) / 2.0f;
        float sigma = (max - mean) / 3.0f;
        return Mathf.Clamp(std * sigma + mean, min, max);
    }
}