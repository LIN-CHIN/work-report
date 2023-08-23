namespace ElasticsearchAPI.DTOs
{
    /// <summary>
    /// 查詢基底DTO
    /// </summary>
    public class QueryBaseDTO
    {
        /// <summary>
        /// 從第幾筆開始取得
        /// </summary>
        public int FromIndex { get; set; } = 0;

        /// <summary>
        /// 取得幾筆
        /// </summary>
        public int Count { get; set; } = 10;
    }
}
