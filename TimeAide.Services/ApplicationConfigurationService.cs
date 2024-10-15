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
    public class ApplicationConfigurationService
    {
        private TimeAideContext db;
        private int _ClientId;
        public ApplicationConfigurationService(int clientId)
        {
            db = new TimeAideContext();
            _ClientId = clientId;
        }

        public ApplicationConfigurationService()
        {
            db = new TimeAideContext();
            _ClientId = SessionHelper.SelectedClientId;
        }

        public int UserActivationCodeDays
        {
            get
            {
                var applicationConfiguration = db.ApplicationConfiguration.FirstOrDefault(c => c.ApplicationConfigurationName == "UserActivationCodeDays" && c.ClientId == _ClientId);
                if (applicationConfiguration != null)
                {
                    int userActivationCodeDays;
                    int.TryParse(applicationConfiguration.ApplicationConfigurationValue, out userActivationCodeDays);
                    return userActivationCodeDays;
                }
                return 0;
            }
        }

        public int EmployeeTimeSheetApproval
        {
            get
            {
                var applicationConfiguration = db.ApplicationConfiguration.FirstOrDefault(c => c.ApplicationConfigurationName == "EmployeeTimeSheetApproval" && c.ClientId == _ClientId);
                if (applicationConfiguration != null)
                {
                    int employeeTimeSheetApproval;
                    int.TryParse(applicationConfiguration.ApplicationConfigurationValue, out employeeTimeSheetApproval);
                    return employeeTimeSheetApproval;
                }
                return 0;
            }
        }

        public int SupervisorTimeSheetApproval
        {
            get
            {
                var applicationConfiguration = db.ApplicationConfiguration.FirstOrDefault(c => c.ApplicationConfigurationName == "SupervisorTimeSheetApproval" && c.ClientId == _ClientId);
                if (applicationConfiguration != null)
                {
                    int supervisorTimeSheetApproval;
                    int.TryParse(applicationConfiguration.ApplicationConfigurationValue, out supervisorTimeSheetApproval);
                    return supervisorTimeSheetApproval;
                }
                return 0;
            }
        }
        public static bool IsWidgetAvailable(string widgetName)
        {
            TimeAideContext db1 = new TimeAideContext();
            var applicationConfiguration = db1.ApplicationConfiguration.FirstOrDefault(c => c.ApplicationConfigurationName == widgetName && c.ClientId == SessionHelper.SelectedClientId && (c.CompanyId == SessionHelper.SelectedCompanyId || !c.CompanyId.HasValue));
            if (applicationConfiguration != null)
                return applicationConfiguration.ValueAsBoolean;
            return false;
        }
        public static bool GetConfigurationStatus(string configurationName)
        {
            //if (SecurityHelper.IsSuperAdmin)
            //    return true;
            TimeAideContext db1 = new TimeAideContext();
            var applicationConfiguration = db1.ApplicationConfiguration.FirstOrDefault(c => c.ApplicationConfigurationName == configurationName && c.ClientId == SessionHelper.SelectedClientId && (c.CompanyId == SessionHelper.SelectedCompanyId || !c.CompanyId.HasValue));
            if (applicationConfiguration != null)
                return applicationConfiguration.ValueAsBoolean;
            return false;
        }
        //public static ApplicationConfiguration GetApplicationConfigurationByName(string configurationName)
        //{
        //    TimeAideContext db1 = new TimeAideContext();

        //    return applicationConfiguration;
        //}
        public static ApplicationConfiguration GetApplicationConfiguration(string configurationName)
        {
            return GetApplicationConfiguration(configurationName, SessionHelper.SelectedCompanyId, SessionHelper.SelectedClientId);
        }
        public static ApplicationConfiguration GetApplicationConfiguration(string configurationName, int? companyId, int clientId)
        {
            TimeAideContext db1 = new TimeAideContext();
            var applicationConfiguration = db1.GetAllByCompany<ApplicationConfiguration>(companyId, clientId).FirstOrDefault(c => c.ApplicationConfigurationName == configurationName);
            if (applicationConfiguration == null)
                applicationConfiguration = db1.GetAll<ApplicationConfiguration>(clientId).FirstOrDefault(c => c.ApplicationConfigurationName == configurationName);
            return applicationConfiguration;
        }
        //SessionTimeOut
    }
}
