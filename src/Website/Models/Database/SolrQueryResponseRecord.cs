using System;
using System.Collections.Generic;

namespace Website.Models.Database
{
    public partial class SolrQueryResponseRecord
    {
        public int Id { get; set; }
        public string UserId { get; set; }
        public string Guid { get; set; }
        public DateTime? CreateDateTime { get; set; }
        public string SolrQueryResponse { get; set; }
        public string SolrQuery { get; set; }

        public AspNetUsers User { get; set; }
    }
}
