using RecordWorker.DAOs;
using RecordWorker.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RecordWorker.Services.WorkReports
{
    /// <summary>
    /// 報工紀錄Service
    /// </summary>
    public class WorkReportRecordService : IWorkReportRecordService
    {
        private readonly IWorkReportRecordDAO _workReportRecordDAO;

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="workReportRecordDAO"></param>
        public WorkReportRecordService(IWorkReportRecordDAO workReportRecordDAO)
        {
            _workReportRecordDAO = workReportRecordDAO;
        }

        ///<inheritdoc/>
        public void Insert(WorkReportRecord workReportRecord)
        {
            _workReportRecordDAO.Insert(workReportRecord);
        }
    }
}
