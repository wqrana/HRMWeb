using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TimeAide.AdminPanel.Helpers
{
    public enum DefaultValueForEmptySourceSPs
    {
        sp_CreateEmptySourceDefaultCobraStatus = 1,
        sp_CreateEmptySourceDefaultVeteranStatus = 2,
        sp_CreateEmptySourceDefaultTerminationEligibility = 3,
        //sp_CreateEmptySourceDefaultOSHAInjuryClassification = 4,
        //sp_CreateEmptySourceDefaultOSHACaseClassification = 5,
        sp_CreateEmptySourceDefaultMaritalStatus = 6,
        sp_CreateEmptySourceDefaultInsuranceType = 7,
        sp_CreateEmptySourceDefaultInsuranceStatus = 8,
        sp_CreateEmptySourceDefaultGender = 9,
        sp_CreateEmptySourceDefaultEthnicity = 10,
        sp_CreateEmptySourceDefaultDisability = 11,
    }
}
