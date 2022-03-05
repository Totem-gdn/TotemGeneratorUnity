using System;
using System.Collections.Generic;

namespace consts
{
    public class NaturalSkinColors
    {
        private static List<string> _skinColors = new List<string>
        {
            "f9d4ab", "efd2c4", "e2c6c2", "e0d0bb", "ebb77d", "dca788", "cda093", "ccab80", "c58351", 
            "b37652", "81574b", "8a6743", "7a3e10", "5c2a19", "472422", "362714"
        };
        
        public static List<string> GetOptions()
        {
            return _skinColors;
        }

        public string GetRandom()
        {
            var r = Random.Range(0, _skinColors.Count);
            return _skinColors[r];
        }
    }
}