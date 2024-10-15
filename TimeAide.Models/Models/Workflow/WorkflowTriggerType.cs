using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TimeAide.Web.Models
{
    public class WorkflowTriggerType : BaseCompanyObjects
    {
        public WorkflowTriggerType()
        {
            WorkflowTrigger = new HashSet<WorkflowTrigger>();
        }
        [Display(Name = "Id")]
        [Column("WorkflowTriggerTypeId")]
        public override int Id { get; set; }
        [Display(Name = "Name")]
        public string WorkflowTriggerTypeName { get; set; }
        [Display(Name = "Description")]
        public string WorkflowTriggerTypeDescription { get; set; }
        public virtual ICollection<WorkflowTrigger> WorkflowTrigger { get; set; }
    }

    public enum WorkflowTriggerTypes
    {
        ChangeRequestPersonalInformation = 1,
        ChangeRequestEmploymentInformation = 2
    }
}
