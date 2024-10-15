using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using TimeAide.Common.Helpers;
using TimeAide.Web.Models;

namespace TimeAide.Web.Controllers
{
    public class CompanyEmployeeTabController : TimeAideWebControllers<CompanyEmployeeTab>
    {

        // POST: EmployeeType/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        //[ValidateAntiForgeryToken]
        public ActionResult Create(CompanyEmployeeTab companyEmployeeTab)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    db.CompanyEmployeeTab.Add(companyEmployeeTab);
                    db.SaveChanges();
                    return Json(companyEmployeeTab);
                }
                catch(Exception ex)
                {
                    Helpers.ErrorLogHelper.InsertLog(Helpers.ErrorLogType.Error, ex, this.ControllerContext);
                }
            }

            return GetErrors();
        }


        // POST: EmployeeType/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        //[ValidateAntiForgeryToken]
        public ActionResult Edit(CompanyEmployeeTab companyEmployeeTab)
        {
            if (ModelState.IsValid)
            {
                companyEmployeeTab.SetUpdated<CompanyEmployeeTab>();
                db.Entry(companyEmployeeTab).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return GetErrors();
        }
        public override bool CheckBeforeDelete(int id)
        {
            return true;
        }

        public ActionResult CreateEdit(int? userId)
        {
            //var lidt = db.GetAllByCompany<UserInformation>(SessionHelper.SelectedCompanyId, SessionHelper.SelectedClientId);
            IEnumerable<SelectListItem> notificationList = null;
            //var Tabs = db.CompanyEmployeeTab.Where(u => u.TabUserId == userId && u.DataEntryStatus == 1).ToList();
            notificationList = db.GetAll<Form>(1).
                               Where(u => u.IsTab).
                               Select(s => new SelectListItem
                               {
                                   Text = s.Label,
                                   Value = s.Id.ToString()
                               });
            ViewBag.TabList = notificationList;
            string[] TempData = db.GetAll<CompanyEmployeeTab>(SessionHelper.SelectedClientId).Where(e => e.CompanyId == userId).Select(s => s.FormId.ToString()).ToArray<string>();
            ViewBag.SelectedTabList = TempData;
            ViewBag.EmployeeTabId = userId;
            return PartialView();
        }

        [HttpPost]
        public JsonResult CreateEdit(int id, string selectedCredentialIds)
        {
            string status = "Success";
            string message = "Successfully Added/Updated!";
            try
            {
                var selectedCredentialsList = selectedCredentialIds.Split(',').ToList();
                List<CompanyEmployeeTab> credentialAddList = new List<CompanyEmployeeTab>();
                List<CompanyEmployeeTab> credentialRemoveList = new List<CompanyEmployeeTab>();
                var existingCredentialList = db.CompanyEmployeeTab.Where(w => w.CompanyId == id).ToList();

                foreach (var credentialItem in existingCredentialList)
                {
                    var RecCnt = selectedCredentialsList.Where(w => w == credentialItem.FormId.ToString()).Count();
                    if (RecCnt == 0)
                    {
                        credentialRemoveList.Add(credentialItem);
                    }

                }
                foreach (var selectedCredentialId in selectedCredentialsList)
                {
                    if (selectedCredentialId == "") continue;
                    int credentialId = int.Parse(selectedCredentialId);
                    var recExists = existingCredentialList.Where(w => w.FormId == credentialId).Count();
                    if (recExists == 0)
                    {
                        credentialAddList.Add(new CompanyEmployeeTab() { FormId = credentialId, CompanyId = id });

                    }
                }

                db.CompanyEmployeeTab.RemoveRange(credentialRemoveList);
                db.CompanyEmployeeTab.AddRange(credentialAddList);

                db.SaveChanges();

            }
            catch (Exception ex)
            {
                Helpers.ErrorLogHelper.InsertLog(Helpers.ErrorLogType.Error, ex, this.ControllerContext);
                status = "Error";
                message = ex.Message;
            }

            return Json(new { status = status, message = message });
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
