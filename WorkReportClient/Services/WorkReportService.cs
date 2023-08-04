using Newtonsoft.Json;
using System.Net.Http;
using System.Text;

namespace WorkReportClient.Services
{
    /// <summary>
    /// 報工Service
    /// </summary>
    public class WorkReportService : IWorkReportService
    {
        const string REPORT_URL = "http://localhost:7001/api/WorkReport";
        private readonly HttpClient _httpClient;

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="httpClient"></param>
        public WorkReportService(HttpClient httpClient) 
        {
            _httpClient = httpClient;
        }

        ///<inheritdoc/>
        public void Report(ReportModel reportModel)
        {
            var requestMsg = new HttpRequestMessage(HttpMethod.Post, REPORT_URL);
            var body = JsonConvert.SerializeObject(reportModel);
            requestMsg.Content = new StringContent(body, Encoding.UTF8, "application/json");

            // 發送請求
            var response = _httpClient.SendAsync(requestMsg).GetAwaiter().GetResult();

            // 成功
            if (response.StatusCode.ToString() != "OK")
            {
                throw new Exception("Call remove api error");
            }
            else
            {
                Console.WriteLine("報工完成");
            }
        }
    }
}
