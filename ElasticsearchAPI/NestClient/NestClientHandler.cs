using ElasticsearchAPI.Settings;
using Nest;

namespace ElasticsearchAPI.NestClient
{
    /// <summary>
    /// NEST Client Handler
    /// </summary>
    public class NestClientHandler : INestClientHandler
    {
        private readonly ElasticSettings _elasticSettings; 

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="elasticSettings"></param>
        public NestClientHandler(ElasticSettings elasticSettings) 
        {
            _elasticSettings = elasticSettings;
        }

        ///<inheritdoc/>
        public ElasticClient CreateClient(string defaultIndex)
        {
            var settings = new ConnectionSettings(new Uri(_elasticSettings.Url))
                .DefaultIndex(defaultIndex);

            var client = new ElasticClient(settings);

            return client;
        }
    }
}
