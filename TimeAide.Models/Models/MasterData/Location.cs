namespace TimeAide.Web.Models
{
    using System;
    using System.Linq;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;

    [Table("Location")]
    public partial class Location: BaseCompanyObjects
    {
        public Location()
        {
            EmploymentHistory = new HashSet<EmploymentHistory>();
        }

        [Column("LocationId")]
        public override int Id { get; set; }

        [Required]
        [StringLength(50)]
        [Display(Name = "Location")]
        public string LocationName { get; set; }

        [StringLength(200)]
        [Display(Name = "Description")]
        public string LocationDescription { get; set; }

        public string Address { get; set; }
        [StringLength(100)]
        public string Address2 { get; set; }
       
        public int? LocationCityId { get; set; }
        public int? LocationStateId { get; set; }
        public int? LocationCountryId { get; set; }
                
        public string ZipCode { get; set; }

        public string PhysicalAddress1 { get; set; }
        [StringLength(100)]
        public string PhysicalAddress2 { get; set; }

        public int? PhysicalCityId { get; set; }
        public int? PhysicalStateId { get; set; }
        [StringLength(9)]
        public string PhysicalZipCode { get; set; }

        [StringLength(10)]
        public string PhoneNumber1 { get; set; }

        [StringLength(10)]
        public string ExtensionNumber1 { get; set; }

        [StringLength(10)]
        public string PhoneNumber2 { get; set; }

        [StringLength(10)]
        public string ExtensionNumber2 { get; set; }

        [StringLength(10)]
        public string PhoneNumber3 { get; set; }

        [StringLength(10)]
        public string ExtensionNumber3 { get; set; }

        [StringLength(10)]
        public string FaxNumber { get; set; }

        public virtual Country LocationCountry { get; set; }
        public virtual State LocationState { get; set; }
        public virtual City LocationCity { get; set; }

        public virtual ICollection<EmploymentHistory> EmploymentHistory { get; set; }

        public override List<int?> GetRefferredCompanies()
        {
            var list = this.EmploymentHistory.Where(t => t.DataEntryStatus == 1).Select(t => t.CompanyId).Distinct();
            return list.ToList();
        }
    }
}
