using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Website.Models.Explain
{
    public class ExplainModel
    {
        public string DocumentId { get; set; }
        public string RawValue { get; set; }
        public double DocumentScore { get; set; }
        public ExplainLine ExplainLine { get; set; }

        // For pie chart
        public string[] PieChartFieldNames { get; set; }
        public double[] PieChartDataPoints { get; set; }
        public decimal?[] PieChartDataPointsPercent { get; set; }
        public string[] PieChartColors { get; set; }

        /// <summary>
        /// Returns a flat list of all explain lines that are associated with specific fields
        /// </summary>
        /// <param name="explainLine">The explainLine to start from, if null uses the ExplainModel's first line</param>
        /// <param name="explainLines">Do not use from the initial calling method</param>
        /// <returns></returns>
        public List<ExplainLine> GetFieldExplainLines(ExplainLine explainLine = null, List<ExplainLine> explainLines = null, bool requireScorePercent = false)
        {
            if(explainLine == null)
            {
                explainLine = ExplainLine;
            }

            if(explainLines == null)
                explainLines = new List<ExplainLine>();

            if (!string.IsNullOrEmpty(explainLine.FieldName) && (explainLine.ScorePercent != 0 || requireScorePercent == false))
                explainLines.Add(explainLine);

            if (explainLine.Children == null)
                return explainLines;

            foreach(var child in explainLine.Children)
            {
                GetFieldExplainLines(child, explainLines, requireScorePercent);
            }

            return explainLines;
        }        
    }
}
