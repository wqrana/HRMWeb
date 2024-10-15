namespace TimeAide.Web.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;

    [Table("PayScale")]
    public partial class PayScale : BaseEntity
    {
        public PayScale()
        {
            PayScaleLevel = new HashSet<PayScaleLevel>();
            Position = new HashSet<Position>();
        }
        [Column("PayScaleId")]
        public override int Id { get; set; }
        [Required]
        [StringLength(50)]
        [Display(Name = "Pay Scale")]
        public string PayScaleName { get; set; }
        [Display(Name = "Rate Frequency")]
        public int RateFrequencyId { get; set; }
        public virtual RateFrequency RateFrequency { get; set; }
        [Display(Name = "Level Count")]
        public int? LevelCount { get; set; }
        public virtual ICollection<PayScaleLevel> PayScaleLevel { get; set; }
        public virtual ICollection<Position> Position { get; set; }
    }
}
