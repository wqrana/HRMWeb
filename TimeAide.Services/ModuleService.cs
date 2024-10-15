using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TimeAide.Web.Models;

namespace TimeAide.Services
{
    public class ModuleService
    {
        public static List<Module> GetModules()
        {
            TimeAideContext db = new TimeAideContext();
            return db.GetAll<Module>(1).ToList();
        }
        public static void GetModuleChildren(int parentModuleId, List<int> childIds)
        {
            TimeAideContext db = new TimeAideContext();
            foreach (var each in db.Module.Where(m => m.ParentModuleId == parentModuleId))
            {
                childIds.Add(each.Id);
                GetModuleChildren(each.Id, childIds);
            }
        }

        public static List<Module> GetParentModules()
        {
            TimeAideContext db = new TimeAideContext();
            return db.GetAll<Module>(1).Where(c => !c.ParentModuleId.HasValue).ToList();
        }

        public static List<Module> GetChildModules(int parentModuleId)
        {
            TimeAideContext db = new TimeAideContext();
            return db.GetAll<Module>(1).Where(c => c.ParentModuleId == parentModuleId).ToList();
        }
    }
}
