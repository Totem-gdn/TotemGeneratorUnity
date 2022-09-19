using System;
using TotemConsts;
using DefaultNamespace;
using TotemEntities;
using TotemEnums;
using UnityEngine;
using Random = UnityEngine.Random;

public class TotemGenerator: MonoBehaviour
{
    public static TotemSpear GenerateSpear(TipMaterialEnum? tip=null, ElementEnum? element=null, Color? shaftColor = null, float? range=null, float? damage=null)
    {
        if (tip == null)
        {
            tip = (TipMaterialEnum) (4 * ExponentialRandom(0, 1));
        }
        if (element == null)
        {
            element = GetRandomEnum<ElementEnum>();
        }
        if (shaftColor == null)
        {
            shaftColor = new Color(Random.value, Random.value, Random.value, 1f);
        }
        if (range == null) {
            var randomNumber = NormalizedRandom(0f, 100f);
            range = randomNumber;
        } 
        if (damage == null) {
            var randomNumber = NormalizedRandom(0f, 100f);
            damage = randomNumber;
        }
        return new TotemSpear((TipMaterialEnum)tip, (ElementEnum)element, (Color) shaftColor, (float)range, (float)damage);
    }

    public static TotemAvatar GenerateAvatar(SexEnum? sex=null, Color? skinColor=null, Color? hairColor=null, HairStyleEnum? hairStyle=null, Color? eyeColor=null, BodyFatEnum? bodyFat=null, BodyMusclesEnum? bodyMuscles=null) {
        if (sex == null)
        {
            sex = GetRandomEnum<SexEnum>();
        }
        if (skinColor == null)
        {
            skinColor = NaturalSkinColors.GetRandom();
        }
        if (hairColor == null)
        {
            hairColor = NaturalHairColors.GetRandom();
        }
        if (hairStyle == null)
        {
            hairStyle = GetRandomEnum<HairStyleEnum>();
        }
        if (eyeColor == null)
        {
            eyeColor = NaturalEyeColors.GetRandom();
        }
        if (bodyFat == null)
        {
            bodyFat = GetRandomEnum<BodyFatEnum>();
        }
        if (bodyMuscles == null)
        {
            bodyMuscles = GetRandomEnum<BodyMusclesEnum>();
        }
        return new TotemAvatar(
            (SexEnum)sex, 
            (Color) skinColor, 
            (Color) hairColor, 
            (HairStyleEnum)hairStyle, 
            (Color) eyeColor,  
            (BodyFatEnum)bodyFat,
            (BodyMusclesEnum)bodyMuscles);
    }

    public static TotemSword GenerateSword(TipMaterialEnum? tip = null, ElementEnum? element = null, Color? shaftColor = null, float? damage = null)
    {
        if (tip == null)
        {
            tip = (TipMaterialEnum)(4 * ExponentialRandom(0, 1));
        }
        if (element == null)
        {
            element = GetRandomEnum<ElementEnum>();
        }
        if (shaftColor == null)
        {
            shaftColor = new Color(Random.value, Random.value, Random.value, 1f);
        }
        if (damage == null)
        {
            var randomNumber = NormalizedRandom(0f, 100f);
            damage = randomNumber;
        }
        return new TotemSword((TipMaterialEnum)tip, (ElementEnum)element, (Color)shaftColor, (float)damage);
    }

    private static T GetRandomEnum<T>()
    {
        var a = System.Enum.GetValues(typeof(T));
        var v = (T)a.GetValue(Random.Range(0,a.Length));
        return v;
    }

    private static float NormalizedRandom(float min, float max) 
    {
        float u; 
        float s;

        do
        {
            u = 2.0f * Random.value - 1.0f;
            var v = 2.0f * Random.value - 1.0f;
            s = u * u + v * v;
        }
        while (s >= 1.0f);
    
        // Standard Normal Distribution
        var std = u * Mathf.Sqrt(-2.0f * Mathf.Log(s) / s);
    
        // Normal Distribution centered between the min and max value
        // and clamped following the "three-sigma rule"
        var mean = (min + max) / 2.0f;
        var sigma = (max - mean) / 3.0f;
        return Mathf.Clamp(std * sigma + mean, min, max);
    }

    private static float ExponentialRandom(float a, float b, float rate=2f)
    {
        var expRate = Mathf.Exp(- rate * a);
        return (float)-Math.Log(expRate - Random.value * (expRate - Mathf.Exp(-rate * b))) / rate;
    }
}
