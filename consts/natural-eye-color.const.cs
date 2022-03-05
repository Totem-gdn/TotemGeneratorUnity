namespace DefaultNamespace
{
    public class NaturalEyeColors
    {
        private static List<string> _eyeColors =
        {
            "b1b1b1", "070504", "341c0d", "62422e", "914329", "cd622b", "ad7b41", "e4b877"
        };
        
        public static List<string> GetOptions()
        {
            return _eyeColors;
        }
        public string GetRandom()
        {
            var r = Random.Range(0, _skinColors.Count);
            return _eyeColors[r];
        }
    }
}