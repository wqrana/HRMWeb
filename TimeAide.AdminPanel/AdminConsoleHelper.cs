using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Shapes;
using TimeAide.AdminPanel.Helpers;
using TimeAide.Common.Helpers;
using TimeAide.Data;
using TimeAide.Models.ViewModel;
using TimeAide.Services;
using TimeAide.Web.Models;
using OfficeExcel= Microsoft.Office.Interop.Excel;
using DataTable = System.Data.DataTable;
using Microsoft.Office.Interop.Excel;
using System.Runtime.InteropServices;
using System.Drawing.Text;
using System.Collections.ObjectModel;
using TimeAide.AdminPanel.Models;
using Path = System.IO.Path;
using Syncfusion.XlsIO;
using System.Data.Entity.Core.EntityClient;

namespace TimeAide.AdminPanel
{
    public class AdminConsoleHelper
    {
        public AdminConsoleHelper(System.Windows.Controls.TextBox textBox)
        {
            DbContext = new TimeAideContext();
            LogHelper = new LogHelper(DbContext, textBox);
            ScriptsHelper = new ScriptsHelper(DbContext, LogHelper);
        }

        public String TargetDatabase { get; set; }
        public LogHelper LogHelper { get; set; }
        public ScriptsHelper ScriptsHelper { get; set; }
        public TimeAideContext DbContext { get; set; }
        public TimeAideWindowContext TAWindowDBContext { get;set; }      
        public IProgress<string> DocLogStatus { get; set; }
        public IProgress<int> DocMigProgress { get; set; }        
       
        public void UpdateContext()
        {
            LogHelper.dbContext = DbContext;
            ScriptsHelper.dbContext = DbContext;
        }
       
        public Client GetMigrationClient()
        {
            var clientName = ConfigurationHelper.ClientName;
            return DbContext.Client.FirstOrDefault(c => c.ClientName == clientName);
        }

        public Client GetClient(int clientId)
        {
            return DbContext.Client.FirstOrDefault(c => c.Id == clientId);
        }
        public Client CreateMigrationClient()
        {
            Client ClientInformation = new Client()
            {
                ClientName = ConfigurationHelper.ClientName,
                CreatedBy = 1
            };
            DbContext.Add<Client>(ClientInformation);
            DbContext.SaveChanges();
            return ClientInformation;
        }
        public UserInformation CreateSeedUser()
        {

            Client client = new Client()
            {
                ClientName = "Identech, Inc.",
                CreatedBy = 1,
                CreatedDate = DateTime.Now,
                DataEntryStatus = 1

            };
            DbContext.Client.Add(client);
            CreateCompany("TimeAide", client, 1);
            UserInformation userInformation = new UserInformation()
            {
                EmployeeId = 1,
                ShortFullName = ConfigurationHelper.DefaultShortFullName,
                ClientId = 1,
                CompanyId = 1

            };
            DbContext.UserInformation.Add(userInformation);
            EmployeeStatus employeeStatusActive = new EmployeeStatus()
            {
                Id = 1,
                EmployeeStatusName = "Active",
                ClientId = 1,
            };
            DbContext.EmployeeStatus.Add(employeeStatusActive);
            EmployeeStatus employeeStatusInactive = new EmployeeStatus()
            {
                Id = 2,
                EmployeeStatusName = "Inactive",
                ClientId = 1,
            };
            DbContext.EmployeeStatus.Add(employeeStatusInactive);
            EmployeeStatus employeeStatusClosed = new EmployeeStatus()
            {
                Id = 3,
                EmployeeStatusName = "Closed",
                ClientId = 1,
            };
            DbContext.EmployeeStatus.Add(employeeStatusClosed);

            EmployeeStatus employeeStatusCompanyTransfer = new EmployeeStatus()
            {
                Id = 4,
                EmployeeStatusName = "Company Transfer",
                ClientId = 1
            };
            DbContext.EmployeeStatus.Add(employeeStatusCompanyTransfer);

            string loginEmail = ConfigurationHelper.DefaultAdminEmail;
            UserContactInformation userContactInformation = new UserContactInformation()
            {
                LoginEmail = loginEmail
            };
            userInformation.UserContactInformations.Add(userContactInformation);
            //DbContext.DataMigrationUser = userInformation;
            DbContext.SaveChanges();


            return userInformation;
        }
        public Company CreateCompany(string companyName, Client client, int dataEntryStatus)
        {
            if (DbContext.Company.Any(c => c.CompanyName.ToLower() == companyName.ToLower() && c.ClientId == client.Id))
                return null;
            Company company = new Company()
            {
                CompanyName = companyName,
                Client = client,
                CreatedBy = 1,
                CreatedDate = DateTime.Now,
                DataEntryStatus = dataEntryStatus

            };
            DbContext.Company.Add(company);
            return company;
        }
        public TerminationType CreateTerminationType(string terminationTypeName, int clientId, int dataEntryStatus)
        {
            if (DbContext.TerminationType.Any(c => c.TerminationTypeName.ToLower() == terminationTypeName.ToLower() && c.ClientId != clientId))
                return null;
            TerminationType terminationType = new TerminationType()
            {
                TerminationTypeName = terminationTypeName,
                ClientId = clientId,
                CreatedBy = 1,
                CreatedDate = DateTime.Now,
                DataEntryStatus = dataEntryStatus

            };
            DbContext.TerminationType.Add(terminationType);
            return terminationType;
        }
        public TerminationReason CreateTerminationReason(string terminationReasonName, int clientId, int dataEntryStatus)
        {
            if (DbContext.TerminationReason.Any(c => c.TerminationReasonName.ToLower() == terminationReasonName.ToLower() && c.ClientId != clientId))
                return null;
            TerminationReason terminationReason = new TerminationReason()
            {
                TerminationReasonName = terminationReasonName,
                ClientId = clientId,
                CreatedBy = 1,
                CreatedDate = DateTime.Now,
                DataEntryStatus = dataEntryStatus

            };
            DbContext.TerminationReason.Add(terminationReason);
            return terminationReason;
        }
        public void CreateAddDBOwner()
        {
           // string sqlConnectionString = DbContext.Database.Connection.ConnectionString;
            string sqlConnectionString = ConfigurationHelper.ConnectionString;

            var sqlScript = "CREATE USER TimeAideDbAdmin FROM LOGIN TimeAideDbAdmin; EXEC sp_addrolemember 'db_owner', 'TimeAideDbAdmin'";

            using (var conn = new SqlConnection(sqlConnectionString))
            {
                if (conn.State == System.Data.ConnectionState.Closed)
                    conn.Open();

                using (var command = new SqlCommand(sqlScript.ToString(), conn))
                {
                    command.ExecuteNonQuery();
                }
            }
        }
        public bool IsValidSourceDatabase()
        {
            string sqlConnectionString = ConfigurationHelper.ConnectionString;

            using (var conn = new SqlConnection(sqlConnectionString))
            {
                try
                {
                    if (conn.State == System.Data.ConnectionState.Closed)
                        conn.Open();
                   
                }
                catch(Exception ex)
                {
                    return false;
                }
            }
            return true;
        }
        public int LoadFilesFromDatabase<T>(int clientId, string tableName, string binColumnNames, string fileNameColumn, string idColumn, string subFolderName, string sourceDatabase) where T : BaseEntity, new()
        {
            string folderPath = ConfigurationHelper.FilesDownloadPath;

            string sqlConnectionString = DbContext.Database.Connection.ConnectionString;
            SqlConnection con = new SqlConnection(sqlConnectionString);
            con.Open();
            string sql = "Select " + fileNameColumn + ", " + binColumnNames + "," + idColumn + " from " + sourceDatabase + ".dbo." + tableName + " where not " + binColumnNames + " is null";
            SqlCommand cmd = new SqlCommand(sql, con);
            SqlDataAdapter da = new SqlDataAdapter(cmd);
            DataSet ds = new DataSet();
            da.Fill(ds);
            if (ds.Tables[0].Rows.Count > 0)
            {
                foreach (DataRow each in ds.Tables[0].Rows)
                {
                    byte[] bytes = (byte[])each[binColumnNames];
                    MemoryStream memStream = new MemoryStream(bytes);
                    string fileName = each[fileNameColumn].ToString();
                    int companyId = 0;
                    string id = each[2].ToString();
                    int recordId;
                    Int32.TryParse(id, out recordId);
                    UserInformation employee = null;
                    Company company = null;
                    T entity = null;
                    if (tableName.ToLower() == "tblCompany".ToLower())
                    {
                        Int32.TryParse(id, out companyId);
                        company = DbContext.Company.FirstOrDefault(c => c.ClientId == clientId && c.Old_Id == companyId);
                        companyId = company.Id;
                    }
                    else if (tableName.ToLower() == "tblEmployee".ToLower())
                    {
                        employee = DbContext.UserInformation.FirstOrDefault(u => u.EmployeeId.HasValue && u.EmployeeId.ToString() == id && u.ClientId == clientId);
                        if (employee != null)
                        {
                            companyId = employee.CompanyId ?? 0;
                        }
                        else
                            companyId = 0;


                    }
                    else if (typeof(T).IsSubclassOf(typeof(BaseUserObjects)))
                    {
                        entity = DbContext.GetAll<T>(clientId).FirstOrDefault(e => e.Old_Id == recordId);
                        //var userInfo = DbContext.SP_UserInformationById<UserInformation>((entity as BaseUserObjects).UserInformationId ?? 0);
                        //var userRecord = DbContext.SP_UserInformation<UserInformationViewModel>(userInfo.EmployeeId??0, "", 0, 1, 1, SessionHelper.SelectedClientId, SessionHelper.SelectedCompanyId).FirstOrDefault();//UserInformationService.GetUserInformationViewModel(userInfo.EmployeeId ?? 0);
                        if (entity == null)
                        {
                            continue;
                        }
                        UserInformation userRecord = GetUserRecord(entity);
                        companyId = userRecord.CompanyId ?? 0;
                    }
                    if (!string.IsNullOrEmpty(fileName) && !fileName.Contains("."))
                        fileName = fileName + ".jpg";

                    FilePathHelper filePathHelper = new FilePathHelper(clientId, companyId, folderPath);
                    string path = "";

                    if (typeof(T).IsSubclassOf(typeof(BaseUserObjects)))
                    {
                        path = filePathHelper.GetPath(subFolderName, ref fileName, (entity as BaseUserObjects).UserInformationId ?? 0, entity.Id);
                        if ((entity as EmployeeDocument) != null)
                            (entity as EmployeeDocument).DocumentPath = path.Replace(filePathHelper.ApplicationPath, "");
                        else if ((entity as EmployeeCredential) != null)
                            (entity as EmployeeCredential).DocumentPath = path.Replace(filePathHelper.ApplicationPath, "");
                        else if ((entity as Employment) != null)
                            (entity as Employment).DocumentPath = path.Replace(filePathHelper.ApplicationPath, "");
                        else if ((entity as EmployeeDependent) != null)
                            (entity as EmployeeDependent).DocFilePath = path.Replace(filePathHelper.ApplicationPath, "");
                        else if ((entity as EmployeeAction) != null)
                            (entity as EmployeeAction).DocFilePath = path.Replace(filePathHelper.ApplicationPath, "");
                        else if ((entity as EmployeeEducation) != null)
                            (entity as EmployeeEducation).DocFilePath = path.Replace(filePathHelper.ApplicationPath, "");
                        else if ((entity as EmployeePerformance) != null)
                            (entity as EmployeePerformance).DocFilePath = path.Replace(filePathHelper.ApplicationPath, "");
                        else if ((entity as EmployeeTraining) != null)
                            (entity as EmployeeTraining).DocFilePath = path.Replace(filePathHelper.ApplicationPath, "");
                    }
                    else
                    {
                        path = filePathHelper.GetPath(subFolderName, fileName);
                        if (tableName.ToLower() == "tblEmployee".ToLower())
                        {
                            if (employee != null)
                            {
                                if (subFolderName == "resumes")
                                    employee.ResumeFilePath = path.Replace(filePathHelper.ApplicationPath, "");
                                else
                                    employee.PictureFilePath = path.Replace(filePathHelper.ApplicationPath, "");
                            }
                        }
                        else if (tableName.ToLower() == "tblCompany".ToLower())
                        {
                        }
                    }

                    File.WriteAllBytes(path, bytes);

                }
            }
            DbContext.SaveChanges();
            return ds.Tables[0].Rows.Count;
        }

