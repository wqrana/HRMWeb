using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TimeAide.Web.Models
{
    public class WorkflowActionType : BaseCompanyObjects
    {
        public WorkflowActionType()
        {
            WorkflowTriggerRequestDetail = new HashSet<WorkflowTriggerRequestDetail>();

        }

        [Display(Name = "Id")]
        [Column("WorkflowActionTypeId")]
        public override int Id { get; set; }
        [Display(Name = "Name")]
        public string WorkflowActionTypeName { get; set; }
        [Display(Name = "Description")]
        public string WorkflowActionTypeDescription { get; set; }
        public virtual ICollection<WorkflowTriggerRequestDetail> WorkflowTriggerRequestDetail { get; set; }
    }
}
