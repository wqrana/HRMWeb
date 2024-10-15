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
    public partial class ChangeRequestEmployeeDependent : ChangeRequestBase
    {
        public ChangeRequestEmployeeDependent()
        {
            WorkflowTriggerRequest = new HashSet<WorkflowTriggerRequest>();
        }
        [Key]
        [Column("ChangeRequestEmployeeDependentId")]
        public override int Id { get; set; }

        //[Required]
        public string NewFirstName { get; set; }

        public string NewLastName { get; set; }

        public DateTime? NewBirthDate { get; set; }

        public string NewSSN { get; set; }
        [NotMapped]
        public string NewSSNComputed
        {
            get
            {
                if (!string.IsNullOrEmpty(NewSSN))
                {
                    var ssnDecrypted = Common.Helpers.Encryption.Decrypt(NewSSN);
                    if (ssnDecrypted != null && ssnDecrypted.Length == 9)
                        return string.Format("{0}-{1}-{2}", ssnDecrypted.Substring(0, 3), ssnDecrypted.Substring(3, 2), ssnDecrypted.Substring(5, 4));
                    else return null;
                }
                else
                    return null;
            }
            
        }

        public int? NewDependentStatusId { get; set; }
        public virtual DependentStatus NewDependentStatus { get; set; }

        public int? NewGenderId { get; set; }
        public virtual Gender NewGender { get; set; }

        public int? NewRelationshipId { get; set; }
        public virtual Relationship NewRelationship { get; set; }

        public DateTime? NewExpiryDate { get; set; }

        public bool? NewIsHealthInsurance { get; set; }

        public bool? NewIsDentalInsurance { get; set; }

        public bool? NewIsTaxPurposes { get; set; }

        public bool? NewIsFullTimeStudent { get; set; }

        public string NewSchoolAttending { get; set; }

        //[Required]
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
        public virtual DependentStatus DependentStatus { get; set; }
        public int? GenderId { get; set; }
        public virtual Gender Gender { get; set; }

        public int? RelationshipId { get; set; }
        public virtual Relationship Relationship { get; set; }

        public DateTime? ExpiryDate { get; set; }

        public bool IsHealthInsurance { get; set; }

        public bool IsDentalInsurance { get; set; }

        public bool IsTaxPurposes { get; set; }

        public bool IsFullTimeStudent { get; set; }

        public string SchoolAttending { get; set; }

        public int? EmployeeDependentId { get; set; }
        public virtual EmployeeDependent EmployeeDependent { get; set; }
        public string ReasonForDelete { get; set; }


    }
}
