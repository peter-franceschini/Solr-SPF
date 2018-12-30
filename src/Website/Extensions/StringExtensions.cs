using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Website.Extensions
{
    public static class StringExtensions
    {
        /// <summary>
        /// Highlights every instance of the key as bold in the inputted string
        /// </summary>
        /// <param name="input">String to highlight</param>
        /// <param name="key">Word to highlight</param>
        /// <returns></returns>
        public static string Highlight(this string input, string key)
        {
            if (key == null)
                return input;

            string returnValue = input;
            returnValue = returnValue.Replace(key, "<b>" + key + "</b>");
            
            return returnValue;
        }

        /// <summary>
        /// Removes the score from a raw explainLine
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public static string RemoveScore(this string input)
        {
            if (!input.Contains("="))
                return input;
            var equalsIndex = input.IndexOf("=");

            return input.Substring(equalsIndex + 1).Trim();
        }

        /// <summary>
        /// Truncates the inputted string to the defined maximum length
        /// </summary>
        /// <param name="value"></param>
        /// <param name="maxLength"></param>
        /// <returns></returns>
        public static string Truncate(this string value, int maxLength)
        {
            if (string.IsNullOrEmpty(value))
                return value;

            if (value.Length > maxLength)
            {
                value = value.Substring(0, maxLength) + "...";
            }

            return value;
        }
    }
}
