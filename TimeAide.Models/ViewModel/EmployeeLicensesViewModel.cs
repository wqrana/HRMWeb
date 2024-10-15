using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TimeAide.Models.ViewModel
{
   public class EmployeeLicensesAndBalances
    {
        public IEnumerable<EmployeeAccrualRuleViewModel> EmployeeAccrualRules { get; set; }
    }

    public class EmployeeAccrualRuleViewModel
    {
        public int UserInformationId { get; set; }
        public int EmployeeAccrualRuleId { get; set; }
	    public DateTime StartOfRuleDate { get; set; }
        public int AccrualTypeId { get; set; }
	    public string AccrualTypeName { get; set; }
        public int AccrualRuleId { get; set; }
        public string AccrualRuleName { get; set; }
        public Decimal? AccrualDailyHours { get; set; }
        public int IsCurrent { get; set; }
        public DateTime? BalanceStartDate { get; set; }
        public Decimal? AccruedHours { get; set; }
    }
    public class EmployeeAccrualBalanceViewModel
    {
        public int UserInformationId { get; set; }
        public int EmployeeAccrualBalanceId { get; set; }
        public DateTime BalanceStartDate { get; set; }
        public int AccrualTypeId { get; set; }
        public string AccrualTypeName { get; set; }     
        public Decimal? AccruedHours { get; set; }
        public int IsCurrent { get; set; }
    }


}
