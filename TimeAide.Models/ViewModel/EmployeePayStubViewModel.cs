
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TimeAide.Web.Models;

namespace TimeAide.Models.ViewModel
{
   public class EmployeePayStubViewModel
    {
        public EmployeePayStubBatch EmployeePayStubBatch { get; set; }
        public EmployeePayStubCompany EmployeePayStubCompany { get; set; }
        public List<EmployeePayStubCompensation> EmployeePayStubCompensations { get; set; }
        public List<EmployeePayStubWithholding> EmployeePayStubWithholdings { get; set; }
    }
}
