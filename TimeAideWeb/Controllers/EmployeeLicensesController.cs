
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using TimeAide.Web.Models;
using TimeAide.Web.ViewModel;
using TimeAide.Common.Helpers;
using TimeAide.Services.Helpers;
using System.Data.Entity.Validation;
using System.Diagnostics;
using System.IO;
using TimeAide.Models.ViewModel;
using System.Data.SqlClient;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using TimeAide.Web.Extensions;

namespace TimeAide.Web.Controllers
{
   
    public class EmployeeLicensesController : BaseTAWindowRoleRightsController<EmployeeAttendenceSchedule>
    {                                                    
        public ActionResult Index()
        {
            try
            {
              
                return PartialView();
            }
            catch (Exception ex)
            {
                Helpers.ErrorLogHelper.InsertLog(Helpers.ErrorLogType.Error, ex, this.ControllerContext);
                HandleErrorInfo handleErrorInfo = new HandleErrorInfo(ex, "EmployeeLicenses", "Index");
                return PartialView("Error", handleErrorInfo);
            }
        }

        public ActionResult GetDashboardMyCurrentLicensesWidget(int? id)
        {
           // var userId = 248;//SessionHelper.LoginId;
           var userId = id ?? SessionHelper.LoginId;
            List<CompensationBalance> compensationBalances = null;
            try
            {
                var userInfo = timeAideWebContext.UserInformation.Where(w => w.Id == userId).Select(s => new { EmployeeId = s.EmployeeId, OldCompanyId = s.Company.Old_Id }).FirstOrDefault();
                var accuralTypes = timeAideWindowContext.tAccrualTypes.Select(s => s.sAccrualCodeID).ToArray();
                var accuralTypeStr = string.Join(",", accuralTypes);
                compensationBalances = timeAideWindowContext.spSS_CompensationBalances(userInfo.EmployeeId, accuralTypeStr, DateTime.Today)
                                .AsEnumerable()
                               .Select(s => new CompensationBalance
                               {
                                   UserInformationId = userId,
                                   EmployeeId = userInfo.EmployeeId,
                                   UserID = s.UserID,
                                   AccrualType = s.AccrualType,
                                   Balance = s.Balance,
                                  RemainingBalance = s.RemainingBalance,
                                   SearchDate = s.SearchDate
                               }).OrderByDescending(o=>o.Balance).ToList();
            }

            catch (Exception ex)
            {
                Helpers.ErrorLogHelper.InsertLog(Helpers.ErrorLogType.Error, ex, this.ControllerContext);
                HandleErrorInfo handleErrorInfo = new HandleErrorInfo(ex, "EmployeeLicenses", "GetDashboardMyCurrentLicensesWidget");
                return PartialView("Error", handleErrorInfo);
            }
            return PartialView("DashboardCurrentLicensesWidget", compensationBalances);
        }
    }
}