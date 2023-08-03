using RecordWorker.Entities;

namespace RecordWorker.DAOs
{
    /// <summary>
    /// 報工紀錄DAO Interface
    /// </summary>
    public interface IWorkReportRecordDAO
    {
        /// <summary>
        /// 新增報工紀錄
        /// </summary>
        /// <param name="workReportRecord"></param>
        void Insert(WorkReportRecord workReportRecord);
    }
}
