using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TimeAide.Web.Models
{
    public class WorkflowLevel : BaseCompanyObjects
    {
        public WorkflowLevel()
        {
            WorkflowLevelGroup = new HashSet<WorkflowLevelGroup>();
            WorkflowTriggerRequestDetail = new HashSet<WorkflowTriggerRequestDetail>();

        }
        [Display(Name = "Workflow Level Id")]
        [Column("WorkflowLevelId")]
        public override int Id { get; set; }
        [Required]
        [Display(Name = "Workflow Level Name")]
        public string WorkflowLevelName { get; set; }
        public virtual int WorkflowId { get; set; }
        [NotMapped]
        [Display(Name = "Level Number")]
        public int LevelNumber
        {
            get
            {
                int levelNumber = 0;
                if (Id>0 && Workflow != null && Workflow.WorkflowLevel != null && Workflow.WorkflowLevel.Count > 1)
                {
                    levelNumber = Workflow.WorkflowLevel.Where(w => w.Id < Id && w.DataEntryStatus==1).Count();
                }

                return levelNumber + 1;
            }
        }
        [Display(Name = "Workflow Level Type")]
        public virtual int WorkflowLevelTypeId { get; set; }
        public virtual Workflow Workflow { get; set; }
        public virtual WorkflowLevelType WorkflowLevelType { get; set; }
        public virtual ICollection<WorkflowLevelGroup> WorkflowLevelGroup
        {
            get;
            set;
        }
        public virtual ICollection<WorkflowTriggerRequestDetail> WorkflowTriggerRequestDetail
        {
            get;
            set;
        }
        public virtual int NotificationMessageId { get; set; }
        public NotificationMessage NotificationMessage { get; set; }
        [NotMapped]
        public string SelectedWorkflowLevelGroupId { get; set; }
        [NotMapped]
        public string SelectedWorkflowLevelGroupNames
        {
            get
            {
                if (WorkflowLevelGroup != null && WorkflowLevelGroup.Count > 0)
                    return String.Join(",", WorkflowLevelGroup.Where(w => w.DataEntryStatus == 1 && w.EmployeeGroup!=null).Select(c => c.EmployeeGroup.EmployeeGroupName.ToString()));
                return "";
            }

        }
    }
}
