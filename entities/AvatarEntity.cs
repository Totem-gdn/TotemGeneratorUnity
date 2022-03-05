using System;
using enums;
using UnityEngine;

namespace entities
{
    [Serializable]
    public class TotemAvatar 
    {
        public SexEnum sex;
        public Color skinColor;
        public Color hairColor;
        public HairStyleEnum hairStyle;
        public Color eyeColor;
        public BodyFatEnum bodyFat;
        public BodyMusclesEnum bodyMuscles;

        public TotemAvatar(SexEnum aSex, Color aSkinColor, Color aHairColor, HairStyleEnum aHairStyle, Color aEyeColor, BodyFatEnum aBodyFat, BodyMusclesEnum aBodyMuscles) {
            sex = aSex;
            skinColor = aSkinColor;
            hairColor = aHairColor;
            hairStyle = aHairStyle;
            eyeColor = aEyeColor;
            bodyFat = aBodyFat;
            bodyMuscles = aBodyMuscles;
        }

        public override string ToString() {
            return $"Sex:{sex},SkinColor:{skinColor}HairColor:{hairColor},HairStyle:{hairStyle},EyeColor:{eyeColor},BodyFat:{bodyFat},BodyMuscles:{bodyMuscles}";
        }
    }
}