using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TimeAide.Web.Models;

namespace TimeAide.AdminConsole.Helpers
{
    public class LogHelper
    {
        public LogHelper(TimeAideContext context)
        {
            dbContext = context;
        }
        public TimeAideContext dbContext { get; set; }
        public DataMigrationLog LogProgress(int clientId, string logName, string logDescription, string logRemarks, int status)
        {
            if (logName == LogEvent.Exception.ToString())
                Console.WriteLine((logName + " " + logDescription).Trim());
            else
                Console.WriteLine(((status == 1 ? "" : "\t ####") + logName + " " + logDescription + " " + (status == 1 ? "Started" : "Completed")).Trim());
            DataMigrationLog dataMigrationLog = new DataMigrationLog
            {
                CreatedBy = 1,
                ClientId = clientId,
                CreatedDate = DateTime.Now,
                DataEntryStatus = status,
                LogName = logName,
                LogDescription = logDescription,
                LogRemarks = logRemarks,
            };
            dbContext.DataMigrationLog.Add(dataMigrationLog);
            dbContext.SaveChanges();

            return dataMigrationLog;

        }
        public void LogProgressDetail(int clientId, string logCommandName, string logDescription, string logDetailName, DataMigrationLog dataMigrationLog,int rowCount)
        {
            Console.WriteLine(("\t **** " + logCommandName + " " + logDescription).Trim());
            DataMigrationLogDetail dataMigrationLogDetail = new DataMigrationLogDetail
            {
                CreatedBy = 1,
                ClientId = clientId,
                CreatedDate = DateTime.Now,
                DataEntryStatus = 1,
                LogDescription = logDescription + " " + dataMigrationLog.LogName,
                DataMigrationLog = dataMigrationLog,
                LogCommandName = logCommandName,
                LogDetailName = logDetailName,
                RowCount = rowCount
            };
            dbContext.DataMigrationLogDetail.Add(dataMigrationLogDetail);
            dbContext.SaveChanges();

        }

        public List<DataMigrationLog> GetPreviousMigrationHistory(int clientId)
        {
            return dbContext.DataMigrationLog.Where(c => c.ClientId == clientId).ToList();
        }

        public bool IsAlreadyExecuted(List<DataMigrationLog> databaseCreationLog, LogEvent logEvent)
        {
            return databaseCreationLog != null &&
                   databaseCreationLog.Any(c => c.DataEntryStatus == 2 && c.LogName == logEvent.ToString());
        }

        public DataMigrationLog GetExecutedLog(int clientId, List<DataMigrationLog> databaseCreationLog, LogEvent logEvent, int status)
        {
            if (databaseCreationLog == null)
                return null;
            return databaseCreationLog.FirstOrDefault(c => c.DataEntryStatus == status && c.LogName == logEvent.ToString() && c.ClientId == clientId);
        }


        public DataMigrationLog GetLog(List<DataMigrationLog> databaseCreationLog, LogEvent logEvent, int clientId)
        {
            DataMigrationLog log = GetExecutedLog(clientId, databaseCreationLog, logEvent, 1);
            if (log == null)
                log = LogProgress(clientId, logEvent.ToString(), "", "", 1);
            return log;
        }
        public List<DataMigrationLog> SearchInMigratedDatabase(string sourceDatabase)
        {
            return dbContext.DataMigrationLog.Where(c => c.LogRemarks == sourceDatabase).ToList();
        }
    }
}
