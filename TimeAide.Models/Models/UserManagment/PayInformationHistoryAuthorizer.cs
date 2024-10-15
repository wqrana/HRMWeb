using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TimeAide.Web.Models
{
    public class PayInformationHistoryAuthorizer : BaseEntity
    {
        [Display(Name = "Pay Information History Authorizer Id")]
        [Column("PayInformationHistoryAuthorizerId")]
        public override int Id { get; set; }

        [Display(Name = "Authorize By")]
        public int? AuthorizeById { get; set; }

        [Display(Name = "Authorize By")]
        public int? PayInformationHistoryId { get; set; }
        public virtual PayInformationHistory PayInformationHistory { get; set; }
        public virtual UserInformation AuthorizeBy { get; set; }

    }
}