        private UserInformation GetUserRecord<T>(T entity) where T : BaseEntity, new()
        {
            if (typeof(T) == typeof(EmployeeDocument))
            {
                return (entity as EmployeeDocument).UserInformation;
            }
            else if (typeof(T) == typeof(Employment))
            {
                return (entity as Employment).UserInformation;
            }
            else if (typeof(T) == typeof(EmployeeCredential))
            {
                return (entity as EmployeeCredential).UserInformation;
            }
            else
            {
                return DbContext.UserInformation.Find((entity as BaseUserObjects).UserInformationId);
            }
        }

        public DataTable GetInvalidHiringDate(string sourceDatabase)
        {
            //string sqlConnectionString = DbContext.Database.Connection.ConnectionString;
            string sqlConnectionString = ConfigurationHelper.ConnectionString;

            SqlConnection con = new SqlConnection(sqlConnectionString);
            con.Open();
            string sql = "Select [strEmployeeID],[strFirstName],[strMiddleInitial],[strLastName],[dtOriginalHireDate], [dtEffectiveHireDate], DATEDIFF(Day, [dtOriginalHireDate], [dtEffectiveHireDate]) DaysDiff " +
                         "from [TimeAideSource].[dbo].[tblEmployment] h " +
                         "Inner join [TimeAideSource].[dbo].[tblEmployee] e on h.strEmpID = e.strEmployeeID " +
                         "where[dtOriginalHireDate] > [dtEffectiveHireDate] ";
            sql = sql.Replace("[TimeAideSource]", "[" + sourceDatabase + "]");
            SqlCommand cmd = new SqlCommand(sql, con);
            SqlDataAdapter da = new SqlDataAdapter(cmd);
            DataSet ds = new DataSet();
            da.Fill(ds);
            if (ds.Tables[0].Rows.Count > 0)
            {
                return ds.Tables[0];
            }
            return null;
        }

        public void UpdateHiringDateWithRehiringDate(string sourceDatabase)
        {
            //string sqlConnectionString = DbContext.Database.Connection.ConnectionString;
            string sqlConnectionString = ConfigurationHelper.ConnectionString;

            var sqlScript = "Update [TimeAideSource].[dbo].[tblEmployment] Set [dtEffectiveHireDate] = [dtOriginalHireDate] where [dtOriginalHireDate] > [dtEffectiveHireDate];";

            sqlScript = sqlScript.Replace("[TimeAideSource]", "[" + sourceDatabase + "]");

            using (var conn = new SqlConnection(sqlConnectionString))
            {
                if (conn.State == System.Data.ConnectionState.Closed)
                    conn.Open();

                using (var command = new SqlCommand(sqlScript.ToString(), conn))
                {
                    command.ExecuteNonQuery();
                }
            }
        }

        public void UpdateReHiringDateWithHiringDate(string sourceDatabase)
        {
            //string sqlConnectionString = DbContext.Database.Connection.ConnectionString;
            string sqlConnectionString = ConfigurationHelper.ConnectionString;
            var sqlScript = "Update [TimeAideSource].[dbo].[tblEmployment] Set [dtOriginalHireDate] = [dtEffectiveHireDate] where [dtOriginalHireDate] > [dtEffectiveHireDate];";

            sqlScript = sqlScript.Replace("[TimeAideSource]", "[" + sourceDatabase + "]");

            using (var conn = new SqlConnection(sqlConnectionString))
            {
                if (conn.State == System.Data.ConnectionState.Closed)
                    conn.Open();

                using (var command = new SqlCommand(sqlScript.ToString(), conn))
                {
                    command.ExecuteNonQuery();
                }
            }
        }

