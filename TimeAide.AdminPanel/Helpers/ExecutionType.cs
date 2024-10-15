using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TimeAide.AdminPanel.Helpers
{
    public enum ExecutionType
    {
        DataMigration=0,
        NewEmptyDatabase=1,
        NewClient=2,
        NewClientWithDefaultValues=3,
    }
    public enum DocumentActionType
    {
        EmployeeDocuments = 0,
        EmployeeCredentials = 1,
        EmployeeEducations = 2,
        EmployeeTrainings=3,
        EmployeePerformances = 4,
        EmploymentContracts = 5,

    }
    public enum ExportDataType
    {
        AllFiles=-1,
        CompanySetUpTA7= 0,
        MasterfileTAW,
        MasterfileTA7,
        PositionInfoTAW,
        TaxInfoTA7,
        AdditionalEarningsInfoTA7,
        DeductionsGoalsTA7,
        DirectDepositTA7,
        EmergencyContactTAW,
        AllowedAndTakenTA7,
        BalancesTA7,
        WFNEmployeesTA7,
        WFNDirectDepositTA7,
        WFNBalancesTA7,
        WFNVAnSABalancesTA7
    }
    public enum ImportDataType
    {
        PayrollTA7
    }
}
