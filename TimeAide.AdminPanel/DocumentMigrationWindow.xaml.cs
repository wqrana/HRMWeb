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

namespace TimeAide.AdminPanel
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml7 
    /// </summary>
    public partial class DocumentMigrationWindow : Window
    {
        AdminConsoleHelper adminConsoleHelper;
        List<DataMigrationLog> databaseCreationLog = null;        
        DataTable dataTable = null;
        public DocumentMigrationWindow()
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
            txtTargetDatabase.Text = adminConsoleHelper.DbContext.Database.Connection.Database;
            adminConsoleHelper.TargetDatabase = txtTargetDatabase.Text;
            
            txtSourceDatabase.Text = ConfigurationHelper.SourceDatabase;
            txtAppRootPath.Text = ConfigurationHelper.AppRootPath;
            txtSourceClient.Text = ConfigurationHelper.ClientName;
            LoadDropdown();
        }

        private void LoadDropdown()
        {
            cboExecutionType.Items.Add("Employee Documents");
            cboExecutionType.Items.Add("Employee Credentials");
            cboExecutionType.Items.Add("Employee Educations");
            cboExecutionType.Items.Add("Employee Trainings");
            cboExecutionType.Items.Add("Employee Performances");
            cboExecutionType.Items.Add("Employment Contracts");

            cboExecutionType.SelectedIndex = (int)DocumentActionType.EmployeeDocuments;
        }

        private void BtnExecuteMigration_Click(object sender, RoutedEventArgs e)
        {
            if (!string.IsNullOrEmpty(txtSourceDatabase.Text.Trim()))
            {
                ConfigurationHelper.SourceDatabase = txtSourceDatabase.Text;
            }
            else
            {
                MessageBox.Show("Missing source database");
                return;
            }
            if (!string.IsNullOrEmpty(txtSourceClient.Text.Trim()))
            {
                ConfigurationHelper.ClientName = txtSourceClient.Text;
            }
            else
            {
                MessageBox.Show( "Missing client name");
                return;
            }
            //if (adminConsoleHelper.TargetDatabase != txtTargetDatabase.Text)
            {
                //SqlConnectionStringBuilder conn = new SqlConnectionStringBuilder(adminConsoleHelper.DbContext.Database.Connection.ConnectionString)
                //{ InitialCatalog = txtTargetDatabase.Text }; // you can add other parameters.
                SqlConnectionStringBuilder conn = new SqlConnectionStringBuilder(ConfigurationHelper.ConnectionString)
                { InitialCatalog = txtTargetDatabase.Text }; // you can add other parameters.
                ConfigurationHelper.ConnectionString= conn.ConnectionString;
                adminConsoleHelper.DbContext = new TimeAideContext(conn.ConnectionString);
                adminConsoleHelper.UpdateContext();
            }
            if (!adminConsoleHelper.DbContext.Database.Exists())
            {
               // MessageBox.Show(adminConsoleHelper.DbContext.Database.Connection.ConnectionString);
                MessageBox.Show("Target Database " + adminConsoleHelper.DbContext.Database.Connection.Database + " does not exsist.");
                return;
            }
                Client client = client = adminConsoleHelper.GetMigrationClient();
                if (client == null)
                {
                    MessageBox.Show("Invalid client name");
                    return;

                }
                if (!Directory.Exists(txtAppRootPath.Text))
                {
                    MessageBox.Show("Application root path doesn't exist.");
                    return;
                }
            DocMigProgress.Value = 0;
            txtLogStatus.Text="";
            StartMigration(client);


        }
        private async Task StartMigration(Client client)
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
                    var docMigType = cboExecutionType.SelectedIndex;
                    btnExecuteDocMigration.IsEnabled = false;
                    btnCloseApplication.IsEnabled = false;
                    //adminConsoleHelper.DocumentMigration(client, (DocumentActionType)docMigType, ConfigurationHelper.SourceDatabase, adminConsoleHelper);
                    await Task.Run(() => adminConsoleHelper.DocumentMigration(client, (DocumentActionType)docMigType, ConfigurationHelper.SourceDatabase, adminConsoleHelper));
                    btnExecuteDocMigration.IsEnabled = true;
                    btnCloseApplication.IsEnabled = true;
                }


               MessageBox.Show("Process compeleted. Please see log for detail");
            }
            catch (Exception ex)
            {
                MessageBox.Show("Process compeleted. Please see log for detail");
                btnExecuteDocMigration.IsEnabled = true;
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
    }

    
}
