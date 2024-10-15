using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Data.Entity.Validation;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using TimeAide.Common.Helpers;
using TimeAide.Models.ViewModel;
using TimeAide.Services;
using TimeAide.Services.Helpers;
using TimeAide.Web.Extensions;
using TimeAide.Web.Models;
using TimeAide.Web.ViewModel;

namespace TimeAide.Web.Controllers
{
    public class UserContactInformationController : TimeAideWebBaseControllers
    {
        string FormName
        {
            get;
            set;
        }
        private TimeAideContext db = new TimeAideContext();
        RoleFormPrivilegeViewModel1 privileges;
        public UserContactInformationController()
        {
            FormName = typeof(UserContactInformation).Name;
            var form = db.Form.FirstOrDefault(p => p.FormName == FormName);
            if (SecurityHelper.IsSuperAdmin || SecurityHelper.IsAdmin)
                privileges = new RoleFormPrivilegeViewModel1() { Form = form, FormId = form.Id, RoleId = 0, AllowAdd = true, AllowDelete = true, AllowEdit = true, AllowView = true, AllowChangeHistory = true };
            else
            {
                var userRole = db.UserInformationRole.FirstOrDefault(p => p.UserInformationId == SessionHelper.LoginId);
                if (userRole == null)
                    privileges = new RoleFormPrivilegeViewModel1() { Form = form, FormId = form.Id, RoleId = 0, AllowAdd = false, AllowDelete = false, AllowEdit = false, AllowView = false, AllowChangeHistory = true };
                else
                {
                    var roleFormPrivilege = db.RoleFormPrivilege.Where(p => p.RoleId == userRole.RoleId && p.Form.FormName == form.FormName).ToList();
                    privileges = (new RoleFormPrivilegeService()).GetView(roleFormPrivilege, form, userRole.RoleId);
                }
            }
            ViewBag.AllowEdit = privileges.AllowEdit;
            ViewBag.AllowAdd = privileges.AllowAdd;
            ViewBag.AllowView = privileges.AllowView;
            ViewBag.AllowDelete = privileges.AllowDelete;
            ViewBag.AllowChangeHistory = privileges.AllowChangeHistory;
            ViewBag.FormName = FormName;
            ViewBag.Title = UtilityHelper.Pluralize(FormName);
            ViewBag.Label = form.Label;
            ViewBag.LabelPlural = form.LabelPlural;
        }


        // GET: UserContactInformation
        public ActionResult Index()
        {
            //  var userContactInformation = db.UserContactInformation.Include(u => u.City).Include(u => u.City1).Include(u => u.State).Include(u => u.State1);
            var userContactInformation = db.UserContactInformation;
            ViewBag.Label = ViewBag.Label + " - Detail";
            return View(userContactInformation.ToList());
        }

        // GET: UserContactInformation/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            UserContactInformation userContactInformation = db.UserContactInformation.FirstOrDefault(u => u.UserInformationId == id);
            if (userContactInformation == null)
            {
                userContactInformation = new UserContactInformation() { UserInformationId = id ?? 0 };
            }
            return PartialView(userContactInformation);
        }

