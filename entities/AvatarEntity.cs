using System;
using System.Collections.Generic;
using System.Linq;
using enums;
using JetBrains.Annotations;
using UnityEngine;

namespace TotemEntities
{
    [Serializable]
    public class TotemAvatar
    {
        private List<TotemUser> _owners;

        public string id;
        public string skinColor;
        public string hairColor;
        public string eyeColor;

        public SexEnum sex;
        public Color eyeColorRGB;
        public Color skinColorRGB;
        public Color hairColorRGB;
        public HairStyleEnum hairStyle;
        public BodyFatEnum bodyFat;
        public BodyMusclesEnum bodyMuscles;

        public TotemAvatar(SexEnum aSex, Color aSkinColor, Color aHairColor, HairStyleEnum aHairStyle, Color aEyeColor, BodyFatEnum aBodyFat, BodyMusclesEnum aBodyMuscles)
        {
            _owners = new List<TotemUser>();
            sex = aSex;
            skinColorRGB = aSkinColor;
            hairColorRGB = aHairColor;
            hairStyle = aHairStyle;
            eyeColorRGB = aEyeColor;
            bodyFat = aBodyFat;
            bodyMuscles = aBodyMuscles;
        }
        
        
        public List<TotemUser> GetOwners()
        {
            return _owners;
        }
        
        [CanBeNull]
        public TotemUser GetCurrentOwner()
        {
            return _owners.Count == 0 ? null : _owners.Last();
        }

        public void SetOwner(TotemUser owner)
        {
            _owners.Add(owner);
        }

        public override string ToString() {
            return $"Sex:{sex},SkinColor:{skinColorRGB}HairColor:{hairColorRGB},HairStyle:{hairStyle},EyeColor:{eyeColorRGB},BodyFat:{bodyFat},BodyMuscles:{bodyMuscles}";
        }
    }
}