using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using Website.Extensions;
using Website.Models;
using Website.Models.Explain;
using Website.Helpers;
using Website.Builders;

namespace Website.Builders
{
    public class SolrQueryResponseBuilder
    {
        private SolrQueryResponse SolrQueryResponse = new SolrQueryResponse();

        public SolrQueryResponseBuilder(string rawSolrQueryResponse)
        {
            SolrQueryResponse = JsonConvert.DeserializeObject<SolrQueryResponse>(rawSolrQueryResponse);

            SolrQueryResponse.Debug.ExplainModels = BuildExplainModels(SolrQueryResponse.Debug.explain);
            SolrQueryResponse.Response.DocumentModels = BuildDocumentModels(SolrQueryResponse.Response.Docs);
        }

        public SolrQueryResponse GetSolrQueryResponse()
        {
            return SolrQueryResponse;
        }

        /// <summary>
        /// Builds strongly typed document models from the raw json documents provided by Solr
        /// </summary>
        /// <param name="docs"></param>
        /// <returns></returns>
        private IEnumerable<DocumentModel> BuildDocumentModels(JObject[] docs)
        {
            // Loop through documents
            foreach(var doc in docs)
            {
                var document = BuildDocumentModel(doc);
                yield return document;
            }
        }

        /// <summary>
        /// Builds a strongly typed document model from the json document model provided by Solr
        /// </summary>
        /// <param name="doc"></param>
        /// <returns></returns>
        private DocumentModel BuildDocumentModel(JObject doc)
        {
            var document = new DocumentModel();

            foreach (var property in doc.Properties())
            {
                document.Fields.Add(new Field()
                    {
                        Name = property.Name,
                        Value = property.Value.ToString()
                    }
                );
            }

            return document;
        }

        private IEnumerable<ExplainModel> BuildExplainModels(JObject explain)
        {
            foreach (var item in explain)
            {
                var explainModelBuilder = new ExplainModelBuilder(item.Key, item.Value.ToString());
                yield return explainModelBuilder.GetExplainModel();
            }
        }

        
    }
}
