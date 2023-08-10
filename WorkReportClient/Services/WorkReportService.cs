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
        private readonly HttpClient _httpClient;
        private readonly AppSettings _appSettings;

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="httpClient"></param>
        /// <param name="appSettings"></param>
        public WorkReportService(HttpClient httpClient, AppSettings appSettings) 
        {
            _httpClient = httpClient;
            _appSettings = appSettings;
        }

        ///<inheritdoc/>
        public void Report(ReportModel reportModel)
        {
            var requestMsg = new HttpRequestMessage(HttpMethod.Post, _appSettings.WorkReportUrl);
            var body = JsonConvert.SerializeObject(reportModel);
            requestMsg.Content = new StringContent(body, Encoding.UTF8, "application/json");

            // 發送請求
            var response = _httpClient.SendAsync(requestMsg).GetAwaiter().GetResult();

            // 成功
            if (response.StatusCode.ToString() != "OK")
            {
                throw new Exception("Call report api error");
            }
            else
            {
                Console.WriteLine("Work report completed");
            }
        }
    }
}
