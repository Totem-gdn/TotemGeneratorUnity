using System.Collections.Generic;
using UnityEngine;

namespace TotemConsts
{
    public static class NaturalHairColors
    {
        private static List<string> HColors { get; } = new List<string>
        {
            "b1b1b1", "070504", "341c0d", "62422e", "914329", "cd622b", "ad7b41", "e4b877"
        };
        
        public static Color GetRandom()
        {
            var index = Random.Range(0, HColors.Count);
            ColorUtility.TryParseHtmlString($"#{HColors[index]}", out var c);
            return c;
        }
        
        public static Color GetColorByString(string colorHex)
        {
            Debug.Assert(colorHex != null, nameof(colorHex) + " != null");
            var color = HColors.Find(c=> c == colorHex.ToLower());
            Debug.Assert(color != null, nameof(colorHex) + " isn't a valid hair color!");
            ColorUtility.TryParseHtmlString($"#{color}", out var outColor);
            return outColor;
        }
    }
}