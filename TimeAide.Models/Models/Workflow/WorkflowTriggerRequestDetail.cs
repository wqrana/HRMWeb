using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TimeAide.Web.Models
{
    public class WorkflowTriggerRequestDetail : BaseCompanyObjects
    {
        public WorkflowTriggerRequestDetail()
        {

            WorkflowTriggerRequestDetailEmail = new HashSet<WorkflowTriggerRequestDetailEmail>();
        }
        [Display(Name = "Id")]
        [Column("WorkflowTriggerRequestDetailId")]
        public override int Id { get; set; }
        public string ActionRemarks { get; set; }
        public virtual int? WorkflowLevelId { get; set; }
        public virtual WorkflowLevel WorkflowLevel { get; set; }
        public virtual int WorkflowTriggerRequestId { get; set; }
        public virtual int WorkflowActionTypeId { get; set; }
        public virtual int? ActionById { get; set; }
        public virtual UserInformation ActionBy { get; set; }
        public DateTime? ActionDate { get; set; }
        public virtual WorkflowActionType WorkflowActionType { get; set; }
        public virtual WorkflowTriggerRequest WorkflowTriggerRequest { get; set; }
        public virtual ICollection<WorkflowTriggerRequestDetailEmail> WorkflowTriggerRequestDetailEmail { get; set; }
        public virtual WorkflowTriggerRequestDetail PreviousWorkflowTriggerRequest
        {
            get { return WorkflowTriggerRequest.WorkflowTriggerRequestDetail.Where(d => d.Id != Id).OrderByDescending(r => r.Id).FirstOrDefault(); }
        }
        [NotMapped]
        public int CurrentLevel
        {
            get
            {
                try
                {
                    if (WorkflowTriggerRequest.WorkflowTriggerRequestDetail.Count == 0)
                        return 0;
                    return WorkflowTriggerRequest.WorkflowTriggerRequestDetail.OrderBy(c => c.Id).ToList().IndexOf(this) + 1;
                }
                catch (Exception ex)
                {
                    return 0;
                }
            }
        }
    }
}
