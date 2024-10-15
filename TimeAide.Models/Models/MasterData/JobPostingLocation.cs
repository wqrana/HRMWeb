using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace TimeAide.Web.Models
{

    [Table("JobPostingLocation")]
    public partial class JobPostingLocation : BaseEntity
    {

        [Column("JobPostingLocationId")]
        public override int Id { get; set; }       

        [Display(Name = "Location")]
        public int LocationId { get; set; }
        public int JobPostingDetailId { get; set; }
        public virtual Location Location { get; set; }       
        public virtual JobPostingStatus JobPostingDetail { get; set; }
       
    }
}
