using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Text;
using System.Web;
using System.Web.Mvc;
using TimeAide.Common.Helpers;
using TimeAide.Data;
using TimeAide.Services;
using TimeAide.Web.Models;
using TimeAide.Web.ViewModel;

namespace TimeAide.Web.Controllers
{
    public class WebPunchConfigurationController : TimeAideWebControllers<WebPunchConfiguration>
    {
        [HttpPost]
        //[ValidateAntiForgeryToken]
        public ActionResult Create(WebPunchConfiguration model)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    WebPunchSessionTokenService.wsGetSessionToken token = new WebPunchSessionTokenService.wsGetSessionToken(model.PunchServiceUrl + "wsGetSessionToken.asmx");
                    WebPunchSessionTokenService.clsGetSessionTokenResult result = token.GetSessionToken(model.APIKey, model.PunchServiceCompanyPassword, model.PunchServiceCompanyId.ToString());
                    if (result.intGetSessionTokenResult < 1)
                    {
                        ModelState.AddModelError("PunchServiceUrl", result.strGetSessionTokenResult.ToString());
                    }
                }
                catch (Exception ex)
                {
                    ModelState.AddModelError("PunchServiceUrl", "Configurations can not be validated");
                }
                if (ModelState.IsValid)
                {
                    db.WebPunchConfiguration.Add(model);
                    db.SaveChanges();
                    return Json(model);
                }
            }

            return GetErrors();
        }

        [HttpPost]
        public ActionResult Edit(WebPunchConfiguration model)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    WebPunchSessionTokenService.wsGetSessionToken token = new WebPunchSessionTokenService.wsGetSessionToken(model.PunchServiceUrl + "wsGetSessionToken.asmx");
                    WebPunchSessionTokenService.clsGetSessionTokenResult result = token.GetSessionToken(model.APIKey, model.PunchServiceCompanyPassword, model.PunchServiceCompanyId.ToString());
                    if (result.intGetSessionTokenResult < 1)
                    {
                        ModelState.AddModelError("PunchServiceUrl", result.strGetSessionTokenResult.ToString());
                    }
                }
                catch (Exception ex)
                {
                    ModelState.AddModelError("PunchServiceUrl", "Configurations can not be validated");
                }
                if (ModelState.IsValid)
                {
                    model.SetUpdated<WebPunchConfiguration>();
                    db.Entry(model).State = EntityState.Modified;
                    db.SaveChanges();
                    return RedirectToAction("Index");
                }
            }
            return GetErrors();
        }


        [HttpPost]
        public ActionResult WebPunch(GeoLocationViewModel model)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    var configuration = db.GetAllByCompany<WebPunchConfiguration>(SessionHelper.SelectedCompanyId, SessionHelper.SelectedClientId).FirstOrDefault();
                    if (configuration == null)
                    {
                        return Json(new { status = "Error", message = "Web Punch is not Configured, please contact system admin." });
                    }
                    WebPunchLoginService.wsLogin loginService = new WebPunchLoginService.wsLogin(configuration.PunchServiceUrl + "wsLogin.asmx");
                    WebPunchLoginService.clsLoginResult loginServiceResult = loginService.Login(configuration.APIKey, configuration.PunchServiceCompanyPassword);
                    if (loginServiceResult.intLoginResult < 1)
                    {
                        return Json(new { status = "Error", message = "There is an error while generating session token, please contact system admin." });
                    }
                    string validateUrl = configuration.PunchServiceUrl + "wsValidateEvent.asmx";
                    WebPunchValidateEventService.wsValidateEvent validationService = new WebPunchValidateEventService.wsValidateEvent(validateUrl);
                    WebPunchValidateEventService.clsValidateEventResult validationServiceResult;
                    string eventStartTime = DateTime.Now.ToString("MM/dd/yyyy");
                    string eventEndTime = DateTime.Now.ToString("HH:mm:ss");
                    if (configuration.UseGPSCoordinates)
                        validationServiceResult = validationService.ValidateEvent_GPS(loginServiceResult.strSessionToken, SessionHelper.LoginEmployeeId.ToString(), eventStartTime, eventEndTime, "1", "0", model.Latitude.ToString(), model.Longitude.ToString(), "0");
                    else
                        validationServiceResult = validationService.ValidateEvent(loginServiceResult.strSessionToken, SessionHelper.LoginEmployeeId.ToString(), eventStartTime, eventEndTime, "1", "0");
                    if (validationServiceResult.intValidateEventResult != 1)
                        return Json(new { status = "Error", message = "There is an error while punch. " + validationServiceResult.strValidateEventResult.ToString() });
                    eventEndTime = DateTime.Now.ToString("MM/dd/yyyy");
                    WebPunchComputeService2.wsComputeTimeSheet computeService = new WebPunchComputeService2.wsComputeTimeSheet(configuration.PunchServiceUrl + "wsComputeTimeSheet.asmx");
                    WebPunchComputeService2.clsComputeEventResult computeEventResult = computeService.ComputeEvent(loginServiceResult.strSessionToken, SessionHelper.LoginEmployeeId.ToString(), eventStartTime, eventEndTime);

                    //WebGetPunchesService.wsGetPunches getPunchesService = new WebGetPunchesService.wsGetPunches(configuration.PunchServiceUrl + "wsGetPunches.asmx");
                    //WebGetPunchesService.clsGetPunchesResult punchesServiceResult = getPunchesService.GetPunchesList(loginServiceResult.strSessionToken, SessionHelper.LoginEmployeeId.ToString(), eventStartTime);
                    //List<clsPunchDataInfoItem> punchList = punchesServiceResult.LstPunchDataList.ToList();
                    return Json(new { status = "Success", message = "Success" });
                }
                catch (Exception ex)
                {
                    //ModelState.AddModelError("PunchServiceUrl", "Configurations can not be validated");
                    StringBuilder errorMessage = new StringBuilder();

                    while (ex != null)
                    {
                        errorMessage.Append(ex.Message);
                        ex = ex.InnerException;
                    }
                    return Json("Error", errorMessage.ToString());
                }
            }
            return GetErrors();
        }


    
        public ActionResult GetWebPunchSummary(int? id)
        {
            WebPunchViewModel model = new WebPunchViewModel();
            var userInfo = UserInformationService.GetUserInformation(id ?? SessionHelper.LoginId);
            model.tPunchDate = DataHelper.GetToday(userInfo.EmployeeId);
            var activityList = DataHelper.PunchData<tPunchData>(userInfo.EmployeeId);
            //if (activityList == null || activityList.Count == 0)
            //{
            //    activityList = new List<tPunchData>();
            //    activityList.Add(new tPunchData() { DTPunchDate = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day, 10, 0, 0) });
            //    activityList.Add(new tPunchData() { DTPunchDate = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day, 11, 0, 0) });
            //    activityList.Add(new tPunchData() { DTPunchDate = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day, 11, 15, 0) });
            //    activityList.Add(new tPunchData() { DTPunchDate = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day, 13, 30, 0) });
            //    activityList.Add(new tPunchData() { DTPunchDate = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day, 14, 0, 0) });
            //    activityList.Add(new tPunchData() { DTPunchDate = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day, 17, 30, 0) });
            //}
            model.tPunchData = activityList;

            return PartialView("WebPunchSummaryView", model);
        }

        public override bool CheckBeforeDelete(int id)
        {
            return true;
        }
        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
