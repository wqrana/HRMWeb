namespace TimeAide.Web.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
 

    [Table("CFSECode")]
    public partial class CFSECode : BaseEntity
    {
        
        public CFSECode()
        {
            SubDepartments = new HashSet<SubDepartment>();
            Departments = new HashSet<Department>();
        }

        [Column("CFSECodeId")]
        public override int Id { get; set; }


        [Required]
        [StringLength(150)]
        [Display(Name = "CFSE Code")]
        public string CFSECodeName { get; set; }

        [StringLength(150)]
        [Display(Name = "Code Description")]
        public string CFSECodeDescription { get; set; }

        
        public virtual ICollection<SubDepartment> SubDepartments { get; set; }

        
        public virtual ICollection<Department> Departments { get; set; }
    }
}
