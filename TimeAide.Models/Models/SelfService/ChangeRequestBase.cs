using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TimeAide.Web.Models;

namespace TimeAide.Web.Models
{
    public abstract class ChangeRequestBase : BaseEntity
    {
        public ChangeRequestBase()
        {
            WorkflowTriggerRequest = new HashSet<WorkflowTriggerRequest>();
        }

        public int UserInformationId { get; set; }
        
        public int ChangeRequestStatusId { get; set; }
        public virtual UserInformation UserInformation { get; set; }
        public virtual ChangeRequestStatus ChangeRequestStatus { get; set; }


        public virtual ICollection<WorkflowTriggerRequest> WorkflowTriggerRequest { get; set; }
        
        [NotMapped]
        public string ChangeRequestRemarks { get; set; }
        //public abstract Dictionary<String, String> CanCreateRequest();
    }
}
