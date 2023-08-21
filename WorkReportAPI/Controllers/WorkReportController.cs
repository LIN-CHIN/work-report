using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using WorkReportAPI.DTOs;
using WorkReportAPI.Services;

namespace WorkReportAPI.Controllers
{
    /// <summary>
    /// 報工Controller
    /// </summary>
    [Route("api/[controller]")]
    [ApiController]
    public class WorkReportController : ControllerBase
    {
        private readonly IWorkReportService _workReportService;

        public WorkReportController(IWorkReportService workReportService)
        {
            _workReportService = workReportService;
        }

        [HttpPost("test")]
        public IActionResult Test(Test test)
        {
            return Ok(new
            {
                Response = "回傳訊息",
                Message = "這是訊息",
                IsEnable = false
            }) ;
        }

        /// <summary>
        /// 報工
        /// </summary>
        /// <param name="reportModel"></param>
        /// <returns></returns>
        [HttpPost]
        public IActionResult Report(ReportDTO reportModel)
        {
            _workReportService.Report(reportModel);
            return Ok();
        }
    }
}

public class Test
{
    public string Id { get; set; }
    public string Name { get; set; }
}