        // GET: UserContactInformation/Create
        public ActionResult Create(int? id)
        {
            UserContactInformation userContactInformation = null;

            userContactInformation = db.UserContactInformation.FirstOrDefault(u => u.UserInformationId == id);

            if (userContactInformation == null)
            {
                userContactInformation = new UserContactInformation();
                ViewBag.Label = ViewBag.Label + " - Add";

                ViewBag.HomeCityId = new SelectList(db.City.Take(0), "Id", "CityName");
                ViewBag.MailingCityId = new SelectList(db.City.Take(0), "Id", "CityName");
                ViewBag.MailingStateId = new SelectList(db.State.Take(0), "Id", "StateName");
                ViewBag.HomeStateId = new SelectList(db.State.Take(0), "Id", "StateName");
                ViewBag.MailingCountryId = new SelectList(db.Country.Where(u => u.DataEntryStatus == 1), "Id", "CountryName");
                ViewBag.HomeCountryId = new SelectList(db.Country.Where(u => u.DataEntryStatus == 1), "Id", "CountryName");
            }
            else
            {
                ViewBag.Label = ViewBag.Label + " - Edit";

                ViewBag.HomeCityId = new SelectList(db.City.Where(u => u.DataEntryStatus == 1 && u.StateId == userContactInformation.HomeStateId), "Id", "CityName", userContactInformation.HomeCityId);
                ViewBag.MailingCityId = new SelectList(db.City.Where(u => u.DataEntryStatus == 1 && u.StateId == userContactInformation.MailingStateId), "Id", "CityName", userContactInformation.MailingCityId);
                ViewBag.MailingStateId = new SelectList(db.State.Where(u => u.DataEntryStatus == 1 && u.CountryId == userContactInformation.HomeCountryId), "Id", "StateName", userContactInformation.HomeStateId);
                ViewBag.HomeStateId = new SelectList(db.State.Where(u => u.DataEntryStatus == 1 && u.CountryId == userContactInformation.MailingCountryId), "Id", "StateName", userContactInformation.MailingStateId);
                ViewBag.MailingCountryId = new SelectList(db.Country.Where(u => u.DataEntryStatus == 1), "Id", "CountryName", userContactInformation.HomeCountryId);
                ViewBag.HomeCountryId = new SelectList(db.Country.Where(u => u.DataEntryStatus == 1), "Id", "CountryName", userContactInformation.MailingCountryId);

            }
            userContactInformation.UserInformationId = id ?? 0;

            return PartialView(userContactInformation);
        }
        public ActionResult CreateHomeAddress(int? id)
        {
            UserContactInformation userContactInformation = null;

            userContactInformation = db.UserContactInformation.FirstOrDefault(u => u.UserInformationId == id);

            if (userContactInformation == null)
            {
                userContactInformation = new UserContactInformation();
                ViewBag.Label = ViewBag.Label + " - Add Home Address";
                ViewBag.HomeCityId = new SelectList(db.City.Take(0), "Id", "CityName");
                ViewBag.HomeStateId = new SelectList(db.State.Take(0), "Id", "StateName");
                ViewBag.HomeCountryId = new SelectList(db.Country.Where(u => u.DataEntryStatus == 1), "Id", "CountryName");
            }
            else
            {
                ViewBag.Label = ViewBag.Label + " - Edit Home Address";

                ViewBag.HomeCityId = new SelectList(db.City.Where(u => u.DataEntryStatus == 1 && u.StateId == userContactInformation.HomeStateId), "Id", "CityName", userContactInformation.HomeCityId);
                ViewBag.HomeStateId = new SelectList(db.State.Where(u => u.DataEntryStatus == 1 && u.CountryId == userContactInformation.HomeCountryId), "Id", "StateName", userContactInformation.HomeStateId);
                ViewBag.HomeCountryId = new SelectList(db.Country.Where(u => u.DataEntryStatus == 1), "Id", "CountryName", userContactInformation.HomeCountryId);


            }
            userContactInformation.UserInformationId = id ?? 0;

            return PartialView(userContactInformation);
        }
        public ActionResult CreateMailingAddress(int? id)
        {
            UserContactInformation userContactInformation = null;

            userContactInformation = db.UserContactInformation.FirstOrDefault(u => u.UserInformationId == id);

            if (userContactInformation == null)
            {
                userContactInformation = new UserContactInformation();
                ViewBag.Label = ViewBag.Label + " - Add Mailing Address";
                ViewBag.MailingCityId = new SelectList(db.City.Take(0), "Id", "CityName");
                ViewBag.MailingStateId = new SelectList(db.State.Take(0), "Id", "StateName");
                ViewBag.MailingCountryId = new SelectList(db.Country.Where(u => u.DataEntryStatus == 1), "Id", "CountryName");

            }
            else
            {
                ViewBag.Label = ViewBag.Label + " - Edit Mailing Address";
                ViewBag.MailingCityId = new SelectList(db.City.Where(u => u.DataEntryStatus == 1 && u.StateId == userContactInformation.MailingStateId), "Id", "CityName", userContactInformation.MailingCityId);
                ViewBag.MailingStateId = new SelectList(db.State.Where(u => u.DataEntryStatus == 1 && u.CountryId == userContactInformation.MailingCountryId), "Id", "StateName", userContactInformation.MailingStateId);
                ViewBag.MailingCountryId = new SelectList(db.Country.Where(u => u.DataEntryStatus == 1), "Id", "CountryName", userContactInformation.MailingCountryId);


            }
            userContactInformation.UserInformationId = id ?? 0;

            return PartialView(userContactInformation);
        }
        [HttpPost]
        public ActionResult CreateHomeAddress(UserContactInformation userContactInformation)
        {
            if (ModelState.IsValid)
            {
                if (!String.IsNullOrEmpty(userContactInformation.HomeZipCode) && userContactInformation.HomeZipCode.Length < 9)
                {
                    if (userContactInformation.HomeZipCode.Length != 5)
                        ModelState.AddModelError("HomeZipCode", "Invalid format.");
                }

            }
            if (ModelState.IsValid)
            {
                if (userContactInformation.Id == 0)
                {
                    db.UserContactInformation.Add(userContactInformation);
                }
                else
                {
                    var dbObject = db.UserContactInformation.FirstOrDefault(u => u.Id == userContactInformation.Id);
                    dbObject.HomeAddress1 = userContactInformation.HomeAddress1;
                    dbObject.HomeAddress2 = userContactInformation.HomeAddress2;
                    dbObject.HomeCityId = userContactInformation.HomeCityId;
                    dbObject.HomeCountryId = userContactInformation.HomeCountryId;
                    dbObject.HomeStateId = userContactInformation.HomeStateId;
                    dbObject.HomeZipCode = userContactInformation.HomeZipCode;
                    //dbObject.IsSameHomeAddress = userContactInformation.IsSameHomeAddress;

                    //dbObject.MailingAddress1 = userContactInformation.MailingAddress1;
                    //dbObject.MailingAddress2 = userContactInformation.MailingAddress2;
                    //dbObject.MailingCityId = userContactInformation.MailingCityId;
                    //dbObject.MailingCountryId = userContactInformation.MailingCountryId;
                    //dbObject.MailingStateId = userContactInformation.MailingStateId;
                    //dbObject.MailingZipCode = userContactInformation.MailingZipCode;

                    dbObject.ModifiedBy = SessionHelper.LoginId;
                    dbObject.ModifiedDate = DateTime.Now;
                    //db.Entry(userContactInformation).State = EntityState.Modified;
                }
                try
                {
                    db.SaveChanges();
                }
                catch (DbEntityValidationException ex)
                {
                    var validationError = ex.EntityValidationErrors.FirstOrDefault().ValidationErrors.FirstOrDefault().ErrorMessage;
                    Helpers.ErrorLogHelper.InsertLog(Helpers.ErrorLogType.Error, ex, this.ControllerContext);
                    Dictionary<String, String> errors = new Dictionary<string, string>();
                    errors.Add("PopupMessage1", validationError);
                    return GetErrors(errors);

                }
                catch (Exception ex)
                {
                    Helpers.ErrorLogHelper.InsertLog(Helpers.ErrorLogType.Error, ex, this.ControllerContext);
                    Dictionary<String, String> errors = new Dictionary<string, string>();
                    errors.Add("PopupMessage1", ex.Message);
                    return GetErrors(errors);
                }
                return Json(userContactInformation);
            }
            return GetErrors();
        }

