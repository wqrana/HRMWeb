using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TimeAide.Web.Models;

namespace TimeAide.Services
{
    public class EmploymentService
    {
        public static UserInformation GetUserInformation(string loginEmail)
        {
            TimeAideContext db = new TimeAideContext();
            return db.UserContactInformation.Where(w => w.DataEntryStatus == 1 && w.LoginEmail == loginEmail).FirstOrDefault().UserInformation;
        }
        public static List<Employment> GetEmployments(int userInformationId, TimeAideContext db = null)
        {
            if (db == null)
                db = new TimeAideContext();
            return db.Employment.Where(w => w.DataEntryStatus == 1 && w.UserInformationId == userInformationId).OrderByDescending(e => e.OriginalHireDate).ToList();
        }
        public static Employment GetEmployment(int employmentId)
        {
            TimeAideContext db = new TimeAideContext();
            return db.Employment.FirstOrDefault(w => w.DataEntryStatus == 1 && w.Id == employmentId);
        }
        public static Employment GetActiveEmployment(int userInformationId, TimeAideContext db = null)
        {
            var employments = GetEmployments(userInformationId, db);
            return employments.FirstOrDefault(e => !e.TerminationDate.HasValue);
        }

        public static List<Employment> GetClosedEmployments(int userInformationId)
        {
            var employments = GetEmployments(userInformationId);
            return employments.Where(e => e.TerminationDate.HasValue).ToList();
        }
        public static bool IsActiveEmployment(int employmentId)
        {
            var employment = GetEmployment(employmentId);
            if (employment != null && !employment.IsClosed)
                return true;
            return false;
        }
    }
}
