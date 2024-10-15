using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace TimeAide.Web.ViewModel
{
    public class EmployeeRequiredCredentialViewModel
    {
        public int SelectedUserId { get; set; }
        public List<string> CredentialId { get; set; }
        public MultiSelectList Credentials { get; set; }
        public MultiSelectList RequiredCredential { get; set; }
        public List<string> RequiredCredentialId { get; set; }
        //public MultiSelectList NotSupervised { get; set; }
    }
}