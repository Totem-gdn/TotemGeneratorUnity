using System.Collections.Generic;
using UnityEngine;

namespace DefaultNamespace
{
    public static class NaturalEyeColors
    {
        private static readonly List<string> EyeColors = new List<string>
        {
            "b1b1b1", "070504", "341c0d", "62422e", "914329", "cd622b", "ad7b41", "e4b877"
        };
        
        public static List<string> GetOptions()
        {
            return EyeColors;
        }
        public static Color GetRandom()
        {
            var index = Random.Range(0, (int) (EyeColors.Count - 1));
            ColorUtility.TryParseHtmlString($"#{EyeColors[index]}", out var c);
            return c;
        }
    }
}