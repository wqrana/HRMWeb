

 using System;
 using System.Collections.Generic;
 using System.ComponentModel.DataAnnotations;
 using System.ComponentModel.DataAnnotations.Schema;

namespace TimeAide.Web.Models
{
    [Table("ApplicantAction")]
    public partial class ApplicantAction : BaseApplicantObjects
    {
        [Column("ApplicantActionId")]
        public override int Id { get; set; }
        public int? ApprovedById { get; set; }
        public int? ActionTypeId { get; set; }
        public DateTime ActionDate { get; set; }
        public DateTime? ActionEndDate { get; set; }
        public string ActionName { get; set; }
        public string ActionDescription { get; set; }
        public string ActionNotes { get; set; }
        public DateTime? ActionExpiryDate { get; set; }
        public string ActionClosingInfo { get; set; }
        public DateTime? ActionApprovedDate { get; set; }
        public string DocName { get; set; }
        public string DocFilePath { get; set; }
        public virtual ActionType ActionType { get; set; }
        [ForeignKey("ApprovedById")]
        public virtual UserInformation ApprovedBy { get; set; }

    }
}
