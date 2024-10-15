using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.IO;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using TimeAide.Common.Helpers;
using TimeAide.Services;
using TimeAide.Web.Models;

namespace TimeAide.Web.Controllers
{
    public class EmployeeCustomFieldController : TimeAideWebControllers<EmployeeCustomField>
    {
        public override void OnCreate()
        {
            ViewBag.CustomFieldList = db.GetAllByCompany<CustomField>(SessionHelper.SelectedCompanyId, SessionHelper.SelectedClientId).ToList();
        }
        // POST: EmployeeDocument/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        //[ValidateAntiForgeryToken]
        public ActionResult Create(EmployeeCustomField employeeCustomField)
        {
            if (ModelState.IsValid)
            {
                if (!employeeCustomField.IssuanceDate.HasValue && employeeCustomField.ReturnDate.HasValue)
                {
                    ModelState.AddModelError("IssuanceDate", "Issuance Date is required when Return Date is provided.");
                }
                if (employeeCustomField.IssuanceDate.HasValue && employeeCustomField.ReturnDate.HasValue && employeeCustomField.IssuanceDate.Value > employeeCustomField.ReturnDate.Value)
                {
                    ModelState.AddModelError("IssuanceDate", "Issuance Date must be prior to Return Date.");
                }
            }
            if (ModelState.IsValid)
            {
                db.EmployeeCustomField.Add(employeeCustomField);
                db.SaveChanges();
                return RedirectToAction("IndexByUser", new { id = employeeCustomField.UserInformationId });
            }

            return GetErrors();
        }


        public override EmployeeCustomField OnEdit(EmployeeCustomField entity)
        {
            ViewBag.CustomFieldList = db.GetAllByCompany<CustomField>(SessionHelper.SelectedCompanyId, SessionHelper.SelectedClientId).ToList();
            return entity;
        }

        // POST: EmployeeDocument/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        //[ValidateAntiForgeryToken]
        public ActionResult Edit(EmployeeCustomField employeeCustomField)
        {
            if (ModelState.IsValid)
            {
                if (!employeeCustomField.IssuanceDate.HasValue && employeeCustomField.ReturnDate.HasValue)
                {
                    ModelState.AddModelError("IssuanceDate", "Issuance Date is required when Return Date is provided.");
                }
                if (employeeCustomField.IssuanceDate.HasValue && employeeCustomField.ReturnDate.HasValue && employeeCustomField.IssuanceDate.Value > employeeCustomField.ReturnDate.Value)
                {
                    ModelState.AddModelError("IssuanceDate", "Issuance Date must be prior to Return Date.");
                }
            }

            if (ModelState.IsValid)
            {
                db.Entry(employeeCustomField).State = EntityState.Modified;
                var employeeCustomFieldDb = db.EmployeeCustomField.FirstOrDefault(ec => ec.Id == employeeCustomField.Id);
                if (employeeCustomFieldDb.ExpirationDate != employeeCustomField.ExpirationDate)
                    db.NotificationLog.Where(l => l.EmployeeCustomFieldId == employeeCustomField.Id).ToList().ForEach(c => c.DataEntryStatus = 0);
                db.SaveChanges();
                return RedirectToAction("IndexByUser", new { id = employeeCustomField.UserInformationId });
            }
            return GetErrors();
        }
        public override List<EmployeeCustomField> OnIndexByUser(List<EmployeeCustomField> model, int userId)
        {
            var list = CustomFieldService.GetCustomFields(true);
            foreach (var each in list)
            {
                if (model == null || !model.Any(e => e.CustomFieldId == each.Id))
                    model.Add(new EmployeeCustomField() { CustomField = each, CustomFieldId = each.Id, UserInformationId = userId });
            }
            return model;
        }
        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        public FileResult GetReport()
        {
            string ReportURL = "{Your File Path}";
            byte[] FileBytes = System.IO.File.ReadAllBytes(ReportURL);
            return File(FileBytes, "application/pdf");
        }

        public ActionResult UploadDocument(int? id)
        {
            var model = db.EmployeeDocument.Find(id);
            return PartialView(model);
        }
        [HttpPost]
        public JsonResult UploadDocument()
        {
            string status = "Success";
            string message = "Education document is Successfully uploaded!";
            // var model = db.EmployeeEducation.Find(id);
            if (Request.Files.Count > 0)
            {
                try
                {
                    HttpPostedFileBase docFile = Request.Files[0];
                    string employeeDocumentId = Request.Form["eduDocRecordID"];
                    var employeeDocument = db.EmployeeDocument.Find(int.Parse(employeeDocumentId));
                    if (employeeDocument != null)
                    {
                        var fileName = Path.GetFileName(docFile.FileName);
                        string docName = "" + employeeDocument.UserInformationId + "_" + employeeDocumentId + "-" + fileName;

                        FilePathHelper filePathHelper = new FilePathHelper();
                        string serverFilePath = filePathHelper.GetPath("EmployeeCustomField", docName);
                        docFile.SaveAs(serverFilePath);
                        employeeDocument.DocumentPath = filePathHelper.RelativePath;
                        employeeDocument.DocumentName = docName;
                        employeeDocument.ModifiedDate = DateTime.Now;
                        db.SaveChanges();
                        //retResult = new { status = "Success", message = "CV is successfully Uploaded!" };
                        //status = "Success";
                        //message = "CV is successfully Uploaded!";
                    }
                    else
                    {
                        //retResult = new { status = "Error", message = "Invalid record data" };
                        status = "Error";
                        message = "Invalid document record data!";
                    }

                }
                catch (Exception ex)
                {
                    Helpers.ErrorLogHelper.InsertLog(Helpers.ErrorLogType.Error, ex, this.ControllerContext);
                    //retResult = new { status = "Error", message = ex.Message };
                    status = "Error";
                    message = ex.Message;
                }
            }
            return Json(new { status = status, message = message });
        }

        public virtual ActionResult AddEmployeeCustomField(EmployeeCustomField employeeCustomField)
        {
            try
            {
                AllowAdd();
                ViewBag.CustomFieldList = db.GetAllByCompany<CustomField>(SessionHelper.SelectedCompanyId, SessionHelper.SelectedClientId).ToList();
                employeeCustomField.UserInformation = db.UserInformation.FirstOrDefault(u => u.Id == employeeCustomField.UserInformationId);
                employeeCustomField.CustomField = db.CustomField.FirstOrDefault(u => u.Id == employeeCustomField.CustomFieldId);
                ViewBag.Label = ViewBag.Label + " - Add";
                return PartialView("Create", employeeCustomField);
            }
            catch (AuthorizationException ex)
            {
                Exception exception = new Exception(ex.ErrorMessage);
                HandleErrorInfo handleErrorInfo = new HandleErrorInfo(exception, "City", "Index");
                return PartialView("~/Views/Shared/Error.cshtml", handleErrorInfo);
            }
        }
    }
}