        public bool DownloadDocuments(Client client, string sourceDatabase, AdminConsoleHelper adminConsoleHelper, bool isNewDatabase, List<DataMigrationLog> previousLog)
        {
            DataMigrationLog log = new DataMigrationLog();
            if (isNewDatabase || !adminConsoleHelper.LogHelper.IsAlreadyExecuted(previousLog, LogEvent.DownloadDocuments))
            {
                log = adminConsoleHelper.LogHelper.GetLog(previousLog, LogEvent.DownloadDocuments, client.Id);

                if (isNewDatabase || !log.DataMigrationLogDetail.Any(d => d.LogCommandName == LogDetailEvents.DownloadCompanyLogo.ToString()))
                {
                    int rowCount = 0;
                    rowCount = adminConsoleHelper.LoadFilesFromDatabase<Company>(client.Id, "tblCompany", "binCompanyLogo", "strCompany", "intID", "Company", sourceDatabase);
                    adminConsoleHelper.LogHelper.LogProgressDetail(1, LogDetailEvents.DownloadCompanyLogo.ToString(), "", "", log, rowCount);
                }
                if (isNewDatabase || !log.DataMigrationLogDetail.Any(d => d.LogCommandName == LogDetailEvents.DownloadEmployeeDependentDocument.ToString()))
                {
                    int rowCount = 0;
                    rowCount = adminConsoleHelper.LoadFilesFromDatabase<EmployeeDependent>(client.Id, "tblDependents", "docFile", "docName", "intID", "EmployeeDependents", sourceDatabase);
                    adminConsoleHelper.LogHelper.LogProgressDetail(1, LogDetailEvents.DownloadEmployeeDependentDocument.ToString(), "", "", log, rowCount);
                }
                if (isNewDatabase || !log.DataMigrationLogDetail.Any(d => d.LogCommandName == LogDetailEvents.DownloadEmployeePhotos.ToString()))
                {
                    int rowCount = 0;
                    rowCount = adminConsoleHelper.LoadFilesFromDatabase<UserInformation>(client.Id, "tblEmployee", "imgPhoto", "strEmployeeID", "strEmployeeID", "pictures", sourceDatabase);
                    adminConsoleHelper.LogHelper.LogProgressDetail(1, LogDetailEvents.DownloadEmployeePhotos.ToString(), "", "", log, rowCount);
                }
                if (isNewDatabase || !log.DataMigrationLogDetail.Any(d => d.LogCommandName == LogDetailEvents.DownloadEmployeeFiles.ToString()))
                {
                    int rowCount = 0;
                    rowCount = adminConsoleHelper.LoadFilesFromDatabase<UserInformation>(client.Id, "tblEmployee", "docFile", "docName", "strEmployeeID", "resumes", sourceDatabase);
                    adminConsoleHelper.LogHelper.LogProgressDetail(1, LogDetailEvents.DownloadEmployeeFiles.ToString(), "", "", log, rowCount);
                }
                if (isNewDatabase || !log.DataMigrationLogDetail.Any(d => d.LogCommandName == LogDetailEvents.DownloadEmployeeActionDocument.ToString()))
                {
                    int rowCount = 0;
                    rowCount = adminConsoleHelper.LoadFilesFromDatabase<EmployeeAction>(client.Id, "tblEmployeeAction", "docFile", "docName", "intID", "EmployeeAction", sourceDatabase);
                    adminConsoleHelper.LogHelper.LogProgressDetail(1, LogDetailEvents.DownloadEmployeeActionDocument.ToString(), "", "", log, rowCount);
                }
                if (isNewDatabase || !log.DataMigrationLogDetail.Any(d => d.LogCommandName == LogDetailEvents.DownloadEmployeeCredentialsDocument.ToString()))
                {
                    int rowCount = 0;
                    rowCount = adminConsoleHelper.LoadFilesFromDatabase<EmployeeCredential>(client.Id, "tblEmployeeCredentials", "docFile", "docName", "intID", "EmployeeCredentials", sourceDatabase);
                    adminConsoleHelper.LogHelper.LogProgressDetail(1, LogDetailEvents.DownloadEmployeeCredentialsDocument.ToString(), "", "", log, rowCount);
                }
                if (isNewDatabase || !log.DataMigrationLogDetail.Any(d => d.LogCommandName == LogDetailEvents.DownloadEmployeeDocument.ToString()))
                {
                    int rowCount = 0;
                    rowCount = adminConsoleHelper.LoadFilesFromDatabase<EmployeeDocument>(client.Id, "tblEmployeeDocument", "docFile", "docName", "intID", "EmployeeDocuments", sourceDatabase);
                    adminConsoleHelper.LogHelper.LogProgressDetail(1, LogDetailEvents.DownloadEmployeeDocument.ToString(), "", "", log, rowCount);
                }
                if (isNewDatabase || !log.DataMigrationLogDetail.Any(d => d.LogCommandName == LogDetailEvents.DownloadEmployeeEducationDocument.ToString()))
                {
                    int rowCount = 0;
                    rowCount = adminConsoleHelper.LoadFilesFromDatabase<EmployeeEducation>(client.Id, "tblEmployeeEducation", "docFile", "docName", "intID", "EmployeeEducation", sourceDatabase);
                    adminConsoleHelper.LogHelper.LogProgressDetail(1, LogDetailEvents.DownloadEmployeeEducationDocument.ToString(), "", "", log, rowCount);
                }
                if (isNewDatabase || !log.DataMigrationLogDetail.Any(d => d.LogCommandName == LogDetailEvents.DownloadEmployeePerformanceDocument.ToString()))
                {
                    int rowCount = 0;
                    rowCount = adminConsoleHelper.LoadFilesFromDatabase<EmployeePerformance>(client.Id, "tblEmployeePerformance", "docFile", "docName", "intID", "EmployeePerformance", sourceDatabase);
                    adminConsoleHelper.LogHelper.LogProgressDetail(1, LogDetailEvents.DownloadEmployeePerformanceDocument.ToString(), "", "", log, rowCount);
                }
                if (isNewDatabase || !log.DataMigrationLogDetail.Any(d => d.LogCommandName == LogDetailEvents.DownloadEmployeeTrainingDocument.ToString()))
                {
                    int rowCount = 0;
                    rowCount = adminConsoleHelper.LoadFilesFromDatabase<EmployeeTraining>(client.Id, "tblEmployeeTraining", "docFile", "docName", "intID", "EmployeeTraining", sourceDatabase);
                    adminConsoleHelper.LogHelper.LogProgressDetail(1, LogDetailEvents.DownloadEmployeeTrainingDocument.ToString(), "", "", log, rowCount);
                }
                if (isNewDatabase || !log.DataMigrationLogDetail.Any(d => d.LogCommandName == LogDetailEvents.DownloadEmploymentDocument.ToString()))
                {
                    int rowCount = 0;
                    rowCount = adminConsoleHelper.LoadFilesFromDatabase<Employment>(client.Id, "tblEmployment", "docFile", "docName", "intID", "Employment", sourceDatabase);
                    adminConsoleHelper.LogHelper.LogProgressDetail(1, LogDetailEvents.DownloadEmploymentDocument.ToString(), "", "", log, rowCount);
                }
                log = adminConsoleHelper.LogHelper.LogProgress(client.Id, LogEvent.DownloadDocuments.ToString(), "", "", 2);
            }

            return true;
        }
        private IList<string> GetExcelColumnsRange(int columnLength)
        {
            string[] alphabets = new string[] { "", "A", "B", "C", "D", "E", "F", "G", "H", "I", "J", "K", "L", "M", "N", "O", "P", "Q", "R", "S", "T", "U", "V", "W", "X", "Y", "Z" };
            IList<string> columnsList = new List<string>();
            for (int i = 0; i < columnLength; i++)
            {
                var column = alphabets[i / 26] + alphabets[(i % 26) + 1];
                columnsList.Add(column);
            }
            return columnsList;
        }

       
       