        [HttpPost]
        public ActionResult CreateMailingAddress(UserContactInformation userContactInformation)
        {
            if (ModelState.IsValid)
            {
                if (!String.IsNullOrEmpty(userContactInformation.MailingZipCode) && userContactInformation.MailingZipCode.Length < 9)
                {
                    if (userContactInformation.MailingZipCode.Length != 5)
                        ModelState.AddModelError("MailingZipCode", "Invalid format.");
                }

            }
            if (ModelState.IsValid)
            {
                if (userContactInformation.Id == 0)
                {
                    db.UserContactInformation.Add(userContactInformation);
                }
                else
                {
                    var dbObject = db.UserContactInformation.FirstOrDefault(u => u.Id == userContactInformation.Id);

                    dbObject.MailingAddress1 = userContactInformation.MailingAddress1;
                    dbObject.MailingAddress2 = userContactInformation.MailingAddress2;
                    dbObject.MailingCityId = userContactInformation.MailingCityId;
                    dbObject.MailingCountryId = userContactInformation.MailingCountryId;
                    dbObject.MailingStateId = userContactInformation.MailingStateId;
                    dbObject.MailingZipCode = userContactInformation.MailingZipCode;
                    dbObject.IsSameHomeAddress = userContactInformation.IsSameHomeAddress;

                    dbObject.ModifiedBy = SessionHelper.LoginId;
                    dbObject.ModifiedDate = DateTime.Now;
                    //db.Entry(userContactInformation).State = EntityState.Modified;
                }
                try
                {
                    db.SaveChanges();
                }
                catch (DbEntityValidationException ex)
                {
                    var validationError = ex.EntityValidationErrors.FirstOrDefault().ValidationErrors.FirstOrDefault().ErrorMessage;
                    Helpers.ErrorLogHelper.InsertLog(Helpers.ErrorLogType.Error, ex, this.ControllerContext);
                    Dictionary<String, String> errors = new Dictionary<string, string>();
                    errors.Add("PopupMessage1", validationError);
                    return GetErrors(errors);

                }
                catch (Exception ex)
                {
                    Helpers.ErrorLogHelper.InsertLog(Helpers.ErrorLogType.Error, ex, this.ControllerContext);
                    Dictionary<String, String> errors = new Dictionary<string, string>();
                    errors.Add("PopupMessage1", ex.Message);
                    return GetErrors(errors);
                }
                return Json(userContactInformation);
            }
            return GetErrors();
        }

        public virtual ActionResult ChangeHistory(int refrenceId)
        {
            try
            {
                AllowView();
                var userContactInfo = db.UserContactInformation.FirstOrDefault(u => u.UserInformationId == refrenceId);

                var entitySet = db.AuditLogDetail.Where(d => d.AuditLog.ReferenceId == userContactInfo.Id && d.AuditLog.TableName == FormName && d.ClientId == SessionHelper.SelectedClientId).ToList();
               
                var refrenceObject = db.Database.SqlQuery<BaseEntity>("Select * from " + FormName + " where UserInformationId = " + userContactInfo.UserInformationId.ToString() + "").FirstOrDefault();
                if (refrenceObject != null)
                {
                    ViewBag.ReferenceObject = refrenceObject;
                    ViewBag.RefrenceId = userContactInfo.Id;
                    ViewBag.TableName = FormName;
                    ViewBag.UserInformation = db.UserInformation.FirstOrDefault(u => u.Id == refrenceObject.CreatedBy);
                }
                return PartialView("~/Views/AuditLog/ChangeHistory.cshtml", entitySet);
            }
            catch (AuthorizationException ex)
            {
                Exception exception = new Exception(ex.ErrorMessage);
                HandleErrorInfo handleErrorInfo = new HandleErrorInfo(exception, "UserContactInformation", "Index");
                return View("~/Views/Shared/Unauthorized.cshtml", handleErrorInfo);
            }
        }
        public ActionResult CreatePhoneNumbers(int? id)
        {
            UserContactInformation userContactInformation = null;

            userContactInformation = db.UserContactInformation.FirstOrDefault(u => u.UserInformationId == id);

            if (userContactInformation == null)
            {
                userContactInformation = new UserContactInformation();
                ViewBag.Label = ViewBag.Label + " - Add Phone(s)";
            }
            else
            {
                ViewBag.Label = ViewBag.Label + " - Edit Phone(s)";
            }
            userContactInformation.UserInformationId = id ?? 0;

            return PartialView(userContactInformation);
        }

