using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TimeAide.Web.Models
{
    public class ChangePasswordByAdminReason : BaseUserObjects
    {
        [Column("ChangePasswordByAdminReasonId")]
        public override int Id { get; set; }
        [Required]
        [Display(Name = "Reason")]
        public string Reason { get; set; }
        public int ChangeByUserId { get; set; }
        public int? CompanyId { get; set; }
    }
}
