using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TimeAide.Common.Helpers;

namespace TimeAide.Data
{
    public partial class tReportWeek
    {
        public tReportWeek() : base()
        {
            nCompID = SessionHelper.SelectedCompanyId_Old;
        }
        [NotMapped]
        public string SelectedEmployeeIds { get; set; }
        public List<tPunchDate> tPunchDate { get; set; }
        [NotMapped]
        public int UserInformationId { get; set; }

        [NotMapped]
        public string Notes { get; set; }

        //[NotMapped]
        //public int intUserReviewed { get; set; }
        //[NotMapped]
        //public int intUserReviewedID { get; set; }
        //[NotMapped]
        //public string strUserReviewedName { get; set; }
        //[NotMapped]
        //public DateTime dtUserReviewDate { get; set; }
    }
}