        private bool PrepareAndSaveExcelFile(string expDataTypeName, string sourceDatabase, DataTable dt,string fileSplitter,  AdminConsoleHelper adminConsoleHelper)
        {
            string logDescription = "";
            int blankRows = 0;
            string folderPath = ConfigurationHelper.AppRootPath;
            //string expDataTypeName = Enum.GetName(typeof(ExportDataType), expDataType)
            object misValue = System.Reflection.Missing.Value;
            OfficeExcel.Application excelApp = new OfficeExcel.Application();
            OfficeExcel.Workbook excelWorkbook = excelApp.Workbooks.Add(misValue);
            OfficeExcel.Worksheet excelWorksheet = (OfficeExcel.Worksheet)excelWorkbook.Worksheets.get_Item(1);

            //dt = ds.Tables[0];
            // dt.Rows
            //Header column name
            for (int i = 0; i < dt.Columns.Count; i++)
            {
                excelWorksheet.Cells[blankRows + 1, i + 1] = dt.Columns[i].ColumnName;
            }

            DocMigProgress?.Report(40);
            //Data table row data 
            var incRatio = dt.Rows.Count / 4;
            var incProgressStep = 10;
            for (int r = 0; r < dt.Rows.Count; r++)
            {
                for (int c = 0; c < dt.Columns.Count; c++)
                {
                    excelWorksheet.Cells[blankRows + 2 + r, c + 1] = dt.Rows[r][c].ToString();
                }
                if (incRatio > 0 && (r + 1) % incRatio == 0)
                {
                    DocMigProgress?.Report(40 + incProgressStep);
                    incProgressStep += 5;
                }

            }
            string[] cellRangeList = GetExcelColumnsRange(dt.Columns.Count).ToArray();
            var minHeaderCell = cellRangeList[0] + (blankRows + 1);
            var maxHeaderCell = cellRangeList[dt.Columns.Count - 1] + (blankRows + 1);
            var minDetailCell = cellRangeList[0] + (blankRows + 2);
            var maxDetailCell = cellRangeList[dt.Columns.Count - 1] + (dt.Rows.Count + 1);

            OfficeExcel.Range headerColumnRange = excelWorksheet.get_Range(minHeaderCell, maxHeaderCell);
            OfficeExcel.Range detailColumnRange = excelWorksheet.get_Range(minDetailCell, maxDetailCell);

            headerColumnRange.Font.Bold = true;
            headerColumnRange.Interior.Color = System.Drawing.ColorTranslator.ToOle(System.Drawing.Color.LightGray);
            headerColumnRange.Borders.Color = System.Drawing.ColorTranslator.ToOle(System.Drawing.Color.Black);

            detailColumnRange.Borders.Color = System.Drawing.ColorTranslator.ToOle(System.Drawing.Color.Black);

            excelWorksheet.Columns.AutoFit();
            excelWorksheet.Name = expDataTypeName;
            DocMigProgress?.Report(90);
            var fileName = $"{sourceDatabase}_{expDataTypeName}_{DateTime.Now.ToString("yyyyMMddhhmmss")}.xlsx";
            if (!string.IsNullOrEmpty(fileSplitter))            
                fileName = $"{sourceDatabase}_{expDataTypeName}_{fileSplitter}_{DateTime.Now.ToString("yyyyMMddhhmmss")}.xlsx";
            
            var filePath = folderPath.Substring(folderPath.Length - 1) == "\\" ? $"{folderPath}{fileName}" : $"{folderPath}\\{fileName}";
            //excelWorkbook.SaveAs(filePath);
            excelWorkbook.SaveAs(filePath, OfficeExcel.XlFileFormat.xlOpenXMLWorkbook, misValue, misValue, misValue, misValue, OfficeExcel.XlSaveAsAccessMode.xlExclusive, misValue, misValue, misValue, misValue, misValue);
            excelWorkbook.Close();
            excelApp.Quit();
            Marshal.ReleaseComObject(excelWorksheet);
            Marshal.ReleaseComObject(excelWorkbook);
            Marshal.ReleaseComObject(excelApp);

            logDescription = $"Saving Excel file on '{filePath}'.";
            adminConsoleHelper.LogHelper.DocumentMigrationLog(expDataTypeName, logDescription, DocLogStatus);

            return true;
        }
        public void ExportSourceData(ExportDataType expDataType, string sourceDatabase, AdminConsoleHelper adminConsoleHelper)
        {
            
            string expDataTypeName = Enum.GetName(typeof(ExportDataType), expDataType);
            string spName = ConfigurationHelper.ExportDataDBProc[expDataType];
            string sqlConnectionString = ConfigurationHelper.ConnectionString;
          
           
            DataSet ds = new DataSet();
            DataTable dt = null;          
            
            try
            {
                adminConsoleHelper.LogHelper.DocumentMigrationLog(expDataTypeName, "###Starting Export###", DocLogStatus);
                SqlConnection con = new SqlConnection(ConfigurationHelper.ConnectionString);
                adminConsoleHelper.LogHelper.DocumentMigrationLog(expDataTypeName, "Establishing Source Database Connection.", DocLogStatus);
                con.Open();
                DocMigProgress?.Report(10);
               
                SqlDataAdapter da = new SqlDataAdapter(spName, con);
                da.SelectCommand.CommandType = CommandType.StoredProcedure;
                da.SelectCommand.CommandTimeout = 100;
                adminConsoleHelper.LogHelper.DocumentMigrationLog(expDataTypeName, "Executing query for extracting data.", DocLogStatus);
                da.Fill(ds);

                DocMigProgress?.Report(30);
                int totalRecord = ds.Tables[0].Rows.Count;
                dt = ds.Tables[0];
                adminConsoleHelper.LogHelper.DocumentMigrationLog(expDataTypeName, $"Total Record(s) found: {totalRecord}", DocLogStatus);

                if (totalRecord > 0)
                {                    
                    adminConsoleHelper.LogHelper.DocumentMigrationLog(expDataTypeName, "Preparing data in excel format.", DocLogStatus);
                    if(expDataType == ExportDataType.CompanySetUpTA7 && totalRecord>1)
                    {
                        //Spliting data for multiple excel file                        
                        foreach(DataRow dr in dt.Rows) { 
                            var companyName = dr[1].ToString();
                            var companyFEIN = dr[26].ToString();
                            DataTable dtCmp = dt.AsEnumerable().Where(row => row[1].ToString() == companyName).CopyToDataTable();
                            adminConsoleHelper.LogHelper.DocumentMigrationLog(expDataTypeName, $"Preparing data for Compnay: {companyName}", DocLogStatus);
                            PrepareAndSaveExcelFile(expDataTypeName, sourceDatabase, dtCmp, companyFEIN, adminConsoleHelper);
                        }             

                    }
                    else
                    {
                        //single file
                        PrepareAndSaveExcelFile(expDataTypeName, sourceDatabase, dt, "", adminConsoleHelper);
                    }

                    DocMigProgress?.Report(100);
                    adminConsoleHelper.LogHelper.DocumentMigrationLog(expDataTypeName, "###Ending Export###", DocLogStatus);

                }


                else
            {
                adminConsoleHelper.LogHelper.DocumentMigrationLog(expDataTypeName, "No record found to export", DocLogStatus);
            }
        }
        catch (Exception ex)
        {
                adminConsoleHelper.LogHelper.DocumentMigrationLog(expDataTypeName, $"Error in exporting data:{ex.Message}", DocLogStatus);
        }
                  

        }
   
