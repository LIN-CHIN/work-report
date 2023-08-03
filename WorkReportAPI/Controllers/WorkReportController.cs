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
