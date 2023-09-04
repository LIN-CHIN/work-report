using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using PublishWorker.DTOs.LineNotifyDTOs;
using PublishWorker.Services.LogServices;
using PublishWorker.Settings;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;

namespace PublishWorker.Services.LineNotifyServices
{
    /// <summary>
    /// Line Notify Service
    /// </summary>
    public class LineNotifyService : ILineNotifyService
    {
        private readonly HttpClient _httpClient;
        private readonly LineNotifySettings _lineNotifySettings;
        private readonly ILogService _logService;
        private readonly string _logMessagePrefix;

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="httpClient"></param>
        /// <param name="lineNotifySettings"></param>
        /// <param name="logService"></param>
        public LineNotifyService(HttpClient httpClient,
            LineNotifySettings lineNotifySettings,
            ILogService logService) 
        {
            _httpClient = httpClient;
            _lineNotifySettings = lineNotifySettings;
            _logService = logService;
            _logMessagePrefix = "Line Notify API call ";
        }

        ///<inheritdoc/>
        public PublishNotifyResponseDTO? Publish(PublishNotifyDTO publishNotifyDTO)
        {
            var requestMsg = new HttpRequestMessage(HttpMethod.Post, _lineNotifySettings.GetPublishUrl());
            
            var formData = new List<KeyValuePair<string, string>>
            {
                new KeyValuePair<string, string>("message", publishNotifyDTO.Message)
            };

            requestMsg.Content = new FormUrlEncodedContent(formData);
            requestMsg.Headers.Authorization = new AuthenticationHeaderValue("Bearer", _lineNotifySettings.AccessToken);
            
            //發送請求
            var response = _httpClient.SendAsync(requestMsg).GetAwaiter().GetResult();

            string result = response.Content.ReadAsStringAsync().Result.ToString();

            try
            {
                //成功
                if (response.StatusCode == HttpStatusCode.OK)
                {
                    _logService.WriteInfoLog($"{_logMessagePrefix}successful");
                    return JsonConvert.DeserializeObject<PublishNotifyResponseDTO>(result);
                }
                //Token無效
                else if (response.StatusCode == HttpStatusCode.Unauthorized)
                {
                    _logService.WriteErrorLog($"{_logMessagePrefix}failed. Error: 'Invalid access token'");
                    throw new Exception($"{_logMessagePrefix}failed. Error: 'Invalid access token'");
                }
                //其他錯誤
                else
                {
                    _logService.WriteErrorLog($"{_logMessagePrefix}failed. Error: Other Error ");
                    throw new Exception($"{_logMessagePrefix}failed. Error: Other Error, StatusCode = {response.StatusCode}");
                }
            }
            catch (Exception ex) 
            {
                throw new Exception($"{_logMessagePrefix}failed. Excepiton: {ex}");
            }
            
        }
    }
}
