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
        public SexEnum sex;
        public Color skinColor;
        public Color hairColor;
        public HairStyleEnum hairStyle;
        public Color eyeColor;
        public BodyFatEnum bodyFat;
        public BodyMusclesEnum bodyMuscles;

        public TotemAvatar(SexEnum aSex, Color aSkinColor, Color aHairColor, HairStyleEnum aHairStyle, Color aEyeColor, BodyFatEnum aBodyFat, BodyMusclesEnum aBodyMuscles)
        {
            _owners = new List<TotemUser>();
            sex = aSex;
            skinColor = aSkinColor;
            hairColor = aHairColor;
            hairStyle = aHairStyle;
            eyeColor = aEyeColor;
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
            return $"Sex:{sex},SkinColor:{skinColor}HairColor:{hairColor},HairStyle:{hairStyle},EyeColor:{eyeColor},BodyFat:{bodyFat},BodyMuscles:{bodyMuscles}";
        }
    }
}