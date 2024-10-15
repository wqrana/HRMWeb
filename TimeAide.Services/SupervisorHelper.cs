using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TimeAide.Common.Helpers;
using TimeAide.Models.ViewModel;
using TimeAide.Web.Models;

namespace TimeAide.Services
{
    public class SupervisorHelper
    {

        public static string GetSupervisedEmployeeIds(Data.tReportWeek model)
        {
            TimeAideContext db = new TimeAideContext();
            string employeeList = "";
            employeeList = db.sp_GetSupervisedEmployeeIds(SessionHelper.SelectedClientId, SessionHelper.LoginId, model, "sp_GetSupervisedEmployeeIds");
                                    //.Select(row => new UserInformationViewModel() { Id = row.Id, FirstLastName = row.FirstLastName, FirstName = row.FirstName, SecondLastName = row.SecondLastName, ShortFullName = row.ShortFullName }).ToList();

            return employeeList;
        }

        public static string GetSupervisedEmployees(Data.tReportWeek model)
        {
            TimeAideContext db = new TimeAideContext();
            string employeeList = "";
            employeeList = db.sp_GetSupervisedEmployeeIds(SessionHelper.SelectedClientId, SessionHelper.LoginId, model, "sp_GetSupervisedEmployees");
            //.Select(row => new UserInformationViewModel() { Id = row.Id, FirstLastName = row.FirstLastName, FirstName = row.FirstName, SecondLastName = row.SecondLastName, ShortFullName = row.ShortFullName }).ToList();

            return employeeList;
        }
    }
}
