using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Threading;
using TimeAide.Web.Models;

namespace TimeAide.AdminPanel.Helpers
{
    public class LogHelper
    {
        public TextBox TextBox
        {
            get;
            set;
        }
        public LogHelper( TextBox textBox)
        {
           TextBox = textBox;
        }
        public LogHelper(TimeAideContext context, TextBox textBox)
        {
            dbContext = context;
            TextBox = textBox;
        }
        public TimeAideContext dbContext { get; set; }
        public void DocumentMigrationLog(string logName, string logDescription,IProgress<string> logProcess)
        {
            var logText = (logName + ": " + logDescription).Trim() + "\n";
            //TextBox.AppendText((logName + ": " + logDescription).Trim() + "\n");
            logProcess?.Report(logText);
        }
        public void ImportFileLog(string logName, string logDescription, IProgress<string> logProcess)
        {
            var logText = (logName + ": " + logDescription).Trim() + "\n";
            ErrorLogHelper.InsertLog(ErrorLogType.Info, logDescription);
            //TextBox.AppendText((logName + ": " + logDescription).Trim() + "\n");
            logProcess?.Report(logText);
        }
        public DataMigrationLog LogProgress(int clientId, string logName, string logDescription, string logRemarks, int status)
        {
            if (logName == LogEvent.Exception.ToString())
            {
                TextBox.AppendText((logName + " " + logDescription).Trim() + "\n");
                //TextBox.Dispatcher.InvokeAsync(
                //new Action(() => TextBox.AppendText((logName + " " + logDescription).Trim() + "\n")));
            }
            else
            {
                TextBox.AppendText((status == 1 ? "" : "\t ####") + logName + " " + logDescription + " " + (status == 1 ? "Started" : "Completed").Trim() + "\n");

                //TextBox.Dispatcher.InvokeAsync(
                //new Action(() => TextBox.AppendText((status == 1 ? "" : "\t ####") + logName + " " + logDescription + " " + (status == 1 ? "Started" : "Completed").Trim() + "\n")));
            }
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
        public void LogProgressDetail(int clientId, string logCommandName, string logDescription, string logDetailName, DataMigrationLog dataMigrationLog, int rowCount)
        {
            TextBox.AppendText(("\t **** " + logCommandName + " " + logDescription).Trim() + "\n");
            //TextBox.Dispatcher.InvokeAsync(
            //    new Action(() => TextBox.AppendText(("\t **** " + logCommandName + " " + logDescription).Trim() + "\n")));
            try
            {
                DataMigrationLogDetail dataMigrationLogDetail = new DataMigrationLogDetail
                {
                    CreatedBy = 1,
                    ClientId = clientId,
                    CreatedDate = DateTime.Now,
                    DataEntryStatus = 1,
                    LogDescription = logDescription + " " + dataMigrationLog.LogName,
                   // DataMigrationLog = dataMigrationLog,
                    DataMigrationLogId = dataMigrationLog.Id,
                    LogCommandName = logCommandName,
                    LogDetailName = logDetailName,
                    RowCount = rowCount
                };
                dbContext.DataMigrationLogDetail.Add(dataMigrationLogDetail);
                dbContext.SaveChanges();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
            }

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
            return dbContext.DataMigrationLog.Where(c => c.LogRemarks == sourceDatabase && !string.IsNullOrEmpty(sourceDatabase)).ToList();
        }
    }
}
