using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WorkReportClient.Services.WorkReports
{
    /// <summary>
    /// 報工Service Interface
    /// </summary>
    public interface IWorkReportService
    {
        /// <summary>
        /// 報工
        /// </summary>
        /// <param name="reportModel"></param>
        void Report(ReportModel reportModel);
    }
}
