using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TimeAide.Web.Models;

namespace TimeAide.Services
{
    public class EmploymentHistoryService
    {
        public static List<UserInformation> GetSupervisors(int? companyId, int clientId)
        {
            TimeAideContext db = new TimeAideContext();
            return db.GetAll<UserEmployeeGroup>(clientId).Where(u => u.EmployeeGroup.EmployeeGroupTypeId == 2).Select(U => U.UserInformation).Distinct().ToList();
        }
        public static string GetSupervisorSelectedIds(int userInformationId)
        {
            TimeAideContext db = new TimeAideContext();
            //return String.Join(",", db.EmployeeSupervisor.Where(w => w.DataEntryStatus == 1 && w.EmploymentHistoryId == employmentHistoryId).Select(c => c.SupervisorUserId.ToString()));
            return String.Join(",", db.EmployeeSupervisor.Where(w => w.DataEntryStatus == 1 && w.EmployeeUserId== userInformationId).Select(c => c.SupervisorUser.Id.ToString()));
        }
        
        public static EmploymentHistory GetActiveEmploymentHistory(int userInformationId,int employmentId, TimeAideContext db = null)
        {
            if (db == null)
                db = new TimeAideContext();
            return db.EmploymentHistory.Where(w => w.DataEntryStatus == 1 && w.UserInformationId == userInformationId && w.EmploymentId== employmentId).OrderByDescending(e => e.StartDate).FirstOrDefault();
        }
        public static EmploymentHistory GetEmploymentHistory(int id)
        {
            TimeAideContext db = new TimeAideContext();
            return GetEmploymentHistory(id, db);
        }
        public static EmploymentHistory GetEmploymentHistory(int id, TimeAideContext db)
        {
            return db.EmploymentHistory.FirstOrDefault(w => w.DataEntryStatus == 1 && w.Id == id);
        }
    }
}
