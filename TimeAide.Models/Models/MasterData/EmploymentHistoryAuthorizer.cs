using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TimeAide.Web.Models
{
    public class EmploymentHistoryAuthorizer : BaseEntity
    {
        [Display(Name = "Employment History Authorizer Id")]
        [Column("EmploymentHistoryAuthorizerId")]
        public override int Id { get; set; }

        [Display(Name = "Authorize By")]
        public int? AuthorizeById { get; set; }
        //public virtual UserInformation AuthorizeBy { get; set; }

        [Display(Name = "Authorize By")]
        public int? EmploymentHistoryId { get; set; }
        public virtual EmploymentHistory EmploymentHistory { get; set; }
        public UserInformation AuthorizeBy { get; set; }
    }
}
