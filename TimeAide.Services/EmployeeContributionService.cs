using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TimeAide.Common.Helpers;
using TimeAide.Web.Models;

namespace TimeAide.Services
{
    public class EmployeeContributionService
    {
        public static List<EmployeeContribution> ActiveContribution(int userInformationId)
        {
            TimeAideContext db = new TimeAideContext();
            return db.GetAllByUser<EmployeeContribution>(userInformationId, SessionHelper.SelectedClientId).Where(e => !e.EndDate.HasValue || e.EndDate >= DateTime.Now.Date).OrderByDescending(e => e.CreatedDate).ToList(); ;
        }
        public static EmployeeContribution ActiveContribution(int userInformationId,int companyContributionId)
        {
            TimeAideContext db = new TimeAideContext();
            return db.GetAllByUser<EmployeeContribution>(userInformationId, SessionHelper.SelectedClientId).Where(e => (!e.EndDate.HasValue || e.EndDate >= DateTime.Now.Date) &&  e.CompanyContributionId== companyContributionId).OrderByDescending(e => e.CreatedDate).FirstOrDefault();
        }
        public static List<EmployeeContribution> InactiveContribution(int userInformationId)
        {
            TimeAideContext db = new TimeAideContext();
            return db.GetAllByUser<EmployeeContribution>(userInformationId, SessionHelper.SelectedClientId).Where(e => e.EndDate.HasValue && e.EndDate < DateTime.Now.Date).OrderByDescending(e => e.CreatedDate).ToList();

        }

        public static List<EmployeeContribution> GetContribution(int userInformationId, int companyContributionId)
        {
            TimeAideContext db = new TimeAideContext();
            return db.GetAllByUser<EmployeeContribution>(userInformationId, SessionHelper.SelectedClientId).Where(e => e.CompanyContributionId == companyContributionId).OrderBy(e => e.StartDate).ToList();
        }
    }
}
