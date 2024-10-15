using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TimeAide.Web.Models;

namespace TimeAide.Models.Models
{
    public class SecurityCode:BaseUserObjects
    {

            public override int Id { get; set; }

            [Required]
            [Display(Name = "Code")]
            public int Code { get; set; }

            public bool IsUsed { get; set; }
        public bool IsValid { get; set; }
        public int DeviceMetaDataId { get; set; }
          

           
        
    }
}
