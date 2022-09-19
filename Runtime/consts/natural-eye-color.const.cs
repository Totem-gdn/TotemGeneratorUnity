using System.Collections.Generic;
using TotemConsts;
using DefaultNamespace;
using UnityEngine;

namespace DefaultNamespace
{
    public static class NaturalEyeColors
    {
        private static List<string> EyeColors { get; } = new List<string>
        {
            "b5d6e0", "90b4ca", "a7ad7f", "7c8b4f", "c4a05f", "a97e33", "7a3411", "3d0d04"
        };
        
        public static Color GetRandom()
        {
            var index = Random.Range(0, EyeColors.Count);
            ColorUtility.TryParseHtmlString($"#{EyeColors[index]}", out var c);
            return c;
        }
        
        public static Color GetColorByString(string colorHex)
        {
            Debug.Assert(colorHex != null, nameof(colorHex) + " != null");
            var color = EyeColors.Find(c=> c == colorHex.ToLower());
            Debug.Assert(color != null, nameof(colorHex) + " isn't a valid eye color!");
            ColorUtility.TryParseHtmlString($"#{color}", out var outColor);
            return outColor;
        }
    }
}