        public void DocumentMigration(Client client,DocumentActionType documentMigType ,string sourceDatabase, AdminConsoleHelper adminConsoleHelper)
        {
            switch(documentMigType)
            {
                case DocumentActionType.EmployeeDocuments:
                    MigrateEmployeeDocument(client.Id, sourceDatabase, adminConsoleHelper);
                    break;
                case DocumentActionType.EmployeeCredentials:
                    MigrateEmployeeCredential(client.Id, sourceDatabase, adminConsoleHelper);
                    break;
                case DocumentActionType.EmployeeEducations:
                    MigrateEmployeeEducation(client.Id, sourceDatabase, adminConsoleHelper);
                    break;
                case DocumentActionType.EmployeeTrainings:
                    MigrateEmployeeTraining(client.Id, sourceDatabase, adminConsoleHelper);
                    break;
                case DocumentActionType.EmployeePerformances:
                    MigrateEmployeePerformance(client.Id, sourceDatabase, adminConsoleHelper);
                    break;
                case DocumentActionType.EmploymentContracts:
                    MigrateEmploymentContract(client.Id, sourceDatabase, adminConsoleHelper);
                    break;
            }            


           
        }
        private int GetProgressPct(int totRecord, int currentRecord)
        {

            return (int)(((double)currentRecord / (double)totRecord) * 100);
        }
        private dynamic GetDocumentFetchSize(string sourceDataQuery)
        {
            string sqlConnectionString = ConfigurationHelper.ConnectionString;
            SqlConnection con = new SqlConnection(ConfigurationHelper.ConnectionString);
            con.Open();
            SqlCommand cmd = new SqlCommand(sourceDataQuery, con);
            var countObject = cmd.ExecuteScalar();
            con.Close();
            var totalRec = int.Parse(countObject.ToString());
            var pageSize = 1000;
            var noOfPage = (int)Math.Ceiling((double)totalRec / pageSize);
            return new { TotalRecord = totalRec, NoOfPage = noOfPage, PageSize = pageSize };
        }
        public void MigrateEmployeeDocument(int clientId, string sourceDatabase, AdminConsoleHelper adminConsoleHelper) 
        {
            string logDescription = "";
            string folderPath = ConfigurationHelper.AppRootPath;
            //string sqlConnectionString = DbContext.Database.Connection.ConnectionString;
            string sqlConnectionString = ConfigurationHelper.ConnectionString;
            SqlConnection con = new SqlConnection(ConfigurationHelper.ConnectionString);
            con.Open();          
            string query = "Select intID, strEmpID, docFile, docName " + 
                         " From " + sourceDatabase + ".dbo.tblEmployeeDocument" +
                         " Where docFile is not null";

            SqlCommand cmd = new SqlCommand(query, con);
            SqlDataAdapter da = new SqlDataAdapter(cmd);
          
            DataSet ds = new DataSet();
            da.Fill(ds);
            int totalRecord = ds.Tables[0].Rows.Count;
            int currRecord = 0;
            int currProgressPct = 0;
           adminConsoleHelper.LogHelper.DocumentMigrationLog("EmployeeDocument", "###Starting Migration###", DocLogStatus);
            if (totalRecord > 0)
            {
                foreach (DataRow dataRec in ds.Tables[0].Rows)
                {
                    currRecord++;

                    if(currProgressPct<GetProgressPct(totalRecord, currRecord)){
                        currProgressPct++;
                        if (currProgressPct <= 100)
                        {
                            DocMigProgress?.Report(currProgressPct);
                        }
                    }
                    byte[] bytes = (byte[])dataRec["docFile"];
                    MemoryStream memStream = new MemoryStream(bytes);
                    string fileName = dataRec["docName"].ToString();
                    int companyId = 0;
                    string idStr = dataRec[0].ToString();
                    string empIdStr = dataRec[1].ToString();
                    int recordId,employeeId;
                    Int32.TryParse(idStr, out recordId);
                    Int32.TryParse(empIdStr, out employeeId);
                    
                    logDescription = $"Migrating (employee_id:{empIdStr}, document_id:{idStr}, file_name:{fileName})";
                    adminConsoleHelper.LogHelper.DocumentMigrationLog("EmployeeDocument", logDescription, DocLogStatus);
                    var documentEntity = DbContext.GetAll<EmployeeDocument>(clientId).FirstOrDefault(e => e.Old_Id == recordId);
                    if (documentEntity == null)
                    {
                        adminConsoleHelper.LogHelper.DocumentMigrationLog("EmployeeDocument", "Record is not found in target db", DocLogStatus); 
                        continue;
                    }
                    else if (!string.IsNullOrEmpty(documentEntity.DocumentPath))
                    {
                        adminConsoleHelper.LogHelper.DocumentMigrationLog("EmployeeDocument",  "Record file is already migrated/updated in target db", DocLogStatus); ;
                        continue;
                    }
                    UserInformation userRecord = GetUserRecord(documentEntity);
                    companyId = userRecord.CompanyId ?? 0;
                    try
                    {
                        FilePathHelper filePathHelper = new FilePathHelper(clientId, companyId, folderPath);
                        string docName = fileName;
                        string serverFilePath = filePathHelper.GetPath("EmployeeDocuments", ref docName, userRecord.Id, documentEntity.Id);
                        documentEntity.DocumentName = fileName;
                        documentEntity.DocumentPath = filePathHelper.RelativePath;
                        File.WriteAllBytes(serverFilePath, bytes);
                        DbContext.SaveChanges();
                    }
                    catch (Exception ex)
                    {
                        adminConsoleHelper.LogHelper.DocumentMigrationLog("EmployeeDocument", $"Error in migrating record file:{ex.Message}", DocLogStatus);
                    }
                }
                DocMigProgress?.Report(100);

            }
            else
            {
               adminConsoleHelper.LogHelper.DocumentMigrationLog("EmployeeDocument", "No record found for migration", DocLogStatus);
            }
            adminConsoleHelper.LogHelper.DocumentMigrationLog("EmployeeDocument", "###Ending Migration###", DocLogStatus);

        }
        
