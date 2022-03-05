using entities;
using enums;
using UnityEngine;

public class TotemGenerator: MonoBehaviour
{
    public static TotemSpear GenerateSpear(TipMaterialEnum? tip=null, ElementEnum? element=null, ColorEntity shaftColor = null, float? range=null, float? damage=null) {
        tip ??= GetRandomEnum<TipMaterialEnum>();
        element ??= GetRandomEnum<ElementEnum>();
        shaftColor ??=
            new ColorEntity(Random.Range(0, 255), Random.Range(0, 255), Random.Range(0, 255));
        if (range == null) {
            var randomNumber = NormalizedRandom(0f, 100f);
            range = randomNumber;
        } 
        if (damage == null) {
            var randomNumber = NormalizedRandom(0f, 100f);
            damage = randomNumber;
        }
        return new TotemSpear((TipMaterialEnum)tip, (ElementEnum)element, shaftColor, (float)range, (float)damage);
    }

    public static TotemAvatar GenerateAvatar(SexEnum? sex=null, ColorEntity skinColor=null, ColorEntity hairColor=null, HairStyleEnum? hairStyle=null, ColorEntity eyeColor=null, BodyFatEnum? bodyFat=null, BodyMusclesEnum? bodyMuscles=null) {
        sex ??= GetRandomEnum<SexEnum>();
        skinColor ??= new ColorEntity(Random.Range(0, 255), Random.Range(0, 255), Random.Range(0, 255));
        hairColor ??= new ColorEntity(Random.Range(0, 255), Random.Range(0, 255), Random.Range(0, 255));
        hairStyle ??= GetRandomEnum<HairStyleEnum>(); 
        eyeColor ??= new ColorEntity(Random.Range(0, 255), Random.Range(0, 255), Random.Range(0, 255));
        bodyFat ??= GetRandomEnum<BodyFatEnum>();
        bodyMuscles ??= GetRandomEnum<BodyMusclesEnum>();
        return new TotemAvatar(
            (SexEnum)sex, 
            skinColor, 
            hairColor, 
            (HairStyleEnum)hairStyle, 
            eyeColor,  
            (BodyFatEnum)bodyFat,
            (BodyMusclesEnum)bodyMuscles);
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
}