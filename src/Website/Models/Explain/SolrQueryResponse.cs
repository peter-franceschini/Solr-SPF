using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.Collections.Generic;
using Website.Models.Explain;

namespace Website.Models.Explain
{
    public class SolrQueryResponse
    {
        public Responseheader responseHeader { get; set; }
        [JsonProperty(PropertyName = "response")]
        public Response Response { get; set; }
        [JsonProperty(PropertyName = "debug")]
        public Debug Debug { get; set; }
    }

    public class Responseheader
    {
        [JsonProperty(PropertyName = "status")]
        public int Status { get; set; }
        public int QTime { get; set; }
        public Params _params { get; set; }
    }

    public class Params
    {
        public string q { get; set; }
        public string indent { get; set; }
        public string wt { get; set; }
        public string debugQuery { get; set; }
    }

    public class Response
    {
        public int numFound { get; set; }
        public int start { get; set; }
        [JsonProperty(PropertyName = "docs")]
        public JObject[] Docs { get; set; }

        public IEnumerable<DocumentModel> DocumentModels { get; set; }
    }

    public class DocumentModel
    {
        public List<Field> Fields { get; set; }
        
        public DocumentModel()
        {
            Fields = new List<Field>();
        }
    }

    public class Field
    {
        public string Name { get; set; }
        public string Value { get; set; }
    }

    public class Debug
    {
        public string rawquerystring { get; set; }
        public string querystring { get; set; }
        public string parsedquery { get; set; }
        public string parsedquery_toString { get; set; }
        public JObject explain { get; set; }
        public string QParser { get; set; }
        public Timing timing { get; set; }

        // Built from Raw Explains
        public IEnumerable<ExplainModel> ExplainModels { get; set; }
    }

    public class Timing
    {
        public float time { get; set; }
        public Prepare prepare { get; set; }
        public Process process { get; set; }
    }

    public class Prepare
    {
        public float time { get; set; }
        public Query query { get; set; }
        public Facet facet { get; set; }
        public Facet_Module facet_module { get; set; }
        public Mlt mlt { get; set; }
        public Highlight highlight { get; set; }
        public Stats stats { get; set; }
        public Expand expand { get; set; }
        public Debug debug { get; set; }
    }

    public class Query
    {
        public float time { get; set; }
    }

    public class Facet
    {
        public float time { get; set; }
    }

    public class Facet_Module
    {
        public float time { get; set; }
    }

    public class Mlt
    {
        public float time { get; set; }
    }

    public class Highlight
    {
        public float time { get; set; }
    }

    public class Stats
    {
        public float time { get; set; }
    }

    public class Expand
    {
        public float time { get; set; }
    }

    public class Process
    {
        public float time { get; set; }
        public Query query { get; set; }
        public Facet facet { get; set; }
        public Facet_Module facet_module { get; set; }
        public Mlt mlt { get; set; }
        public Highlight highlight { get; set; }
        public Stats stats { get; set; }
        public Expand expand { get; set; }
        public Debug debug { get; set; }
    }
}