        public void MigrateEmployeeCredential(int clientId, string sourceDatabase, AdminConsoleHelper adminConsoleHelper)
        {
            string logDescription = "";
            string folderPath = ConfigurationHelper.AppRootPath;
            //string sqlConnectionString = DbContext.Database.Connection.ConnectionString;
            string sqlConnectionString = ConfigurationHelper.ConnectionString;
            SqlConnection con = new SqlConnection(ConfigurationHelper.ConnectionString);
           
            string queryRecCount = $" Select count(intID)" +
                                   $" From {sourceDatabase}.dbo.tblEmployeeCredentials" +
                                   $" Where docFile is not null ";

            var fetchSize= GetDocumentFetchSize(queryRecCount);
            var pageCount = fetchSize.NoOfPage;
            var pageSize = fetchSize.PageSize;
            int totalRecord = fetchSize.TotalRecord;
            int currRecord = 0;
            int currProgressPct = 0;
            adminConsoleHelper.LogHelper.DocumentMigrationLog("EmployeeCredential", "###Starting Migration###", DocLogStatus);
            if (totalRecord > 0)
            {
                var targentdocumentList = DbContext.GetAll<EmployeeCredential>(clientId);
                for (int page = 0; page < pageCount; page++)
                {

                    var offsetCount = page * pageSize;

                    string sourceQuery = $"Select intID, strEmpID, docName,docFile" +
                                       $" From {sourceDatabase}.dbo.tblEmployeeCredentials" +
                                       $" Where docFile is not null" +
                                       $" Order by intID" +
                                       $" OFFSET {offsetCount} ROWS" +
                                       $" FETCH NEXT {pageSize} ROWS ONLY";

                    con.Open();
                    SqlCommand cmd = new SqlCommand(sourceQuery, con);
                    SqlDataAdapter da = new SqlDataAdapter(cmd);
                    DataSet ds = new DataSet();
                    da.Fill(ds);
                    con.Close();
                    int totalFetchRecords = ds.Tables[0].Rows.Count;

                    if (totalFetchRecords > 0)
                    {
                        try
                        {
                            foreach (DataRow dataRec in ds.Tables[0].Rows)
                            {
                                currRecord++;

                                if (currProgressPct < GetProgressPct(totalRecord, currRecord))
                                {
                                    currProgressPct++;
                                    if (currProgressPct <= 100)
                                    {
                                        DocMigProgress?.Report(currProgressPct);
                                    }
                                }
                                byte[] bytes = (byte[])dataRec["docFile"];
                                MemoryStream memStream = new MemoryStream(bytes);
                                string fileName = dataRec["docName"].ToString();
                                int companyId = 0;
                                string idStr = dataRec[0].ToString();
                                string empIdStr = dataRec[1].ToString();
                                int recordId, employeeId;
                                Int32.TryParse(idStr, out recordId);
                                Int32.TryParse(empIdStr, out employeeId);

                                logDescription = $"Migrating (employee_id:{empIdStr}, credential_id:{idStr}, file_name:{fileName})";
                                adminConsoleHelper.LogHelper.DocumentMigrationLog("EmployeeCredential", logDescription, DocLogStatus);
                                //var documentEntity = DbContext.GetAll<EmployeeCredential>(clientId).FirstOrDefault(e => e.Old_Id == recordId);
                                var documentEntity = targentdocumentList.FirstOrDefault(e => e.Old_Id == recordId);
                                if (documentEntity == null)
                                {
                                    adminConsoleHelper.LogHelper.DocumentMigrationLog("EmployeeCredential", "Record is not found in target db", DocLogStatus);
                                    continue;
                                }
                                else if (!string.IsNullOrEmpty(documentEntity.DocumentPath))
                                {
                                    adminConsoleHelper.LogHelper.DocumentMigrationLog("EmployeeCredential", "Record file is already migrated/updated in target db", DocLogStatus); ;
                                    continue;
                                }
                                UserInformation userRecord = GetUserRecord(documentEntity);
                                companyId = userRecord.CompanyId ?? 0;
                                try
                                {
                                    FilePathHelper filePathHelper = new FilePathHelper(clientId, companyId, folderPath);
                                    string docName = fileName;
                                    string serverFilePath = filePathHelper.GetPath("EmployeeCredentialDocs", ref docName, userRecord.Id, documentEntity.Id);
                                    documentEntity.DocumentName = fileName;
                                    documentEntity.DocumentPath = filePathHelper.RelativePath;
                                    File.WriteAllBytes(serverFilePath, bytes);
                                    DbContext.SaveChanges();
                                }
                                catch (Exception ex)
                                {
                                    adminConsoleHelper.LogHelper.DocumentMigrationLog("EmployeeCredential", $"Error in migrating record file:{ex.Message}", DocLogStatus);
                                }
                            }

                           
                        }
                        catch(Exception ex)
                        {
                            adminConsoleHelper.LogHelper.DocumentMigrationLog("EmployeeCredential", $"Error in saving record(s) info:{ex.Message}", DocLogStatus);
                        }

                    }
                }
                DocMigProgress?.Report(100);
            }
            else
            {
                adminConsoleHelper.LogHelper.DocumentMigrationLog("EmployeeCredential", "No record found for migration", DocLogStatus);
            }
            adminConsoleHelper.LogHelper.DocumentMigrationLog("EmployeeCredential", "###Ending Migration###", DocLogStatus);

        }
        public void MigrateEmployeeEducation(int clientId, string sourceDatabase, AdminConsoleHelper adminConsoleHelper)
        {
            string logDescription = "";
            string folderPath = ConfigurationHelper.AppRootPath;
           // string sqlConnectionString = DbContext.Database.Connection.ConnectionString;
            string sqlConnectionString = ConfigurationHelper.ConnectionString;
            SqlConnection con = new SqlConnection(ConfigurationHelper.ConnectionString);
            con.Open();
            string query = "Select intID, strEmpID, docFile, docName " +
                         " From " + sourceDatabase + ".dbo.tblEmployeeEducation" +
                         " Where docFile is not null";

            SqlCommand cmd = new SqlCommand(query, con);
            SqlDataAdapter da = new SqlDataAdapter(cmd);
            DataSet ds = new DataSet();
            da.Fill(ds);
            int totalRecord = ds.Tables[0].Rows.Count;
            int currRecord = 0;
            int currProgressPct = 0;
            adminConsoleHelper.LogHelper.DocumentMigrationLog("EmployeeEducation", "###Starting Migration###", DocLogStatus);
            if (totalRecord > 0)
            {
                foreach (DataRow dataRec in ds.Tables[0].Rows)
                {
                    currRecord++;

                    if (currProgressPct < GetProgressPct(totalRecord, currRecord))
                    {
                        currProgressPct++;
                        if (currProgressPct <= 100)
                        {
                            DocMigProgress?.Report(currProgressPct);
                        }
                    }
                    byte[] bytes = (byte[])dataRec["docFile"];
                    MemoryStream memStream = new MemoryStream(bytes);
                    string fileName = dataRec["docName"].ToString();
                    int companyId = 0;
                    string idStr = dataRec[0].ToString();
                    string empIdStr = dataRec[1].ToString();
                    int recordId, employeeId;
                    Int32.TryParse(idStr, out recordId);
                    Int32.TryParse(empIdStr, out employeeId);

                    logDescription = $"Migrating (employee_id:{empIdStr}, record_id:{idStr}, file_name:{fileName})";
                    adminConsoleHelper.LogHelper.DocumentMigrationLog("EmployeeEducation", logDescription, DocLogStatus);
                    var documentEntity = DbContext.GetAll<EmployeeEducation>(clientId).FirstOrDefault(e => e.Old_Id == recordId);
                    if (documentEntity == null)
                    {
                        adminConsoleHelper.LogHelper.DocumentMigrationLog("EmployeeEducation", "Record is not found in target db", DocLogStatus);
                        continue;
                    }
                    else if (!string.IsNullOrEmpty(documentEntity.DocFilePath))
                    {
                        adminConsoleHelper.LogHelper.DocumentMigrationLog("EmployeeEducation", "Record file is already migrated/updated in target db", DocLogStatus); ;
                        continue;
                    }
                    UserInformation userRecord = GetUserRecord(documentEntity);
                    companyId = userRecord.CompanyId ?? 0;
                    try
                    {
                        FilePathHelper filePathHelper = new FilePathHelper(clientId, companyId, folderPath);
                        string docName = fileName;
                        string serverFilePath = filePathHelper.GetPath("educationDocs", ref docName, userRecord.Id, documentEntity.Id);
                        documentEntity.DocName = fileName;
                        documentEntity.DocFilePath = filePathHelper.RelativePath;
                        File.WriteAllBytes(serverFilePath, bytes);
                        DbContext.SaveChanges();
                    }
                    catch (Exception ex)
                    {
                        adminConsoleHelper.LogHelper.DocumentMigrationLog("EmployeeEducation", $"Error in migrating record file:{ex.Message}", DocLogStatus);
                    }
                }
                DocMigProgress?.Report(100);

            }
            else
            {
                adminConsoleHelper.LogHelper.DocumentMigrationLog("EmployeeEducation", "No record found for migration", DocLogStatus);
            }
            adminConsoleHelper.LogHelper.DocumentMigrationLog("EmployeeCredential", "###Ending Migration###", DocLogStatus);

        }
        public void MigrateEmployeeTraining(int clientId, string sourceDatabase, AdminConsoleHelper adminConsoleHelper)
        {
            string logDescription = "";
            string folderPath = ConfigurationHelper.AppRootPath;
            //string sqlConnectionString = DbContext.Database.Connection.ConnectionString;
            string sqlConnectionString = ConfigurationHelper.ConnectionString;
            SqlConnection con = new SqlConnection(ConfigurationHelper.ConnectionString);

            string queryRecCount = $" Select count(intID)" +
                                   $" From {sourceDatabase}.dbo.tblEmployeeTraining" +
                                   $" Where docFile is not null ";

            var fetchSize = GetDocumentFetchSize(queryRecCount);
            var pageCount = fetchSize.NoOfPage;
            var pageSize = fetchSize.PageSize;
            int totalRecord = fetchSize.TotalRecord;
            int currRecord = 0;
            int currProgressPct = 0;
            adminConsoleHelper.LogHelper.DocumentMigrationLog("EmployeeTraining", "###Starting Migration###", DocLogStatus);
            if (totalRecord > 0)
            {
                var targetdocumentList = DbContext.GetAll<EmployeeTraining>(clientId);
                for (int page = 0; page < pageCount; page++)
                {

                    var offsetCount = page * pageSize;

                    string sourceQuery = $"Select intID, strEmpID, docName,docFile" +
                                       $" From {sourceDatabase}.dbo.tblEmployeeTraining" +
                                       $" Where docFile is not null" +
                                       $" Order by intID" +
                                       $" OFFSET {offsetCount} ROWS" +
                                       $" FETCH NEXT {pageSize} ROWS ONLY";

                    con.Open();
                    SqlCommand cmd = new SqlCommand(sourceQuery, con);
                    SqlDataAdapter da = new SqlDataAdapter(cmd);
                    DataSet ds = new DataSet();
                    da.Fill(ds);
                    con.Close();
                    int totalFetchRecords = ds.Tables[0].Rows.Count;

                    if (totalFetchRecords > 0)
                    {
                        try
                        {

                            foreach (DataRow dataRec in ds.Tables[0].Rows)
                            {
                                currRecord++;

                                if (currProgressPct < GetProgressPct(totalRecord, currRecord))
                                {
                                    currProgressPct++;
                                    if (currProgressPct <= 100)
                                    {
                                        DocMigProgress?.Report(currProgressPct);
                                    }
                                }
                                byte[] bytes = (byte[])dataRec["docFile"];
                                MemoryStream memStream = new MemoryStream(bytes);
                                string fileName = dataRec["docName"].ToString();
                                int companyId = 0;
                                string idStr = dataRec[0].ToString();
                                string empIdStr = dataRec[1].ToString();
                                int recordId, employeeId;
                                Int32.TryParse(idStr, out recordId);
                                Int32.TryParse(empIdStr, out employeeId);

                                logDescription = $"Migrating (employee_id:{empIdStr}, training_id:{idStr}, file_name:{fileName})";
                                adminConsoleHelper.LogHelper.DocumentMigrationLog("EmployeeTraining", logDescription, DocLogStatus);
                                //var documentEntity = DbContext.GetAll<EmployeeTraining>(clientId).FirstOrDefault(e => e.Old_Id == recordId);
                                var documentEntity = targetdocumentList.FirstOrDefault(e => e.Old_Id == recordId);
                                if (documentEntity == null)
                                {
                                    adminConsoleHelper.LogHelper.DocumentMigrationLog("EmployeeTraining", "Record is not found in target db", DocLogStatus);
                                    continue;
                                }
                                else if (!string.IsNullOrEmpty(documentEntity.DocFilePath))
                                {
                                    adminConsoleHelper.LogHelper.DocumentMigrationLog("EmployeeTraining", "Record file is already migrated/updated in target db", DocLogStatus); ;
                                    continue;
                                }
                                UserInformation userRecord = GetUserRecord(documentEntity);
                                companyId = userRecord.CompanyId ?? 0;
                                try
                                {
                                    FilePathHelper filePathHelper = new FilePathHelper(clientId, companyId, folderPath);
                                    string docName = fileName;
                                    string serverFilePath = filePathHelper.GetPath("trainingDocs", ref docName, userRecord.Id, documentEntity.Id);
                                    documentEntity.DocName = fileName;
                                    documentEntity.DocFilePath = filePathHelper.RelativePath;
                                    File.WriteAllBytes(serverFilePath, bytes);
                                    DbContext.SaveChanges();
                                }
                                catch (Exception ex)
                                {
                                    adminConsoleHelper.LogHelper.DocumentMigrationLog("EmployeeTraining", $"Error in migrating record file:{ex.Message}", DocLogStatus);
                                }
                            }
                           
                        }
                        catch(Exception ex)
                        {
                            adminConsoleHelper.LogHelper.DocumentMigrationLog("EmployeeTraining", $"Error in migrating record(s) saving info:{ex.Message}", DocLogStatus);
                        }
                    }
                }
                DocMigProgress?.Report(100);
            }
            else
            {
                adminConsoleHelper.LogHelper.DocumentMigrationLog("EmployeeTraining", "No record found for migration", DocLogStatus);
            }
            adminConsoleHelper.LogHelper.DocumentMigrationLog("EmployeeTraining", "###Ending Migration###", DocLogStatus);

        }
        public void MigrateEmployeePerformance(int clientId, string sourceDatabase, AdminConsoleHelper adminConsoleHelper)
        {
            string logDescription = "";
            string folderPath = ConfigurationHelper.AppRootPath;
            //string sqlConnectionString = DbContext.Database.Connection.ConnectionString;
            string sqlConnectionString = ConfigurationHelper.ConnectionString;
            SqlConnection con = new SqlConnection(ConfigurationHelper.ConnectionString);

            string queryRecCount = $" Select count(intID)" +
                                   $" From {sourceDatabase}.dbo.tblEmployeePerformance" +
                                   $" Where docFile is not null ";

            var fetchSize = GetDocumentFetchSize(queryRecCount);
            var pageCount = fetchSize.NoOfPage;
            var pageSize = fetchSize.PageSize;
            int totalRecord = fetchSize.TotalRecord;
            int currRecord = 0;
            int currProgressPct = 0;
            adminConsoleHelper.LogHelper.DocumentMigrationLog("EmployeePerformance", "###Starting Migration###", DocLogStatus);
            if (totalRecord > 0)
            {
                var targetDocumentList = DbContext.GetAll<EmployeePerformance>(clientId);
                for (int page = 0; page < pageCount; page++)
                {

                    var offsetCount = page * pageSize;

                    string sourceQuery = $"Select intID, strEmpID, docName,docFile" +
                                       $" From {sourceDatabase}.dbo.tblEmployeePerformance" +
                                       $" Where docFile is not null" +
                                       $" Order by intID" +
                                       $" OFFSET {offsetCount} ROWS" +
                                       $" FETCH NEXT {pageSize} ROWS ONLY";

                    con.Open();
                    SqlCommand cmd = new SqlCommand(sourceQuery, con);
                    SqlDataAdapter da = new SqlDataAdapter(cmd);
                    DataSet ds = new DataSet();
                    da.Fill(ds);
                    con.Close();
                    int totalFetchRecords = ds.Tables[0].Rows.Count;

                    if (totalFetchRecords > 0)
                    {
                        try
                        {

                            foreach (DataRow dataRec in ds.Tables[0].Rows)
                            {
                                currRecord++;

                                if (currProgressPct < GetProgressPct(totalRecord, currRecord))
                                {
                                    currProgressPct++;
                                    if (currProgressPct <= 100)
                                    {
                                        DocMigProgress?.Report(currProgressPct);
                                    }
                                }
                                byte[] bytes = (byte[])dataRec["docFile"];
                                MemoryStream memStream = new MemoryStream(bytes);
                                string fileName = dataRec["docName"].ToString();
                                int companyId = 0;
                                string idStr = dataRec[0].ToString();
                                string empIdStr = dataRec[1].ToString();
                                int recordId, employeeId;
                                Int32.TryParse(idStr, out recordId);
                                Int32.TryParse(empIdStr, out employeeId);

                                logDescription = $"Migrating (employee_id:{empIdStr}, record_id:{idStr}, file_name:{fileName})";
                                adminConsoleHelper.LogHelper.DocumentMigrationLog("EmployeePerformance", logDescription, DocLogStatus);
                                //var documentEntity = DbContext.GetAll<EmployeeTraining>(clientId).FirstOrDefault(e => e.Old_Id == recordId);
                                var documentEntity = targetDocumentList.FirstOrDefault(e => e.Old_Id == recordId);
                                if (documentEntity == null)
                                {
                                    adminConsoleHelper.LogHelper.DocumentMigrationLog("EmployeePerformance", "Record is not found in target db", DocLogStatus);
                                    continue;
                                }
                                else if (!string.IsNullOrEmpty(documentEntity.DocFilePath))
                                {
                                    adminConsoleHelper.LogHelper.DocumentMigrationLog("EmployeePerformance", "Record file is already migrated/updated in target db", DocLogStatus); ;
                                    continue;
                                }
                                UserInformation userRecord = GetUserRecord(documentEntity);
                                companyId = userRecord.CompanyId ?? 0;
                                try
                                {
                                    FilePathHelper filePathHelper = new FilePathHelper(clientId, companyId, folderPath);
                                    string docName = fileName;
                                    string serverFilePath = filePathHelper.GetPath("performanceDocs", ref docName, userRecord.Id, documentEntity.Id);
                                    documentEntity.DocName = fileName;
                                    documentEntity.DocFilePath = filePathHelper.RelativePath;
                                    File.WriteAllBytes(serverFilePath, bytes);
                                    DbContext.SaveChanges();
                                }
                                catch (Exception ex)
                                {
                                    adminConsoleHelper.LogHelper.DocumentMigrationLog("EmployeePerformance", $"Error in migrating record file:{ex.Message}", DocLogStatus);
                                }
                            }

                        }
                        catch (Exception ex)
                        {
                            adminConsoleHelper.LogHelper.DocumentMigrationLog("EmployeePerformance", $"Error in migrating record(s) saving info:{ex.Message}", DocLogStatus);
                        }
                    }
                }
                DocMigProgress?.Report(100);
            }
            else
            {
                adminConsoleHelper.LogHelper.DocumentMigrationLog("EmployeePerformance", "No record found for migration", DocLogStatus);
            }
            adminConsoleHelper.LogHelper.DocumentMigrationLog("EmployeePerformance", "###Ending Migration###", DocLogStatus);

        }
        public void MigrateEmploymentContract(int clientId, string sourceDatabase, AdminConsoleHelper adminConsoleHelper)
        {
            string logDescription = "";
            string folderPath = ConfigurationHelper.AppRootPath;
            //string sqlConnectionString = DbContext.Database.Connection.ConnectionString;
            string sqlConnectionString = ConfigurationHelper.ConnectionString;
            SqlConnection con = new SqlConnection(ConfigurationHelper.ConnectionString);

            string queryRecCount = $" Select count(intID)" +
                                   $" From {sourceDatabase}.dbo.tblEmployment" +
                                   $" Where docFile is not null ";

            var fetchSize = GetDocumentFetchSize(queryRecCount);
            var pageCount = fetchSize.NoOfPage;
            var pageSize = fetchSize.PageSize;
            int totalRecord = fetchSize.TotalRecord;
            int currRecord = 0;
            int currProgressPct = 0;
            adminConsoleHelper.LogHelper.DocumentMigrationLog("EmploymentContract", "###Starting Migration###", DocLogStatus);
            if (totalRecord > 0)
            {
                var targetDocumentList = DbContext.GetAll<Employment>(clientId);
                for (int page = 0; page < pageCount; page++)
                {

                    var offsetCount = page * pageSize;

                    string sourceQuery = $"Select intID, strEmpID, docName,docFile" +
                                       $" From {sourceDatabase}.dbo.tblEmployment" +
                                       $" Where docFile is not null" +
                                       $" Order by intID" +
                                       $" OFFSET {offsetCount} ROWS" +
                                       $" FETCH NEXT {pageSize} ROWS ONLY";

                    con.Open();
                    SqlCommand cmd = new SqlCommand(sourceQuery, con);
                    SqlDataAdapter da = new SqlDataAdapter(cmd);
                    DataSet ds = new DataSet();
                    da.Fill(ds);
                    con.Close();
                    int totalFetchRecords = ds.Tables[0].Rows.Count;

                    if (totalFetchRecords > 0)
                    {
                        try
                        {

                            foreach (DataRow dataRec in ds.Tables[0].Rows)
                            {
                                currRecord++;

                                if (currProgressPct < GetProgressPct(totalRecord, currRecord))
                                {
                                    currProgressPct++;
                                    if (currProgressPct <= 100)
                                    {
                                        DocMigProgress?.Report(currProgressPct);
                                    }
                                }
                                byte[] bytes = (byte[])dataRec["docFile"];
                                MemoryStream memStream = new MemoryStream(bytes);
                                string fileName = dataRec["docName"].ToString();
                                int companyId = 0;
                                string idStr = dataRec[0].ToString();
                                string empIdStr = dataRec[1].ToString();
                                int recordId, employeeId;
                                Int32.TryParse(idStr, out recordId);
                                Int32.TryParse(empIdStr, out employeeId);

                                logDescription = $"Migrating (employee_id:{empIdStr}, employment_id:{idStr}, file_name:{fileName})";
                                adminConsoleHelper.LogHelper.DocumentMigrationLog("EmploymentContract", logDescription, DocLogStatus);
                                //var documentEntity = DbContext.GetAll<EmployeeTraining>(clientId).FirstOrDefault(e => e.Old_Id == recordId);
                                var documentEntity = targetDocumentList.FirstOrDefault(e => e.Old_Id == recordId);
                                if (documentEntity == null)
                                {
                                    adminConsoleHelper.LogHelper.DocumentMigrationLog("EmploymentContract", "Record is not found in target db", DocLogStatus);
                                    continue;
                                }
                                else if (!string.IsNullOrEmpty(documentEntity.DocumentPath))
                                {
                                    adminConsoleHelper.LogHelper.DocumentMigrationLog("EmploymentContract", "Record file is already migrated/updated in target db", DocLogStatus); ;
                                    continue;
                                }
                                UserInformation userRecord = GetUserRecord(documentEntity);
                                companyId = userRecord.CompanyId ?? 0;
                                try
                                {
                                    FilePathHelper filePathHelper = new FilePathHelper(clientId, companyId, folderPath);
                                    string docName = fileName;
                                    string serverFilePath = filePathHelper.GetPath("EmploymentContract", ref docName, userRecord.Id, documentEntity.Id);
                                    documentEntity.DocumentName = fileName;
                                    documentEntity.DocumentPath = filePathHelper.RelativePath;
                                    File.WriteAllBytes(serverFilePath, bytes);
                                    DbContext.SaveChanges();
                                }
                                catch (Exception ex)
                                {
                                    adminConsoleHelper.LogHelper.DocumentMigrationLog("EmploymentContract", $"Error in migrating record file:{ex.Message}", DocLogStatus);
                                }
                            }

                        }
                        catch (Exception ex)
                        {
                            adminConsoleHelper.LogHelper.DocumentMigrationLog("EmploymentContract", $"Error in migrating record(s) saving info:{ex.Message}", DocLogStatus);
                        }
                    }
                }
                DocMigProgress?.Report(100);
            }
            else
            {
                adminConsoleHelper.LogHelper.DocumentMigrationLog("EmploymentContract", "No record found for migration", DocLogStatus);
            }
            adminConsoleHelper.LogHelper.DocumentMigrationLog("EmploymentContract", "###Ending Migration###", DocLogStatus);

        }
    }
}
