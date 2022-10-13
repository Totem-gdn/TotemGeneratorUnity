using System.Text;

namespace TotemUtils
{
    public static class Base64UrlDecoder
    {
        public static string Base64UrlToUTF8(string str)
        {
            string decrypted = ToBase64(str);
            return Encoding.UTF8.GetString(System.Convert.FromBase64String(decrypted));
        }

        private static string ToBase64(string arg)
        {
            var s = arg
                    .PadRight(arg.Length + (4 - arg.Length % 4) % 4, '=')
                    .Replace("_", "/")
                    .Replace("-", "+");

            return s;
        }
    }
}
