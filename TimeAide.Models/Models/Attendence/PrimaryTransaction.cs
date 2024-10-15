namespace TimeAide.Web.Models
{
    using System;
    using System.Linq;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;


    [Table("PrimaryTransaction")]
    public partial class PrimaryTransaction : BaseGlobalEntity
    {

        public PrimaryTransaction()
        {
            TransactionConfiguration = new HashSet<TransactionConfiguration>();
        }

        [Display(Name = "Primary Transaction Id")]
        [Column("PrimaryTransactionId")]
        public override int Id { get; set; }

        [Required]
        [StringLength(150)]
        [Display(Name = "Code")]
        public string PrimaryTransactionName { get; set; }

        [StringLength(150)]
        [Display(Name = "Description")]
        public string PrimaryTransactionDescription { get; set; }
        public virtual ICollection<TransactionConfiguration> TransactionConfiguration { get; set; }

    }
}
