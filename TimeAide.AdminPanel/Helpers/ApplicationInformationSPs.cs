using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TimeAide.AdminPanel.Helpers
{
    enum ApplicationInformationSPs
    {
        sp_CreateDefaultModule = 1,
        sp_CreateDefaultForm = 2,
        sp_CreateDefaultPrivilege = 3,
        sp_CreateDefaultRoleType = 4,
        //sp_CreateDefaultRole = 5,
    }

    enum ApplicationGlobalDefaultValuesSP
    {
        sp_DM_IncidentType = 1,
        sp_CreateEmptySourceDefaultOSHACaseClassification = 2,
        sp_CreateEmptySourceDefaultOSHAInjuryClassification = 3,
        sp_CreateDefaultEmailType = 4,
        sp_CreateDefaultEmployeeGroupType=5,
        sp_CreateDefaultClosingNotificationType=6,
        sp_CreateDefaultWorkflowLevelType = 7,
        sp_CreateDefaultWorkflowTriggerType = 8,
        sp_CreateDefaultWorkflowActionType = 9,
        sp_CreateDefaultNotificationType = 10,
        sp_CreateDefaultChangeRequestStatus =11,
        sp_CreateDefaultApplicationConfiguration=12,
        sp_CreateDefaultDocumentRequiredBy = 13
    }
}
