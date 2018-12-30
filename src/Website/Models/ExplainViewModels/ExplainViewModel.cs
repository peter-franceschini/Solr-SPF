using Microsoft.AspNetCore.Mvc.Rendering;
using System.Collections.Generic;
using Website.Models.Database;
using Website.Models.Explain;

namespace Website.Models.ExplainViewModels
{
    public class ExplainViewModel
    {
        public string Guid { get; set; }
        public SolrQueryResponse SolrQueryResponse { get; set; }
        public List<SelectListItem> ComparisonDocumentSelectList { get; set;}
        public int DocumentOneIndex { get; set; }
        public int DocumentTwoIndex { get; set; }

        public ExplainViewModel(string guid, SolrQueryResponse solrQueryResponse)
        {
            Guid = guid;
            SolrQueryResponse = solrQueryResponse;
            ComparisonDocumentSelectList = GetComparisonDocumentSelectList(SolrQueryResponse.Debug.ExplainModels);
        }

        private List<SelectListItem> GetComparisonDocumentSelectList(IEnumerable<ExplainModel> explainModels)
        {
            var list = new List<SelectListItem>();

            var i = 0;
            foreach (var model in explainModels)
            {
                list.Add(new SelectListItem
                {
                    Text = string.Format("(#{0}) {1}", i + 1, model.DocumentId),
                    Value = i.ToString()
                });
                i++;
            }

            return list;
        }

    }
}
