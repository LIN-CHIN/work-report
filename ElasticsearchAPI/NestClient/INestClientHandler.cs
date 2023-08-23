using Nest;

namespace ElasticsearchAPI.NestClient
{
    /// <summary>
    /// NEST Client Handler Interface
    /// </summary>
    public interface INestClientHandler
    {
        /// <summary>
        /// 建立NEST Client
        /// </summary>
        /// <param name="defaultIndex">預設的Index</param>
        /// <returns></returns>
        ElasticClient CreateClient(string defaultIndex);
    }
}
