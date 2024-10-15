using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using TimeAide.Common.Helpers;
using TimeAide.Web.Models;

namespace TimeAide.Services.Helpers
{
    public static class SecurityHelper
    {
        public static bool IsSuperAdmin
        {
            get
            {
                if (SessionHelper.RoleTypeId == 1)
                {
                    return true;
                }
                return false;
            }
        }
        public static bool IsAdmin
        {
            get
            {
                if (SessionHelper.RoleTypeId == 2)
                {
                    return true;
                }
                return false;
            }
        }
        public static bool IsSupervisor
        {
            get
            {
                if (SessionHelper.RoleTypeId == 3)
                {
                    return true;
                }
                return false;
            }
        }
        public static bool IsUser
        {
            get
            {
                if (SessionHelper.RoleTypeId == 4)
                {
                    return true;
                }
                return false;
            }
        }
        public static bool IsAvailable(string formName)
        {
            var privilege = (new RoleFormPrivilegeService()).GetRoleFormPrivilege(formName, SessionHelper.LoginId);
            if (privilege.IsFormDeleted)
                return false;
            if (SecurityHelper.IsSuperAdmin)
                return true;
            return privilege.AllowView;
        }
        public static bool IsModuleAvailable(string moduleName)
        {
            var privilege = (new RoleFormPrivilegeService()).GetRoleModulePrivilege(moduleName, SessionHelper.LoginId);
            return privilege;
        }
        public static bool AllowEdit(string formName)
        {
            var privilege = (new RoleFormPrivilegeService()).GetRoleFormPrivilege(formName, SessionHelper.LoginId);
            if (privilege == null)
                return false;
            return privilege.AllowEdit;
        }
        public static bool AllowView(string formName)
        {
            var privilege = (new RoleFormPrivilegeService()).GetRoleFormPrivilege(formName, SessionHelper.LoginId);
            if (privilege == null)
                return false;
            return privilege.AllowView;
        }
        public static bool AllowDelete(string formName)
        {
            var privilege = (new RoleFormPrivilegeService()).GetRoleFormPrivilege(formName, SessionHelper.LoginId);
            if (privilege == null)
                return false;
            return privilege.AllowDelete;
        }
        public static bool AllowAdd(string formName)
        {
            var privilege = (new RoleFormPrivilegeService()).GetRoleFormPrivilege(formName, SessionHelper.LoginId);
            if (privilege == null)
                return false;
            return privilege.AllowAdd;
        }
        public static bool AllowChangeHistory(string formName)
        {
            var privilege = (new RoleFormPrivilegeService()).GetRoleFormPrivilege(formName, SessionHelper.LoginId);
            if (privilege == null)
                return false;
            return privilege.AllowChangeHistory;
        }
        public static string LoginUser
        {
            get
            {
                return System.Web.HttpContext.Current.Request.LogonUserIdentity.Name;
            }
        }

        public static bool HasEmployeeTimeAndAttendanceSetting
        {
            get
            {
                var model = (new TimeAideContext()).GetAllByUser<EmployeeTimeAndAttendanceSetting>(SessionHelper.LoginId, SessionHelper.SelectedClientId).FirstOrDefault();
                if(model!=null)
                {
                   return model.EnableWebPunch;
                }
                return false;
            }
        }
        public static bool HasServiceConfiguration
        {
            get
            {
                var configuration = (new TimeAideContext()).GetAllByCompany<WebPunchConfiguration>(SessionHelper.SelectedCompanyId, SessionHelper.SelectedClientId).FirstOrDefault();
                if (configuration == null)
                    return false;
                else
                    return true;
            }
        }

        public static bool IsSecuirtyCodeExpired(this DateTime date)
        {
            DateTime CodeCreationDate = Convert.ToDateTime(date);
            DateTime ending = CodeCreationDate.AddHours(23).AddMinutes(59).AddSeconds(59);

            var n = DateTime.Compare(ending, DateTime.Now);
            if (n == -1)
            {
                // greater than 24 hour
                return true;
            }
            else
            {
                                
             // within 24 hour from creation
               return false;
            }
        }


    }
}