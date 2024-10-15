using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TimeAide.Web.Models
{
    public class WorkflowLevelType : BaseCompanyObjects
    {
        public WorkflowLevelType()
        {
            WorkflowLevel = new HashSet<WorkflowLevel>();

        }

        [Display(Name = "Id")]
        [Column("WorkflowLevelTypeId")]
        public override int Id { get; set; }
        [Display(Name = "Name")]
        public string WorkflowLevelTypeName { get; set; }
        [Display(Name = "Description")]
        public string WorkflowLevelTypeDescription { get; set; }
        public virtual ICollection<WorkflowLevel> WorkflowLevel { get; set; }
    }
}
