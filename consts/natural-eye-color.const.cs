using System.Collections.Generic;

namespace consts
{
    public class NaturalEyeColorConst
    {
        private static List<string> _sColors = new List<string>
        {
            "b5d6e0", "90b4ca", "a7ad7f", "7c8b4f", "c4a05f", "a97e33", "7a3411", "3d0d04"
        };

        public static List<string> GetOptions()
        {
            return _sColors;
        }
    }
}