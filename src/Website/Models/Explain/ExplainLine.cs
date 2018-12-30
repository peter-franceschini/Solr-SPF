using System;
using System.Collections.Generic;
using System.Linq;
using Website.Helpers;

namespace Website.Models.Explain
{
    public class ExplainLine
    {
        public string Raw { get; set; }
        public int Indent { get; set; }
        public double Score { get; set; }
        public decimal? ScorePercent { get; set; }
        public string ScoringFactor { get; set; }
        public string FunctionOf { get; set; }
        public string FieldName { get; set; }
        public string FieldQuery { get; set; }

        public List<ExplainLine> Children { get; set; }
        public ExplainLine Parent { get; set; }

        public void UpdateScorePercent(double documentTotalScore)
        {
            // If this is the first explainline
            if (Parent == null)
            {
                ScorePercent = 100;
                return;
            }

            // If the parent has a score of 0
            if(Parent.Score == 0)
            {
                ScorePercent = 0;
                return;
            }

            // If parent is sum of
            if (Parent.FunctionOf.ToLower().Contains("sum"))
            {
                ScorePercent = GetSumScorePercent();
            }
            // If parent is product of
            else if (Parent.FunctionOf.ToLower().Contains("product"))
            {
                ScorePercent = GetProductScorePercent();
            } 
            // If parent is result of
            else if (Parent.FunctionOf.ToLower().Contains("result"))
            {
                ScorePercent = GetResultScorePercent();
            } 
            // If parent is max of
            else if (Parent.FunctionOf.ToLower().Contains("max"))
            {
                ScorePercent = GetMaxScorePercent();
            }
        }

        private decimal? GetResultScorePercent()
        {
            return Parent.ScorePercent;
        }

        private decimal? GetMaxScorePercent()
        {
            // Only the line that contains the highest score of all its siblings has a scorepercent, all other siblings are worth 0%

            // Get all of the siblings
            var siblings = Parent.Children;

            // If this item isnt the highest of all the siblings, then it's score is
            var maxSiblingScore = siblings.Max(s => s.Score);
            if (Score < maxSiblingScore)
            {
                return 0;
            }
            else
            {
                return Parent.ScorePercent;
            }
        }

        private decimal? GetProductScorePercent()
        {
            // ScorePercent is the the % of the siblings score * % of the parents score 

            // Get all of the siblings
            var siblings = Parent.Children;
            var totalSiblingsScore = siblings.Sum(s => s.Score);
            var percentSiblingTotal = Score / totalSiblingsScore;
            var parentPercent = (double)(Parent.ScorePercent / 100);
            var percentTotalScore = (decimal)(percentSiblingTotal * parentPercent) * 100;

            return Math.Round(percentTotalScore, 2);
        }

        private decimal? GetSumScorePercent()
        {
            // ScorePercent is the percentage of how much the parents score is from this line (compared to other siblings)

            // Get percent of parent
            var parentPercent = (double)(Parent.ScorePercent / 100);
            var percentOfParent = Score / Parent.Score;
            var scorePercent = (decimal)((percentOfParent * parentPercent) * 100);

            return Math.Round(scorePercent, 2);
        }

        /// <summary>
        /// Determines if this ExplainLine has any children ExplainLines
        /// </summary>
        /// <returns></returns>
        public bool HasChildren()
        {
            if (Children != null && Children.Any())
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        /// <summary>
        /// Adds a child directly bellow this ExplainLine in the tree
        /// </summary>
        /// <param name="node"></param>
        public void AddChild(ExplainLine node)
        {
            // Set Child's parent
            node.Parent = this;

            // Add child
            if (Children == null)
            {
                Children = new List<ExplainLine>();
            }
            Children.Add(node);
        }

        public override string ToString()
        {
            return $"Score: {Score}, ScorePercent: {ScorePercent}, FunctionOf: {FunctionOf}, ScoringFactor: {ScoringFactor}, FieldName: {FieldName}, FieldQuery: {FieldQuery}";
        }

    }
}
