using System.Collections.Generic;
using consts;
using DefaultNamespace;
using UnityEngine;

namespace DefaultNamespace
{
    public static class NaturalEyeColors
    {
        private static List<string> EyeColors { get; } = new List<string>
        {
            "b1b1b1", "070504", "341c0d", "62422e", "914329", "cd622b", "ad7b41", "e4b877"
        };
        
        public static Color GetRandom()
        {
            var index = Random.Range(0, (int) (EyeColors.Count - 1));
            ColorUtility.TryParseHtmlString($"#{EyeColors[index]}", out var c);
            return c;
        }
    }
}