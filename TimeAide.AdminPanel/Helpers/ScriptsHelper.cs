using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using TimeAide.Web.Models;

namespace TimeAide.AdminPanel.Helpers
{
    public class ScriptsHelper
    {
        public ScriptsHelper(TimeAideContext context, LogHelper logHelper)
        {
            dbContext = context;
            LogHelper = logHelper;
        }

        private LogHelper LogHelper { get; set; }
        public TimeAideContext dbContext { get; set; }

        public bool CreateScripts(string folderName, int clientId, LogEvent logEvent, bool isNewDatabase, List<DataMigrationLog> databaseCreationLog, string sourceDatabase)
        {
            if (isNewDatabase || !LogHelper.IsAlreadyExecuted(databaseCreationLog, logEvent))
            {
                var log = LogHelper.GetLog(databaseCreationLog, logEvent, clientId);
                string[] fileEntries = Directory.GetFiles("Scripts\\" + folderName);
                foreach (string filePath in fileEntries)
                {
                    string fileName = Path.GetFileName(filePath);
                    try
                    {
                        int rowCount = 0;
                        //string sqlConnectionString = dbContext.Database.Connection.ConnectionString;
                        string sqlConnectionString = ConfigurationHelper.ConnectionString;
                        var sqlScript = File.ReadAllText(filePath).Replace("[TimeAideSource]", "[" + sourceDatabase + "]");
                        if (isNewDatabase || !log.DataMigrationLogDetail.Any(d => d.LogCommandName == fileName))
                        {
                            using (var conn = new SqlConnection(sqlConnectionString))
                            {
                                if (conn.State == System.Data.ConnectionState.Closed)
                                    conn.Open();

                                using (var command = new SqlCommand(sqlScript.ToString(), conn))
                                {
                                    rowCount = command.ExecuteNonQuery();
                                }
                            }

                            LogHelper.LogProgressDetail(clientId, fileName, "Exeuting " + filePath, "Exeuting " + filePath, log, rowCount);
                        }
                    }
                    catch (Exception ex)
                    {
                        StringBuilder exceptionMessage = new StringBuilder();
                        string stackTraceLog = ex.StackTrace.ToString();
                        if (stackTraceLog.Length > 2000)
                            stackTraceLog = stackTraceLog.Substring(0, 1990);

                        while (ex != null)
                        {
                            exceptionMessage.Append(ex.Message);
                            ex = ex.InnerException;
                        }

                        string exceptionMessageLog = exceptionMessage.ToString();
                        if (exceptionMessageLog.Length > 2000)
                            exceptionMessageLog = exceptionMessageLog.Substring(0, 1990);

                        log = LogHelper.LogProgress(clientId, LogEvent.Exception.ToString(), exceptionMessageLog, stackTraceLog, 1);
                        LogHelper.LogProgressDetail(clientId, "Exception in file " + fileName, "Exeuting " + filePath, "Exeuting " + filePath, log, 0);

                        MessageBox.Show("Process did not compelete successfully");
                        return false;
                    }
                }
                LogHelper.LogProgress(clientId, logEvent.ToString(), "", "", 2);
            }

            return true;
        }

        public bool CreateScripts(string folderName, int clientId, LogEvent logEvent, bool isNewDatabase, string sourceDatabase)
        {
            string[] fileEntries = Directory.GetFiles("Scripts\\" + folderName);
            foreach (string filePath in fileEntries)
            {
                string fileName = Path.GetFileName(filePath);
                try
                {
                    int rowCount = 0;
                    //string sqlConnectionString = dbContext.Database.Connection.ConnectionString;
                    string sqlConnectionString = ConfigurationHelper.ConnectionString;
                    var sqlScript = File.ReadAllText(filePath).Replace("[TimeAideSource]", "[" + sourceDatabase + "]");
                    using (var conn = new SqlConnection(sqlConnectionString))
                    {
                        if (conn.State == System.Data.ConnectionState.Closed)
                            conn.Open();

                        using (var command = new SqlCommand(sqlScript.ToString(), conn))
                        {
                            rowCount = command.ExecuteNonQuery();
                        }
                    }

                    //LogHelper.LogProgressDetail(clientId, fileName, "Exeuting " + filePath, "Exeuting " + filePath, log, rowCount);
                }
                catch (Exception ex)
                {
                    StringBuilder exceptionMessage = new StringBuilder();
                    string stackTraceLog = ex.StackTrace.ToString();
                    if (stackTraceLog.Length > 2000)
                        stackTraceLog = stackTraceLog.Substring(0, 1990);

                    while (ex != null)
                    {
                        exceptionMessage.Append(ex.Message);
                        ex = ex.InnerException;
                    }

                    string exceptionMessageLog = exceptionMessage.ToString();
                    if (exceptionMessageLog.Length > 2000)
                        exceptionMessageLog = exceptionMessageLog.Substring(0, 1990);

                    //log = LogHelper.LogProgress(clientId, LogEvent.Exception.ToString(), exceptionMessageLog, stackTraceLog, 1);
                    //LogHelper.LogProgressDetail(clientId, "Exception in file " + fileName, "Exeuting " + filePath, "Exeuting " + filePath, log, 0);

                    MessageBox.Show("Process did not compelete successfully");
                    return false;
                }
            }
            LogHelper.LogProgress(clientId, logEvent.ToString(), "", "", 2);

            return true;
        }

