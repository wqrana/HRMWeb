using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using TimeAide.Data;

namespace TimeAide.Web.ViewModel
{
    public class WebPunchViewModel
    {
        public tPunchDate tPunchDate { get; set; }
        public List<tPunchData> tPunchData { get; set; }
    }
}