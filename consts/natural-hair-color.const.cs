using System.Collections.Generic;
using UnityEngine;

namespace consts
{
    public static class NaturalHairColors
    {
        private static List<string> HColors { get; } = new List<string>
        {
            "f9d4ab", "efd2c4", "e2c6c2", "e0d0bb", "ebb77d", "dca788", "cda093", "ccab80", "c58351", 
            "b37652", "81574b", "8a6743", "7a3e10", "5c2a19", "472422", "362714"
        };
        
        public static Color GetRandom()
        {
            var index = Random.Range(0, (int) (HColors.Count - 1));
            ColorUtility.TryParseHtmlString($"#{HColors[index]}", out var c);
            return c;
        }
    }
}