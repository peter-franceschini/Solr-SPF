using System;
using System.Collections.Generic;
using Website.Builders;
using Website.Models.Explain;
using Website.Models.Database;
using Website.Repositories;

namespace Website.Services
{
    public class SolrResponseStorageService : ISolrResponseStorageService
    {
        private IGenericRepository<SolrQueryResponseRecord> SolrResponseRepository { get; set; }

        public SolrResponseStorageService(IGenericRepository<SolrQueryResponseRecord> solrResponseRepository)
        {
            SolrResponseRepository = solrResponseRepository;
        }

        public string SaveSolrQueryResponse(string rawJson, string userId)
        {
            var solrQueryResponseBuilder = new SolrQueryResponseBuilder(rawJson);
            var solrQueryResponse = solrQueryResponseBuilder.GetSolrQueryResponse();

            var solrQueryResponseRecord = new SolrQueryResponseRecord()
            {
                Guid = Guid.NewGuid().ToString("N"),
                CreateDateTime = DateTime.UtcNow,
                SolrQueryResponse = rawJson,
                SolrQuery = solrQueryResponse.Debug.querystring,
                UserId = userId
            };

            SolrResponseRepository.Insert(solrQueryResponseRecord);

            return solrQueryResponseRecord.Guid;
        }

        public IEnumerable<SolrQueryResponseRecord> GetSolrQueryResponseRecords(string userId)
        {
            return SolrResponseRepository.GetAll(s => s.UserId == userId);
        }

        public IEnumerable<SolrQueryResponseRecord> GetPublicSolrQueryResponseRecords()
        {
            return SolrResponseRepository.GetAll(s => s.User == null);
        }

        /// <summary>
        /// Gets and builds the SolrQueryResponse model from storage
        /// </summary>
        /// <returns></returns>
        public SolrQueryResponse GetSolrQueryResponseFromStorage(string guid)
        {
            var rawSolrQueryResponse = SolrResponseRepository.First(s => s.Guid == guid).SolrQueryResponse;
            var solrQueryResponseBuilder = new SolrQueryResponseBuilder(rawSolrQueryResponse);
            var solrQueryResponse = solrQueryResponseBuilder.GetSolrQueryResponse();
            return solrQueryResponse;
        }

        public SolrQueryResponseRecord GetSolrQueryResponseRecord(string guid)
        {
            return SolrResponseRepository.First(s => s.Guid == guid);
        }
        
    }
}
