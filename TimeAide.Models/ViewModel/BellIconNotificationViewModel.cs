using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TimeAide.Models.ViewModel
{
    public class BellIconNotificationViewModel
    {
        public string RequestType { get; set; }
        public int ReferenceId { get; set; }
        public string ControllerName { get; set; }
        public string ActionName { get; set; }
        public string ShortFullName { get; set; }
        public DateTime CreatedDate { get; set; }
        public int WorkflowTriggerRequestDetailId { get; set; }

        public int WorkflowTriggerRequestId { get; set; }
        public int WorkflowActionTypeId { get; set; }
        public int WorkflowTriggerTypeId { get; set; }

        public string WorkflowActionTypeName { get; set; }
        public int EmployeeId { get; set; }
        public int TotalLevel { get; set; } 
        public int CurrentLevel { get; set; }
        public string ActionType { get; set; }
        public bool CanAppprove { get; set; }
        public string CreatedSince
        {
            get
            {
                DateTime d2 = DateTime.Now;
                TimeSpan dateDifference = d2 - CreatedDate;
                string timeIlapsed = "";
                if (dateDifference.Days > 0)
                    timeIlapsed += dateDifference.Days + " Days ";
                if (dateDifference.Hours > 0)
                    timeIlapsed += dateDifference.Hours + " Hours ";
                if (dateDifference.Minutes > 0)
                    timeIlapsed += dateDifference.Minutes + " Mins ";
                timeIlapsed += "ago";
                if (dateDifference.Days == 0 && dateDifference.Hours == 0 && dateDifference.Minutes == 0)
                    timeIlapsed = "just now";
                return timeIlapsed;
            }
        }

        public int? PrevWorkflowTriggerRequestDetailId { get; set; }

        public string PrevWorkflowActionTypeName { get; set; }
        public int? PrevWorkflowActionTypeId { get; set; }

        public int? ActionById { get; set; }

        public DateTime? ActionDate { get; set; }

        public string ActionPersonShortFullName { get; set; }
    }
}
