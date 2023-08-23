using Nest;

namespace ElasticsearchAPI.Documents
{
    /// <summary>
    /// Document 基本欄位
    /// </summary>
    public class BaseFields
    {
        [Date(Name = "@timestamp")]
        public DateTime Timestamp { get; set; }

        [Keyword(Name = "level")]
        public string Level { get; set; }

        [Text(Name = "messageTemplate")]
        public string MessageTemplate { get; set; }

        [Text(Name = "message")]
        public string Message { get; set; }

        [Nested(Name = "fields")]
        public Fields Fields { get; set; }
    }
}
