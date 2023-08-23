using Nest;

namespace ElasticsearchAPI.NestClient
{
    /// <summary>
    /// NEST Client Handler
    /// </summary>
    public class NestClientHandler : INestClientHandler
    {
        ///<inheritdoc/>
        public ElasticClient CreateClient(string defaultIndex)
        {
            var settings = new ConnectionSettings(new Uri("http://localhost:9200"))
                .DefaultIndex(defaultIndex);

            var client = new ElasticClient(settings);

            return client;
        }
    }
}
