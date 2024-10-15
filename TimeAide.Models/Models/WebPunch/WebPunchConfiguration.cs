using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TimeAide.Web.Models
{
    public class WebPunchConfiguration : BaseCompanyObjects
    {
        public WebPunchConfiguration()
        {
        }

        [Display(Name = "Id")]
        [Column("WebPunchConfigurationId")]
        public override int Id { get; set; }
        [Display(Name = "Service Url")]
        [Required]
        [StringLength(1000)]
        public string PunchServiceUrl { get; set; }
        [Display(Name = "Company Id")]
        [Required]
        public int PunchServiceCompanyId { get; set; }
        [Display(Name = "Company Password")]
        [Required]
        [StringLength(500)]
        public string PunchServiceCompanyPassword { get; set; }
        [Display(Name = "Use GPS Coordinates")]
        public Boolean UseGPSCoordinates { get; set; }
        [Display(Name = "Use Job Code")]
        public Boolean UseJobCode { get; set; }
        [Display(Name = "API Key")]
        [StringLength(500)]
        public string APIKey { get; set; }
        
    }
}
