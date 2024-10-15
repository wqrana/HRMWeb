using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;
using TimeAide.Common.Helpers;

namespace TimeAide.Web.Models
{
    public class BaseEntity 
    {
        public BaseEntity()
        {
            if (SessionHelper.LoginId != 0)
                CreatedBy = SessionHelper.LoginId;
            CreatedDate = DateTime.Now;
            DataEntryStatus = 1;
            ClientId = SessionHelper.SelectedClientId;
        }
        public virtual int Id
        {
            get;
            set;
        }
        public int? ClientId { get; set; }
        [ScaffoldColumn(false)]
        public int CreatedBy { get; set; }

        [ScaffoldColumn(false)]
        public DateTime CreatedDate { get; set; }

        [ScaffoldColumn(false)]
        public int DataEntryStatus { get; set; }

        [ScaffoldColumn(false)]
        public int? ModifiedBy { get; set; }

        [ScaffoldColumn(false)]
        public DateTime? ModifiedDate { get; set; }

        public virtual int? Old_Id
        {
            get;
            set;
        }
    }
    
}