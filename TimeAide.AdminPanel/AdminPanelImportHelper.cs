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
//using TimeAide.Data;
using TimeAide.Models.ViewModel;
//using TimeAide.Services;
//using TimeAide.Web.Models;
using OfficeExcel = Microsoft.Office.Interop.Excel;
using DataTable = System.Data.DataTable;
using Microsoft.Office.Interop.Excel;
using System.Runtime.InteropServices;
using System.Drawing.Text;
using System.Collections.ObjectModel;
using TimeAide.AdminPanel.Models;
using Path = System.IO.Path;
using Syncfusion.XlsIO;
using System.Data.Entity.Core.EntityClient;
using System.Data.Entity;
using TimeAide.AdminPanel.DAL;
using TimeAide.AdminConsole;
using System.Data.Entity.Core.Objects;
using System.Xml.Serialization;
using System.Diagnostics.Eventing.Reader;
using System.Data.Entity.Validation;
using System.CodeDom;

namespace TimeAide.AdminPanel
{
    public class AdminPanelImportHelper
    {
        public AdminPanelImportHelper(System.Windows.Controls.TextBox textBox) 
        {
            LogHelper = new LogHelper(textBox);
        }
        public String TargetDatabase { get; set; }
        public LogHelper LogHelper { get; set; }
        public IProgress<string> ImpLogStatus { get; set; }
        public IProgress<int> ImportProgress { get; set; }
        public TA7_ImportData_Context TAWinPayrollImportDBContext { get; set; }
        
