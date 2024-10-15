using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TimeAide.Common.Helpers;
using TimeAide.Web.Models;

namespace TimeAide.Services
{
    public class EmployeeCompensationService
    {
        public static List<EmployeeCompensation> ActiveCompensation(int userInformationId)
        {
            TimeAideContext db = new TimeAideContext();
            return db.GetAllByUser<EmployeeCompensation>(userInformationId, SessionHelper.SelectedClientId).Where(e => !e.EndDate.HasValue || e.EndDate >= DateTime.Now.Date).OrderByDescending(e => e.CreatedDate).ToList(); ;
        }
        public static EmployeeCompensation ActiveCompensation(int userInformationId,int companyCompensationId)
        {
            TimeAideContext db = new TimeAideContext();
            return db.GetAllByUser<EmployeeCompensation>(userInformationId, SessionHelper.SelectedClientId).Where(e => (!e.EndDate.HasValue || e.EndDate >= DateTime.Now.Date) &&  e.CompanyCompensationId== companyCompensationId).OrderByDescending(e => e.CreatedDate).FirstOrDefault();
        }
        public static List<EmployeeCompensation> InactiveCompensation(int userInformationId)
        {
            TimeAideContext db = new TimeAideContext();
            return db.GetAllByUser<EmployeeCompensation>(userInformationId, SessionHelper.SelectedClientId).Where(e => e.EndDate.HasValue && e.EndDate < DateTime.Now.Date).OrderByDescending(e => e.CreatedDate).ToList();

        }

        public static List<EmployeeCompensation> GetCompensation(int userInformationId, int companyCompensationId)
        {
            TimeAideContext db = new TimeAideContext();
            return db.GetAllByUser<EmployeeCompensation>(userInformationId, SessionHelper.SelectedClientId).Where(e => e.CompanyCompensationId == companyCompensationId).OrderBy(e => e.StartDate).ToList();
        }
    }
}
