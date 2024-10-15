using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TimeAide.Web.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;

    public partial class EmployeeStatus : BaseEntity
    {
        
        public EmployeeStatus()
        {
            UserInformations = new HashSet<UserInformation>();
        }

        //   [DatabaseGenerated(DatabaseGeneratedOption.None)]
       
        [Column("EmployeeStatusId")]
        public override int Id { get; set; }

        [Required]
        [StringLength(500)]
        [Display(Name = "Status")]
        public string EmployeeStatusName { get; set; }

        [StringLength(1000)]
        [Display(Name = "Description")]
        public string EmployeeStatusDescription { get; set; }
        public virtual ICollection<UserInformation> UserInformations { get; set; }
    }

    public enum EmployeeStatusNames
    {
        Active=1,
        Inactive=2,
        Closed =3,
        CompanyTransfer=4,
        
    }
}
