namespace TimeAide.Web.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;

    [Table("PayType")]
    public partial class PayType : BaseEntity
    {
        public PayType()
        {
            PayInformationHistory = new HashSet<PayInformationHistory>();
        }

        [Column("PayTypeId")]
        [Display(Name = "Pay Type Id")]
        public override int Id { get; set; }

        [Required]
        [StringLength(50)]
        [Display(Name = "Pay Type Name")]
        public string PayTypeName { get; set; }

        [StringLength(200)]
        [Display(Name = "Pay Type Description")]
        public string PayTypeDescription { get; set; }

        
        public virtual ICollection<PayInformationHistory> PayInformationHistory { get; set; }
    }
}
