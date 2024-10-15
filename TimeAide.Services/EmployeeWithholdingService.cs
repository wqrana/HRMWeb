using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TimeAide.Common.Helpers;
using TimeAide.Web.Models;

namespace TimeAide.Services
{
    public class EmployeeWithholdingService
    {
        public static List<EmployeeWithholding> ActiveEmployeeWithholding(int userInformationId)
        {
            TimeAideContext db = new TimeAideContext();
            return db.GetAllByUser<EmployeeWithholding>(userInformationId, SessionHelper.SelectedClientId).Where(e => !e.EndDate.HasValue || e.EndDate >= DateTime.Now.Date).OrderByDescending(e => e.CreatedDate).ToList(); ;
        }
        public static EmployeeWithholding ActiveEmployeeWithholding(int userInformationId,int companyWithholdingId)
        {
            TimeAideContext db = new TimeAideContext();
            return db.GetAllByUser<EmployeeWithholding>(userInformationId, SessionHelper.SelectedClientId).Where(e => (!e.EndDate.HasValue || e.EndDate >= DateTime.Now.Date) &&  e.CompanyWithholdingId== companyWithholdingId).OrderByDescending(e => e.CreatedDate).FirstOrDefault();
        }
        public static List<EmployeeWithholding> InactiveEmployeeWithholding(int userInformationId)
        {
            TimeAideContext db = new TimeAideContext();
            return db.GetAllByUser<EmployeeWithholding>(userInformationId, SessionHelper.SelectedClientId).Where(e => e.EndDate.HasValue && e.EndDate < DateTime.Now.Date).OrderByDescending(e => e.CreatedDate).ToList();

        }

        public static List<EmployeeWithholding> GetWithholding(int userInformationId, int companyWithholdingId)
        {
            TimeAideContext db = new TimeAideContext();
            return db.GetAllByUser<EmployeeWithholding>(userInformationId, SessionHelper.SelectedClientId).Where(e => e.CompanyWithholdingId == companyWithholdingId).OrderBy(e => e.StartDate).ToList();
        }
    }
}
