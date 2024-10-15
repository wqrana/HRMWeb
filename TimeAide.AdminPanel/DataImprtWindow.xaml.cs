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
using System.Collections.ObjectModel;
using TimeAide.AdminPanel.Models;

//using Microsoft.Office.Interop.Excel;


namespace TimeAide.AdminPanel
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml7 
    /// </summary>
    public partial class DataImprtWindow : Window
    {
        //AdminConsoleHelper adminConsoleHelper;
        AdminPanelImportHelper adminConsoleHelper;
        public DataImprtWindow()
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
            adminConsoleHelper = new AdminPanelImportHelper(txtLogStatus);
            
            txtSourceClient.Text = ConfigurationHelper.ClientName;
            txtTA7Database.Text = ConfigurationHelper.TA7ExportDatabase;           
         
          
        }
       
        private void btnBrowseFilePath_Click(object sender, RoutedEventArgs e)
        {
            System.Windows.Forms.OpenFileDialog openFileDlg = new System.Windows.Forms.OpenFileDialog();
            openFileDlg.Filter = "xlsx files (*.xlsx)|*.xlsx";
            var result = openFileDlg.ShowDialog();
            if (result== System.Windows.Forms.DialogResult.OK)
            {
                txtImportFilePath.Text = openFileDlg.FileName;
                ConfigurationHelper.AppRootPath = openFileDlg.FileName;                
            }            
        }
       
      private (int,string) validateImportFileDatabase()
      {
            var retCode = 1;
            var retMsg = string.Empty;
           
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
                    retMsg = $"Missing TA7 database";
                    retCode=0 ;
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
        private async Task StartProcessingData()
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
                adminConsoleHelper.ImpLogStatus = logStatus;
                adminConsoleHelper.ImportProgress = progress;
                enabledDisabledControls(false);
                
               ProgressHeading.Text = $"Processing Payroll file...";
               await Task.Run(() => adminConsoleHelper.ProcessImportedPayrollData());
               ProgressHeading.Text = $"Process compeleted for Payroll file";
               enabledDisabledControls(true);
            }
            catch (Exception ex)
            {

                System.Windows.MessageBox.Show("Process compeleted. Please see log for detail");

                ProgressHeading.Text = $"Error in Processing file";
                enabledDisabledControls(true);
                StringBuilder exceptionMessage = new StringBuilder();
                while (ex != null)
                {
                    exceptionMessage.Append(ex.Message);
                    ex = ex.InnerException;
                }

                txtLogStatus.AppendText(exceptionMessage.ToString() + "\nProcess did not compelete successfully");

            }
        }         
        private void enabledDisabledControls(bool isEnable)
        {
            if (isEnable)
            {
                btnCloseApplication.IsEnabled = true;
                btnProcessImportData.IsEnabled = true;
                btnSaveMappingData.IsEnabled = true;
                btnShowFieldsMapping.IsEnabled = true;
            }
            else
            {
                btnCloseApplication.IsEnabled = false;
                btnProcessImportData.IsEnabled = false;
                btnSaveMappingData.IsEnabled = false;
                btnShowFieldsMapping.IsEnabled = false;
            }
        }
        private void btnCloseApplication_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void cboExecutionType_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
           
        }

        private void btnShowFieldMapping_Click(object sender, RoutedEventArgs e)
        {
            // Console.WriteLine("data grid clicked");
            var selectedItem = dgImportFile.SelectedItem;
            if (selectedItem == null)
            {
                return;
            }
            var mappingPopup = new MappingPopupWindow();           
            var valObj = (PayrollImportFieldMapping)selectedItem;
            mappingPopup.SelectedDatagridItem = valObj;
            mappingPopup.SelectedMappingFieldItem = new PayrollImportFieldMapping() { FieldIndex= valObj.FieldIndex,FieldName= valObj.FieldName,FieldNameAsValue=valObj.FieldNameAsValue, FieldMapping=valObj.FieldMapping,MappingTable=valObj.MappingTable,MappingColumn=valObj.MappingColumn,ColumnDataType=valObj.ColumnDataType };
            mappingPopup.AdminConsoleHelperService = adminConsoleHelper;
            
            mappingPopup.Owner = this;
            var result = mappingPopup.ShowDialog();
            if(result == true)
            {
                dgImportFile.Items.Refresh();
                //.ItemsSource = adminConsoleHelper.PayrollImportFieldMappingData;
            }
;

        }
         
        private void btnSaveMappingData_Click(object sender, RoutedEventArgs e)
        {
            if (adminConsoleHelper.PayrollImportFieldMappingData.Count > 0)
            {
                if (string.IsNullOrEmpty(txtImportFileTemplateName.Text.Trim()))
                {
                    System.Windows.MessageBox.Show("Please enter template name");
                    return;
                }
                else
                {
                   var unmappedCount = adminConsoleHelper.PayrollImportFieldMappingData.Where(w=>w.MappingStatus.Contains("Pending")||w.MappingStatus.Contains("Doesn't match")).Count();
                    if(unmappedCount>0)
                    {
                        System.Windows.MessageBox.Show("Please map all Pending fields. If there is any error, please check in Mapping status field.");
                        return;
                    }
                }

            }
            else
            {
                System.Windows.MessageBox.Show("No data found to save");
                return;
            }
            var templateId = int.Parse(txtImportFileTemplateId.Text.Trim());
            var templateName = txtImportFileTemplateName.Text.Trim();

            var result = adminConsoleHelper.SavePayrollImportTemplate(templateId,templateName);
            System.Windows.MessageBox.Show(result.Item2);
            adminConsoleHelper.IsPayrollImportFieldMappingChanged = false;
            if(result.Item1==1 && templateId == 0)
            {
               var id= adminConsoleHelper.PayrollImportFieldMappingData.Max(w => w.TemplateId);
                txtImportFileTemplateId.Text = id.ToString();
                txtImportFileTemplateName.Text = templateName;
                txtImportFileTemplateName.IsEnabled = false;
                btnProcessImportData.IsEnabled = true;
            }
        }

        private void btnProcessImportData_Click(object sender, RoutedEventArgs e)
        {
            if (adminConsoleHelper.IsPayrollImportFieldMappingChanged)
            {
                System.Windows.MessageBox.Show("Please save mapping data changes.");
                return;
            }
            importTab.SelectedIndex = 1;
            txtLogStatus.Text = string.Empty;
            StartProcessingData();
        }

        private void btnShowFieldsMapping_Click(object sender, RoutedEventArgs e)
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
                System.Windows.MessageBox.Show("Missing Import Type Template");
                return;
            }
            if (!string.IsNullOrEmpty(txtImportFilePath.Text.Trim()))
            {
                ConfigurationHelper.AppRootPath = txtImportFilePath.Text;
            }
            else
            {
                System.Windows.MessageBox.Show("Please browse and select the xlsx file.");
                return;
            }
            adminConsoleHelper.GetPayrollColumnList();
            var selectedTemplateId = (int)cboExecutionType.SelectedValue;
            txtImportFileTemplateId.Text = selectedTemplateId.ToString();
            var ret = adminConsoleHelper.ProcessPayrollExcelFileMapping(selectedTemplateId);
            txtNotify.Text = ret.Item2;
            if (adminConsoleHelper.PayrollImportFieldMappingData.Count > 0)
            {
                if (selectedTemplateId == 0)
                {
                    txtImportFileTemplateName.IsEnabled = true;
                    txtImportFileTemplateName.Text = string.Empty;
                }
                else
                {
                    txtImportFileTemplateName.Text = cboExecutionType.Text;
                    txtImportFileTemplateName.IsEnabled = false;
                }

                if(selectedTemplateId>0 && ret.Item1 == 1)
                {
                    btnProcessImportData.IsEnabled = true;
                }
                else
                {
                    btnProcessImportData.IsEnabled = false;
                }
                btnSaveMappingData.IsEnabled = true;
                adminConsoleHelper.IsPayrollImportFieldMappingChanged= false;
                dgImportFile.ItemsSource = adminConsoleHelper.PayrollImportFieldMappingData;
            }
        }

        private void btnLoadTemplates_Click(object sender, RoutedEventArgs e)
        {
            var result = validateImportFileDatabase();
            if (result.Item1 == 0)
            {
                System.Windows.MessageBox.Show(result.Item2);
                return;
            }
           
            cboExecutionType.IsEnabled = false;
            adminConsoleHelper.InitTAWinPayrollImportDBContext();
            var templates = adminConsoleHelper.GetPayrollImportFileTemplates();
            cboExecutionType.ItemsSource = templates;                      
            cboExecutionType.DisplayMemberPath = "sTemplateName";
            cboExecutionType.SelectedValuePath = "intTemplateId";
           
            cboExecutionType.IsEnabled = true;

        }
    }

    
}