        public bool ExecuteDataMigrationSP(int clientId, string spName, DataMigrationLog log)
        {
            int rowCount = 0;
            try
            {
                //string sqlConnectionString = dbContext.Database.Connection.ConnectionString;
                string sqlConnectionString = ConfigurationHelper.ConnectionString;
                using (SqlConnection con = new SqlConnection(sqlConnectionString))
                {
                    using (SqlCommand cmd = new SqlCommand(spName, con))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.Add("@ClientId", SqlDbType.Int).Value = clientId;
                        if (spName == "sp_CreateDefaultDisability")
                        {
                            if (ConfigurationHelper.ExecutionType != "Data Migration")
                                cmd.Parameters.Add("@IsNewClient", SqlDbType.Int).Value = 1;
                            else
                                cmd.Parameters.Add("@IsNewClient", SqlDbType.Int).Value = 0;
                        }
                        con.Open();
                        rowCount = cmd.ExecuteNonQuery();
                    }
                }

                LogHelper.LogProgressDetail(1, spName, "Exeuting " + spName, spName, log, rowCount);

                return true;
            }
            catch (Exception ex)
            {
                StringBuilder exceptionMessage = new StringBuilder();
                string stackTraceLog = ex.StackTrace.ToString();
                if (stackTraceLog.Length > 2000)
                    stackTraceLog = stackTraceLog.Substring(0, 1990);

                while (ex != null)
                {
                    exceptionMessage.Append(ex.Message);
                    ex = ex.InnerException;
                }

                string exceptionMessageLog = exceptionMessage.ToString();
                if (exceptionMessageLog.Length > 2000)
                    exceptionMessageLog = exceptionMessageLog.Substring(0, 1990);

                log = LogHelper.LogProgress(clientId, LogEvent.Exception.ToString(), exceptionMessageLog, stackTraceLog, 1);
                LogHelper.LogProgressDetail(clientId, "Exception in executing sp " + spName, "Exeuting sp " + spName, "Exeuting " + spName, log, rowCount);

                MessageBox.Show("Process did not compelete successfully");
                return false;
            }
        }

        public bool ExecuteDataMigrationSP(string spName, DataMigrationLog log, bool isNewDatabase)
        {
            try
            {
                int rowCount = 0;
                if (isNewDatabase || !log.DataMigrationLogDetail.Any(d => d.LogCommandName == spName))
                {
                    // string sqlConnectionString = dbContext.Database.Connection.ConnectionString;
                    string sqlConnectionString = ConfigurationHelper.ConnectionString;
                    using (SqlConnection con = new SqlConnection(sqlConnectionString))
                    {
                        using (SqlCommand cmd = new SqlCommand(spName, con))
                        {
                            cmd.CommandType = CommandType.StoredProcedure;
                            con.Open();
                            rowCount = cmd.ExecuteNonQuery();
                        }
                    }
                    LogHelper.LogProgressDetail(1, spName, "Exeuting " + spName, "Exeuting " + spName, log, rowCount);
                }
                return true;
            }
            catch (Exception ex)
            {
                StringBuilder exceptionMessage = new StringBuilder();
                string stackTraceLog = ex.StackTrace.ToString();
                if (stackTraceLog.Length > 2000)
                    stackTraceLog = stackTraceLog.Substring(0, 1990);

                while (ex != null)
                {
                    exceptionMessage.Append(ex.Message);
                    ex = ex.InnerException;
                }

                string exceptionMessageLog = exceptionMessage.ToString();
                if (exceptionMessageLog.Length > 2000)
                    exceptionMessageLog = exceptionMessageLog.Substring(0, 1990);

                log = LogHelper.LogProgress(1, LogEvent.Exception.ToString(), exceptionMessageLog, stackTraceLog, 1);
                LogHelper.LogProgressDetail(1, "Exception in executing sp " + spName, "Exeuting sp " + spName, "Exeuting " + spName, log, 0);

                MessageBox.Show("Process did not compelete successfully");
                return false;
            }
        }

        public bool DropDataMigrationSP(int clientId, string spName, DataMigrationLog log)
        {
            try
            {
                int rowCount = 0;
                // string sqlConnectionString = dbContext.Database.Connection.ConnectionString;
                string sqlConnectionString = ConfigurationHelper.ConnectionString;

                using (SqlConnection con = new SqlConnection(sqlConnectionString))
                {
                    using (SqlCommand cmd = new SqlCommand("DROP PROCEDURE " + spName + "", con))
                    {
                        cmd.CommandType = CommandType.Text;
                        con.Open();
                        rowCount = cmd.ExecuteNonQuery();
                    }
                    LogHelper.LogProgressDetail(1, spName, "DROP SP " + spName, "DROP SP " + spName, log, rowCount);
                }
                return true;
            }
            catch (Exception ex)
            {
                StringBuilder exceptionMessage = new StringBuilder();
                string stackTraceLog = ex.StackTrace.ToString();
                if (stackTraceLog.Length > 2000)
                    stackTraceLog = stackTraceLog.Substring(0, 1990);

                while (ex != null)
                {
                    exceptionMessage.Append(ex.Message);
                    ex = ex.InnerException;
                }

                string exceptionMessageLog = exceptionMessage.ToString();
                if (exceptionMessageLog.Length > 2000)
                    exceptionMessageLog = exceptionMessageLog.Substring(0, 1990);

                log = LogHelper.LogProgress(1, LogEvent.Exception.ToString(), exceptionMessageLog, stackTraceLog, 1);
                LogHelper.LogProgressDetail(1, "Exception in DROP PROCEDURE " + spName, "DROP PROCEDURE " + spName, "Drop " + spName, log, 0);

                MessageBox.Show("Process did not compelete successfully");
                return false;
            }
        }
    }
}
