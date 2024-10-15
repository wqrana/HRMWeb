using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TimeAide.Web.Models
{
    public class WorkflowTriggerRequest : BaseCompanyObjects
    {
        public WorkflowTriggerRequest()
        {
            WorkflowTriggerRequestDetail = new HashSet<WorkflowTriggerRequestDetail>();
            NotificationLogMessageReadBy = new HashSet<NotificationLogMessageReadBy>();
        }

        [Display(Name = "Id")]
        [Column("WorkflowTriggerRequestId")]
        public override int Id { get; set; }
        public virtual int WorkflowTriggerId { get; set; }
        public virtual int? ChangeRequestAddressId { get; set; }
        public virtual int? ChangeRequestEmergencyContactId { get; set; }
        public virtual int? SelfServiceEmployeeDocumentId  { get; set; }
        public virtual int? SelfServiceEmployeeCredentialId { get; set; }
        public virtual int? EmployeeTimeOffRequestId { get; set; }
        public virtual WorkflowTrigger WorkflowTrigger { get; set; }
        public virtual ChangeRequestAddress ChangeRequestAddress { get; set; }
        public virtual SelfServiceEmployeeDocument SelfServiceEmployeeDocument { get; set; }
        public virtual SelfServiceEmployeeCredential SelfServiceEmployeeCredential { get; set; }
        public virtual ChangeRequestEmergencyContact ChangeRequestEmergencyContact { get; set; }
        public virtual int? ChangeRequestEmailNumbersId { get; set; }
        public virtual ChangeRequestEmailNumbers ChangeRequestEmailNumbers { get; set; }
        public virtual int? ChangeRequestEmployeeDependentId { get; set; }
        public virtual EmployeeTimeOffRequest EmployeeTimeOffRequest { get; set; }
        public virtual ChangeRequestEmployeeDependent ChangeRequestEmployeeDependent { get; set; }
        public virtual ICollection<WorkflowTriggerRequestDetail> WorkflowTriggerRequestDetail { get; set; }
        [NotMapped]
        public string CreatedSince
        {
            get
            {
                DateTime d2 = DateTime.Now;
                TimeSpan dateDifference = d2 - CreatedDate;
                string timeIlapsed = "";
                if (dateDifference.Days > 0)
                    timeIlapsed += dateDifference.Days + " Days ";
                if (dateDifference.Hours > 0)
                    timeIlapsed += dateDifference.Hours + " Hours ";
                if (dateDifference.Minutes > 0)
                    timeIlapsed += dateDifference.Minutes + " Mins ";
                timeIlapsed += "ago";
                if (dateDifference.Days == 0 && dateDifference.Hours == 0 && dateDifference.Minutes == 0)
                    timeIlapsed = "just now";
                return timeIlapsed;
            }
        }
        [NotMapped]
        public virtual int ChangeRequestId
        {
            get
            {
                if (ChangeRequestAddressId.HasValue)
                    return ChangeRequestAddressId.Value;
                else
                    return ChangeRequestEmailNumbersId ?? 0;
            }
        }
        [NotMapped]
        public virtual ChangeRequestBase ChangeRequest
        {
            get
            {
                if (ChangeRequestAddressId.HasValue)
                    return ChangeRequestAddress;
                else if (ChangeRequestEmailNumbersId.HasValue)
                    return ChangeRequestEmailNumbers;
                else if (ChangeRequestEmergencyContactId.HasValue)
                {
                    return ChangeRequestEmergencyContact;

                }
                else if (ChangeRequestEmployeeDependent != null)
                    return ChangeRequestEmployeeDependent;
                else if (SelfServiceEmployeeDocument != null)
                    return SelfServiceEmployeeDocument;
                else if (SelfServiceEmployeeCredential != null)
                    return SelfServiceEmployeeCredential;

                else if (EmployeeTimeOffRequest != null)
                    return EmployeeTimeOffRequest;
                else
                    return null;
            }
        }
        public virtual ICollection<NotificationLogMessageReadBy> NotificationLogMessageReadBy { get; set; }
    }
}
