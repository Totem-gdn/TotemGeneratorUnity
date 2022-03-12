using System.Collections.Generic;
using UnityEngine;

namespace consts
{
    public static class NaturalHairColors
    {
        private static List<string> HColors { get; } = new List<string>
        {
            "b1b1b1", "070504", "341c0d", "62422e", "914329", "cd622b", "ad7b41", "e4b877"
        };
        
        public static Color GetRandom()
        {
            var index = Random.Range(0, (int) (HColors.Count - 1));
            ColorUtility.TryParseHtmlString($"#{HColors[index]}", out var c);
            return c;
        }
    }
}