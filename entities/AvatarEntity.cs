using System;
using enums;

namespace entities
{
    [Serializable]
    public class TotemAvatar 
    {
        public SexEnum sex;
        public ColorEntity skinColor;
        public ColorEntity hairColor;
        public HairStyleEnum hairStyle;
        public ColorEntity eyeColor;
        public BodyFatEnum bodyFat;
        public BodyMusclesEnum bodyMuscles;

        public TotemAvatar(SexEnum aSex, ColorEntity aSkinColor, ColorEntity aHairColor, HairStyleEnum aHairStyle, ColorEntity aEyeColor, BodyFatEnum aBodyFat, BodyMusclesEnum aBodyMuscles) {
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