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
using TimeAide.Services;
using TimeAide.Data;

namespace TimeAide.Web.Controllers
{
    public class UserDashboardController : TimeAideWebBaseControllers
    {
        private TimeAideContext db = new TimeAideContext();
        // GET: UserDashboard
        public ActionResult Index()
        {

            var privileges = RoleFormPrivilegeService.GetFormPrivileges("Dashboard");
            if (!privileges.AllowAdd)
            {
                throw new AuthorizationException();
            }
            var model = new DashboardViewModel();
            var userInfo = db.UserInformation.Where(w => w.Id == SessionHelper.LoginId).FirstOrDefault();
            model.UserName = userInfo.ShortFullName;
            model.EmployeeId = userInfo.EmployeeId;
            model.UserProfilePicture = SessionHelper.UserProfilePicture;
            var stats = new CompanyStats();
            //stats.TotalEmpoyees = db.GetAllByCompany<UserInformation>(SessionHelper.SelectedCompanyId, SessionHelper.SelectedClientId).Where(w=>w.EmployeeStatusId==1).Count();
            //stats.FemaleEmployees = db.GetAllByCompany<UserInformation>(SessionHelper.SelectedCompanyId, SessionHelper.SelectedClientId).Where(u => u.GenderId == 2 && u.EmployeeStatusId==1).Count();
            //stats.MaleEmployees = db.GetAllByCompany<UserInformation>(SessionHelper.SelectedCompanyId, SessionHelper.SelectedClientId).Where(u => u.GenderId == 1 && u.EmployeeStatusId == 1).Count();
            //var first = new DateTime(DateTime.Now.Year, DateTime.Now.Month, 1);
            //var last = first.AddMonths(1).AddDays(-1);
            //stats.NewHiring = db.GetAll<Employment>(SessionHelper.SelectedClientId).Where(u => u.OriginalHireDate >= first && u.OriginalHireDate <= last && u.UserInformation.CompanyId == SessionHelper.SelectedCompanyId &&(u.UserInformation.EmployeeStatusId==1 || u.UserInformation.EmployeeStatusId ==2)).Count();
            //stats.NewTerminations = db.GetAll<Employment>(SessionHelper.SelectedClientId).Where(u => u.TerminationDate >= first && u.TerminationDate <= last && u.UserInformation.CompanyId == SessionHelper.SelectedCompanyId && u.UserInformation.EmployeeStatusId == 3).Count();
            //model.CompanyStats = stats;
            if (SessionHelper.RoleTypeId == 4)
                return View("MyDashboard", model);

            return View(model);
        }
        public ActionResult MyDashboard(string id)
        {
            var privileges = RoleFormPrivilegeService.GetFormPrivileges("SelfService");
            if (!privileges.AllowView)
            {
                throw new AuthorizationException();
            }
            var userId = id==null ? SessionHelper.LoginId : int.Parse(Encryption.DecryptURLParm(id));
            var model = new DashboardViewModel();
            var userInfo = db.UserInformation.Where(w => w.Id == userId).FirstOrDefault();
            model.UserName = userInfo.ShortFullName;
            model.EmployeeId = userInfo.EmployeeId;
            model.UserId = userId;
            model.UserProfilePicture = SessionHelper.UserProfilePicture;
            try
            {
                ViewBag.PunchClocktPunchDate = DataHelper.GetToday(userInfo.EmployeeId);
                var activityList  = DataHelper.PunchData<tPunchData>(userInfo.EmployeeId);
                //if(activityList==null || activityList.Count==0)
                //{
                //    activityList = new List<tPunchData>();
                //    activityList.Add(new tPunchData() {DTPunchDate=new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day, 10,0,0) });
                //    activityList.Add(new tPunchData() { DTPunchDate = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day, 11, 0, 0) });
                //    activityList.Add(new tPunchData() { DTPunchDate = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day, 11, 15, 0) });
                //    activityList.Add(new tPunchData() { DTPunchDate = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day, 13, 30, 0) });
                //    activityList.Add(new tPunchData() { DTPunchDate = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day, 14, 0, 0) });
                //    activityList.Add(new tPunchData() { DTPunchDate = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day, 17, 30, 0) });
                //}
                ViewBag.PunchClockPunchData = activityList;
            }
            catch (Exception ex)
            { 
            }



            //if (id != null)
            //{
            //    model.UserId = id;
            //    return PartialView("MyDashboardPopup", model);
            //}
            return PartialView(model);
        }

    }
}