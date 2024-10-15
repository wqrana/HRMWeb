namespace TimeAide.Web.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
 

    [Table("Gender")]
    public partial class Gender : BaseEntity
    {
        
        public Gender()
        {
            EmployeeDependent = new HashSet<EmployeeDependent>();
        }
        [Column("GenderId")]
        public override int Id { get; set; }


        [Required]
        [StringLength(20)]
        [Display(Name = "Gender")]
        public string GenderName { get; set; }

        [StringLength(50)]
        [Display(Name = "Gender Description")]
        public string GenderDescription { get; set; }

        
        public virtual ICollection<EmployeeDependent> EmployeeDependent { get; set; }

    }
}
