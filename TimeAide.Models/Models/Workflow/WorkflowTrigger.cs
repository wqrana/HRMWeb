using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TimeAide.Web.Models
{
    public class WorkflowTrigger : BaseCompanyObjects
    {
        public WorkflowTrigger()
        {

        }

        [Display(Name = "Id")]
        [Column("WorkflowTriggerId")]
        public override int Id { get; set; }
        [Display(Name = "Name")]
        public string WorkflowTriggerName { get; set; }
        [Display(Name = "Description")]
        public string WorkflowTriggerDescription { get; set; }
        public virtual int WorkflowId { get; set; }
        public virtual Workflow Workflow { get; set; }
        public virtual int WorkflowTriggerTypeId { get; set; }
        public virtual WorkflowTriggerType WorkflowTriggerType { get; set; }
    }
}
