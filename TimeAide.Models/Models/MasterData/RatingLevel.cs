using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TimeAide.Web.Models
{
    [Table("RatingLevel")]
    public partial class RatingLevel : BaseEntity
    {
        public RatingLevel()
        {
            AppraisalRatingScaleDetail = new HashSet<AppraisalRatingScaleDetail>();
        }

        [Column("RatingLevelId")]
        [Display(Name = "Rating Level Id")]
        public override int Id { get; set; }

        public string RatingLevelName { get; set; }

        
        public virtual ICollection<AppraisalRatingScaleDetail> AppraisalRatingScaleDetail { get; set; }
    }
}
