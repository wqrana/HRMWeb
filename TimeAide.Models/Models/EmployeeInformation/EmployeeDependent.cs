using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using TimeAide.Web.Models;

namespace TimeAide.Web.Models
{  
    [Table("EmployeeDependent")]
    public partial class EmployeeDependent : BaseUserObjects
    {
        public EmployeeDependent()
        {
            ChangeRequestEmployeeDependent = new HashSet<ChangeRequestEmployeeDependent>();
        }

        [Column("EmployeeDependentId")]
        public override int Id { get; set; }
        [Required]
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public DateTime? BirthDate { get; set; }
        public string SSN { get; set; }
        [NotMapped]
        public string SSNComputed
        {
            get
            {
                if (!string.IsNullOrEmpty(SSN))
                {
                    var ssnDecrypted = Common.Helpers.Encryption.Decrypt(SSN);
                    if (ssnDecrypted != null && ssnDecrypted.Length == 9)
                        return string.Format("{0}-{1}-{2}", ssnDecrypted.Substring(0, 3), ssnDecrypted.Substring(3, 2), ssnDecrypted.Substring(5, 4));
                    else return null;
                }
                else
                    return null;
            }
        }
        public int? DependentStatusId { get; set; }
        [Required]
        public int? GenderId { get; set; }
        [Required]
        public int? RelationshipId { get; set; }        
        public DateTime? ExpiryDate { get; set; }
        public bool IsHealthInsurance { get; set; }
        public bool IsDentalInsurance { get; set; }
        public bool IsTaxPurposes { get; set; }
        public bool IsFullTimeStudent { get; set; }
        public string SchoolAttending { get; set; }

        public string DocName { get; set; }
        public string DocFilePath { get; set; }

        public virtual DependentStatus DependentStatus { get; set; }
        public virtual Relationship Relationship { get; set; }
        public virtual Gender Gender { get; set; }
        public virtual ICollection<ChangeRequestEmployeeDependent> ChangeRequestEmployeeDependent { get; set; }
        [NotMapped]
        //[Required]
        public string ReasonForDelete { get; set; }

    }
}
