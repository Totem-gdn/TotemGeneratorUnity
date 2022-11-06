using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace TotemEntities.DNA
{
    public class TotemDNADefaultAvatar
    {
        public bool sex_bio;
        public bool body_strength;
        public bool body_type;
        public string human_skin_color;
        public string human_hair_color;
        public string human_eye_color;
        public string hair_styles;
        public Color primary_color;

        public override string ToString()
        {
            return $"sex_bio: {sex_bio} | body_strength: {body_strength} | human_skin_color: {human_skin_color} | human_hair_color: {human_hair_color} | " +
                $"human_eye_color: {human_eye_color} | hair_styles: {hair_styles} | " +
                $"primary_color: {primary_color}";
        }
    }
}