        public ActionResult CreateEmails(int? id)
        {
            UserContactInformation userContactInformation = null;

            userContactInformation = db.UserContactInformation.FirstOrDefault(u => u.UserInformationId == id);

            if (userContactInformation == null)
            {
                userContactInformation = new UserContactInformation();
                ViewBag.Label = ViewBag.Label + " - Add Email(s)";
            }
            else
            {
                ViewBag.Label = ViewBag.Label + " - Edit Email(s)";
            }
            userContactInformation.UserInformationId = id ?? 0;

            return PartialView(userContactInformation);
        }

        [HttpPost]
        public ActionResult CreatePhoneNumbers(UserContactInformation userContactInformation)
        {
            if (ModelState.IsValid)
            {
                if (!String.IsNullOrEmpty(userContactInformation.CelNumber) && userContactInformation.CelNumber.Length < 10)
                {
                    ModelState.AddModelError("CelNumber", "Invalid format.");
                }
                if (!String.IsNullOrEmpty(userContactInformation.HomeNumber) && userContactInformation.HomeNumber.Length < 10)
                {
                    ModelState.AddModelError("HomeNumber", "Invalid format.");
                }
                if (!String.IsNullOrEmpty(userContactInformation.OtherNumber) && userContactInformation.OtherNumber.Length < 10)
                {
                    ModelState.AddModelError("OtherNumber", "Invalid format.");
                }
                if (!String.IsNullOrEmpty(userContactInformation.WorkNumber) && userContactInformation.WorkNumber.Length < 10)
                {
                    ModelState.AddModelError("WorkNumber", "Invalid format.");
                }
                if (!String.IsNullOrEmpty(userContactInformation.FaxNumber) && userContactInformation.FaxNumber.Length < 10)
                {
                    ModelState.AddModelError("FaxNumber", "Invalid format.");
                }
            }
            if (ModelState.IsValid)
            {
                if (String.IsNullOrEmpty(userContactInformation.HomeNumber) &&
                    String.IsNullOrEmpty(userContactInformation.CelNumber) &&
                    String.IsNullOrEmpty(userContactInformation.WorkNumber) &&
                    String.IsNullOrEmpty(userContactInformation.OtherNumber)
                    )

                {
                    ModelState.AddModelError("MandatoryContact", "Atleast one contact number is mandatory.");
                }
            }
            if (ModelState.IsValid)
            {
                if (userContactInformation.Id == 0)
                {
                    db.UserContactInformation.Add(userContactInformation);
                }
                else
                {
                    var dbObject = db.UserContactInformation.FirstOrDefault(u => u.Id == userContactInformation.Id);

                    dbObject.HomeNumber = userContactInformation.HomeNumber;
                    dbObject.CelNumber = userContactInformation.CelNumber;
                    dbObject.FaxNumber = userContactInformation.FaxNumber;
                    dbObject.WorkNumber = userContactInformation.WorkNumber;
                    dbObject.WorkExtension = userContactInformation.WorkExtension;
                    dbObject.OtherNumber = userContactInformation.OtherNumber;

                    dbObject.ModifiedBy = SessionHelper.LoginId;
                    dbObject.ModifiedDate = DateTime.Now;

                }
                try
                {
                    db.SaveChanges();
                }
                catch (DbEntityValidationException ex)
                {
                    var error = ex.EntityValidationErrors.FirstOrDefault().ValidationErrors.FirstOrDefault().ErrorMessage;
                    Helpers.ErrorLogHelper.InsertLog(Helpers.ErrorLogType.Error, error, this.ControllerContext);
                    ModelState.AddModelError("PopupMessage1", ex.Message);
                    return GetErrors();
                }
                catch (Exception ex)
                {
                    Helpers.ErrorLogHelper.InsertLog(Helpers.ErrorLogType.Error, ex, this.ControllerContext);
                    ModelState.AddModelError("PopupMessage1", ex.Message);
                    return GetErrors();
                }
                return Json(userContactInformation);
            }
            return GetErrors();
        }

