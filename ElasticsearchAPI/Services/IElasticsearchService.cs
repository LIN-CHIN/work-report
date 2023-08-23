using ElasticsearchAPI.Documents;
using ElasticsearchAPI.DTOs;

namespace ElasticsearchAPI.Services
{
    /// <summary>
    /// Elasticsearch Service Interface
    /// </summary>
    public interface IElasticsearchService
    {
        /// <summary>
        /// 查詢Document
        /// </summary>
        /// <param name="queryDTO"></param>
        /// <param name="queryBaseDTO"></param>
        /// <returns></returns>
        IEnumerable<BaseFields> Get(QueryDocumentDTO queryDTO, QueryBaseDTO queryBaseDTO);

        /// <summary>
        /// 根據事件id取得document
        /// </summary>
        /// <param name="eventId">事件id</param>
        /// <param name="queryBaseDTO">查詢用的基底DTO</param>
        /// <returns></returns>
        IEnumerable<BaseFields> GetByEventId(string eventId, QueryBaseDTO queryBaseDTO);
    }
}
