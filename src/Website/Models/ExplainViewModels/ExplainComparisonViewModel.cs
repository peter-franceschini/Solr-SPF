using System.Collections.Generic;
using System.Linq;
using Website.Extensions;
using Website.Models.Explain;

namespace Website.Models.ExplainViewModels
{
    public class ExplainComparisonViewModel
    {
        public DocumentModel DocumentModelOne { get; set; }
        public ExplainModel ExplainModelOne { get; set; }
        public DocumentModel DocumentModelTwo { get; set; }
        public ExplainModel ExplainModelTwo { get; set; }
        public List<FieldQuery> CombinedFieldQueryList { get; set; }
        public string[] RadarChartLabels { get; set; }
        public List<RadarChartDataSet> RadarChartDatasetList { get; set; }

        public ExplainComparisonViewModel(SolrQueryResponse solrQueryResponse, int documentOneIndex, int documentTwoIndex)
        {
            DocumentModelOne = solrQueryResponse.Response.DocumentModels.ToList()[documentOneIndex];
            ExplainModelOne = solrQueryResponse.Debug.ExplainModels.ToList()[documentOneIndex];
            DocumentModelTwo = solrQueryResponse.Response.DocumentModels.ToList()[documentTwoIndex];
            ExplainModelTwo = solrQueryResponse.Debug.ExplainModels.ToList()[documentTwoIndex];

            // Setup combinedFieldList
            CombinedFieldQueryList = BuildCombinedFieldList();

            // Setup chart data
            RadarChartLabels = BuildChartLabels();
            RadarChartDatasetList = BuildChartDatasetList();
        }

        private List<RadarChartDataSet> BuildChartDatasetList()
        {
            var radarChartDatasetList = new List<RadarChartDataSet>();

            // ExplainOne
            var datasetOne = new RadarChartDataSet();
            datasetOne.Label = ExplainModelOne.DocumentId;
            datasetOne.RadarChartDataPoints = new double?[CombinedFieldQueryList.Count];
            datasetOne.ColorOne = "rgb(255, 99, 132)";
            datasetOne.ColorTwo = "rgba(255, 99, 132, 0.2)";
            int i = 0;
            foreach (var fieldQuery in CombinedFieldQueryList)
            {
                datasetOne.RadarChartDataPoints[i] = ExplainModelOne.GetFieldExplainLines().FirstOrDefault(f => f.FieldName == fieldQuery.Name && f.FieldQuery == fieldQuery.Query)?.Score;
                i++;
            }
            radarChartDatasetList.Add(datasetOne);

            // ExplainTwo
            var datasetTwo = new RadarChartDataSet();
            datasetTwo.Label = ExplainModelTwo.DocumentId;
            datasetTwo.RadarChartDataPoints = new double?[CombinedFieldQueryList.Count];
            datasetTwo.ColorOne = "rgb(54, 162, 235)";
            datasetTwo.ColorTwo = "rgba(54, 162, 235, 0.2)";
            i = 0;
            foreach (var fieldQuery in CombinedFieldQueryList)
            {
                datasetTwo.RadarChartDataPoints[i] = ExplainModelTwo.GetFieldExplainLines().FirstOrDefault(f => f.FieldName == fieldQuery.Name && f.FieldQuery == fieldQuery.Query)?.Score;
                i++;
            }
            radarChartDatasetList.Add(datasetTwo);

            return radarChartDatasetList;
        }

        private string[] BuildChartLabels()
        {
            var result = new List<string>();
            var combinedFieldList = BuildCombinedFieldList();
            foreach(var field in combinedFieldList)
            {
                var label = $"{field.Name.Truncate(32)}:{field.Query.Truncate(32)}";
                result.Add(label);
            }
            return result.ToArray();
        }

        private List<FieldQuery> BuildCombinedFieldList()
        {
            var fieldList = new List<FieldQuery>();

            // ExplainOne
            foreach (var explainLine in ExplainModelOne.GetFieldExplainLines(null, null, true))
            {
                fieldList.Add(new FieldQuery(explainLine.FieldName, explainLine.FieldQuery));
            }

            // ExplainTwo
            foreach (var explainLine in ExplainModelTwo.GetFieldExplainLines(null, null, true))
            {
                if (!fieldList.Any(f => f.Name == explainLine.FieldName && f.Query == explainLine.FieldQuery))
                {
                    fieldList.Add(new FieldQuery(explainLine.FieldName, explainLine.FieldQuery));
                }
            }

            return fieldList;
        }
        
    }
}
