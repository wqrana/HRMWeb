using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TimeAide.Web.Models;

namespace TimeAide.Services
{
    public class PayInformationHistoryService
    {
        public static PayInformationHistory GetPayInformationHistory(int id, TimeAideContext db)
        {
            return db.PayInformationHistory.FirstOrDefault(w => w.DataEntryStatus == 1 && w.Id == id);
        }

        public static PayInformationHistory GetActivePayInformationHistory(int userInformationId)
        {
            TimeAideContext db = new TimeAideContext();
            return db.PayInformationHistory.Where(w => w.DataEntryStatus == 1 && w.UserInformationId == userInformationId).OrderByDescending(e => e.StartDate).FirstOrDefault();
        }
    }
}
