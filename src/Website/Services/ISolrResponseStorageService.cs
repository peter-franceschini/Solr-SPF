using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Website.Models.Database;
using Website.Models.Explain;

namespace Website.Services
{
    public interface ISolrResponseStorageService
    {
        string SaveSolrQueryResponse(string rawJson, string userId);
        IEnumerable<SolrQueryResponseRecord> GetSolrQueryResponseRecords(string userId);
        IEnumerable<SolrQueryResponseRecord> GetPublicSolrQueryResponseRecords();
        SolrQueryResponse GetSolrQueryResponseFromStorage(string guid);
        SolrQueryResponseRecord GetSolrQueryResponseRecord(string guid);
    }
}
