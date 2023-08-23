namespace ElasticsearchAPI.DTOs
{
    /// <summary>
    /// 查詢Document DTO
    /// </summary>
    public class QueryDocumentDTO
    {
        /// <summary>
        /// 日期時間(起)
        /// </summary>
        public DateTime? TimestampStart { get; set; }

        /// <summary>
        /// 日期時間(迄)
        /// </summary>
        public DateTime? TimestampEnd { get; set; }

        /// <summary>
        /// Log等級
        /// </summary>
        public string? Level { get; set; }

        /// <summary>
        /// 訊息
        /// </summary>
        public string? Message { get; set; }

    }
}
