using Nest;

namespace ElasticsearchAPI.Documents
{
    /// <summary>
    /// Document中的 Fields 欄位
    /// </summary>
    public class Fields
    {
        [Keyword(Name = "SourceContext")]
        public string SourceContext { get; set; }

        [Keyword(Name = "EventID")]
        public string EventID { get; set; }
    }
}
