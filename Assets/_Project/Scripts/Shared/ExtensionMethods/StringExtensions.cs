using TriplanoTest.Shared.Utils;
using System;
using UnityEngine;

namespace TriplanoTest.Shared.ExtensionMethods
{
    public static class StringExtensions
    {
        public static T ConvertToEnum<T>(this string value) where T : Enum
        {
            return (T)Enum.Parse(typeof(T), value);
        }

        public static string AddRichTextColor(this string text, Color color)
        {
            string hex = ColorUtils.GetStringFromColor(color);
            return $"<color=#{hex}>{text}</color>";
        }

        public static string AddRichBold(this string text) => $"<b>{text}</b>";

        public static bool ContainsOtherString(this string thisString, string otherString)
        {
            return otherString.ToLower().Contains(thisString.ToLower());
        }

        /// <summary>
        /// Ignore everything before the first '_'
        /// </summary>
        /// <returns>Before: Name_SubName -> After: SubName</returns>
        public static string StringReduction(this string value, char matchCharacter = '_')
        {
            string shortName = string.Empty;
            bool findedUnderscore = false;

            foreach (char c in value)
            {
                if (findedUnderscore)
                {
                    shortName += c;
                }
                else if (c == matchCharacter)
                {
                    findedUnderscore = true;
                }
            }

            if (findedUnderscore)
            {
                return shortName;
            }

            return value;
        }

        /// <summary>
        /// Ignore everything before the first '_'
        /// </summary>
        /// <returns>Before: Name_SubName -> After: SubName</returns>
        public static string StringSimplified(this string value, char matchCharacter = '_')
        {
            string shortName = string.Empty;

            foreach (char c in value)
            {
                if (c == matchCharacter)                
                    break;

                shortName += c;
            }

            return shortName;
        }

        public static string GetNiceString(this string value)
        {
            string result = string.Empty;

            // Loop through our string
            for (int i = 0; i < value.Length; i++)
            {
                if (i == 0)
                {
                    result += char.ToUpper(value[0]);
                }
                else if (value[i] == '_')
                {
                    result += ' ';
                }
                else 
                {
                    char last = value[i - 1];
                    if (char.IsUpper(value[i]) == true && last != ' ' && !char.IsUpper(last))
                    {
                        result += ' ';
                    }

                    result += value[i];
                }
            }

            return result;
        }

        public static string LimitMaxCharacters(this string value, int max)
        {
            if (value.Length <= max)
                return value;

            int start = value.Length - max;
            string result = "...";
            for (int i = start; i < value.Length; i++)
            {
                result += value[i];
            }

            return result;
        }
    }
}