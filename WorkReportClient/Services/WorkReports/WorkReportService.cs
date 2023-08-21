using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using Serilog;
using System.Net.Http;
using System.Text;
using WorkReportClient.AppLogs;
using WorkReportClient.Services.LogServices;
using WorkReportClient.Settings;

namespace WorkReportClient.Services.WorkReports
{
    /// <summary>
    /// 報工Service
    /// </summary>
    public class WorkReportService : IWorkReportService
    {
        private readonly HttpClient _httpClient;
        private readonly AppSettings _appSettings;
        private readonly ILogService _logService;

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="httpClient"></param>
        /// <param name="appSettings"></param>
        /// <param name="logger"></param>
        public WorkReportService(HttpClient httpClient,
            AppSettings appSettings,
            ILogService logService)
        {
            _httpClient = httpClient;
            _appSettings = appSettings;
            _logService = logService;
        }

        ///<inheritdoc/>
        public void Report(ReportModel reportModel)
        {
            _logService.WriteInfoLog($"Call WorkReportAPI, URL = {_appSettings.WorkReportUrl}");
            var requestMsg = new HttpRequestMessage(HttpMethod.Post, _appSettings.WorkReportUrl);
            var body = JsonConvert.SerializeObject(reportModel);

            string eventId = Guid.NewGuid().ToString();
            requestMsg.Headers.Add("EventId", eventId.ToString());
            requestMsg.Content = new StringContent(body, Encoding.UTF8, "application/json");

            //紀錄Log
            _logService.WriteBody(eventId, LogMessageTypeEnum.Request, reportModel);

            // 發送請求
            var response = _httpClient.SendAsync(requestMsg).GetAwaiter().GetResult();

            // 成功
            if (response.StatusCode.ToString() != "OK")
            {
                _logService.WriteErrorLog("Call report api error");
                throw new Exception("Call report api error");
            }
            else
            {
                _logService.WriteInfoLog("Work report completed");
            }
        }
    }
}
