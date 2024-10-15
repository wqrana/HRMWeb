using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TimeAide.Web.Models;

namespace TimeAide.Services
{
    public class RoleService
    {
        public static List<Role> GetRole(int? companyId, int clientId)
        {
            TimeAideContext db = new TimeAideContext();
            return db.GetAll<Role>(clientId).Where(u => (u.RoleTypeId != 1 && u.ClientId==clientId)).ToList();
        }

        public static Role GetRole(int roleId)
        {
            TimeAideContext db = new TimeAideContext();
            return db.Role.FirstOrDefault(r=>r.Id == roleId);
        }
        public static List<RoleType> GetRoleType()
        {
            TimeAideContext db = new TimeAideContext();
            return db.RoleType.Where(u => (u.DataEntryStatus != 2)).ToList();
        }
        
    }
}
