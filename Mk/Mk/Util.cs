using System;
using System.Collections.Generic;
using System.Security.Cryptography;
using System.Text;

namespace Mk
{
    public static class Util
    {
        public static string LanguageFromCode(string code)
        {
            switch (code)
            {
                case "sv":
                    return "Swedish";
                case "en":
                    return "English";
                case "ar":
                    return "Arabic";
                case "fa":
                    return "Farsi";
            }
            return string.Empty;
        }

        public static string CodeFromLanguage(string language)
        {
            switch (language)
            {
                case "Swedish":
                    return "sv";
                case "English":
                    return "en";
                case "Arabic":
                    return "ar";
                case "Farsi":
                    return "fa";
            }
            return string.Empty;
        }

        public static List<string> SupportedLanguages()
        {
            return new List<string>
            {
                "Swedish",
                "English",
                "Arabic",
                "Farsi"
            };
        }

        public static string Crypt(string s)
        {
            return Convert.ToBase64String(ProtectedData.Protect(Encoding.Unicode.GetBytes(s), null,
                    DataProtectionScope.LocalMachine));
        }

        public static string Decrypt(string s)
        {
            return Encoding.Unicode.GetString(ProtectedData.Unprotect(Convert.FromBase64String(s), null,
                    DataProtectionScope.LocalMachine));
        }
    }
}
