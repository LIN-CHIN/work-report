using RecordWorker.Entities;

namespace RecordWorker.Services.WorkReports
{
    /// <summary>
    /// 報工紀錄Service Interface
    /// </summary>
    public interface IWorkReportRecordService
    {
        /// <summary>
        /// 新增報工紀錄
        /// </summary>
        /// <param name="workReportRecord"></param>
        void Insert(WorkReportRecord workReportRecord);
    }
}
