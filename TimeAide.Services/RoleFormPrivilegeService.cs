using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TimeAide.Common.Helpers;
using TimeAide.Services.Helpers;
using TimeAide.Web.Models;
using TimeAide.Web.ViewModel;

namespace TimeAide.Services
{
    public class RoleFormPrivilegeService
    {
        private TimeAideContext db = new TimeAideContext();
        public void GetMatrixFromDB(RoleFormPrivilegeMatrixViewModel1 model)
        {
            var tempRoleId = model.RoleId;
            model.RoleId = tempRoleId;
            model.RoleFormPrivileges = new List<RoleFormPrivilegeViewModel1>();
            var roleFormPrivileges = db.RoleFormPrivilege.Where(p => p.RoleId == model.RoleId);
            var forms = db.Form.Where(f=>f.DataEntryStatus==1).AsQueryable();
            if (model.ModuleId.HasValue && model.ModuleId > 0)
            {
                List<int> ids = new List<int>();
                ids.Add(model.ModuleId ?? 0);
                ModuleService.GetModuleChildren(model.ModuleId ?? 0, ids);
                roleFormPrivileges = roleFormPrivileges.Where(r => ids.Contains(r.Form.ModuleId));
                forms = forms.Where(f => ids.Contains(f.ModuleId));
            }

            foreach (var eachForm in forms.Where(f => (string.IsNullOrEmpty(model.FormName) || f.FormName.ToLower().Contains(model.FormName.ToLower())) && f.DataEntryStatus==1))
            {
                var itemInDbs = roleFormPrivileges.Where(p => p.FormId == eachForm.Id && p.RoleId == model.RoleId).ToList();
                if (itemInDbs == null || itemInDbs.Count() == 0)
                {
                    model.RoleFormPrivileges.Add(new RoleFormPrivilegeViewModel1() { Form = eachForm, FormId = eachForm.Id, RoleId = model.RoleId, AllowAdd = false, AllowEdit = false, AllowDelete = false, AllowView = false, AllowChangeHistory = false });
                }
                else
                {
                    model.RoleFormPrivileges.Add(GetView(itemInDbs, eachForm, model.RoleId));

                }
            }
            //model.RoleFormPrivileges = roleTypeFormPrivileges;
        }

        public RoleFormPrivilegeViewModel1 GetView(List<RoleFormPrivilege> itemInDbs, Form eachForm, int? roleID)
        {
            var newItem = new RoleFormPrivilegeViewModel1() { Form = eachForm, FormId = eachForm == null ? 0 : eachForm.Id, RoleId = roleID };
            var itemInDb = itemInDbs.FirstOrDefault(p => p.PrivilegeId == 1);
            if (itemInDb == null)
                newItem.AllowAdd = false;
            else
                newItem.AllowAddInt = itemInDb.DataEntryStatus;

            itemInDb = itemInDbs.FirstOrDefault(p => p.PrivilegeId == 2);
            if (itemInDb == null)
                newItem.AllowEdit = false;
            else
                newItem.AllowEditInt = itemInDb.DataEntryStatus;

            itemInDb = itemInDbs.FirstOrDefault(p => p.PrivilegeId == 3);
            if (itemInDb == null)
                newItem.AllowDelete = false;
            else
                newItem.AllowDeleteInt = itemInDb.DataEntryStatus;

            itemInDb = itemInDbs.FirstOrDefault(p => p.PrivilegeId == 4);
            if (itemInDb == null)
                newItem.AllowView = false;
            else
                newItem.AllowViewInt = itemInDb.DataEntryStatus;

            itemInDb = itemInDbs.FirstOrDefault(p => p.PrivilegeId == 5);
            if (itemInDb == null)
                newItem.AllowChangeHistory = false;
            else
                newItem.AllowChangeHistoryInt = itemInDb.DataEntryStatus;

            return newItem;
        }

