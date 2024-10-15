using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace TimeAide.Web.ViewModel
{
    public class PagingViewModel
    {
        public int TotolRecords { get; set; }
        public int TotalPages { get; set; }
        public int StartRecord { get; set; }
        public int EndRecord  { get; set;}
        public int CurrentPage { get; set; }
    }
}