        [HttpPost]
        public ActionResult CreateEmails(UserContactInformation userContactInformation)
        {
            UserContactInformation dbObject = null;
            if (ModelState.IsValid && userContactInformation.Id > 0)
            {
                if (
                    String.IsNullOrEmpty(userContactInformation.WorkEmail) &&
                    String.IsNullOrEmpty(userContactInformation.PersonalEmail) &&
                    String.IsNullOrEmpty(userContactInformation.OtherEmail)
                   )
                {
                    dbObject = db.UserContactInformation.FirstOrDefault(u => u.Id == userContactInformation.Id);
                    if (!String.IsNullOrEmpty(dbObject.LoginEmail))
                    {
                        var user = UserManagmentService.FindByEmail(dbObject.LoginEmail);
                        if (user == null)
                            dbObject.LoginEmail = string.Empty;
                        else
                            ModelState.AddModelError("MandatoryContact", "Email is associated with login, please remove login email");
                    }
                    else
                    {
                        var userInformation = db.UserInformation.FirstOrDefault(u => u.Id == userContactInformation.UserInformationId);
                        if (userInformation != null)
                        {
                            var user = UserManagmentService.FindByEmployee(userInformation);
                            if (user != null)
                                UserManagmentService.DeleteUser(user.Email);
                        }
                    }
                }
            }
            if (ModelState.IsValid)
            {
                if (userContactInformation.Id == 0)
                {
                    db.UserContactInformation.Add(userContactInformation);
                }
                else
                {
                    if (dbObject == null)
                        dbObject = db.UserContactInformation.FirstOrDefault(u => u.Id == userContactInformation.Id);
                    dbObject.PersonalEmail = userContactInformation.PersonalEmail;
                    dbObject.WorkEmail = userContactInformation.WorkEmail;
                    dbObject.OtherEmail = userContactInformation.OtherEmail;
                    dbObject.ModifiedBy = SessionHelper.LoginId;
                    dbObject.ModifiedDate = DateTime.Now;
                    dbObject.SetUpdated<UserContactInformation>();

                }
                db.SaveChanges();
                return Json(userContactInformation);
            }
            return GetErrors();
        }

        // POST: UserContactInformation/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        public ActionResult Create(UserContactInformation userContactInformation)
        {
            if (ModelState.IsValid)
            {
                if (userContactInformation.Id == 0)
                {
                    db.UserContactInformation.Add(userContactInformation);
                }
                else
                {
                    userContactInformation.ModifiedBy = SessionHelper.LoginId;
                    userContactInformation.ModifiedDate = DateTime.Now;
                    db.Entry(userContactInformation).State = EntityState.Modified;
                }
                db.SaveChanges();
                db.Entry(userContactInformation).Reference(s => s.HomeCity).Load();
                db.Entry(userContactInformation).Reference(s => s.HomeState).Load();
                db.Entry(userContactInformation).Reference(s => s.HomeCountry).Load();

                db.Entry(userContactInformation).Reference(s => s.MailingCity).Load();
                db.Entry(userContactInformation).Reference(s => s.MailingState).Load();
                db.Entry(userContactInformation).Reference(s => s.MailingCountry).Load();

                return PartialView("Details", userContactInformation);
            }

            ViewBag.HomeCityId = new SelectList(db.City.Where(u => u.DataEntryStatus == 1), "Id", "CityName");
            ViewBag.MailingCityId = new SelectList(db.City.Where(u => u.DataEntryStatus == 1), "Id", "CityName");
            ViewBag.MailingStateId = new SelectList(db.State.Where(u => u.DataEntryStatus == 1), "Id", "StateName");
            ViewBag.HomeStateId = new SelectList(db.State.Where(u => u.DataEntryStatus == 1), "Id", "StateName");
            ViewBag.UserID = new SelectList(db.UserInformation.Where(u => u.DataEntryStatus == 1), "Id", "IdNumber");
            return View(userContactInformation);
        }



