using System;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class TotemAvatar 
{
    public SexEnum Sex;
    public ColorEntity SkinColor;
    public ColorEntity HairColor;
    public HairStyleEnum HairStyle;
    public ColorEntity EyeColor;
    public bool BodyFat;
    public bool BodyMuscles;

    public TotemAvatar(SexEnum aSex, ColorEntity aSkinColor, ColorEntity aHairColor, HairStyleEnum aHairStyle, ColorEntity aEyeColor, bool aBodyFat, bool aBodyMuscles) {
        Sex = aSex;
        SkinColor = aSkinColor;
        HairColor = aHairColor;
        HairStyle = aHairStyle;
        EyeColor = aEyeColor;
        BodyFat = aBodyFat;
        BodyMuscles = aBodyMuscles;
    }

    override public string ToString() {
        return $"Sex:{Sex},SkinColor:{SkinColor}HairColor:{HairColor},HairStyle:{HairStyle},EyeColor:{EyeColor},BodyFat:{BodyFat},BodyMuscles:{BodyMuscles}";
    }
}