namespace TimeAide.Web.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;

    [Table("Credential")]
    public partial class Credential : BaseEntity
    {

        public Credential()
        {
            EmployeeCredential = new HashSet<EmployeeCredential>();
            //EmployeeRequiredCredential = new HashSet<EmployeeRequiredCredential>();
        }

        [Display(Name = "Credential")]
        [Column("CredentialId")]
        public override int Id { get; set; }

        [StringLength(1000)]
        [Display(Name = "Credential")]
        public string CredentialName { get; set; }
        public bool SelfServiceDisplay { get; set; }
        public bool SelfServiceUpload { get; set; }
        [StringLength(1000)]
        [Display(Name = "Credential Description")]
        public string CredentialDescription { get; set; }
        [NotMapped]
        public string CredentialDDLName {
            get {
                var credenialDDLName = CredentialName;
                credenialDDLName+= string.IsNullOrEmpty(CredentialDescription)?"":string.Format(" ({0})",CredentialDescription);
                                   
                return credenialDDLName;
            } }
        public virtual ICollection<EmployeeCredential> EmployeeCredential { get; set; }
        public int? NotificationScheduleId { get; set; }
        public NotificationSchedule NotificationSchedule { get; set; }
    }
}