        // GET: UserContactInformation/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            UserContactInformation userContactInformation = db.UserContactInformation.Where(u => u.Id == id.Value).FirstOrDefault();
            if (userContactInformation == null)
            {
                userContactInformation = new UserContactInformation();
                //return HttpNotFound();
            }
            ViewBag.HomeCityId = new SelectList(db.City.Where(u => u.DataEntryStatus == 1), "Id", "CityName");
            ViewBag.MailingCityId = new SelectList(db.City.Where(u => u.DataEntryStatus == 1), "Id", "CityName");
            ViewBag.MailingStateId = new SelectList(db.State.Where(u => u.DataEntryStatus == 1), "Id", "StateName");
            ViewBag.HomeStateId = new SelectList(db.State.Where(u => u.DataEntryStatus == 1), "Id", "StateName");
            ViewBag.UserInformationId = new SelectList(db.UserInformation.Where(u => u.DataEntryStatus == 1), "Id", "IdNumber");
            return View(userContactInformation);
        }

        // POST: UserContactInformation/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "UserContactInformationId,UserID,HomeAddress1,HomeAddress2,HomeCityId,HomeStateId,HomeZipCode,MailingAddress1,MailingAddress2,MailingCityId,MailingStateId,MailingZipCode,HomeNumber,CelNumber,FaxNumber,OtherNumber,WorkEmail,PersonalEmail,OtherEmail,EmergencyContact,EmergencyRelationship,EmergencyNumber1,EmergencyNumber2,WorkNumber,WorkExtension,CreatedBy,CreatedDate,DataEntryStatus,ModifiedBy,ModifiedDate,IsSameHomeAddress")] UserContactInformation userContactInformation)
        {
            if (ModelState.IsValid)
            {
                if (userContactInformation.Id == 0)
                {
                    db.UserContactInformation.Add(userContactInformation);
                }
                else
                {
                    userContactInformation.ModifiedBy = SessionHelper.LoginId;
                    userContactInformation.ModifiedDate = DateTime.Now;
                    db.Entry(userContactInformation).State = EntityState.Modified;
                }
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(userContactInformation);
        }

        // GET: UserContactInformation/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            UserContactInformation userContactInformation = db.UserContactInformation.Find(id);
            if (userContactInformation == null)
            {
                return HttpNotFound();
            }
            return View(userContactInformation);
        }

        // POST: UserContactInformation/Delete/5
        [HttpPost, ActionName("Delete")]
        public ActionResult DeleteConfirmed(int id)
        {
            UserContactInformation userContactInformation = db.UserContactInformation.Find(id);
            //db.UserContactInformation.Remove(userContactInformation);
            userContactInformation.ModifiedBy = SessionHelper.LoginId;
            userContactInformation.ModifiedDate = DateTime.Now;
            userContactInformation.DataEntryStatus = 0;
            db.SaveChanges();
            return RedirectToAction("Index");
        }
        public void AllowDelete()
        {
            if (!privileges.AllowDelete)
            {
                throw new AuthorizationException();
            }
        }
        public void AllowEdit()
        {
            if (!privileges.AllowEdit)
            {
                throw new AuthorizationException();
            }
        }
        public virtual void AllowAdd()
        {
            if (!privileges.AllowAdd)
            {
                throw new AuthorizationException();
            }
        }
        public virtual void AllowView()
        {
            if (!privileges.AllowView)
            {
                throw new AuthorizationException();
            }
        }
        public void AllowChangeHistory()
        {
            if (!privileges.AllowChangeHistory)
            {
                throw new AuthorizationException();
            }
        }
        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        [HttpPost]
        public ActionResult ChangeRequestAddresses(UserContactInformation model)
        {
            if (ModelState.IsValid)
            {
                if (model.AddressType== "Home" && !String.IsNullOrEmpty(model.HomeZipCode) && model.HomeZipCode.Length < 9)
                {
                    if (model.HomeZipCode.Length != 5)
                        ModelState.AddModelError("ActiveUserContactInformation.HomeZipCode", "Invalid format.");
                }
                if (model.AddressType == "Mailing" && !String.IsNullOrEmpty(model.MailingZipCode) && model.MailingZipCode.Length < 9)
                {
                    if (model.MailingZipCode.Length != 5)
                        ModelState.AddModelError("ActiveUserContactInformation.MailingZipCode", "Invalid format.");
                }
            }
            if (ModelState.IsValid)
            {
                try
                {
                    var dbObject = db.Find<UserContactInformation>(model.Id, SessionHelper.SelectedClientId);
                    if (dbObject.ChangeRequestAddress.Count > 0 && dbObject.ChangeRequestAddress.Any(r => r.ChangeRequestStatusId == 1 && r.AddressType == model.AddressType))
                    {
                        Dictionary<String, String> errors = new Dictionary<string, string>();
                        errors.Add("PopupMessage1", "There is already a pending change request for selcted emplyee.");
                        return GetErrors(errors);
                    }

                    ChangeRequestAddress changeRequest = new ChangeRequestAddress();
                    changeRequest = GetChanges(model, dbObject);
                    if (!changeRequest.IsModified)
                    {
                        Dictionary<String, String> errors = new Dictionary<string, string>();
                        errors.Add("PopupMessage1", "No changes to submit for approval.");
                        return GetErrors(errors);
                    }
                    db.ChangeRequestAddress.Add(changeRequest);

                    WorkflowTriggerRequestDetail workflowTriggerRequestDetail = WorkflowService.StratWorkflow(db, 6);
                    if (workflowTriggerRequestDetail.WorkflowLevel.Workflow.IsZeroLevel)
                    {
                        WorkflowService.ApplyChanges(db, changeRequest as ChangeRequestAddress);

                        db.SaveChanges();

                        WorkflowService.ProcesstWorkflowClosingNotification(db, workflowTriggerRequestDetail.WorkflowTriggerRequest, changeRequest);
                        //TimeAide.Services.Helpers.UtilityHelper.SendEmailByWorkflowClosingNotification(model.UserInformationId, changeRequest.Id, workflowTriggerRequestDetail.WorkflowLevel.Workflow, workflowTriggerRequestDetail, toEmails, db);
                    }
                    else
                    {
                        workflowTriggerRequestDetail.WorkflowTriggerRequest.ChangeRequestAddress = changeRequest;
                        db.SaveChanges();
                        try
                        {
                            TimeAide.Services.Helpers.UtilityHelper.SendEmailByWorkflow(model.UserInformationId, workflowTriggerRequestDetail, changeRequest.Id, db);
                        }
                        catch (Exception ex)
                        {
                            Helpers.ErrorLogHelper.InsertLog(Helpers.ErrorLogType.Error, ex, this.ControllerContext);
                        }
                    }

                    return Json(model);
                }
                catch (Exception ex)
                {
                    Helpers.ErrorLogHelper.InsertLog(Helpers.ErrorLogType.Error, ex, this.ControllerContext);
                }

            }
            return GetErrors();
        }

        private ChangeRequestAddress GetChanges(UserContactInformation model, UserContactInformation dbObject)
        {
            ChangeRequestAddress changeRequest = new ChangeRequestAddress();
            changeRequest.UserInformationId = model.UserInformationId;
            changeRequest.UserContactInformationId = dbObject.Id;
            changeRequest.ChangeRequestStatusId = 1;
            changeRequest.AddressType = dbObject.AddressType = model.AddressType;
            int changes = 0;

            if (dbObject.Address1 != model.Address1)
            {
                changes++;
                changeRequest.NewAddress1 = model.Address1;
                changeRequest.Address1 = dbObject.Address1;
            }
            if (dbObject.Address2 != model.Address2)
            {
                changes++;
                changeRequest.NewAddress2 = model.Address2;
                changeRequest.Address2 = dbObject.Address2;
            }
            if (dbObject.CityId != model.CityId)
            {
                changes++;
                changeRequest.NewCityId = model.CityId;
                changeRequest.CityId = dbObject.CityId;
            }
            if (dbObject.CountryId != model.CountryId)
            {
                changes++;
                changeRequest.NewCountryId = model.CountryId;
                changeRequest.CountryId = dbObject.CountryId;
            }
            if (dbObject.StateId != model.StateId)
            {
                changes++;
                changeRequest.NewStateId = model.StateId;
                changeRequest.StateId = dbObject.StateId;
            }
            if (dbObject.ZipCode != model.ZipCode)
            {
                changes++;
                changeRequest.NewZipCode = model.ZipCode;
                changeRequest.ZipCode = dbObject.ZipCode;
            }
            changeRequest.IsModified = changes > 0;
            return changeRequest;
        }

        [HttpPost]
        public ActionResult ChangeRequestEmailNumbers(UserContactInformation model)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    var dbObject = db.Find<UserContactInformation>(model.Id, SessionHelper.SelectedClientId);
                    if (dbObject.ChangeRequestEmailNumbers.Count > 0 && dbObject.ChangeRequestEmailNumbers.Any(r => r.ChangeRequestStatusId == 1))
                    {
                        Dictionary<String, String> errors = new Dictionary<string, string>();
                        errors.Add("PopupMessage1", "There is already a pending change request for selected employee.");
                        return GetErrors(errors);
                    }

                    ChangeRequestEmailNumbers changeRequest = new ChangeRequestEmailNumbers();
                    changeRequest = GetEmailAndNumberChanges(model, dbObject);
                    if(!changeRequest.IsModified)
                    {
                        Dictionary<String, String> errors = new Dictionary<string, string>();
                        errors.Add("PopupMessage1", "No changes to submit for approval.");
                        return GetErrors(errors);
                    }
                    db.ChangeRequestEmailNumbers.Add(changeRequest);

                    WorkflowTriggerRequestDetail workflowTriggerRequestDetail = WorkflowService.StratWorkflow(db, 3);
                    if (workflowTriggerRequestDetail.WorkflowLevel.Workflow.IsZeroLevel)
                    {
                        WorkflowService.ApplyChanges(db, changeRequest as ChangeRequestEmailNumbers);

                        db.SaveChanges();
                        //var userInformationList = db.SP_GetEmployeeSupervisors<UserInformationViewModel>(model.UserInformationId, SessionHelper.SelectedClientId);
                        //Dictionary<string, string> toEmails = userInformationList.ToDictionary(k => k.ShortFullName, v => v.LoginEmail);
                        //TimeAide.Services.Helpers.UtilityHelper.SendEmailByWorkflowClosingNotification(model.UserInformationId, changeRequest.Id, workflowTriggerRequestDetail.WorkflowLevel.Workflow, workflowTriggerRequestDetail, toEmails, db);
                        WorkflowService.ProcesstWorkflowClosingNotification(db, workflowTriggerRequestDetail.WorkflowTriggerRequest, changeRequest);
                    }
                    else
                    {
                        workflowTriggerRequestDetail.WorkflowTriggerRequest.ChangeRequestEmailNumbers = changeRequest;

                        db.SaveChanges();
                        try
                        {
                            TimeAide.Services.Helpers.UtilityHelper.SendEmailByWorkflow(model.UserInformationId, workflowTriggerRequestDetail, changeRequest.Id, db);
                        }
                        catch (Exception ex)
                        {
                            Helpers.ErrorLogHelper.InsertLog(Helpers.ErrorLogType.Error, ex, this.ControllerContext);
                        }
                    }

                    return Json(model);
                }
                catch (Exception ex)
                {
                    Helpers.ErrorLogHelper.InsertLog(Helpers.ErrorLogType.Error, ex, this.ControllerContext);
                }

            }
            return GetErrors();
        }

        private ChangeRequestEmailNumbers GetEmailAndNumberChanges(UserContactInformation model, UserContactInformation dbObject)
        {
            ChangeRequestEmailNumbers changeRequest = new ChangeRequestEmailNumbers();
            changeRequest.UserInformationId = model.UserInformationId;
            changeRequest.UserContactInformationId = dbObject.Id;
            changeRequest.ChangeRequestStatusId = 1;
            int changes = 0;
            if (dbObject.HomeNumber != model.HomeNumber)
            {
                changes++;
                changeRequest.NewHomeNumber = model.HomeNumber;
                changeRequest.HomeNumber = dbObject.HomeNumber;
            }
            if (dbObject.CelNumber != model.CelNumber)
            {
                changes++;
                changeRequest.NewCelNumber = model.CelNumber;
                changeRequest.CelNumber = dbObject.CelNumber;
            }
            if (dbObject.WorkNumber != model.WorkNumber)
            {
                changes++;
                changeRequest.NewWorkNumber = model.WorkNumber;
                changeRequest.WorkNumber = dbObject.WorkNumber;
            }
            if (dbObject.FaxNumber != model.FaxNumber)
            {
                changes++;
                changeRequest.NewFaxNumber = model.FaxNumber;
                changeRequest.FaxNumber = dbObject.FaxNumber;
            }
            if (dbObject.OtherNumber != model.OtherNumber)
            {
                changes++;
                changeRequest.NewOtherNumber = model.OtherNumber;
                changeRequest.OtherNumber = dbObject.OtherNumber;
            }
            if (dbObject.WorkEmail != model.WorkEmail)
            {
                changes++;
                changeRequest.NewWorkEmail = model.WorkEmail;
                changeRequest.WorkEmail = dbObject.WorkEmail;
            }
            if (dbObject.PersonalEmail != model.PersonalEmail)
            {
                changes++;
                changeRequest.NewPersonalEmail = model.PersonalEmail;
                changeRequest.PersonalEmail = dbObject.PersonalEmail;
            }
            if (dbObject.OtherEmail != model.OtherEmail)
            {
                changes++;
                changeRequest.NewOtherEmail = model.OtherEmail;
                changeRequest.OtherEmail = dbObject.OtherEmail;
            }
            if (dbObject.WorkExtension != model.WorkExtension)
            {
                changes++;
                changeRequest.NewWorkExtension = model.WorkExtension;
                changeRequest.WorkExtension = dbObject.WorkExtension;
            }

            changeRequest.IsModified = changes > 0;
            return changeRequest;
        }

        protected ActionResult GetErrors(Dictionary<String, String> errorList)
        {
            Response.StatusCode = (int)HttpStatusCode.BadRequest;

            Dictionary<String, String> errors = new Dictionary<string, string>();
            foreach (var eachState in errorList)
            {
                if (eachState.Value != null)
                {
                    errors.Add(eachState.Key, eachState.Value);
                }
            }
            var entries = string.Join(",", errors.Select(x => "{" + string.Format("\"Key\":\"{0}\",\"Message\":\"{1}\"", x.Key, x.Value) + "}"));
            var jsonResult = Json(new { success = false, errors = "[" + string.Join(",", entries) + "]" }, JsonRequestBehavior.AllowGet);
            jsonResult.MaxJsonLength = int.MaxValue;
            jsonResult.ContentType = "application/json";
            jsonResult.ContentEncoding = System.Text.Encoding.UTF8;   //charset=utf-8
            string json = JsonConvert.SerializeObject(jsonResult);
            return jsonResult;
        }

        protected ActionResult GetErrors()
        {
            Response.StatusCode = (int)HttpStatusCode.BadRequest;

            Dictionary<String, String> errors = new Dictionary<string, string>();
            foreach (var eachState in ModelState)
            {
                if (eachState.Value != null && eachState.Value.Errors != null && eachState.Value.Errors.Count > 0)
                {
                    errors.Add(eachState.Key, eachState.Value.Errors[0].ErrorMessage);
                }
            }
            var entries = string.Join(",", errors.Select(x => "{" + string.Format("\"Key\":\"{0}\",\"Message\":\"{1}\"", x.Key, x.Value) + "}"));
            var jsonResult = Json(new { success = false, errors = "[" + string.Join(",", entries) + "]" }, JsonRequestBehavior.AllowGet);
            jsonResult.MaxJsonLength = int.MaxValue;
            jsonResult.ContentType = "application/json";
            jsonResult.ContentEncoding = System.Text.Encoding.UTF8;   //charset=utf-8
            string json = JsonConvert.SerializeObject(jsonResult);
            return jsonResult;
        }



    }
}
