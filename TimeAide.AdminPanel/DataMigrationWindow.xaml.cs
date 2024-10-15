using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
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
using TimeAide.AdminPanel.Helpers;
using TimeAide.Web.Models;

namespace TimeAide.AdminPanel
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml7 
    /// </summary>
    public partial class DataMigrationWindow : Window
    {
        AdminConsoleHelper adminConsoleHelper;
        List<DataMigrationLog> databaseCreationLog = null;
        bool isNewDatabase = false;
        public DataMigrationWindow()
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
            if (!adminConsoleHelper.DbContext.Database.Exists())
            {
                lblTargetDatabaseType.Text = "Target Database " + adminConsoleHelper.DbContext.Database.Connection.Database + " does not exsist.\n" +
                                             "Fresh database will be created?";
            }
            else
            {
                lblTargetDatabaseType.Text = "Target Database " + adminConsoleHelper.DbContext.Database.Connection.Database + " already exsist.\n" +
                    "Executing database migration will append information in current database.";
                databaseCreationLog = adminConsoleHelper.LogHelper.GetPreviousMigrationHistory(1);
            }

            if (ConfigurationHelper.ExecutionType != null && ConfigurationHelper.ExecutionType == "CreateEmptyDatabase")
            {
                lblExecutionType.Text = ("This execution is set to create empty database only, do you want to procced?");
            }
            else
            {
                lblExecutionType.Text = ("This execution is set to migrate data from " + ConfigurationHelper.SourceDatabase + " database to\n " + adminConsoleHelper.DbContext.Database.Connection.Database);
            }

            txtSourceDatabase.Text = ConfigurationHelper.SourceDatabase;
            txtSourceClient.Text = ConfigurationHelper.ClientName;
            LoadDropdown();
        }

        private void LoadDropdown()
        {
            cboExecutionType.Items.Add("Data Migration");
            cboExecutionType.Items.Add("New Empty Database");
            cboExecutionType.Items.Add("New Client");
            cboExecutionType.Items.Add("New Client With DefaultValues");
            cboExecutionType.SelectedIndex = (int)Enum.Parse(typeof(ExecutionType), ConfigurationHelper.ExecutionType);
        }

        private void BtnExecuteMigration_Click(object sender, RoutedEventArgs e)
        {
           // if (adminConsoleHelper.TargetDatabase != txtTargetDatabase.Text)
            {
                //SqlConnectionStringBuilder conn = new SqlConnectionStringBuilder(adminConsoleHelper.DbContext.Database.Connection.ConnectionString)
                //{ InitialCatalog = txtTargetDatabase.Text }; // you can add other parameters.
                SqlConnectionStringBuilder conn = new SqlConnectionStringBuilder(ConfigurationHelper.ConnectionString)
                { InitialCatalog = txtTargetDatabase.Text }; // you can add other parameters.
                ConfigurationHelper.ConnectionString = conn.ConnectionString;
                adminConsoleHelper.DbContext = new TimeAideContext(conn.ConnectionString);
               
                adminConsoleHelper.UpdateContext();
            }


            ConfigurationHelper.ClientName = txtSourceClient.Text;
            if (cboExecutionType.Text == "Data Migration")
                ConfigurationHelper.SourceDatabase = txtSourceDatabase.Text;
            else
                ConfigurationHelper.SourceDatabase = "";
            Client client = new Client();
            try
            {
                if (!adminConsoleHelper.DbContext.Database.Exists())
                {
                    if (MessageBox.Show("Target Database " + adminConsoleHelper.DbContext.Database.Connection.Database + " does not exsist.Do you want to continue with fresh database creation?", "", MessageBoxButton.YesNo) == MessageBoxResult.No)
                    {
                        MessageBox.Show("Thanks for using Time Aide Web, admin console.");
                        return;
                    }
                    if (ConfigurationHelper.ExecutionType != null && ConfigurationHelper.ExecutionType == "CreateEmptyDatabase")
                    {
                        if (MessageBox.Show("This execution is set to create empty database only, do you want to procced?", "", MessageBoxButton.YesNo) == MessageBoxResult.No)
                        {
                            MessageBox.Show("Thanks for using Time Aide Web, admin console.");
                            return;
                        }
                    }
                    adminConsoleHelper.LogHelper.LogProgress(1, LogEvent.CreatingDatabase.ToString(), "", "", 1);
                    adminConsoleHelper.LogHelper.LogProgress(1, LogEvent.CreatingDatabase.ToString(), "", "", 2);
                    adminConsoleHelper.DbContext.Database.ExecuteSqlCommand(@"CREATE UNIQUE NONCLUSTERED INDEX [IX_LoginEmail] ON [dbo].[UserContactInformation] ([LoginEmail]) WHERE [LoginEmail] IS NOT NULL");

                    if (ConfigurationHelper.ExecutionType != null && ConfigurationHelper.ExecutionType == "CreateEmptyDatabase")
                    {
                        MessageBox.Show("Process compeleted successfully.\nThanks for using Time Aide Web, admin console.");
                        return;
                    }

                    isNewDatabase = true;
                }
                else
                {
                    isNewDatabase = false;
                    if (MessageBox.Show("Target Database " + adminConsoleHelper.DbContext.Database.Connection.Database + " already exsist.\nExecuting database migration will append information in current database.\nDo you to continue?", "", MessageBoxButton.YesNo) == MessageBoxResult.No)
                    {
                        MessageBox.Show("Thanks for using Time Aide Web, admin console.");
                        return;
                    }
                    //databaseCreationLog = adminConsoleHelper.LogHelper.GetPreviousMigrationHistory(1);
                }
                DataMigrationLog log;
                databaseCreationLog = adminConsoleHelper.LogHelper.GetPreviousMigrationHistory(1);
                if (!adminConsoleHelper.ScriptsHelper.CreateScripts("0-ASPNet Schema", client.Id, LogEvent.CreateASPNET_Tables, isNewDatabase, ConfigurationHelper.SourceDatabase))
                    return;
                SetupIdntechClient(adminConsoleHelper, databaseCreationLog, isNewDatabase);
                //AddDBOwner(adminConsoleHelper, databaseCreationLog, isNewDatabase);
                //System.Diagnostics.Process.Start("TimeAide.AdminConsole.exe");
                if (!CreateViewsAndSp(adminConsoleHelper, databaseCreationLog, isNewDatabase, ConfigurationHelper.SourceDatabase))
                    return;
                if (!SetupIdenTechSuperAdmin(adminConsoleHelper, databaseCreationLog, isNewDatabase))
                    return;
                if (!ExecuteApplicationGlobalDefaultValuesSP(adminConsoleHelper, databaseCreationLog, isNewDatabase))
                    return;


                CreateRoles(adminConsoleHelper, databaseCreationLog, 1);
                //CreateEmployeeGroups(adminConsoleHelper, databaseCreationLog, 1);

                List<DataMigrationLog> previousLog = null;
                var migratedDatabaseHistory = adminConsoleHelper.LogHelper.SearchInMigratedDatabase(ConfigurationHelper.SourceDatabase);
                if (migratedDatabaseHistory != null && migratedDatabaseHistory.Count > 0)
                {
                    var sourceClient = adminConsoleHelper.GetClient(migratedDatabaseHistory.First().ClientId ?? 0);
                    if (sourceClient != null && sourceClient.ClientName != ConfigurationHelper.ClientName)
                    {
                        MessageBox.Show("Source database " + ConfigurationHelper.SourceDatabase + " already migrated for client " + sourceClient.ClientName + ".\n" +
                            "You are again trying to migrate database " + ConfigurationHelper.SourceDatabase + " for client " + ConfigurationHelper.ClientName + ".\n" +
                            "Please verify migration settings and try again.\n" +
                            "Thanks for using Time Aide Web, admin console.");
                        return;
                    }
                }
                client = adminConsoleHelper.GetMigrationClient();
                if (client == null)
                {
                    client = adminConsoleHelper.CreateMigrationClient();
                    log = adminConsoleHelper.LogHelper.LogProgress(client.Id, LogEvent.CreateMigrationClient.ToString(), "", ConfigurationHelper.SourceDatabase, 1);
                }
                else
                {
                    previousLog = adminConsoleHelper.LogHelper.GetPreviousMigrationHistory(client.Id);
                    if (previousLog != null || previousLog.Count > 0)
                    {
                        var pLog = previousLog.FirstOrDefault(l => l.LogName == "CreateMigrationClient");
                        if (pLog != null && pLog.LogRemarks != ConfigurationHelper.SourceDatabase)
                        {
                            MessageBox.Show("Selected client " + client.ClientName + " has migrated data from database " + pLog.LogDescription + ".\nCurrently you are trying to migrate data from another database " + ConfigurationHelper.SourceDatabase + ".\nPlease verify migration settings and try again\n" +
                                "Thanks for using Time Aide Web, admin console.");
                            return;
                        }
                    }
                    if (MessageBox.Show("Migration Client " + client.ClientName + " already exsist.\nExecuting database migration will append information to above client.\nDo you to continue?", "", MessageBoxButton.YesNo) == MessageBoxResult.No)
                    {
                        MessageBox.Show("Thanks for using Time Aide Web, admin console.");
                        return;
                    }
                    //MessageBox.Show("System is looking for previous progress.");
                    if (previousLog == null || previousLog.Count == 0)
                    {
                        //MessageBox.Show("There is no history for previous migration. System will continue migration progress");
                    }
                    else
                    {
                        MessageBox.Show("Below are the completed events of previous migration.");
                        foreach (var each in previousLog.Select(p => p).Distinct())
                        {
                            txtLogStatus.AppendText(each.LogName + "\n");
                        }
                    }
                }

                CreateRoles(adminConsoleHelper, previousLog, client.Id);
                //CreateEmployeeGroups(adminConsoleHelper, databaseCreationLog, 1);

                if (!CreateScripts(client, adminConsoleHelper, isNewDatabase, previousLog, ConfigurationHelper.SourceDatabase))
                    return;
                if (!ExecuteDefaultValuesScripts(client, adminConsoleHelper, isNewDatabase, previousLog))
                    return;
                if (!string.IsNullOrEmpty(ConfigurationHelper.SourceDatabase) && cboExecutionType.Text == "Data Migration")
                {
                    //if (!CreateNonCompany(client, adminConsoleHelper))
                    //    return;
                    if (!ExecuteMigrationScripts(client, adminConsoleHelper, isNewDatabase, previousLog))
                        return;
                }
                if (!ExecuteDefaultValuesForEmptySourceScripts(client, adminConsoleHelper, isNewDatabase, previousLog))
                    return;
                if (!ExecuteCompanyWizeDataChangeScripts(client, adminConsoleHelper, isNewDatabase, previousLog))
                    return;
                if (!CleanupMigrationScripts(client, adminConsoleHelper, isNewDatabase, previousLog))
                    return;
                //if (!string.IsNullOrEmpty(ConfigurationHelper.SourceDatabase))
                //{
                //    if (!adminConsoleHelper.DownloadDocuments(client, ConfigurationHelper.SourceDatabase, adminConsoleHelper, isNewDatabase, previousLog))
                //        return;
                //}
                if (!CreateEmployeeTransferDefaultValues(client, adminConsoleHelper))
                    return;

                MessageBox.Show("Process compeleted successfully");
            }
            catch (Exception ex)
            {
                MessageBox.Show("Process did not compelete successfully");
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

                adminConsoleHelper.LogHelper.LogProgress(client.Id, LogEvent.Exception.ToString(), exceptionMessageLog, stackTraceLog, 1);

                txtLogStatus.AppendText(exceptionMessage.ToString() + "\nProcess did not compelete successfully");
                // txtLogStatus.Dispatcher.InvokeAsync(
                //new Action(() => txtLogStatus.AppendText(exceptionMessage.ToString() + "\nProcess did not compelete successfully")));
            }
        }

        private bool CreateRoles(AdminConsoleHelper adminConsoleHelper, List<DataMigrationLog> databaseCreationLog, int clientId)
        {
            DataMigrationLog log;
            log = adminConsoleHelper.LogHelper.GetLog(databaseCreationLog, LogEvent.ExecuteCreateRoles, clientId);
            if (!adminConsoleHelper.ScriptsHelper.ExecuteDataMigrationSP(clientId, "sp_CreateDefaultRole", log))
                return false;
            log = adminConsoleHelper.LogHelper.LogProgress(clientId, LogEvent.ExecuteCreateRoles.ToString(), "", "", 2);

            return true;
        }

        //private bool CreateEmployeeGroups(AdminConsoleHelper adminConsoleHelper, List<DataMigrationLog> databaseCreationLog, int clientId)
        //{
        //    DataMigrationLog log;
        //    log = adminConsoleHelper.LogHelper.GetLog(databaseCreationLog, LogEvent.ExecuteCreateEmployeeGroup, clientId);
        //    if (!adminConsoleHelper.ScriptsHelper.ExecuteDataMigrationSP(clientId, "sp_CreateDefaultEmployeeGroupType", log))
        //        return false;
        //    log = adminConsoleHelper.LogHelper.LogProgress(clientId, LogEvent.ExecuteCreateEmployeeGroup.ToString(), "", "", 2);

        //    return true;
        //}

        private bool SetupIdenTechSuperAdmin(AdminConsoleHelper adminConsoleHelper, List<DataMigrationLog> databaseCreationLog, bool isNewDatabase)
        {
            DataMigrationLog log;
            if (isNewDatabase || !adminConsoleHelper.LogHelper.IsAlreadyExecuted(databaseCreationLog, LogEvent.ExecuteApplicationInformationSPs))
            {
                log = adminConsoleHelper.LogHelper.GetLog(databaseCreationLog, LogEvent.ExecuteApplicationInformationSPs, 1);
                foreach (var sp in Enum.GetValues(typeof(ApplicationInformationSPs)))
                {
                    if (!adminConsoleHelper.ScriptsHelper.ExecuteDataMigrationSP(((ApplicationInformationSPs)sp).ToString(), log, isNewDatabase))
                        return false;
                }
                if (!adminConsoleHelper.ScriptsHelper.ExecuteDataMigrationSP(1, "sp_ExecuteSetupUserAsSuperAdmin", log))
                    return false;
                log = adminConsoleHelper.LogHelper.LogProgress(1, LogEvent.ExecuteApplicationInformationSPs.ToString(), "", "", 2);
            }
            return true;
        }

        private bool ExecuteApplicationGlobalDefaultValuesSP(AdminConsoleHelper adminConsoleHelper, List<DataMigrationLog> databaseCreationLog, bool isNewDatabase)
        {
            if (isNewDatabase || !adminConsoleHelper.LogHelper.IsAlreadyExecuted(databaseCreationLog, LogEvent.ExecuteApplicationGlobalDefaultValuesSP))
            {
                if (!adminConsoleHelper.ScriptsHelper.CreateScripts("1-DefaultValuesGlobal", 1, LogEvent.CreateDefaultValuesSP, isNewDatabase, null, "Global Values"))
                    return false;
            }
            DataMigrationLog log;
            if (isNewDatabase || !adminConsoleHelper.LogHelper.IsAlreadyExecuted(databaseCreationLog, LogEvent.ExecuteApplicationGlobalDefaultValuesSP))
            {
                log = adminConsoleHelper.LogHelper.GetLog(databaseCreationLog, LogEvent.ExecuteApplicationGlobalDefaultValuesSP, 1);
                foreach (var sp in Enum.GetValues(typeof(ApplicationGlobalDefaultValuesSP)))
                {
                    if (!adminConsoleHelper.ScriptsHelper.ExecuteDataMigrationSP(((ApplicationGlobalDefaultValuesSP)sp).ToString(), log, isNewDatabase))
                        return false;
                }
                log = adminConsoleHelper.LogHelper.LogProgress(1, LogEvent.ExecuteApplicationGlobalDefaultValuesSP.ToString(), "", "", 2);
            }
            if (isNewDatabase || !adminConsoleHelper.LogHelper.IsAlreadyExecuted(databaseCreationLog, LogEvent.DropApplicationGlobalDefaultValuesSP))
            {
                log = adminConsoleHelper.LogHelper.LogProgress(1, LogEvent.DropApplicationGlobalDefaultValuesSP.ToString(), "", "", 1);
                foreach (var sp in Enum.GetValues(typeof(ApplicationGlobalDefaultValuesSP)))
                {
                    if (!adminConsoleHelper.ScriptsHelper.DropDataMigrationSP(1, ((ApplicationGlobalDefaultValuesSP)sp).ToString(), log))
                        return false;
                }
                log = adminConsoleHelper.LogHelper.LogProgress(1, LogEvent.DropApplicationGlobalDefaultValuesSP.ToString(), "", "", 2);
            }
            return true;
        }

        private bool CreateViewsAndSp(AdminConsoleHelper adminConsoleHelper, List<DataMigrationLog> databaseCreationLog, bool isNewDatabase, string sourceDatabase)
        {
            if (!adminConsoleHelper.ScriptsHelper.CreateScripts("2-Functions", 1, LogEvent.CreateFunctions, isNewDatabase, databaseCreationLog, sourceDatabase))
                return false;
            if (!adminConsoleHelper.ScriptsHelper.CreateScripts("5-Views", 1, LogEvent.CreateViews, isNewDatabase, databaseCreationLog, sourceDatabase))
                return false;
            if (!adminConsoleHelper.ScriptsHelper.CreateScripts("3-StoredProcedures", 1, LogEvent.CreateStoredProcedures, isNewDatabase, databaseCreationLog, sourceDatabase))
                return false;
            if (!adminConsoleHelper.ScriptsHelper.CreateScripts("4-UserLogin", 1, LogEvent.CreateApplicationInformationSPs, isNewDatabase, databaseCreationLog, sourceDatabase))
                return false;
            return true;
        }
        private void AddDBOwner(AdminConsoleHelper adminConsoleHelper, List<DataMigrationLog> databaseCreationLog, bool isNewDatabase)
        {
            DataMigrationLog log;
            if (isNewDatabase || !adminConsoleHelper.LogHelper.IsAlreadyExecuted(databaseCreationLog, LogEvent.AddDBOwner))
            {
                log = adminConsoleHelper.LogHelper.GetLog(databaseCreationLog, LogEvent.AddDBOwner, 1);
                adminConsoleHelper.CreateAddDBOwner();
                log = adminConsoleHelper.LogHelper.LogProgress(1, LogEvent.AddDBOwner.ToString(), "", "", 2);
            }
        }
        private void SetupIdntechClient(AdminConsoleHelper adminConsoleHelper, List<DataMigrationLog> databaseCreationLog, bool isNewDatabase)
        {
            DataMigrationLog log;
            UserInformation userInformation = null;
            if (isNewDatabase || !adminConsoleHelper.LogHelper.IsAlreadyExecuted(databaseCreationLog, LogEvent.SetupIdenTechInformation))
            {
                log = adminConsoleHelper.LogHelper.GetLog(databaseCreationLog, LogEvent.SetupIdenTechInformation, 1);

                if (isNewDatabase || !log.DataMigrationLogDetail.Any(d => d.LogCommandName == LogDetailEvents.CreateSeedUser.ToString()))
                {
                    userInformation = adminConsoleHelper.CreateSeedUser();
                    adminConsoleHelper.LogHelper.LogProgressDetail(1, LogDetailEvents.CreateSeedUser.ToString(), "", "", log, 1);
                }

                if (isNewDatabase || !log.DataMigrationLogDetail.Any(d => d.LogCommandName == LogDetailEvents.CreateDefaultUserLogin.ToString()))
                {
                    string loginEmail = ConfigurationHelper.DefaultAdminEmail;
                    //string userId = UserManagmentService.Create(loginEmail, ";Identech01");
                    //userInformation.AspNetUserId = userId;
                    adminConsoleHelper.LogHelper.LogProgressDetail(1, LogDetailEvents.CreateDefaultUserLogin.ToString(), "", "", log, 1);
                }

                log = adminConsoleHelper.LogHelper.LogProgress(1, LogEvent.SetupIdenTechInformation.ToString(), "", "", 2);
            }
        }
        private bool CreateScripts(Client client, AdminConsoleHelper adminConsoleHelper, bool isNewDatabase, List<DataMigrationLog> previousLog, string sourceDatabase)
        {
            
            if (!adminConsoleHelper.ScriptsHelper.CreateScripts("1-DefaultValues", client.Id, LogEvent.CreateDefaultValuesSP, isNewDatabase, previousLog, sourceDatabase))
                return false;
            if (!string.IsNullOrEmpty(sourceDatabase))
            {
                if (!adminConsoleHelper.ScriptsHelper.CreateScripts("6-Migration Scripts\\1-Master Data", client.Id, LogEvent.CreateMigrationScriptsMasterData, isNewDatabase, previousLog, sourceDatabase))
                    return false;
                if (!adminConsoleHelper.ScriptsHelper.CreateScripts("6-Migration Scripts\\2-EmployeeInformation", client.Id, LogEvent.CreateMigrationScriptsEmployeeInformation, isNewDatabase, previousLog, sourceDatabase))
                    return false;
                if (!adminConsoleHelper.ScriptsHelper.CreateScripts("6-Migration Scripts\\3-EmployeeTabs", client.Id, LogEvent.CreateMigrationScriptsEmployeeTabs, isNewDatabase, previousLog, sourceDatabase))
                    return false;
            }
            if (!adminConsoleHelper.ScriptsHelper.CreateScripts("7-DefaultValuesForEmptySource", client.Id, LogEvent.CreateDefaultValuesForEmptySource, isNewDatabase, previousLog, sourceDatabase))
                return false;

            if (!adminConsoleHelper.ScriptsHelper.CreateScripts("8-CompanyWizeDataChange", client.Id, LogEvent.DropCompanyWizeDataChange, isNewDatabase, previousLog, sourceDatabase))
                return false;
            return true;
        }
        private bool ExecuteDefaultValuesScripts(Client client, AdminConsoleHelper adminConsoleHelper, bool isNewDatabase, List<DataMigrationLog> previousLog)
        {
            DataMigrationLog log = new DataMigrationLog();
            if (isNewDatabase || !adminConsoleHelper.LogHelper.IsAlreadyExecuted(previousLog, LogEvent.ExecuteDefaultValuesSP))
            {
                log = adminConsoleHelper.LogHelper.LogProgress(client.Id, LogEvent.ExecuteDefaultValuesSP.ToString(), "", "", 1);
                foreach (var sp in Enum.GetValues(typeof(DefaultValueSPs)))
                    if (!adminConsoleHelper.ScriptsHelper.ExecuteDataMigrationSP(client.Id, ((DefaultValueSPs)sp).ToString(), log))
                        return false;
                log = adminConsoleHelper.LogHelper.LogProgress(client.Id, LogEvent.ExecuteDefaultValuesSP.ToString(), "", "", 2);
            }
            return true;
        }
        //private bool CreateNonCompany(Client client, AdminConsoleHelper adminConsoleHelper)
        //{
        //    adminConsoleHelper.CreateCompany("None-Company", client.Id, 2);
        //    adminConsoleHelper.DbContext.SaveChanges();
        //    return true;
        //}
        private bool CreateEmployeeTransferDefaultValues(Client client, AdminConsoleHelper adminConsoleHelper)
        {
            adminConsoleHelper.CreateTerminationReason("Employee Transfer", client.Id, 2);
            adminConsoleHelper.CreateTerminationType("Employee Transfer", client.Id, 2);
            adminConsoleHelper.DbContext.SaveChanges();
            return true;
        }
        private bool ExecuteMigrationScripts(Client client, AdminConsoleHelper adminConsoleHelper, bool isNewDatabase, List<DataMigrationLog> previousLog)
        {
            DataMigrationLog log = new DataMigrationLog();

            if (isNewDatabase || !adminConsoleHelper.LogHelper.IsAlreadyExecuted(previousLog, LogEvent.ExecuteMigrationScriptsMasterData))
            {
                log = adminConsoleHelper.LogHelper.LogProgress(client.Id, LogEvent.ExecuteMigrationScriptsMasterData.ToString(), "", "", 1);
                foreach (var sp in Enum.GetValues(typeof(MasterDataSPs)))
                    if (!adminConsoleHelper.ScriptsHelper.ExecuteDataMigrationSP(client.Id, ((MasterDataSPs)sp).ToString(), log))
                        return false;
                log = adminConsoleHelper.LogHelper.LogProgress(client.Id, LogEvent.ExecuteMigrationScriptsMasterData.ToString(), "", "", 2);
            }
            if (isNewDatabase || !adminConsoleHelper.LogHelper.IsAlreadyExecuted(previousLog, LogEvent.ExecuteMigrationScriptsEmployeeInformation))
            {
                log = adminConsoleHelper.LogHelper.LogProgress(client.Id, LogEvent.ExecuteMigrationScriptsEmployeeInformation.ToString(), "", "", 1);
                foreach (var sp in Enum.GetValues(typeof(EmployeeInformationSPs)))
                    if (!adminConsoleHelper.ScriptsHelper.ExecuteDataMigrationSP(client.Id, ((EmployeeInformationSPs)sp).ToString(), log))
                        return false;
                log = adminConsoleHelper.LogHelper.LogProgress(client.Id, LogEvent.ExecuteMigrationScriptsEmployeeInformation.ToString(), "", "", 2);
            }
            if (isNewDatabase || !adminConsoleHelper.LogHelper.IsAlreadyExecuted(previousLog, LogEvent.ExecuteMigrationScriptsEmployeeTabs))
            {
                log = adminConsoleHelper.LogHelper.LogProgress(client.Id, LogEvent.ExecuteMigrationScriptsEmployeeTabs.ToString(), "", "", 1);
                foreach (var sp in Enum.GetValues(typeof(EmployeeTabsSPs)))
                    if (!adminConsoleHelper.ScriptsHelper.ExecuteDataMigrationSP(client.Id, ((EmployeeTabsSPs)sp).ToString(), log))
                        return false;
                log = adminConsoleHelper.LogHelper.LogProgress(client.Id, LogEvent.ExecuteMigrationScriptsEmployeeTabs.ToString(), "", "", 2);
            }
            return true;
        }
        private bool ExecuteDefaultValuesForEmptySourceScripts(Client client, AdminConsoleHelper adminConsoleHelper, bool isNewDatabase, List<DataMigrationLog> previousLog)
        {
            DataMigrationLog log = new DataMigrationLog();
            if (isNewDatabase || !adminConsoleHelper.LogHelper.IsAlreadyExecuted(previousLog, LogEvent.ExecuteDefaultValuesForEmptySource))
            {
                log = adminConsoleHelper.LogHelper.LogProgress(client.Id, LogEvent.ExecuteDefaultValuesForEmptySource.ToString(), "", "", 1);
                foreach (var sp in Enum.GetValues(typeof(DefaultValueForEmptySourceSPs)))
                    if (!adminConsoleHelper.ScriptsHelper.ExecuteDataMigrationSP(client.Id, ((DefaultValueForEmptySourceSPs)sp).ToString(), log))
                        return false;
                log = adminConsoleHelper.LogHelper.LogProgress(client.Id, LogEvent.ExecuteDefaultValuesForEmptySource.ToString(), "", "", 2);
            }
            return true;
        }
        private bool ExecuteCompanyWizeDataChangeScripts(Client client, AdminConsoleHelper adminConsoleHelper, bool isNewDatabase, List<DataMigrationLog> previousLog)
        {
            DataMigrationLog log = new DataMigrationLog();
            if (isNewDatabase || !adminConsoleHelper.LogHelper.IsAlreadyExecuted(previousLog, LogEvent.ExecuteCompanyWizeDataChange))
            {
                log = adminConsoleHelper.LogHelper.LogProgress(client.Id, LogEvent.ExecuteDefaultValuesForEmptySource.ToString(), "", "", 1);
                foreach (var sp in Enum.GetValues(typeof(CompanyWizeDataChangeSPs)))
                    if (!adminConsoleHelper.ScriptsHelper.ExecuteDataMigrationSP(client.Id, ((CompanyWizeDataChangeSPs)sp).ToString(), log))
                        return false;
                log = adminConsoleHelper.LogHelper.LogProgress(client.Id, LogEvent.ExecuteCompanyWizeDataChange.ToString(), "", "", 2);
            }
            return true;
        }

        private bool CleanupMigrationScripts(Client client, AdminConsoleHelper adminConsoleHelper, bool isNewDatabase, List<DataMigrationLog> previousLog)
        {
            DataMigrationLog log = new DataMigrationLog();

            if (isNewDatabase || !adminConsoleHelper.LogHelper.IsAlreadyExecuted(previousLog, LogEvent.DropDefaultValuesSP))
            {
                log = adminConsoleHelper.LogHelper.LogProgress(client.Id, LogEvent.DropDefaultValuesSP.ToString(), "", "", 1);
                foreach (var sp in Enum.GetValues(typeof(DefaultValueSPs)))
                {
                    if (!adminConsoleHelper.ScriptsHelper.DropDataMigrationSP(client.Id, ((DefaultValueSPs)sp).ToString(), log))
                        return false;
                }
                log = adminConsoleHelper.LogHelper.LogProgress(client.Id, LogEvent.DropDefaultValuesSP.ToString(), "", "", 2);
            }


            if (!string.IsNullOrEmpty(ConfigurationHelper.SourceDatabase))
            {
                if (isNewDatabase || !adminConsoleHelper.LogHelper.IsAlreadyExecuted(previousLog, LogEvent.DropMigrationScriptsMasterData))
                {
                    log = adminConsoleHelper.LogHelper.LogProgress(client.Id, LogEvent.DropMigrationScriptsMasterData.ToString(), "", "", 1);
                    foreach (var sp in Enum.GetValues(typeof(MasterDataSPs)))
                    {
                        if (!adminConsoleHelper.ScriptsHelper.DropDataMigrationSP(client.Id, ((MasterDataSPs)sp).ToString(), log))
                            return false;
                    }
                    log = adminConsoleHelper.LogHelper.LogProgress(client.Id, LogEvent.DropMigrationScriptsMasterData.ToString(), "", "", 2);
                }
                if (isNewDatabase || !adminConsoleHelper.LogHelper.IsAlreadyExecuted(previousLog, LogEvent.DropMigrationScriptsEmployeeInformation))
                {
                    log = adminConsoleHelper.LogHelper.LogProgress(client.Id, LogEvent.DropMigrationScriptsEmployeeInformation.ToString(), "", "", 1);
                    foreach (var sp in Enum.GetValues(typeof(EmployeeInformationSPs)))
                    {
                        if (!adminConsoleHelper.ScriptsHelper.DropDataMigrationSP(client.Id, ((EmployeeInformationSPs)sp).ToString(), log))
                            return false;
                    }
                    log = adminConsoleHelper.LogHelper.LogProgress(client.Id, LogEvent.DropMigrationScriptsEmployeeInformation.ToString(), "", "", 2);
                }

                if (isNewDatabase || !adminConsoleHelper.LogHelper.IsAlreadyExecuted(previousLog, LogEvent.DropMigrationScriptsEmployeeTabs))
                {
                    log = adminConsoleHelper.LogHelper.LogProgress(client.Id, LogEvent.DropMigrationScriptsEmployeeTabs.ToString(), "", "", 1);
                    foreach (var sp in Enum.GetValues(typeof(EmployeeTabsSPs)))
                    {
                        if (!adminConsoleHelper.ScriptsHelper.DropDataMigrationSP(client.Id, ((EmployeeTabsSPs)sp).ToString(), log))
                            return false;
                    }
                    log = adminConsoleHelper.LogHelper.LogProgress(client.Id, LogEvent.DropMigrationScriptsEmployeeTabs.ToString(), "", "", 2);
                }
            }


            if (isNewDatabase || !adminConsoleHelper.LogHelper.IsAlreadyExecuted(previousLog, LogEvent.DropDefaultValuesForEmptySource))
            {
                log = adminConsoleHelper.LogHelper.LogProgress(client.Id, LogEvent.DropDefaultValuesForEmptySource.ToString(), "", "", 1);
                foreach (var sp in Enum.GetValues(typeof(DefaultValueForEmptySourceSPs)))
                {
                    if (!adminConsoleHelper.ScriptsHelper.DropDataMigrationSP(client.Id, ((DefaultValueForEmptySourceSPs)sp).ToString(), log))
                        return false;
                }
                log = adminConsoleHelper.LogHelper.LogProgress(client.Id, LogEvent.DropDefaultValuesForEmptySource.ToString(), "", "", 2);
            }

            if (isNewDatabase || !adminConsoleHelper.LogHelper.IsAlreadyExecuted(previousLog, LogEvent.DropCompanyWizeDataChange))
            {
                log = adminConsoleHelper.LogHelper.LogProgress(client.Id, LogEvent.DropCompanyWizeDataChange.ToString(), "", "", 1);
                foreach (var sp in Enum.GetValues(typeof(CompanyWizeDataChangeSPs)))
                {
                    if (!adminConsoleHelper.ScriptsHelper.DropDataMigrationSP(client.Id, ((CompanyWizeDataChangeSPs)sp).ToString(), log))
                        return false;
                }
                log = adminConsoleHelper.LogHelper.LogProgress(client.Id, LogEvent.DropCompanyWizeDataChange.ToString(), "", "", 2);
            }
            return true;
        }

        private void TxtTargetDatabase_LostFocus(object sender, RoutedEventArgs e)
        {
            //ConfigurationHelper.
        }

        private void CboExecutionType_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (cboExecutionType.SelectedItem.ToString() == "New Client")
            {
                txtSourceDatabase.Text = "";
                txtSourceDatabase.IsEnabled = false;
            }
            else
            {
                txtSourceDatabase.IsEnabled = true;
            }
        }
        DataTable dataTable = null;
        private void BtnExecutePreprcessing_Click(object sender, RoutedEventArgs e)
        {
            var sourceDatabase = txtSourceDatabase.Text;
            dataTable = adminConsoleHelper.GetInvalidHiringDate(sourceDatabase);
            if (dataTable != null)
                grdResult.ItemsSource = dataTable.DefaultView;
            tcOpenPages.SelectedIndex = 1;
        }

        private void BtnExportToExcel_Click(object sender, RoutedEventArgs e)
        {


            ExportAndEmail(false);

        }

        private void ExportAndEmail(bool sendEmail)
        {
            //Helpers.ExcelUtlity obj = new Helpers.ExcelUtlity();
            ////string path = "C:\\TimeAideWeb_Temp";
            ////if (!Directory.Exists(path))
            ////{
            ////    Directory.CreateDirectory(path);
            ////}
            //obj.WriteDataTableToExcel(dataTable, "Invalid Hiring Data", "D:\\testPersonExceldata.xlsx", "Details");
            string email = "";
            InputMessageBox inputDialog = new InputMessageBox("Please enter recipient email:", "");
            if (inputDialog.ShowDialog() == true)
                email = inputDialog.Answer;

            Helpers.ExcelUtlity obj = new Helpers.ExcelUtlity();
            string path = "C:\\TimeAideWeb_Temp";
            if (!Directory.Exists(path))
            {
                Directory.CreateDirectory(path);
            }
            path += "\\InvalidHiringData_" + txtSourceDatabase.Text + ".xlsx";
            obj.WriteDataTableToExcel(dataTable, "Invalid Hiring Data", path, "Details");
            if (sendEmail)
            {
                string message = "Attach excel file contains Invalid Hiring Data, please review and correct";
                Dictionary<string, string> attachmentKeyValuePairs = new Dictionary<string, string>();
                attachmentKeyValuePairs.Add("Invlid Hring data", path);

                Dictionary<string, string> toEmailKeyValuePairs = new Dictionary<string, string>();
                toEmailKeyValuePairs.Add(email, "");

                TimeAide.Services.Helpers.UtilityHelper.SendEmail(toEmailKeyValuePairs, "", "", attachmentKeyValuePairs, message, "Invalid Hiring Data");
            }
        }

        private void BtnSendEmailAlert_Click(object sender, RoutedEventArgs e)
        {
            ExportAndEmail(true);

        }

        private void BtnUpdateHireDateWithRehireDate_Click(object sender, RoutedEventArgs e)
        {
            var sourceDatabase = txtSourceDatabase.Text;
            adminConsoleHelper.UpdateHiringDateWithRehiringDate(sourceDatabase);

            var result = adminConsoleHelper.GetInvalidHiringDate(sourceDatabase);
            if (result != null)
                grdResult.ItemsSource = result.DefaultView;
            tcOpenPages.SelectedIndex = 1;
        }

        private void BtnUpdateReHireDateWithHireDate_Click(object sender, RoutedEventArgs e)
        {
            var sourceDatabase = txtSourceDatabase.Text;
            adminConsoleHelper.UpdateReHiringDateWithHiringDate(sourceDatabase);

            var result = adminConsoleHelper.GetInvalidHiringDate(sourceDatabase);
            if (result != null)
                grdResult.ItemsSource = result.DefaultView;
            tcOpenPages.SelectedIndex = 1;
        }

        private void btnUpdateData_Click(object sender, RoutedEventArgs e)
        {

        }

        private void btnCloseApplication_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
    }

    public class DepartmentCompanyModelView
    {
        public int DepartmentId { get; set; }
        public string DepartmentName { get; set; }
        public int? SubDepartmentId { get; set; }
        public string SubDepartmentName { get; set; }
        public int? CompanyId { get; set; }
        public string CompanyName { get; set; }
    }
}
