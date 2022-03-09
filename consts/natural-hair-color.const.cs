using System.Collections.Generic;
using UnityEngine;

namespace consts
{
    public static class NaturalHairColors
    {
        private static List<string> HColors { get; }= new()
        {
            "b5d6e0", "90b4ca", "a7ad7f", "7c8b4f", "c4a05f", "a97e33", "7a3411", "3d0d04"
        };
        
        public static Color GetRandom()
        {
            var index = Random.Range(0, (int) (HColors.Count - 1));
            ColorUtility.TryParseHtmlString($"#{HColors[index]}", out var c);
            return c;
        }
    }
}