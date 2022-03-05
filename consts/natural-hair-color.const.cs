using System.Collections.Generic;

namespace consts
{
    public class NaturalHairColorConst
    {
        private static List<string> _hColors = new List<string>
        {
            "b5d6e0", "90b4ca", "a7ad7f", "7c8b4f", "c4a05f", "a97e33", "7a3411", "3d0d04"
        };

        public static List<string> GetOptions()
        {
            return _hColors;
        }
        
        public string GetRandom()
        {
            var r = Random.Range(0, _hColors.Count);
            return _hColors[r];
        }
    }
}