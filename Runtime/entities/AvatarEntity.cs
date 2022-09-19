using System;
using System.Collections.Generic;
using System.Linq;
using TotemEnums;
using JetBrains.Annotations;
using UnityEngine;

namespace TotemEntities
{
    [Serializable]
    public class TotemAvatar : ITotemAsset
    {
        public string Id { get; set; }
        public List<TotemUser> Owners { get; set; }

        public string skinColor;
        public string hairColor;
        public string eyeColor;
        public string clothingColor;

        public SexEnum sex;
        public Color eyeColorRGB;
        public Color skinColorRGB;
        public Color hairColorRGB;
        public Color clothingColorRGB;
        public HairStyleEnum hairStyle;
        public BodyFatEnum bodyFat;
        public BodyMusclesEnum bodyMuscles;

        public TotemAvatar(SexEnum aSex, Color aSkinColor, Color aHairColor, HairStyleEnum aHairStyle, Color aEyeColor, BodyFatEnum aBodyFat, BodyMusclesEnum aBodyMuscles)
        {
            Owners = new List<TotemUser>();
            sex = aSex;
            skinColorRGB = aSkinColor;
            hairColorRGB = aHairColor;
            hairStyle = aHairStyle;
            eyeColorRGB = aEyeColor;
            bodyFat = aBodyFat;
            bodyMuscles = aBodyMuscles;
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

        public override string ToString() {
            return $"Sex:{sex},SkinColor:{skinColorRGB}HairColor:{hairColorRGB},HairStyle:{hairStyle},EyeColor:{eyeColorRGB},BodyFat:{bodyFat},BodyMuscles:{bodyMuscles},ClothingColor:{clothingColorRGB}";
        }
    }
}