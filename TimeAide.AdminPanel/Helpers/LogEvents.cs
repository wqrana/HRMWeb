using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TimeAide.AdminPanel.Helpers
{
    public enum LogEvent
    {
        CreatingDatabase=1,
        SetupIdenTechInformation=2,
        AddDBOwner =3,
        CreateDefaultValuesSP=4,
        CreateViews=5,
        CreateStoredProcedures=6,
        CreateFunctions=7,
        CreateMigrationScriptsMasterData=8,
        CreateMigrationScriptsEmployeeInformation=9,
        CreateMigrationScriptsEmployeeTabs=10,
        ExecuteDefaultValuesSP=11,
        ExecuteMigrationScriptsMasterData=12,
        ExecuteMigrationScriptsEmployeeInformation=13,
        ExecuteMigrationScriptsEmployeeTabs=14,
        CreateMigrationClient=15,
        ExecuteApplicationInformationSPs=16,
        CreateApplicationInformationSPs=17,
        Exception=18,
        DownloadDocuments=19,
        DropDefaultValuesSP = 21,
        DropMigrationScriptsMasterData = 22,
        DropMigrationScriptsEmployeeInformation = 23,
        DropMigrationScriptsEmployeeTabs = 24,
        CreateDefaultValuesForEmptySource = 25,
        ExecuteDefaultValuesForEmptySource = 26,
        DropDefaultValuesForEmptySource = 27,
        ExecuteCreateRoles = 28,
        ExecuteCreateEmployeeGroup = 29,
        CreateCompanyWizeDataChange  = 30,
        ExecuteCompanyWizeDataChange = 31,
        DropCompanyWizeDataChange = 32,
        ExecuteApplicationGlobalDefaultValuesSP = 33,
        DropApplicationGlobalDefaultValuesSP = 34,
        CreateASPNET_Tables = 35,
        MigrateEmployeeDocuments=36,
        MigrateEmployeeCredentials=37

    }
}


