using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Website.Extensions;
using Website.Models.Explain;
using Website.Helpers;

namespace Website.Builders
{
    public class ExplainModelBuilder
    {
        private ExplainModel ExplainModel = new ExplainModel();

        public ExplainModelBuilder(string id, string rawExplain)
        {
            // Set DocumentId
            ExplainModel.DocumentId = id;

            // Split rawExplain into list of lines
            var lines = rawExplain.Split("\n");
            lines = FixImproperLineSplitting(lines);

            ExplainLine lastLine = null;

            // Loop through non-empty lines
            foreach (var line in lines.Where(l => !string.IsNullOrEmpty(l)))
            {
                // Build new line
                var explainLineBuilder = new ExplainLineBuilder(line.Trim(), GetIndentAmount(line));
                var newLine = explainLineBuilder.GetExplainLine();

                // Insert new line
                InsertNewLine(ExplainModel, lastLine, line, newLine);

                // set new lastLine
                lastLine = newLine;
            }

            // Calculate the score percentages for every explainline in the explainModel
            CalculateScorePercent(ExplainModel.ExplainLine, ExplainModel.DocumentScore);

            // Build out piechart specific data
            ExplainModel.PieChartFieldNames = BuildPieChartFieldNames(ExplainModel);
            ExplainModel.PieChartDataPoints = BuildPieChartDataPoints(ExplainModel);
            ExplainModel.PieChartDataPointsPercent = BuildPieChartDataPointsPercent(ExplainModel);
            ExplainModel.PieChartColors = BuildPieChartColors(ExplainModel);
        }

        public ExplainModel GetExplainModel()
        {
            return ExplainModel;
        }

        /// <summary>
        /// Determines where in the tree to insert this new line
        /// </summary>
        /// <param name="explainModel"></param>
        /// <param name="lastLine"></param>
        /// <param name="line"></param>
        /// <param name="newLine"></param>
        private void InsertNewLine(ExplainModel explainModel, ExplainLine lastLine, string line, ExplainLine newLine)
        {
            // Determine where to insert

            // If this is the first explainLine
            if (explainModel.ExplainLine == null)
            {
                explainModel.ExplainLine = newLine;
                explainModel.DocumentScore = newLine.Score;
            }
            // If the last inserted line was a lower indent, then make this its child
            else if (lastLine.Indent < GetIndentAmount(line))
            {
                lastLine.AddChild(newLine);
            }
            // If the last inserted line was at the same indent, then give this the same parent as the last insert
            else if (lastLine.Indent == GetIndentAmount(line))
            {
                lastLine.Parent.AddChild(newLine);
            }
            // If the last inserted has a higher indent, find the last insert with a lower indent and be the child of that line
            else if (lastLine.Indent > GetIndentAmount(line))
            {
                var parent = lastLine.Parent;
                while (parent.Indent >= newLine.Indent)
                {

                    parent = parent.Parent;
                }
                parent.AddChild(newLine);
            }
        }

        /// <summary>
        /// If a line begins with ), combine this line with the previous line
        /// </summary>
        /// <param name="lines"></param>
        /// <returns></returns>
        private string[] FixImproperLineSplitting(string[] lines)
        {
            var sbList = new List<StringBuilder>();

            foreach (var line in lines)
            {
                if (line.StartsWith(")"))
                {
                    var lastLine = sbList.Last();
                    lastLine = lastLine.Append(line);
                }
                else
                {
                    sbList.Add(new StringBuilder().Append(line));
                }
            }

            return sbList.Select(sb => sb.ToString()).ToArray();
        }

        /// <summary>
        /// Recursively update score percents for each line from the top down
        /// </summary>
        /// <param name="line"></param>
        /// <param name="documentTotalScore"></param>
        private void CalculateScorePercent(ExplainLine line, double documentTotalScore)
        {
            line.UpdateScorePercent(documentTotalScore);

            if (!line.HasChildren())
                return;

            foreach (var child in line.Children)
            {
                CalculateScorePercent(child, documentTotalScore);
            }
        }

        private string[] BuildPieChartFieldNames(ExplainModel explainModel)
        {
            var explainLines = GetPieChartFieldExplainLines(explainModel);
            var fieldNames = explainLines.OrderByDescending(o => o.ScorePercent).Select(s => $"{s.FieldName.Truncate(32)} : {s.FieldQuery.Truncate(32)} ({s.Score})");
            return fieldNames.ToArray();
        }

        private double[] BuildPieChartDataPoints(ExplainModel explainModel)
        {
            var explainLines = GetPieChartFieldExplainLines(explainModel);
            return explainLines.OrderByDescending(o => o.Score).Select(s => s.Score).ToArray();
        }

        private decimal?[] BuildPieChartDataPointsPercent(ExplainModel explainModel)
        {
            var explainLines = GetPieChartFieldExplainLines(explainModel);
            return explainLines.OrderByDescending(o => o.ScorePercent).Select(s => s.ScorePercent).ToArray();
        }

        private string[] BuildPieChartColors(ExplainModel explainModel)
        {
            var explainLines = GetPieChartFieldExplainLines(explainModel);
            return Constants.RgbaColors.Take(explainLines.Count()).ToArray();
        }

        private static IEnumerable<ExplainLine> GetPieChartFieldExplainLines(ExplainModel explainModel)
        {
            return explainModel.GetFieldExplainLines().Where(e => e.ScorePercent != 0);
        }

        /// <summary>
        /// Gets the count of whitepaces at the beginning of a string
        /// </summary>
        /// <param name="line"></param>
        /// <returns></returns>
        private int GetIndentAmount(string line)
        {
            return line.TakeWhile(Char.IsWhiteSpace).Count();
        }
    }
}
