using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Website.Models.Database;

namespace Website.Models.ExplainViewModels
{
    public class ExplainListingViewModel
    {
        public IEnumerable<SolrQueryResponseRecord> RecordList { get; set; }
    }
}
