using ElasticsearchAPI.Documents;
using ElasticsearchAPI.DTOs;
using ElasticsearchAPI.NestClient;
using Nest;
using Newtonsoft.Json;

namespace ElasticsearchAPI.Services
{
    /// <summary>
    /// Elasticsearch Service
    /// </summary>
    public class ElasticsearchService : IElasticsearchService
    {
        private readonly INestClientHandler _nestClientHandler;
        private readonly ElasticClient _elasticClient;

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="nestClientHandler"></param>
        public ElasticsearchService(INestClientHandler nestClientHandler) 
        {
            _nestClientHandler = nestClientHandler;
            _elasticClient = _nestClientHandler.CreateClient("[workreport]-*");
        }

        ///<inheritdoc/>
        public IEnumerable<BaseFields> Get(QueryDocumentDTO queryDTO, QueryBaseDTO queryBaseDTO)
        {
            var searchResponse = _elasticClient.Search<BaseFields>(s => s
            .Size(queryBaseDTO.Count)
            .From(queryBaseDTO.FromIndex)
            .Query(q =>
                q.DateRange(dr => dr
                    .Field(f => f.Timestamp)
                    .GreaterThanOrEquals(queryDTO.TimestampStart)
                    .LessThanOrEquals(queryDTO.TimestampEnd)) &&
                 q.Term(t => t
                    .Field(f => f.Level.Suffix("keyword"))
                    .Value(queryDTO.Level)) &&
                q.Match(m => m
                    .Field(f => f.Message)
                    .Query(queryDTO.Message))
            ));

            return searchResponse.Documents.ToList();
        }

        ///<inheritdoc/>
        public IEnumerable<BaseFields> GetByEventId(string eventId, QueryBaseDTO queryBaseDTO)
        {
            var searchResponse = _elasticClient.Search<BaseFields>(s => s
               .Size(queryBaseDTO.Count)
               .From(queryBaseDTO.FromIndex)
               .Query(q => q
                   .Term(t => t
                       .Field(f => f.Fields.EventID.Suffix("keyword"))
                       .Value(eventId)
           )));

           return searchResponse.Documents.ToList();
        }
    }
}