        public RoleFormPrivilegeViewModel1 GetRoleFormPrivilege(string formName, int userId)
        {

            try
            {
                var RoleFormPrivileges = new List<RoleFormPrivilegeViewModel1>();
                var form = db.Form.FirstOrDefault(p => p.FormName == formName && p.DataEntryStatus == 1 && p.Module.DataEntryStatus==1);
                if (form == null)
                    return new RoleFormPrivilegeViewModel1() { IsFormDeleted = true };
                var userRole = db.UserInformationRole.FirstOrDefault(p => p.UserInformationId == userId);
                if (userRole == null)
                    return new RoleFormPrivilegeViewModel1() { Form = form, FormId = form.Id, RoleId = 0, AllowAdd = false, AllowDelete = false, AllowEdit = false, AllowView = false, AllowChangeHistory = false };
                var roleFormPrivilege = db.RoleFormPrivilege.Where(p => p.RoleId == userRole.RoleId && p.Form.FormName == formName && p.DataEntryStatus == 1).ToList();
                return GetView(roleFormPrivilege, form, userRole.RoleId);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public bool GetRoleModulePrivilege(string moduleName, int userId)
        {

            try
            {
                var RoleFormPrivileges = new List<RoleFormPrivilegeViewModel1>();
                var module = db.Module.FirstOrDefault(p => p.ModuleName == moduleName && p.DataEntryStatus==1);
                if (module == null)
                    return false;
                if (!CheckModuleAvailibilityByDataEntryStatus(module))
                    return false;
                if (SecurityHelper.IsSuperAdmin)
                    return true;
                var userRole = db.UserInformationRole.FirstOrDefault(p => p.UserInformationId == userId);
                if (userRole == null)
                    return false;
                return CheckModuleAvailibility(module, userRole);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        private bool CheckModuleAvailibility(Module module, UserInformationRole userRole)
        {
            var roleFormPrivilege = db.RoleFormPrivilege.Where(p => p.RoleId == userRole.RoleId && p.Form.ModuleId == module.Id && p.Form.DataEntryStatus==1 && p.DataEntryStatus == 1 && !p.Form.IsSuperUserOnlyForm).ToList();
            if (roleFormPrivilege.Count > 0)
                return true;
            var subModules = db.Module.Where(p => p.ParentModuleId == module.Id && p.DataEntryStatus==1);
            if (subModules.Count() == 0)
                return false;
            foreach (var eachModule in subModules)
            {
                if (CheckModuleAvailibility(eachModule, userRole))
                {
                    return true;
                }
            }
            return false;
        }
        private bool CheckModuleAvailibilityByDataEntryStatus(Module module)
        {
            var subModules = db.Module.Where(p => p.ParentModuleId == module.Id && p.DataEntryStatus == 1 && p.ParentModule.DataEntryStatus==1);
            var forms = db.Form.Where(p => p.ModuleId == module.Id && p.DataEntryStatus == 1);
            if (subModules.Count() == 0 && forms.Count()==0)
                return false;
            //foreach (var eachModule in subModules)
            //{
            //    if (CheckModuleAvailibilityByDataEntryStatus(eachModule))
            //    {
            //        return true;
            //    }
            //}
            return true;
        }

        public static RoleFormPrivilegeViewModel1 GetFormPrivileges(string formName)
        {
            TimeAideContext db1 = new TimeAideContext();
            RoleFormPrivilegeViewModel1 privileges;
            var form = db1.Form.FirstOrDefault(p => p.FormName == formName && p.DataEntryStatus == 1 && p.Module.DataEntryStatus==1);
            if (form == null)
                return  new RoleFormPrivilegeViewModel1() { IsFormDeleted=true};
            if (SecurityHelper.IsSuperAdmin)
                privileges = new RoleFormPrivilegeViewModel1() { Form = form, FormId = form.Id, RoleId = 0, AllowAdd = true, AllowDelete = true, AllowEdit = true, AllowView = true, AllowChangeHistory = true };
            else
            {
                var userRole = db1.UserInformationRole.FirstOrDefault(p => p.UserInformationId == SessionHelper.LoginId);
                if (userRole == null)
                    privileges = new RoleFormPrivilegeViewModel1() { Form = form, FormId = form.Id, RoleId = 0, AllowAdd = false, AllowDelete = false, AllowEdit = false, AllowView = false, AllowChangeHistory = true };
                else
                {
                    var roleFormPrivilege = db1.RoleFormPrivilege.Where(p => p.RoleId == userRole.RoleId && p.Form.FormName == form.FormName).ToList();
                    privileges = (new RoleFormPrivilegeService()).GetView(roleFormPrivilege, form, userRole.RoleId);
                }
            }

            return privileges;
        }
    }
}
