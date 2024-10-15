using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;

namespace TimeAide.Web.Models
{
    [Table("WorkflowTriggerRequestDetailEmail")]
    public class WorkflowTriggerRequestDetailEmail : BaseCompanyObjects
    {


        public WorkflowTriggerRequestDetailEmail()
        {
        }
        [Display(Name = "Workflow Trigger Request Detail Email Id")]
        [Column("WorkflowTriggerRequestDetailEmailId")]
        public override int Id { get; set; }

        public string SenderAddress { get; set; }
        public string ToAddress { get; set; }
        public string CcAddress { get; set; }
        public string BccAddress { get; set; }
        public int? WorkflowTriggerRequestDetailId { get; set; }
        public virtual WorkflowTriggerRequestDetail WorkflowTriggerRequestDetail { get; set; }
        [NotMapped]
        public List<string> ToAddressList
        {
            get
            {
                if (string.IsNullOrEmpty(ToAddress))
                    return new List<string>();
                return ToAddress.Split(',').ToList();

            }
        }
    }
}
