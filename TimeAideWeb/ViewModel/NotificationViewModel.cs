using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using TimeAide.Data;
using TimeAide.Models.ViewModel;
using TimeAide.Web.Models;

namespace TimeAide.Web.ViewModel
{
    public class NotificationViewModel
    {
        public NotificationViewModel()
        {
            WorkflowTriggerRequestDetail = new List<WorkflowTriggerRequestDetail>();
            NotificationLog = new List<NotificationLog>();
        }
        public List<WorkflowTriggerRequestDetail> WorkflowTriggerRequestDetail { get; set; }

        public List<BellIconNotificationViewModel> bellIconNotificationViewModelList { get; set; }
        public List<NotificationLog> NotificationLog { get; set; }
    }
    
}