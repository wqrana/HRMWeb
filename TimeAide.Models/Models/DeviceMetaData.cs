using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TimeAide.Web.Models;

namespace TimeAide.Models.Models
{
    public class DeviceMetaData: BaseUserObjects
    {

     
            public override int Id { get; set; }
       
           public String DeviceDetails { get; set; }
            public String Location { get; set; }
            public DateTime LastLoggedIn { get; set; }

           public string IPaddress { get; set; }
        public bool IsVerified { get; set; }
            
       
    }
}
