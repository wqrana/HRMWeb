namespace TimeAide.Web.Models
{
    using System;
    using System.Linq;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;


    [Table("ProcessCode")]
    public partial class ProcessCode : BaseGlobalEntity
    {
        
        public ProcessCode()
        {
            TransactionConfiguration = new HashSet<TransactionConfiguration>();
        }

        [Display(Name = "Process Code Id")]
        [Column("ProcessCodeId")]
        public override int Id { get; set; }

        [Required]
        [StringLength(150)]
        [Display(Name = "Process Code")]
        public string ProcessCodeName { get; set; }

        [StringLength(150)]
        [Display(Name = "Description")]
        public string ProcessCodeDescription { get; set; }
        public virtual ICollection<TransactionConfiguration> TransactionConfiguration { get; set; }

    }
}
