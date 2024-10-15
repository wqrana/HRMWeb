using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TimeAide.Common.Helpers;
using TimeAide.Web.Models;

namespace TimeAide.Services
{
    public class SubDepartmentService
    {
        public static List<SubDepartment> SubDepartments(int? departmentId = null)
        {
            TimeAideContext db = new TimeAideContext();
            return db.GetAllByCompany<SubDepartment>(SessionHelper.SelectedCompanyId, SessionHelper.SelectedClientId).AsQueryable().Where(SubDepartment.GetPredicate(SessionHelper.SelectedCompanyId, departmentId)).ToList();
        }

        //public static List<SubDepartment> SubDepartments()
        //{
        //    TimeAideContext db = new TimeAideContext();
        //    return db.SubDepartment.Where(c => c.DataEntryStatus == 1 && c.CompanyId == SessionHelper.SelectedCompanyId && c.ClientId == SessionHelper.SelectedClientId).ToList();
        //}
    }
}
