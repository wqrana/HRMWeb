using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace TimeAide.Web.ViewModel
{
    public class EmployeeSupervisorViewModel
    {
        public int SelectedUserId { get; set; }
        public List<string> AllUserId { get; set; }
        public MultiSelectList Users { get; set; }
        public MultiSelectList SupervisedUsers { get; set; }
        public List<string> SupervisedUserId { get; set; }
        //public MultiSelectList NotSupervised { get; set; }
    }
}