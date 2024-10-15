using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Runtime.Remoting.Contexts;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Windows.Threading;
using TimeAide.AdminConsole;
using TimeAide.AdminPanel.Helpers;
using TimeAide.Web.Models;
using Syncfusion.XlsIO;
using System.Windows.Forms;
using System.Reflection;
//using Microsoft.Office.Interop.Excel;


namespace TimeAide.AdminPanel
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml7 
    /// </summary>
    public partial class DataExportWindow : Window
    {
        AdminConsoleHelper adminConsoleHelper;
        List<DataMigrationLog> databaseCreationLog = null;        
        DataTable dataTable = null;
        public DataExportWindow()
        {
            InitializeComponent();
           
        }
        private void SetSetting(string key, string value)
        {
            Configuration configuration =
                ConfigurationManager.OpenExeConfiguration(ConfigurationUserLevel.None);
            configuration.AppSettings.Settings[key].Value = value;
            configuration.Save(ConfigurationSaveMode.Full, true);
            ConfigurationManager.RefreshSection("appSettings");
            
        }
        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            adminConsoleHelper = new AdminConsoleHelper(txtLogStatus);
            //txtTargetDatabase.Text = adminConsoleHelper.DbContext.Database.Connection.Database;
            //adminConsoleHelper.TargetDatabase = txtTargetDatabase.Text;
            
           // txtSourceDatabase.Text = ConfigurationHelper.SourceDatabase;
           // txtAppRootPath.Text = ConfigurationHelper.AppRootPath;
            txtSourceClient.Text = ConfigurationHelper.ClientName;
            txtTA7Database.Text = ConfigurationHelper.TA7ExportDatabase;
            txtTAWDatabase.Text = ConfigurationHelper.TAWExportDatabase;
            txtAppRootPath.Text = ConfigurationHelper.AppRootPath;
            LoadDropdown();
        }

        private void LoadDropdown()
        {
            var exporttypeList = ConfigurationHelper.ExportDataTypeNames.Select(s=>s.Value).ToArray();
            foreach ( var exporttype in exporttypeList )
            {
                cboExecutionType.Items.Add(exporttype);
            }
            //cboExecutionType.Items.Add("Company SetUp");
            //cboExecutionType.Items.Add("Master file");
            //cboExecutionType.Items.Add("Position Info");
            //cboExecutionType.Items.Add("Tax Info");
            //cboExecutionType.Items.Add("Additional Earnings Info");
            //cboExecutionType.Items.Add("Deductions Goals");
           
        }
        private void btnBrowseFilePath_Click(object sender, RoutedEventArgs e)
        {
            System.Windows.Forms.FolderBrowserDialog openFileDlg = new System.Windows.Forms.FolderBrowserDialog();
            var result = openFileDlg.ShowDialog();
            if (result== System.Windows.Forms.DialogResult.OK)
            {
                txtAppRootPath.Text = openFileDlg.SelectedPath;
            }
            
        }
        private async void BtnExportData_Click(object sender, RoutedEventArgs e)
        {
            if (!string.IsNullOrEmpty(txtSourceClient.Text.Trim()))
            {
                ConfigurationHelper.ClientName = txtSourceClient.Text;
            }
            else
            {
                System.Windows.MessageBox.Show("Missing client name");
                return;
            }
            if (cboExecutionType.SelectedIndex == -1)
            {
                System.Windows.MessageBox.Show("Missing Export Type");
                return;
            }
            if (!string.IsNullOrEmpty(txtAppRootPath.Text.Trim()))
            {
                ConfigurationHelper.AppRootPath = txtAppRootPath.Text;
            }
            else
            {
                System.Windows.MessageBox.Show("Please browse and select the file path.");
                return;
            }
            var actionType = (ExportDataType)cboExecutionType.SelectedIndex-1;
            //var dbPrefix = ConfigurationHelper.ExportDataDBPrefix[actionType];
            if (actionType != ExportDataType.AllFiles)
            {
                var result = validateExportFileDatabase(actionType);
                if (result.Item1 == 0)
                {
                    System.Windows.MessageBox.Show(result.Item2);
                    return;
                }

                await StartExportData(actionType,"S");
            }
            else
            {
                foreach(var exportFileType in Enum.GetValues(typeof(ExportDataType)))
                {
                    var allFileActionType = (ExportDataType)exportFileType;
                    if (allFileActionType != ExportDataType.AllFiles)
                    {
                        //Console.WriteLine(allFileActionType);
                        var result = validateExportFileDatabase(allFileActionType);
                        if (result.Item1 == 0)
                        {
                          //  adminConsoleHelper.LogHelper.DocumentMigrationLog(expDataTypeName, "Executing query for extracting data.", DocLogStatus);
                            continue;
                        }
                        await StartExportData(allFileActionType,"A");
                    }
                }
                ProgressHeading.Text = $"Process compeleted for All Files";
                System.Windows.MessageBox.Show("Process compeleted. Please see log for detail");
            }
            //DocMigProgress.Value = 0;
            //txtLogStatus.Text="";
            //Syncfusion.Licensing.SyncfusionLicenseProvider.RegisterLicense("Ngo9BigBOggjHTQxAR8/V1NBaF1cXmhPYVJ/WmFZfVpgcF9HZVZVRmY/P1ZhSXxXdkBhWX5YcXFRRWJcVkE=");
            //using (ExcelEngine excelEngine = new ExcelEngine())
            //{
            //    //Initialize Application
            //    IApplication application = excelEngine.Excel;

            //    //Set the default application version as Excel 2016
            //    application.DefaultVersion = ExcelVersion.Xlsx;

            //    //Create a new workbook
            //    IWorkbook workbook = application.Workbooks.Create(1);

            //    //Access first worksheet from the workbook instance
            //    IWorksheet worksheet = workbook.Worksheets[0];

            //    //Exporting DataTable to worksheet
            //    DataTable dataTable = GetDataTable();
            //    worksheet.ImportDataTable(dataTable, true, 1, 1);
            //    worksheet.UsedRangeIncludesFormatting = true;
            //    worksheet.UsedRange.AutofitColumns();
            //    worksheet.Name = "Company Setup";
            //    var fileName = ConfigurationHelper.AppRootPath + "//Output.xlsx";
            //    //Save the workbook to disk in xlsx format
            //    workbook.SaveAs(fileName);
            //}   

        }
      private (int,string) validateExportFileDatabase(ExportDataType exportFileType)
        {
            var retCode = 1;
            var retMsg = string.Empty;
            var dbSrcType = ConfigurationHelper.ExportDataDBPrefix[exportFileType];
            if (dbSrcType == "TA7")
            {
                if (!string.IsNullOrEmpty(txtTA7Database.Text.Trim()))
                {
                    ConfigurationHelper.SourceDatabase = txtTA7Database.Text;
                    SqlConnectionStringBuilder conn = new SqlConnectionStringBuilder(ConfigurationHelper.ConnectionString)
                    { InitialCatalog = txtTA7Database.Text }; // you can add other parameters.
                    ConfigurationHelper.ConnectionString = conn.ConnectionString;

                }
                else
                {
                    //System.Windows.MessageBox.Show($"Missing {dbSrcType} database");
                    retMsg = $"Missing {dbSrcType} database";
                    retCode=0 ;
                }
            }
            else
            {
                if (!string.IsNullOrEmpty(txtTAWDatabase.Text.Trim()))
                {
                    ConfigurationHelper.SourceDatabase = txtTAWDatabase.Text;
                    SqlConnectionStringBuilder conn = new SqlConnectionStringBuilder(ConfigurationHelper.ConnectionString)
                    { InitialCatalog = txtTAWDatabase.Text }; // you can add other parameters.
                    ConfigurationHelper.ConnectionString = conn.ConnectionString;

                }
                else
                {
                    //System.Windows.MessageBox.Show($"Missing {dbSrcType} database");
                    //return;
                    retMsg = $"Missing {dbSrcType} database";
                    retCode = 0;
                }

            }
            if (!string.IsNullOrEmpty(ConfigurationHelper.SourceDatabase))
            {
                var isValidDb = adminConsoleHelper.IsValidSourceDatabase();
                if (!isValidDb)
                {
                    retMsg = $"{ConfigurationHelper.SourceDatabase} database doesn't exist";
                    retCode = 0;
                }
            }

            return (retCode, retMsg);
        }
        private async Task StartExportData(ExportDataType exportActionType,string ExportType)
        {
            try
            {
                var progress = new Progress<int>(percent =>
                {
                    DocMigProgress.Value = percent;
                });
                var logStatus = new Progress<string>(logText =>
                {
                    txtLogStatus.AppendText(logText);
                    txtLogStatus.ScrollToEnd();
                });
                adminConsoleHelper.DocLogStatus = logStatus;
                adminConsoleHelper.DocMigProgress = progress;
                if (!string.IsNullOrEmpty(ConfigurationHelper.SourceDatabase))
                {
                    //var expActionTypeId = cboExecutionType.SelectedIndex;
                    btnExportData.IsEnabled = false;
                    btnCloseApplication.IsEnabled = false;
                    ProgressHeading.Text = $"Processing {exportActionType} file...";
                    await Task.Run(() => adminConsoleHelper.ExportSourceData(exportActionType, ConfigurationHelper.SourceDatabase, adminConsoleHelper));
                    ProgressHeading.Text = $"Process compeleted for {exportActionType}";
                    btnExportData.IsEnabled = true;
                    btnCloseApplication.IsEnabled = true;
                }

                if (ExportType == "S")
                {
                    System.Windows.MessageBox.Show("Process compeleted. Please see log for detail");
                }
            }
            catch (Exception ex)
            {
                if (ExportType == "S")
                {
                    System.Windows.MessageBox.Show("Process compeleted. Please see log for detail");
                }
                ProgressHeading.Text = $"Error in Processing {exportActionType}";
                btnExportData.IsEnabled = true;
                btnCloseApplication.IsEnabled= true;
                StringBuilder exceptionMessage = new StringBuilder();
                while (ex != null)
                {
                    exceptionMessage.Append(ex.Message);
                    ex = ex.InnerException;
                }

               txtLogStatus.AppendText(exceptionMessage.ToString() + "\nProcess did not compelete successfully");

            }
        } 

        

        private void btnCloseApplication_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void cboExecutionType_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
           var index= cboExecutionType.SelectedIndex-1;
            var actionType = (ExportDataType)index;
            //var dbPrefix = ConfigurationHelper.ExportDataDBPrefix[actionType];
            //var dbSuffix = txtSourceClient.Text;
            //var i = txtSourceClient.Text.IndexOf(" ");
            //if (i != -1) {
            //    dbSuffix=txtSourceClient.Text.Substring(0, i);
            //}
           
            //txtSourceDatabase.Text = dbPrefix + dbSuffix;
        }
    }

    
}
