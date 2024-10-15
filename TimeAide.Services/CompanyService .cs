using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TimeAide.Common.Helpers;
using TimeAide.Services.Helpers;
using TimeAide.Web.Models;

namespace TimeAide.Services
{
    public class CompanyService
    {
        public static List<Company> GetCompany(int clientId)
        {
            TimeAideContext db = new TimeAideContext();
            List<int> companyIds = new List<int>();
            if (SecurityHelper.IsSuperAdmin)
                return db.GetAll<Company>(clientId).Where(c => c.CompanyName != "None-Company").ToList();
            else
            {
                if (!string.IsNullOrEmpty(SessionHelper.SupervisorCompany))
                    companyIds = SessionHelper.SupervisorCompany.Split(',').Select(int.Parse).ToList();
                //var companies = db.Company.Where(c => c.DataEntryStatus == 1 && c.ClientId == SessionHelper.SelectedClientId && companyIds.Contains(c.Id));
                return db.GetAll<Company>(clientId).Where(c => c.CompanyName != "None-Company" && companyIds.Contains(c.Id)).ToList();
            }
        }

        public static string SelectedCompanyName
        {
            get
            {
                TimeAideContext db = new TimeAideContext();
                var company = db.Company.FirstOrDefault(c => c.Id == SessionHelper.SelectedCompanyId);
                if (company != null)
                    return company.CompanyName;
                else
                    return "";
            }
        }

        public static string GetCompanyName(int companyId)
        {
            TimeAideContext db = new TimeAideContext();
            var company = db.Company.FirstOrDefault(c => c.Id == companyId);
            if (company != null)
                return company.CompanyName;
            else
                return "";
        }

        public static string SelectedCompanyLogo
        {
            get
            {
                if (SessionHelper.SelectedCompanyId == 1)
                    return "/Content/Themes/assets/img/logo2.png";

                TimeAideContext db = new TimeAideContext();
                var company = db.Company.FirstOrDefault(c => c.Id == SessionHelper.SelectedCompanyId);
                FilePathHelper filePathHelper = new FilePathHelper(SessionHelper.SelectedClientId, SessionHelper.SelectedCompanyId, "");
                return filePathHelper.GetCompanyLogoPath(CompanyService.SelectedCompanyName);
            }
        }

        public static string GetCompanyLogo(string companyName)
        {
            if (string.IsNullOrEmpty(companyName))
                return "/Content/Themes/assets/img/logo.png";

            TimeAideContext db = new TimeAideContext();
            var company = db.Company.FirstOrDefault(c => c.CompanyName.ToLower() == companyName.ToLower());
            FilePathHelper filePathHelper = new FilePathHelper(company.ClientId ?? 0, company.Id, "");
            return filePathHelper.GetCompanyLogoPath(company.CompanyName);
        }
    }
}
