using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TimeAide.Common.Helpers;
using TimeAide.Web.Models;

namespace TimeAide.Services
{
    public class EmployeeGroupService
    {
        public static List<EmployeeGroupType> GetEmployeeGroupType()
        {
            TimeAideContext db = new TimeAideContext();
            return db.EmployeeGroupType.Where(u => (u.DataEntryStatus != 2)).ToList();
        }
        public static string GetUserEmployeeGroupsAsString(int userInformationId)
        {
            TimeAideContext db = new TimeAideContext();
            return String.Join(", ", GetUserEmployeeGroup(userInformationId).Select(c => c.EmployeeGroup.EmployeeGroupName.ToString()));
        }
        public static List<UserEmployeeGroup> GetUserEmployeeGroup(int userInformationId)
        {
            TimeAideContext db = new TimeAideContext();
            return db.UserEmployeeGroup.Where(w => w.DataEntryStatus == 1 && w.EmployeeGroup.ClientId == SessionHelper.SelectedClientId && w.UserInformationId == userInformationId).ToList();
        }
    }
}
