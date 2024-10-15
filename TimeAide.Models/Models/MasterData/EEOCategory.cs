namespace TimeAide.Web.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;

    [Table("EEOCategory")]
    public partial class EEOCategory : BaseCompanyObjects
    {
        public EEOCategory()
        {
            PayInformationHistory = new HashSet<PayInformationHistory>();
            Position = new HashSet<Position>();
        }
        [Column("EEOCategoryId")]
        [Display(Name = "EEO Category Id")]
        public override int Id { get; set; }

        [Required]
        [Display(Name = "EEO Number")]
        [StringLength(5)]
        public string EEONumber { get; set; }
        
        [Display(Name = "EEO Category Name")]
        [Required]
        [StringLength(50)]
        public string EEOCategoryName { get; set; }
        
        public virtual ICollection<PayInformationHistory> PayInformationHistory { get; set; }
        public virtual ICollection<Position> Position { get; set; }
    }
}
