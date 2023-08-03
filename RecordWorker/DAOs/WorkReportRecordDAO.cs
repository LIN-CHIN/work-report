using RecordWorker.Context;
using RecordWorker.Entities;

namespace RecordWorker.DAOs
{
    /// <summary>
    /// 報工紀錄DAO
    /// </summary>
    public class WorkReportRecordDAO : IWorkReportRecordDAO
    {
        private readonly DataContext _dataContext;
        public WorkReportRecordDAO(DataContext dataContext) 
        {
            _dataContext = dataContext; 
        }

        ///<inheritdoc/>
        public void Insert(WorkReportRecord workReportRecord)
        {
            _dataContext.Add(workReportRecord);
            _dataContext.SaveChanges();
        }
    }
}
