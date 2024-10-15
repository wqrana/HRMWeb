using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TimeAide.Models.ViewModel
{
    public class EmployeeUserInformationView
    {


        public int Id { get; set; }
        public int? EmployeeId { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
            
     
        public string DepartmentName { get; set; }

        public string SubDepartmentName { get; set; }

        public string PositionName { get; set; }

        public string EmployeeTypeName { get; set; }
  
     
      
        public string EmployeeStatusName { get; set; }
        public int CompanyId { get; set; }

        public string CompanyName { get; set; }
 
        public string LoginEmail { get; set; }


  
        public string EmployeeGroups { get; set; }
        public string UserRole { get; set; }
        public string EncryptedId
        {
            get
            {
                var strId = Id.ToString();
                return Common.Helpers.Encryption.EncryptURLParm(strId);

            }
        }
        public string EmpStatusBgClass { get; set; }
        public int EmployeeStatusId { get; set; }
    }
}
