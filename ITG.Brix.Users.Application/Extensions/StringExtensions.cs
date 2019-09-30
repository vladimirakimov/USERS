using System;
using System.Linq;

namespace ITG.Brix.Users.Application.Extensions
{
    public static class StringExtension
    {
        public static string ToCamelCase(this string str)
        {
            if (!string.IsNullOrWhiteSpace(str) && str.Length > 1)
            {
                return Char.ToLowerInvariant(str[0]) + str.Substring(1);
            }
            return str;
        }

        public static bool StartsWithLetter(this string str)
        {
            var result = false;
            if (!string.IsNullOrWhiteSpace(str))
            {
                result = char.IsLetter(str[0]);
            }
            return result;
        }

        public static bool StartsWithCapitalLetter(this string str)
        {
            var result = false;
            if (!string.IsNullOrWhiteSpace(str))
            {
                result = char.IsLetter(str[0]) && str[0].ToString() == str[0].ToString().ToUpperInvariant();
            }
            return result;
        }

        public static bool AtLeastOneSpecialCharacter(this string str)
        {
            var result = false;
            if (!string.IsNullOrWhiteSpace(str))
            {
                result = str.Any(p => !char.IsLetterOrDigit(p));
            }
            return result;
        }

        public static bool HasLettersOrSingleSpaceCharacters(this string str)
        {
            var result = false;
            if (!string.IsNullOrEmpty(str))
            {
                result = !str.StartsWith(" ") && !str.EndsWith(" ") && !str.Contains("  ") &&
                         !str.Any(p => !(char.IsLetter(p) || char.IsWhiteSpace(p)));
            }
            return result;
        }

        public static int? ToNullableInt(this string s)
        {
            if (int.TryParse(s, out int i)) return i;
            return null;
        }

    }
}