        public ObservableCollection<PayrollImportFieldMapping> PayrollImportFieldMappingData { get; set; }
        public bool IsPayrollImportFieldMappingChanged { get; set; }
        public IList<PayrollDBColumn> PayrollDBColumns { get; set; }
        private static string ConvertToEFConnectionString(string conStr)
        {
            // Specify the provider name, server and database.
            var providerName = "System.Data.SqlClient";

            //var providerName = "System.Data.EntityClient";

            // Initialize the EntityConnectionStringBuilder.
            var entityBuilder = new EntityConnectionStringBuilder();

            //Set the provider name.
            entityBuilder.Provider = providerName;

            // Set the provider-specific connection string.
            entityBuilder.ProviderConnectionString = conStr;

            // Set the Metadata location.
            entityBuilder.Metadata = @"res://*/DAL.TAWIndowImportData.csdl|res://*/DAL.TAWIndowImportData.ssdl|res://*/DAL.TAWIndowImportData.msl";

            return entityBuilder.ToString();
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
                catch (Exception ex)
                {
                    return false;
                }
            }
            return true;
        }
        public void InitTAWinPayrollImportDBContext()
        {
            TAWinPayrollImportDBContext = new TA7_ImportData_Context(ConvertToEFConnectionString(ConfigurationHelper.ConnectionString));
        }
        public IList<tblPayrollImportFileTemplate> GetPayrollImportFileTemplates()
        {
            var retList = TAWinPayrollImportDBContext.tblPayrollImportFileTemplates.ToList();
            retList.Add(new tblPayrollImportFileTemplate() { intTemplateId = 0, sTemplateName = "Create New Template" });
            return retList.OrderBy(t => t.intTemplateId).ToList();
        }
        private IList<PayrollImportFieldMapping> FetchPayrollExcelFileFields()
        {
            var retCode = 1;
            var retMsg = "Successfully validated data";
            string importFilePath = ConfigurationHelper.AppRootPath;
            IList<PayrollImportFieldMapping> payrollImportFieldMappings = new List<PayrollImportFieldMapping>();

            if (File.Exists(importFilePath))
            {
                if (Path.GetExtension(Path.GetFileName(importFilePath)) == ".xlsx")
                {
                    OfficeExcel.Application excelApp = new OfficeExcel.Application();
                    OfficeExcel.Workbook excelWorkbook = excelApp.Workbooks.Open(importFilePath);
                    OfficeExcel.Worksheet excelWorksheet = (OfficeExcel.Worksheet)excelWorkbook.Worksheets.get_Item(1);
                    var clumnsrange = excelWorksheet.UsedRange;
                    try
                    {

                        for (int cCnt = 1; cCnt <= clumnsrange.Columns.Count; cCnt++)
                        {
                            if (clumnsrange.Cells[1, cCnt].Value2 != null)
                            {
                                var columnName = (clumnsrange.Cells[1, cCnt].Value2).ToString();
                                payrollImportFieldMappings.Add(new PayrollImportFieldMapping() { FieldName = columnName, FieldIndex = cCnt,FieldMapping = "Not Required" });
                            }
                            //else
                            //{
                            //    var columnName = "NoHeaderCol" + cCnt.ToString();
                            //    payrollImportFieldMappings.Add(new PayrollImportFieldMapping() { FieldName = columnName, FieldIndex = cCnt });
                            //}
                        }
                        //  PayrollImportFieldMappingData = new ObservableCollection<PayrollImportFieldMapping>(payrollImportFieldMappings);

                    }
                    catch (Exception ex)
                    {
                        throw ex;
                    }
                    finally
                    {
                        excelWorkbook.Close();
                        excelApp.Quit();
                        Marshal.ReleaseComObject(excelWorksheet);
                        Marshal.ReleaseComObject(excelWorkbook);
                        Marshal.ReleaseComObject(excelApp);
                    }
                }
                else
                {
                    retCode = -1;
                    retMsg = "Invalid file format";
                }
            }
            else
            {
                retCode = -1;
                retMsg = "File not found";
            }
            if (retCode == -1)
            {
                throw new Exception(retMsg);
            }

            return payrollImportFieldMappings;
        }
        private DataTable ReadPayrollExcelFileData()
        {           
            var retCode = 1;
            var retMsg = "Successfully read data from excel.";
            string importFilePath = ConfigurationHelper.AppRootPath;           
            DataTable dataTable = new DataTable();
            foreach (var colMapping in PayrollImportFieldMappingData)
            {
                dataTable.Columns.Add(colMapping.FieldName, typeof(String));
            }
            if (File.Exists(importFilePath))
            {
                if (Path.GetExtension(Path.GetFileName(importFilePath)) == ".xlsx")
                {
                    OfficeExcel.Application excelApp = new OfficeExcel.Application();
                    OfficeExcel.Workbook excelWorkbook = excelApp.Workbooks.Open(importFilePath);
                    OfficeExcel.Worksheet excelWorksheet = (OfficeExcel.Worksheet)excelWorkbook.Worksheets.get_Item(1);
                    var rowRange = excelWorksheet.UsedRange;
                    int startRow = rowRange.Row;
                    int endRow =  rowRange.Rows.Count - 1;
                    try
                    {
                        for(int rCnt=2; rCnt<= endRow; rCnt++)
                        {
                            if (rowRange.Cells[rCnt, 1].Value2 == null)
                                break;

                            var dataRow = dataTable.Rows.Add();
                            foreach (var colMapping in PayrollImportFieldMappingData)
                            {                               
                                 dataRow[colMapping.FieldIndex-1] = rowRange.Cells[rCnt, colMapping.FieldIndex].Value;                               
                            }
                        }                                               

                    }
                    catch (Exception ex)
                    {
                        retCode = -1;
                        retMsg = ex.Message;
                    }
                    finally
                    {
                        excelWorkbook.Close();
                        excelApp.Quit();
                        Marshal.ReleaseComObject(excelWorksheet);
                        Marshal.ReleaseComObject(excelWorkbook);
                        Marshal.ReleaseComObject(excelApp);
                    }
                }
                else
                {
                    retCode = -1;
                    retMsg = "Invalid file format";
                }
            }
            else
            {
                retCode = -1;
                retMsg = "File not found";
            }
            
            if( retCode == -1)
            {
                throw new Exception(retMsg);
            }
            return dataTable;
        }
        public void ProcessImportedPayrollData()
        {
            var logType = "Import Payroll";
          
            try
            {
                var logHeader = $"Client:{ConfigurationHelper.ClientName}|Source DB:{ConfigurationHelper.SourceDatabase}|File Path:{ConfigurationHelper.AppRootPath}";
                ErrorLogHelper.InsertLog(ErrorLogType.Info, logHeader);
                this.LogHelper.ImportFileLog(logType, "###Starting Payroll Import Process###", ImpLogStatus);
                this.LogHelper.ImportFileLog(logType, "Reading data from excel file...", ImpLogStatus);
                var importDataTable = ReadPayrollExcelFileData();
                ImportProgress?.Report(10);
               
                if (importDataTable.Rows.Count > 0)
                {
                    var totalRec = importDataTable.Rows.Count;
                    var step = (int)Math.Ceiling((decimal)totalRec / 100);
                    var count = 0;
                    var progressStage = 10;
                     foreach (DataRow recRow in importDataTable.Rows)
                     {
                        count++;
                        if(count% step == 0)
                        {
                            progressStage++;
                            if (progressStage < 100)
                            {
                                ImportProgress?.Report(progressStage);
                            }
                        }


                        this.LogHelper.ImportFileLog(logType, "Start Processing Record", ImpLogStatus);
                        this.LogHelper.ImportFileLog(logType, string.Join("|",recRow.ItemArray), ImpLogStatus);
                        using (var processImpPayrollDBTrans = TAWinPayrollImportDBContext.Database.BeginTransaction())
                        {
                            try
                            {
                                this.LogHelper.ImportFileLog(logType, "Processing Payroll Batch", ImpLogStatus);
                                var batchId = ProcessPayrollBatch(recRow);
                                var userId = 0;
                                if (!string.IsNullOrEmpty(batchId))
                                {
                                    this.LogHelper.ImportFileLog(logType, "Processing User Batch", ImpLogStatus);
                                    userId=ProcessPayrollUserBatch(batchId, recRow);

                                    this.LogHelper.ImportFileLog(logType, "Processing User Batch Compensations", ImpLogStatus);
                                    ProcessPayrollUserBatchCompensations(batchId, userId, recRow);

                                    this.LogHelper.ImportFileLog(logType, "Processing User Batch Withholdings", ImpLogStatus);
                                    ProcessPayrollUserBatchWithholdings(batchId, userId, recRow);

                                    this.LogHelper.ImportFileLog(logType, "Processing Company Batch Withholdings", ImpLogStatus);
                                    ProcessPayrollCompanyBatchWithholdings(batchId, userId, recRow);
                                    
                                    
                                }
                                this.LogHelper.ImportFileLog(logType, "Committing user imported payroll data", ImpLogStatus);
                                processImpPayrollDBTrans.Commit();
                                if(!string.IsNullOrEmpty(batchId) && userId > 0)
                                {
                                    try
                                    {
                                        this.LogHelper.ImportFileLog(logType, "Computing user batch payroll", ImpLogStatus);
                                        ComputeUserBatchPayroll(batchId, userId);
                                    }
                                    catch (Exception ex)
                                    {
                                        this.LogHelper.ImportFileLog(logType, ex.Message, ImpLogStatus);                                       
                                        this.LogHelper.ImportFileLog(logType, $"Error:in Computing user payroll batch by using sp (spPay_Compute_UserBatch {batchId}, {userId}, 6666, IDENTECH).", ImpLogStatus);
                                    }
                                }
                                this.LogHelper.ImportFileLog(logType, "End Processing Record", ImpLogStatus);
                            }
                            catch (DbEntityValidationException eValidException)
                            {
                              StringBuilder errorString = new StringBuilder();
                              foreach(var validationError in eValidException.EntityValidationErrors)
                                {
                                    var errorStr = validationError.ValidationErrors.Aggregate("", (a, b) => a + ";" + b.ErrorMessage);
                                   
                                    errorString.Append(errorStr);                                    
                                }
                                
                                this.LogHelper.ImportFileLog(logType, errorString.ToString(), ImpLogStatus);
                                processImpPayrollDBTrans.Rollback();
                                this.LogHelper.ImportFileLog(logType, "Error: User payroll record transation is rolled-back.", ImpLogStatus);

                            }
                            catch (Exception ex)
                            {
                                this.LogHelper.ImportFileLog(logType, ex.Message, ImpLogStatus);
                                processImpPayrollDBTrans.Rollback();
                                this.LogHelper.ImportFileLog(logType, "Error: User payroll record transation is rolled-back.", ImpLogStatus);
                            }   
                        }
                       
                     }
                }
                this.LogHelper.ImportFileLog(logType, "###Ending payroll import Process###", ImpLogStatus);
                ImportProgress?.Report(100);
            }
            catch (Exception ex)
            {
                this.LogHelper.ImportFileLog(logType, $"Error in Processing data:{ex.Message}", ImpLogStatus);
            }
        }
        private string ProcessPayrollBatch(DataRow record)
        {
            string batchId = "";
            var batchTableMapping = PayrollImportFieldMappingData.Where(w=>w.MappingTable== "tblBatch").OrderBy(o=>o.FieldIndex).ToList();
            var userBatchTableMapping = PayrollImportFieldMappingData.Where(w => w.MappingTable == "tblUserBatch").OrderBy(o => o.FieldIndex).ToList();
            var UserIdFieldIndex = userBatchTableMapping.Where(w => w.MappingColumn == "intUserID").Select(s => s.FieldIndex).FirstOrDefault();
            if (UserIdFieldIndex>0)
            {
                var userId = 0;
                var sts= int.TryParse(record[UserIdFieldIndex-1].ToString(),out userId);
                if (!sts)
                {
                    throw new Exception("Error: Invalid User Id format");
                }
                else
                {
                   var isUserExist = TAWinPayrollImportDBContext.viewPay_UserRecord.Where(w => w.intUserID == userId).Count();
                    if (isUserExist == 0)
                    {
                        throw new Exception("Error: User Id does not exist in the system.");
                    }
                }
                //Pay Date
                var payDate = DateTime.Today;
                var payDateFieldId = batchTableMapping.Where(w => w.MappingColumn == "dtPayDate").Select(s => s.FieldIndex).FirstOrDefault();
                if (payDateFieldId > 0)
                    payDate = DateTime.Parse(record[payDateFieldId-1].ToString());
                //Payroll Company
                var payrollCmpFieldId = batchTableMapping.Where(w => w.MappingColumn == "strCompanyName").Select(s => s.FieldIndex).FirstOrDefault();
                var payrollCompany = "";
                if (payrollCmpFieldId > 0)                
                    payrollCompany = record[payrollCmpFieldId - 1].ToString();
                
                if(string.IsNullOrEmpty(payrollCompany))
                    payrollCompany = TAWinPayrollImportDBContext.viewPay_UserRecord.Where(w => w.intUserID == userId).Select(s => s.strCompanyName).FirstOrDefault();
              
                var supervisorId = 6666;
                var supervisorName = "IDENTECH";
                var batchType = 2;               
                var payWeekNum = 0;
                var payMethodType = 0;
                
                var payrollDescription = $"PAYROLL: {payDate.ToString("MM/dd/yyyy")}";
                var templateId = PayrollImportFieldMappingData.Max(f=>f.TemplateId);
                //select * from tblBatch where dtPayDate = @Next@PayDate and strCompanyName = @PayrollCompany and intBatchType = @BATCH_TYPE and intBatchStatus <> -1

                batchId=TAWinPayrollImportDBContext.tblBatches.Where(w=>w.dtPayDate== payDate && w.strCompanyName== payrollCompany && w.intBatchType==batchType && w.intBatchStatus!=-1).Select(s=>s.strBatchID.ToString()).FirstOrDefault();
                if (string.IsNullOrEmpty(batchId))
                {
                    
                    ObjectParameter bATCHID= new ObjectParameter("BATCHID", typeof(string));
                    var res=TAWinPayrollImportDBContext.spPay_Create_CompanyBatch(bATCHID, payrollCompany, payrollDescription, payDate, supervisorId, supervisorName, batchType, templateId).ToList();
                    var retBatchId = bATCHID.Value.ToString();
                    if (!string.IsNullOrEmpty(retBatchId) && retBatchId!="-1")
                    {
                        batchId = retBatchId;
                        tblCompanySchedulesProcessed cmpSchProcessedEntity = new tblCompanySchedulesProcessed();
                        cmpSchProcessedEntity.strBatchID = new Guid(retBatchId);
                        cmpSchProcessedEntity.strCompanyName = payrollCompany;
                        TAWinPayrollImportDBContext.tblCompanySchedulesProcesseds.Add(cmpSchProcessedEntity);
                        TAWinPayrollImportDBContext.SaveChanges();
                        
                    }
                    else if(retBatchId=="-1")
                    {
                        if (res.Count > 0)
                        {
                            var errMsg = res.FirstOrDefault().ErrorMessage;
                            throw new Exception(errMsg);
                        }

                    }
                }               
            }
            else
            {
                throw new Exception("Error: User is not mapped for template.Please check mapping.");
            }
            if (string.IsNullOrEmpty(batchId))
                throw new Exception("Error: Payroll batch is not created");
            return batchId;
        }
        private int ProcessPayrollUserBatch(string batchId,DataRow record)
        {
           int userId = 0; 
           var batchEntity= TAWinPayrollImportDBContext.tblBatches.Where(w => w.strBatchID.ToString() == batchId).FirstOrDefault();
           var userBatchTableMapping = PayrollImportFieldMappingData.Where(w => w.MappingTable == "tblUserBatch" && w.FieldMapping=="Required" ).OrderBy(o => o.FieldIndex).ToList();
           var userBatchEntity = new tblUserBatch();
            
            foreach(var userMappingField in userBatchTableMapping)
            {
                switch (userMappingField.MappingColumn)
                {
                    case "intUserID":
                        userId = int.Parse(record[userMappingField.FieldIndex - 1].ToString());
                        userBatchEntity.intUserID = userId;
                        userBatchEntity.strBatchID = batchId;//set batchId
                        break;
                }
               
            }
            if (userBatchEntity.intUserID > 0)
            {

                var IsExistCount = TAWinPayrollImportDBContext.tblUserBatches
                                            .Where(w => w.intUserID == userBatchEntity.intUserID && w.strBatchID == userBatchEntity.strBatchID)
                                            .Count();
                if (IsExistCount > 0)
                {
                    throw new Exception("Error: User is already added in tblUserBatch against the payroll batch");
                }
                else
                {
                    userBatchEntity.dtStartDatePeriod = batchEntity.dtPayDate;
                    userBatchEntity.dtEndDatePeriod = batchEntity.dtPayDate;
                    userBatchEntity.intPayWeekNum = 0;
                    userBatchEntity.intUserBatchStatus = 0;
                    userBatchEntity.intPayMethodType = 0;
                    var userRecordInfo = TAWinPayrollImportDBContext.viewPay_UserRecord
                                        .Where(w => w.intUserID == userBatchEntity.intUserID)
                                        .FirstOrDefault();
                    if (userRecordInfo != null)
                    {
                        userBatchEntity.intCompanyID = userRecordInfo.nCompanyID;
                        userBatchEntity.intDepartmentID = userRecordInfo.nDeptID;
                        userBatchEntity.intSubdepartmentID = userRecordInfo.nJobTitleID;
                        userBatchEntity.intEmployeeTypeID = userRecordInfo.nEmployeeType;

                    }
                    else
                    {
                        throw new Exception("Error: User doesn't exist in the TA7 database");
                    }
                    TAWinPayrollImportDBContext.tblUserBatches.Add(userBatchEntity);
                    TAWinPayrollImportDBContext.SaveChanges();
                }
            }
            else
            {
                throw new Exception("User is not mapped properly.");
            }
            return userId;
        }
        private bool ProcessPayrollUserBatchCompensations(string batchId,int userId, DataRow record)
        {
            var userBatchEntity = TAWinPayrollImportDBContext.tblUserBatches
                                            .Where(w => w.intUserID == userId && w.strBatchID == batchId).FirstOrDefault();
            
            var userBatchCompensationTableMapping = PayrollImportFieldMappingData.Where(w => w.MappingTable == "tblUserBatchCompensations" && w.FieldMapping == "Required").OrderBy(o => o.FieldNameAsValue).ToList();
            
            tblUserBatchCompensations_ManualEntry userBatchCompensationManualEntity = null;
            IList<tblUserBatchCompensations_ManualEntry> userBatchCompensationList = new List<tblUserBatchCompensations_ManualEntry>();
            var fieldAsVal = "";
            var batchIdGuid = new Guid(batchId);
            var recCount = 0;
            foreach (var mappingFieldRec in userBatchCompensationTableMapping)
            {
                recCount++;
                if(fieldAsVal!= mappingFieldRec.FieldNameAsValue.Trim())
                {
                    fieldAsVal=mappingFieldRec.FieldNameAsValue.Trim();
                    if (userBatchCompensationManualEntity != null)
                    {
                        if (Math.Abs(userBatchCompensationManualEntity.decPay) > 0)  
                        userBatchCompensationList.Add(userBatchCompensationManualEntity);                        
                    }
                    userBatchCompensationManualEntity = new tblUserBatchCompensations_ManualEntry() { strBatchID = batchIdGuid, intUserID = userId, strCompensationName = fieldAsVal, dtPayDate = userBatchEntity.dtStartDatePeriod, dtTimeStamp = DateTime.Now, strGLAccount = "", boolDeleted=false, intSupervisorID=6666,strNote="Imported", intEditType=1 };
                }
                switch (mappingFieldRec.MappingColumn)
                {
                    //
                    case "decPayRate":
                        decimal payRate;
                        decimal.TryParse(record[mappingFieldRec.FieldIndex - 1].ToString(), out payRate);                        
                        userBatchCompensationManualEntity.decPayRate = payRate;
                        break;
                    case "decHours":
                        decimal hours;
                        decimal.TryParse(record[mappingFieldRec.FieldIndex - 1].ToString(), out hours);
                        //var hours = decimal.Parse(record[mappingFieldRec.FieldIndex - 1].ToString());
                        userBatchCompensationManualEntity.decHours = hours;
                        break;
                    case "decPay":
                        decimal payAmt;
                        decimal.TryParse(record[mappingFieldRec.FieldIndex - 1].ToString(), out payAmt);
                        //var payAmt = decimal.Parse(record[mappingFieldRec.FieldIndex - 1].ToString());
                        userBatchCompensationManualEntity.decPay = payAmt;
                        break;
                }

            }
           if(userBatchCompensationManualEntity != null)
            {
               var lastAddedCount= userBatchCompensationList
                            .Where(w=>w.strCompensationName== userBatchCompensationManualEntity.strCompensationName)
                            .Count();

                if (lastAddedCount < 1)
                {
                    if (Math.Abs(userBatchCompensationManualEntity.decPay) > 0)
                        userBatchCompensationList.Add(userBatchCompensationManualEntity);
                }
            }

            if (userBatchCompensationList.Count > 0)
            {
                TAWinPayrollImportDBContext.tblUserBatchCompensations_ManualEntry.AddRange(userBatchCompensationList);
                TAWinPayrollImportDBContext.SaveChanges();
            }
            return true;
        }
        private bool ProcessPayrollUserBatchWithholdings(string batchId, int userId, DataRow record)
        {
            var userBatchEntity = TAWinPayrollImportDBContext.tblUserBatches
                                            .Where(w => w.intUserID == userId && w.strBatchID == batchId).FirstOrDefault();

            var tblUserBatchWithholdingTableMapping = PayrollImportFieldMappingData.Where(w => w.MappingTable == "tblUserBatchWithholdings" && w.FieldMapping == "Required").OrderBy(o => o.FieldNameAsValue).ToList();

            tblUserBatchWithholdings_ManualEntry userBatchWithholdingManualEntity = null;
            IList<tblUserBatchWithholdings_ManualEntry> userBatchWithholdingList = new List<tblUserBatchWithholdings_ManualEntry>();
            var fieldAsVal = "";
            var batchIdGuid = new Guid(batchId);
            var recCount = 0;
            foreach (var mappingFieldRec in tblUserBatchWithholdingTableMapping)
            {
                recCount++;
                if (fieldAsVal != mappingFieldRec.FieldNameAsValue.Trim())
                {
                    fieldAsVal = mappingFieldRec.FieldNameAsValue.Trim();
                    if (userBatchWithholdingManualEntity != null)
                    {
                        if(Math.Abs(userBatchWithholdingManualEntity.decWithholdingsAmount)>0)
                            userBatchWithholdingList.Add(userBatchWithholdingManualEntity);

                    }
                    userBatchWithholdingManualEntity = new tblUserBatchWithholdings_ManualEntry() { strBatchID = batchIdGuid, intUserID = userId, strWithHoldingsName = fieldAsVal, dtPayDate = userBatchEntity.dtStartDatePeriod, dtTimeStamp = DateTime.Now, strGLAccount = "", boolDeleted = false, intSupervisorID = 6666, strNote = "Imported", intEditType = 1 };
                }
                switch (mappingFieldRec.MappingColumn)
                {
                    //
                    case "decBatchEffectivePay":
                        decimal effectivePay;
                        decimal.TryParse(record[mappingFieldRec.FieldIndex - 1].ToString(),out effectivePay);
                        //var effectivePay = decimal.Parse(record[mappingFieldRec.FieldIndex - 1].ToString());
                        userBatchWithholdingManualEntity.decBatchEffectivePay = effectivePay;
                        break;
                    
                    case "decWithholdingsAmount":
                        decimal whAmt;
                        decimal.TryParse(record[mappingFieldRec.FieldIndex - 1].ToString(), out whAmt);
                        //var whAmt = decimal.Parse(record[mappingFieldRec.FieldIndex - 1].ToString());
                        whAmt = whAmt> 0 ? (whAmt*-1) : whAmt;
                        userBatchWithholdingManualEntity.decWithholdingsAmount = whAmt;
                        break;
                }

            }
            if (userBatchWithholdingManualEntity != null)
            {
                var lastAddedCount = userBatchWithholdingList
                             .Where(w => w.strWithHoldingsName == userBatchWithholdingManualEntity.strWithHoldingsName)
                             .Count();

                if (lastAddedCount < 1)
                {
                    if (Math.Abs(userBatchWithholdingManualEntity.decWithholdingsAmount) > 0)
                        userBatchWithholdingList.Add(userBatchWithholdingManualEntity);
                }
            }

            if (userBatchWithholdingList.Count > 0)
            {
                TAWinPayrollImportDBContext.tblUserBatchWithholdings_ManualEntry.AddRange(userBatchWithholdingList);
                TAWinPayrollImportDBContext.SaveChanges();
            }
            return true;
        }
        private bool ProcessPayrollCompanyBatchWithholdings(string batchId, int userId, DataRow record)
        {
            var userBatchEntity = TAWinPayrollImportDBContext.tblUserBatches
                                           .Where(w => w.intUserID == userId && w.strBatchID == batchId).FirstOrDefault();

            var tblCompanyBatchWithholdingTableMapping = PayrollImportFieldMappingData.Where(w => w.MappingTable == "tblCompanyBatchWithholdings" && w.FieldMapping == "Required").OrderBy(o => o.FieldNameAsValue).ToList();

            tblCompanyBatchWithholdings_ManualEntry companyBatchWithholdingManualEntity = null;
            IList<tblCompanyBatchWithholdings_ManualEntry> companyBatchWithholdingList = new List<tblCompanyBatchWithholdings_ManualEntry>();
            var fieldAsVal = "";
            var batchIdGuid = new Guid(batchId);
            var recCount = 0;
            foreach (var mappingFieldRec in tblCompanyBatchWithholdingTableMapping)
            {
                recCount++;
                if (fieldAsVal != mappingFieldRec.FieldNameAsValue.Trim())
                {
                    fieldAsVal = mappingFieldRec.FieldNameAsValue.Trim();
                    if (companyBatchWithholdingManualEntity != null)
                    {
                        if(Math.Abs(companyBatchWithholdingManualEntity.decWithholdingsAmount)>0)
                            companyBatchWithholdingList.Add(companyBatchWithholdingManualEntity);

                    }
                    companyBatchWithholdingManualEntity = new tblCompanyBatchWithholdings_ManualEntry() { strBatchID = batchIdGuid, intUserID = userId, strWithHoldingsName = fieldAsVal, dtPayDate = userBatchEntity.dtStartDatePeriod, dtTimeStamp = DateTime.Now, strGLAccount = "", intPrePostTaxDeduction=0, boolDeleted = false, intSupervisorID = 6666, strNote = "Imported", intEditType = 1,strGLContributionPayable="" };
                }
                switch (mappingFieldRec.MappingColumn)
                {
                    //
                    case "decBatchEffectivePay":
                        decimal effectivePay;
                        decimal.TryParse(record[mappingFieldRec.FieldIndex - 1].ToString(), out effectivePay);
                        //var effectivePay = decimal.Parse(record[mappingFieldRec.FieldIndex - 1].ToString());
                        companyBatchWithholdingManualEntity.decBatchEffectivePay = effectivePay;
                        break;

                    case "decWithholdingsAmount":
                        decimal whAmt;
                        decimal.TryParse(record[mappingFieldRec.FieldIndex - 1].ToString(), out whAmt);
                        //var whAmt = decimal.Parse(record[mappingFieldRec.FieldIndex - 1].ToString());
                        companyBatchWithholdingManualEntity.decWithholdingsAmount = whAmt;
                        break;
                }

            }
            if (companyBatchWithholdingManualEntity != null)
            {
                var lastAddedCount = companyBatchWithholdingList
                             .Where(w => w.strWithHoldingsName == companyBatchWithholdingManualEntity.strWithHoldingsName)
                             .Count();

                if (lastAddedCount < 1)
                {
                    if (Math.Abs(companyBatchWithholdingManualEntity.decWithholdingsAmount) > 0)
                        companyBatchWithholdingList.Add(companyBatchWithholdingManualEntity);
                }
            }

            if (companyBatchWithholdingList.Count > 0)
            {
                TAWinPayrollImportDBContext.tblCompanyBatchWithholdings_ManualEntry.AddRange(companyBatchWithholdingList);
                TAWinPayrollImportDBContext.SaveChanges();
            }
            return true;
        }
        private bool ComputeUserBatchPayroll(string batchId, int userId)
        {
           var res= TAWinPayrollImportDBContext.spPay_Compute_UserBatch(batchId, userId, 6666, "supervisorName");
            return true;
        }
        public void GetPayrollColumnList()
        {
            string sqlQuery = @"SELECT TABLE_NAME as TableName,COLUMN_NAME as ColumnName,DATA_TYPE as DataType
                    FROM INFORMATION_SCHEMA.COLUMNS
                    WHERE TABLE_NAME in( 'tblBatch','tblUserBatch','tblUserBatchCompensations','tblUserBatchWithholdings','tblCompanyBatchWithholdings')
                    ORDER BY TABLE_NAME,COLUMN_NAME";

            var retList = TAWinPayrollImportDBContext.Database.SqlQuery<PayrollDBColumn>(sqlQuery);

            PayrollDBColumns = retList.ToList<PayrollDBColumn>();
        }
        public IList<string> GetSelectedTblItemsList(string tableName)
        {
            IList<string> retList = new List<string>();
            switch (tableName)
            {
                case "tblUserBatchCompensations":
                    retList=TAWinPayrollImportDBContext.tblCompensationItems.Select(s => s.strCompensationName).ToList();
                    break;
                case "tblUserBatchWithholdings":
                    retList = TAWinPayrollImportDBContext.tblWithholdingsItems.Select(s => s.strContributionsName).ToList();
                    break;
                case "tblCompanyBatchWithholdings":
                    retList = TAWinPayrollImportDBContext.tblWithholdingsItems.Where(w=>w.boolCompanyContribution==true).Select(s => s.strContributionsName).ToList();
                    break;

            }

            return retList;
        }
        public (int, string) ProcessPayrollExcelFileMapping(int templateId)
        {
            var retCode = 1;
            var retMsg = "Successfully Fetched Fields/Mappings!";
            //PayrollImportFieldMappingData = new ObservableCollection<PayrollImportFieldMapping>(payrollImportFieldMappings);

            try
            {
                if (templateId == 0)
                {
                    var payrollExcelFileMapping = FetchPayrollExcelFileFields();
                    if (payrollExcelFileMapping.Count > 0)
                    {
                        PayrollImportFieldMappingData = new ObservableCollection<PayrollImportFieldMapping>(payrollExcelFileMapping);
                    }
                    else
                    {
                        retCode = -1;
                        retMsg = "No field(s) found in the excel file";
                    }
                }
                else
                {
                    var templateFieldsMapping = TAWinPayrollImportDBContext.tblPayrollImportFileTemplateDetails.Where(t => t.intTemplateId == templateId)
                                             .Select(s => new PayrollImportFieldMapping() { TemplateId = s.intTemplateId ?? 0, TemplateDetailId = s.intTemplateDetailId, FieldIndex = s.intFieldIndex ?? 0, FieldName = s.sFieldName, FieldNameAsValue = s.sFieldNameAsValue, FieldMapping = s.sFieldMapping, MappingTable = s.sMappingTable, MappingColumn = s.sMappingColumn, ColumnDataType = s.sColumnDataType })
                                             .ToList();
                    var payrollExcelFileMapping = FetchPayrollExcelFileFields();
                    if (templateFieldsMapping.Count != payrollExcelFileMapping.Count)
                    {
                        retCode = -1;
                        retMsg = "Uploaded excel file fields count doesn't match with the selected template";
                    }
                    else
                    {
                        var isValidated = true;
                        foreach (var item in templateFieldsMapping)
                        {
                            var isMapped = payrollExcelFileMapping.Where(w => w.FieldIndex == item.FieldIndex && w.FieldName == item.FieldName).Count();
                            if (isMapped == 0)
                            {
                                isValidated = false;
                                item.MappingError = "Doesn't match with excel file field";
                            }
                        }
                        PayrollImportFieldMappingData = new ObservableCollection<PayrollImportFieldMapping>(templateFieldsMapping);
                        if (!isValidated)
                        {
                            retCode = -1;
                            retMsg = "Uploaded excel file fields doesn't match with the selected template";
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                retCode = -1;
                retMsg = ex.Message;
            }

            return (retCode, retMsg);
        }
        public (int, string) SavePayrollImportTemplate(int templateId, string templateName)
        {
            int retCode = 1;
            string retMsg = "Successfully saved template";
            using (var importTemplateDBTrans = TAWinPayrollImportDBContext.Database.BeginTransaction())
            {
                try
                {
                    if (templateId == 0)
                    {
                        //TAWinPayrollImportDBContext
                        var templateEntity = new tblPayrollImportFileTemplate()
                        {
                            sTemplateName = templateName,
                            dtCreationDate = DateTime.Now
                        };
                        TAWinPayrollImportDBContext.tblPayrollImportFileTemplates.Add(templateEntity);

                        TAWinPayrollImportDBContext.SaveChanges();

                        foreach (var mappingDetail in PayrollImportFieldMappingData)
                        {
                            var templateDetailEntity = new tblPayrollImportFileTemplateDetail()
                            {
                                intTemplateId = templateEntity.intTemplateId,
                                intFieldIndex = mappingDetail.FieldIndex,
                                sFieldName = mappingDetail.FieldName,
                                sFieldNameAsValue = mappingDetail.FieldNameAsValue,
                                sFieldMapping = mappingDetail.FieldMapping,
                                sMappingTable = mappingDetail.MappingTable,
                                sMappingColumn = mappingDetail.MappingColumn,
                                sColumnDataType = mappingDetail.ColumnDataType
                            };
                            TAWinPayrollImportDBContext.tblPayrollImportFileTemplateDetails.Add(templateDetailEntity);
                            TAWinPayrollImportDBContext.SaveChanges();
                            mappingDetail.TemplateId = templateEntity.intTemplateId;
                            mappingDetail.TemplateDetailId = templateDetailEntity.intTemplateDetailId;
                        }


                    }
                    else
                    {
                        var templateDetails = TAWinPayrollImportDBContext.tblPayrollImportFileTemplateDetails.Where(w => w.intTemplateId == templateId);

                        foreach (var templateDetail in templateDetails)
                        {
                            var mappingDetail = PayrollImportFieldMappingData.Where(w => w.TemplateDetailId == templateDetail.intTemplateDetailId).FirstOrDefault();
                            if (mappingDetail != null)
                            {
                                //templateDetail.intFieldIndex = mappingDetail.FieldIndex;
                                //templateDetail.sFieldName = mappingDetail.FieldName;
                                templateDetail.sFieldNameAsValue = mappingDetail.FieldNameAsValue;
                                templateDetail.sFieldMapping = mappingDetail.FieldMapping;
                                templateDetail.sMappingTable = mappingDetail.MappingTable;
                                templateDetail.sMappingColumn = mappingDetail.MappingColumn;
                                templateDetail.sColumnDataType = mappingDetail.ColumnDataType;
                            }

                        }
                        TAWinPayrollImportDBContext.SaveChanges();
                    }

                }
                catch (Exception ex)
                {
                    retCode = -1;
                    retMsg = ex.Message;
                    importTemplateDBTrans.Rollback();
                }
                importTemplateDBTrans.Commit();
            }
            return (retCode, retMsg);
        }
    }
}
