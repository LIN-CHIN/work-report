namespace WorkReportAPI.Services
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
