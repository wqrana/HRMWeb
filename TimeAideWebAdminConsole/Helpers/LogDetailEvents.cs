using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TimeAide.AdminConsole.Helpers
{
    enum LogDetailEvents
    {
        CreateSeedUser = 1,
        CreateDefaultUserLogin = 2,
        AddDBOwner=3,
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
        SetupSuperAdminAccess = 15,
        DownloadCompanyLogo = 16,
        DownloadEmployeePhotos = 17,
        DownloadEmployeeFiles = 18,
        DownloadEmployeeActionDocument = 19,
        DownloadEmployeeCredentialsDocument = 20,
        DownloadEmployeeDocument = 21,
        DownloadEmployeeEducationDocument = 22,
        DownloadEmployeePerformanceDocument = 23,
        DownloadEmployeeTrainingDocument = 24,
        DownloadEmploymentDocument = 25,
        DownloadEmployeeDependentDocument = 26,
    }
}
