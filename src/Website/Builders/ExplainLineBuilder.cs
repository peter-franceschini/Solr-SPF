using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Website.Models.Explain;
using Website.Helpers;

namespace Website.Builders
{
    public class ExplainLineBuilder
    {
        private ExplainLine ExplainLine = new ExplainLine();

        public ExplainLineBuilder(string raw, int indent)
        {
            ExplainLine.Raw = raw;
            ExplainLine.Indent = indent;
            ExplainLine.Score = GetScore(raw);
            ExplainLine.FunctionOf = GetFunctionOf(raw);
            ExplainLine.FieldName = GetFieldName(raw);
            ExplainLine.FieldQuery = GetFieldQuery(raw);
            ExplainLine.ScoringFactor = GetScoringFactor(raw);
        }

        public ExplainLine GetExplainLine()
        {
            return ExplainLine;
        }

        private double GetScore(string raw)
        {
            var equalsIndex = raw.IndexOf("=");
            var rawScore = raw.Substring(0, equalsIndex).Trim(); ;
            return Double.Parse(rawScore);
        }

        private string GetFunctionOf(string raw)
        {
            // Has no function of (probably could also use children)
            if (!raw.EndsWith("of:") && !raw.EndsWith("from:"))
            {
                return null;
            }

            var function = GetRightSide(raw);
            if (function[function.Length - 1].ToString() == ":")
                function = function.Substring(0, function.Length - 1);

            // If it contains a comma, take second half
            if (function.Contains(","))
            {
                function = function.Substring(function.LastIndexOf(",") + 1);
            }

            function = function.Trim();

            return function;
        }

        private string GetScoringFactor(string raw)
        {
            var rightSide = GetRightSide(raw);

            // split at first space
            var firstWord = GetFirstWord(rightSide);

            // split at first paren
            if (firstWord.Contains("("))
            {
                firstWord = firstWord.Split("(").First();
            }

            // split at first comma
            if (firstWord.Contains(","))
            {
                firstWord = firstWord.Split(",").First();
            }

            firstWord = firstWord.Trim();
            if (Constants.FunctionOfWords.Contains(firstWord))
            {
                return null;
            }

            return firstWord;
        }

        private string GetFieldName(string raw)
        {
            // Get the right side of the line
            raw = GetRightSide(raw);

            // FunctionQueries are special
            if (raw.ToLower().Contains("functionquery"))
            {
                return "FunctionQuery";
            }

            // If there is no colon on this line, abort
            if (!raw.Contains(":"))
            {
                return null;
            }
            // If the line ends with a colon, trim it off
            if (raw.EndsWith(":"))
            {
                raw = raw.Substring(0, raw.Length - 1);
            }

            // If whats left doesnt contain a colon, abort
            if (!raw.Contains(":"))
                return null;

            // Get everything to the left of the colon
            raw = raw.Substring(0, raw.IndexOf(":"));

            // Get last word
            raw = raw.Split(" ").Last();

            raw = raw.Trim();

            if (raw.Contains("("))
            {
                raw = raw.Substring(raw.IndexOf("(") + 1);
            }

            return raw;
        }

        private string GetFieldQuery(string raw)
        {
            // Get the right side of the line
            raw = GetRightSide(raw);

            // FunctionQueries are special
            if (raw.ToLower().Contains("functionquery"))
            {
                // Trim out the first "FunctionQuery(" characters
                raw = raw.Substring(14);
                var indexOfParens = raw.LastIndexOf(")");
                raw = raw.Substring(0, indexOfParens);
                return raw;
            }

            // If there is no colon on this line, abort
            if (!raw.Contains(":"))
            {
                return null;
            }
            // If the line ends with a colon, trim it off
            if (raw.EndsWith(":"))
            {
                raw = raw.Substring(0, raw.Length - 1);
            }

            // If whats left doesnt contain a colon, abort
            if (!raw.Contains(":"))
                return null;

            // Get everything to the right of the colon
            raw = raw.Substring(raw.IndexOf(":") + 1);

            // If the word is quoted, take the whole thing
            if (raw.StartsWith("\""))
            {
                var index = raw.IndexOf('"', raw.IndexOf('"') + 1) + 1;
                raw = raw.Substring(0, index);
            }
            // Get the first word
            else
            {
                raw = raw.Split(" ")[0];
            }

            // If the word ends with a comman, trim it off
            if (raw.EndsWith(","))
            {
                raw = raw.Substring(0, raw.Length - 1);
            }

            return raw;
        }

        private string GetFirstWord(string input)
        {
            return input.Split(" ").First();
        }

        /// <summary>
        /// Gets everything to the right of the equals
        /// </summary>
        /// <param name="raw"></param>
        /// <returns></returns>
        private string GetRightSide(string raw)
        {
            var equalsIndex = raw.IndexOf("=");
            var rightSide = raw.Substring(equalsIndex + 1).Trim();
            return rightSide;
        }
    }
}
