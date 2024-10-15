using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TimeAide.Web.Models
{
    public interface IBaseEntity
    {
        int Id
        {
            get;
            set;
        }
        int CreatedBy { get; set; }
        DateTime CreatedDate { get; set; }
        int DataEntryStatus { get; set; }
        int? ModifiedBy { get; set; }
        DateTime? ModifiedDate { get; set; }
    }
}
