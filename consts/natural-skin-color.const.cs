using System.Collections.Generic;
using UnityEngine;

namespace consts
{
    public static class NaturalSkinColors
    {
        private static List<string> SkinColors { get; } = new List<string>
        {
            "b5d6e0", "90b4ca", "a7ad7f", "7c8b4f", "c4a05f", "a97e33", "7a3411", "3d0d04"
        };
        
        public static Color GetRandom()
        {
            var index = Random.Range(0, (int) (SkinColors.Count - 1));
            ColorUtility.TryParseHtmlString($"#{SkinColors[index]}", out var c);
            return c;
        }
    }
}