namespace TimeAide.Web.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;

    [Table("PayScaleLevel")]
    public partial class PayScaleLevel : BaseEntity
    {
        [Column("PayScaleLevelId")]
        public override int Id { get; set; }

        public int PayScaleId { get; set; }
        [Required]
        public decimal PayScaleLevelRate { get; set; }

        public virtual PayScale PayScale { get; set; }
    }
}
