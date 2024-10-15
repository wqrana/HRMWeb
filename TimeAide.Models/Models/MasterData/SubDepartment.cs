namespace TimeAide.Web.Models
{
    using System;
    using System.Linq;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Linq.Expressions;

    public class RequireConditionAttribute : ValidationAttribute
    {

        private readonly string _comparisonProperty;

        public RequireConditionAttribute(string comparisonProperty)
        {
            _comparisonProperty = comparisonProperty;
        }

        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            if (validationContext.DisplayName == "CFSE Code")
            {
                ErrorMessage = ErrorMessageString;
                var currentValue = (int?)value;

                var property = validationContext.ObjectType.GetProperty(_comparisonProperty);

                if (property == null)
                    throw new ArgumentException(_comparisonProperty + " property with this name not found");

                var comparisonValue = (bool)property.GetValue(validationContext.ObjectInstance);

                if (comparisonValue && (!currentValue.HasValue || currentValue == 0))
                {
                    return new ValidationResult("The " + validationContext.DisplayName + " is required.");
                }
            }
            else
            {
                ErrorMessage = ErrorMessageString;
                var currentValue = (decimal?)value;

          
                var property = validationContext.ObjectType.GetProperty(_comparisonProperty);

                if (property == null)
                    throw new ArgumentException(_comparisonProperty + " property with this name not found");

                var comparisonValue = (bool)property.GetValue(validationContext.ObjectInstance);

                if (comparisonValue && (!currentValue.HasValue || currentValue == 0))
                {
                    return new ValidationResult("The " + validationContext.DisplayName + " is required.");
                }
            }
            //if (comparisonValue && )
            //    return new ValidationResult(ErrorMessage);

            return ValidationResult.Success;
        }
    }

    [Table("SubDepartment")]
    public partial class SubDepartment : BaseCompanyObjects
    {
        public SubDepartment()
        {
            EmploymentHistory = new HashSet<EmploymentHistory>();
            UserInformations = new HashSet<UserInformation>();
            SupervisorSubDepartment = new HashSet<SupervisorSubDepartment>();
        }

        [Display(Name = "Sub Department ID")]
        [Column("SubDepartmentId")]
        public override int Id { get; set; }

        [Required]
        [StringLength(50)]
        [Display(Name = "Sub Department")]
        public string SubDepartmentName { get; set; }

        [StringLength(200)]
        [Display(Name = "Description")]
        public string SubDepartmentDescription { get; set; }

        //public bool Enabled { get; set; }

        [Display(Name = "CFSE Assignment")]
        public bool USECFSEAssignment { get; set; }

        [Display(Name = "CFSE Code")]
        [RequireCondition("USECFSEAssignment")]
        public int? CFSECodeId { get; set; }

        [Display(Name = "CFSE Percent")]
        [Range(typeof(decimal), "0", "100")]
        [RequireCondition("USECFSEAssignment")]
        public decimal? CFSECompanyPercent { get; set; }

        [Display(Name = "Department")]
        public int? DepartmentId { get; set; }

        public virtual Department Department { get; set; }
        public virtual CFSECode CFSECode { get; set; }

        
        public virtual ICollection<UserInformation> UserInformations { get; set; }
        public virtual Client Client { get; set; }
        public virtual ICollection<SupervisorSubDepartment> SupervisorSubDepartment { get; set; }

        public virtual Company Company { get; set; }
        public virtual ICollection<EmploymentHistory> EmploymentHistory { get; set; }

        public override List<int?> GetRefferredCompanies()
        {
            var list = this.UserInformations.Where(t => t.DataEntryStatus == 1).Select(t => t.CompanyId).Distinct();
            list = list.Union(this.SupervisorSubDepartment.Where(t => t.DataEntryStatus == 1).Select(t => t.UserInformation.CompanyId).Distinct());
            list = list.Union(this.EmploymentHistory.Where(t => t.DataEntryStatus == 1).Select(t => t.UserInformation.CompanyId).Distinct());
            return list.ToList();
        }

        public static Expression<Func<SubDepartment, bool>> GetPredicate(int companyId,int? departmentId)
        {
             return c =>(!c.CompanyId.HasValue && !c.DepartmentId.HasValue) ||
                        (!c.CompanyId.HasValue &&    (c.DepartmentId == departmentId || !departmentId.HasValue)) ||
                        (c.CompanyId == companyId && (c.DepartmentId == departmentId || !departmentId.HasValue)) ||
                        (c.CompanyId == companyId && !c.DepartmentId.HasValue)
                                                                                                                                 ;
        }
    }
}
