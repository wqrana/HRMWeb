using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TimeAide.AdminConsole.Helpers
{
    public enum LogEvent
    {
        CreatingDatabase=1,
        SetupIdenTechInformation=2,
        AddDBOwner =3,
        CreateDefaultValuesSP=4,
        CreatingViews=5,
        CreatingStoredProcedures=6,
        CreatingFunctions=7,
        CreatingMigrationScriptsMasterData=8,
        CreatingMigrationScriptsEmployeeInformation=9,
        CreatingMigrationScriptsEmployeeTabs=10,
        ExecuteDefaultValuesSP=11,
        ExecuteMigrationScriptsMasterData=12,
        ExecuteMigrationScriptsEmployeeInformation=13,
        ExecuteMigrationScriptsEmployeeTabs=14,
        CreateMigrationClient=15,
        ExecuteApplicationInformationSPs=16,
        CreatingApplicationInformationSPs=17,
        Exception=18,
        DownloadDocuments=19,
        DropDefaultValuesSP = 21,
        DropMigrationScriptsMasterData = 22,
        DropMigrationScriptsEmployeeInformation = 23,
        DropMigrationScriptsEmployeeTabs = 24,
        
        
    }
}


