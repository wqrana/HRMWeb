using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using TimeAide.Common.Helpers;
using TimeAide.Models.ViewModel;
using TimeAide.Web.Models;

namespace TimeAide.Web.Controllers
{
    public class EmployeeCredentialController : TimeAideWebControllers<EmployeeCredential>
    {
        public ActionResult IndexEmployeeCredential(int? id)
        {
            ViewBag.UserInformationId = id;
            List<EmployeeCredential> employeeRequiredCredentials = new List<EmployeeCredential>();
            var res = from abcd in db.EmployeeCredential.Include(u => u.Credential).Where(c => c.UserInformationId == id && c.DataEntryStatus == 1)
                      group abcd by new { abcd.Credential, abcd.UserInformationId }
            into temp
                      select temp.OrderBy(p => p.IssueDate).FirstOrDefault();

            foreach (var cr in res.Where(c => c.DataEntryStatus == 1))
            {
                //employeeRequiredCredentials.Add(new EmployeeCredential { Credential = cr, UserInformationId = id ?? 0,CredentialId=cr.Id });
                employeeRequiredCredentials.Add(cr);
            }

            foreach (var cr in db.Credential.Where(c => c.DataEntryStatus == 1))
            {
                if (!employeeRequiredCredentials.Any(c => c.CredentialId == cr.Id))
                    employeeRequiredCredentials.Add(new EmployeeCredential { Credential = cr, UserInformationId = id ?? 0, CredentialId = cr.Id });
            }


            return PartialView("Index", employeeRequiredCredentials);
        }

        public virtual ActionResult Index1(EmployeeCredential employeeCredential)
        {
            try
            {
                AllowView();
                var entitySet = db.EmployeeCredential.Where(e => e.CredentialId == employeeCredential.CredentialId && e.UserInformationId == employeeCredential.UserInformationId);
                return PartialView(entitySet.OrderByDescending(e => e.CreatedDate).ToList());
            }
            catch (AuthorizationException ex)
            {
                Exception exception = new Exception(ex.ErrorMessage);
                HandleErrorInfo handleErrorInfo = new HandleErrorInfo(exception, typeof(EmployeeCredential).Name, "Index");
                return View("~/Views/Shared/Error.cshtml", handleErrorInfo);
            }
        }


        public virtual ActionResult Index2(EmployeeCredential employeeCredential)
        {
            try
            {
                AllowView();
                IQueryable<EmployeeCredential> entitySet = null;
                List<EmployeeCredential> model = null;
                if (employeeCredential.IsRequired)
                {
                    ////var entitySet = db.EmployeeCredential.Where(e => e.UserInformationId == userId);
                    //entitySet = from abcd in db.EmployeeCredential.Include(u => u.Credential)
                    //                .Where(c => c.UserInformationId == employeeCredential.UserInformationId && c.DataEntryStatus == 1 && c.IsRequired)
                    //            select abcd;
                    entitySet = GetRequiredCredentialList(employeeCredential).AsQueryable();
                    @ViewBag.NewCaption = "Required Credential";
                    model = entitySet.OrderByDescending(e => e.Id).ToList();
                }
                else
                {
                    //var entitySet = db.EmployeeCredential.Where(e => e.UserInformationId == userId);
                    //entitySet = from abcd in db.EmployeeCredential.Include(u => u.Credential)
                    //                .Where(c => c.UserInformationId == employeeCredential.UserInformationId && c.DataEntryStatus == 1 && !c.IsRequired)
                    //            select abcd;

                    entitySet = GetNotRequiredCredentialList(employeeCredential).AsQueryable();

                    @ViewBag.NewCaption = "Non-Required Credential";
                    model = entitySet.OrderByDescending(e => e.Id).ToList();
                }

                return PartialView(model);
            }
            catch (AuthorizationException ex)
            {
                Exception exception = new Exception(ex.ErrorMessage);
                HandleErrorInfo handleErrorInfo = new HandleErrorInfo(exception, typeof(EmployeeCredential).Name, "Index");
                return View("~/Views/Shared/Error.cshtml", handleErrorInfo);
            }
        }

        public virtual ActionResult GetCredentialCheckListPopup(EmployeeCredential employeeCredential)
        {
            try
            {
                AllowView();
                IQueryable<EmployeeCredential> entitySet = null;
                List<EmployeeCredential> model = new List<EmployeeCredential>();
                //Required Credential
                entitySet = GetRequiredCredentialList(employeeCredential).AsQueryable();
                model.AddRange(entitySet.OrderByDescending(e => e.Id).ToList());

                //Non Required Credential
                entitySet = GetNotRequiredCredentialList(employeeCredential).AsQueryable();
                model.AddRange(entitySet.OrderByDescending(e => e.Id).ToList());
                return PartialView(model);
            }

            catch (AuthorizationException ex)
            {
                Exception exception = new Exception(ex.ErrorMessage);
                HandleErrorInfo handleErrorInfo = new HandleErrorInfo(exception, typeof(EmployeeCredential).Name, "Index");
                return View("~/Views/Shared/Error.cshtml", handleErrorInfo);
            }
        }

        private IList<EmployeeCredential> GetNotRequiredCredentialList(EmployeeCredential employeeCredential)
        {
            List<EmployeeCredential> emptyList = new List<EmployeeCredential>();

            var userInfo = db.SP_UserInformationById<UserInformation>(employeeCredential.UserInformationId ?? 0);
            var EmployeeAllCredentialEntitySet = db.EmployeeCredential.Include(u => u.Credential)
                           .Where(c => c.UserInformationId == employeeCredential.UserInformationId && c.DataEntryStatus == 1);

            var EmployeeNotRequiredCredentialEntitySet = db.EmployeeCredential.Include(u => u.Credential)
                           .Where(c => c.UserInformationId == employeeCredential.UserInformationId && c.DataEntryStatus == 1 && !c.IsRequired).ToList();

            var positionBasedCredential = db.PositionCredential.Where(w => w.PositionId == userInfo.PositionId && w.DataEntryStatus == 1 && (w.IsRequired ?? false) == false)
                                                                .Select(s => s.Credential).ToList();
            foreach (var posCredItem in positionBasedCredential)
            {
                if (EmployeeNotRequiredCredentialEntitySet == null)
                {
                    EmployeeNotRequiredCredentialEntitySet = emptyList;
                }
                var isExist = EmployeeAllCredentialEntitySet.Where(w => w.CredentialId == posCredItem.Id).Count();
                if (isExist == 0)
                {
                    EmployeeNotRequiredCredentialEntitySet.Add(new EmployeeCredential() { UserInformationId = employeeCredential.UserInformationId, CredentialId = posCredItem.Id, Credential = posCredItem, IsRequired = false });
                }
            }
            return EmployeeNotRequiredCredentialEntitySet;
        }
        private IList<EmployeeCredential> GetRequiredCredentialList(EmployeeCredential employeeCredential)
        {
            List<EmployeeCredential> emptyList = new List<EmployeeCredential>();

            var userInfo = db.SP_UserInformationById<UserInformation>(employeeCredential.UserInformationId ?? 0);
            var EmployeeAllCredentialEntitySet = db.EmployeeCredential.Include(u => u.Credential)
                           .Where(c => c.UserInformationId == employeeCredential.UserInformationId && c.DataEntryStatus == 1);

            var EmployeeRequiredCredentialEntitySet = db.EmployeeCredential.Include(u => u.Credential)
                           .Where(c => c.UserInformationId == employeeCredential.UserInformationId && c.DataEntryStatus == 1 && c.IsRequired == true).ToList();

            var positionBasedCredential = db.PositionCredential.Where(w => w.PositionId == userInfo.PositionId && w.DataEntryStatus == 1 && w.IsRequired == true)
                                                                .Select(s => s.Credential).ToList();
            foreach (var posCredItem in positionBasedCredential)
            {
                if (EmployeeRequiredCredentialEntitySet == null)
                {
                    EmployeeRequiredCredentialEntitySet = emptyList;
                }
                var isExist = EmployeeAllCredentialEntitySet.Where(w => w.CredentialId == posCredItem.Id).Count();
                if (isExist == 0)
                {
                    EmployeeRequiredCredentialEntitySet.Add(new EmployeeCredential() { UserInformationId = employeeCredential.UserInformationId, CredentialId = posCredItem.Id, Credential = posCredItem, IsRequired = true });
                }
            }
            return EmployeeRequiredCredentialEntitySet;
        }
        public virtual ActionResult Index3(int UserInformationId, bool IsRequired)
        {
            try
            {
                AllowView();
                IQueryable<EmployeeCredential> entitySet = null;
                if (IsRequired)
                {
                    //var entitySet = db.EmployeeCredential.Where(e => e.UserInformationId == userId);
                    entitySet = from abcd in db.EmployeeCredential.Include(u => u.Credential)
                                    .Where(c => c.UserInformationId == UserInformationId && c.DataEntryStatus == 1 && c.IsRequired)
                                select abcd;
                    @ViewBag.NewCaption = "Required Credential";

                }
                else
                {
                    //var entitySet = db.EmployeeCredential.Where(e => e.UserInformationId == userId);
                    entitySet = from abcd in db.EmployeeCredential.Include(u => u.Credential)
                                    .Where(c => c.UserInformationId == UserInformationId && c.DataEntryStatus == 1 && !c.IsRequired)
                                select abcd;

                    @ViewBag.NewCaption = "Non-Required Credential";

                }
                return PartialView("Index2", entitySet.OrderByDescending(e => e.CreatedDate).ToList());
            }
            catch (AuthorizationException ex)
            {
                Exception exception = new Exception(ex.ErrorMessage);
                HandleErrorInfo handleErrorInfo = new HandleErrorInfo(exception, typeof(EmployeeCredential).Name, "Index");
                return View("~/Views/Shared/Error.cshtml", handleErrorInfo);
            }
        }

        public virtual ActionResult AddEmployeeCredential1(EmployeeCredential employeeCredential)
        {
            try
            {
                AllowAdd();
                employeeCredential.UserInformation = db.UserInformation.FirstOrDefault(u => u.Id == employeeCredential.UserInformationId);
                employeeCredential.Credential = db.Credential.FirstOrDefault(u => u.Id == employeeCredential.CredentialId);
                ViewBag.Label = ViewBag.Label + " - Add";
                return PartialView("Create1", employeeCredential);
            }
            catch (AuthorizationException ex)
            {
                Exception exception = new Exception(ex.ErrorMessage);
                HandleErrorInfo handleErrorInfo = new HandleErrorInfo(exception, "City", "Index");
                return PartialView("~/Views/Shared/Error.cshtml", handleErrorInfo);
            }
        }

        public virtual ActionResult AddEmployeeCredential(EmployeeCredential employeeCredential)
        {
            try
            {
                AllowAdd();
                employeeCredential.UserInformation = db.UserInformation.FirstOrDefault(u => u.Id == employeeCredential.UserInformationId);
                employeeCredential.Credential = db.Credential.FirstOrDefault(u => u.Id == employeeCredential.CredentialId);
                employeeCredential.CredentialId = employeeCredential.CredentialId;
                ViewBag.Label = ViewBag.Label + " - Add";
                return PartialView("Create", employeeCredential);
            }
            catch (AuthorizationException ex)
            {
                Exception exception = new Exception(ex.ErrorMessage);
                HandleErrorInfo handleErrorInfo = new HandleErrorInfo(exception, "City", "Index");
                return PartialView("~/Views/Shared/Error.cshtml", handleErrorInfo);
            }
        }

        public virtual ActionResult EditEmployeeCredential(EmployeeCredential employeeCredential)
        {
            try
            {
                AllowAdd();
                if (employeeCredential.Id > 0)
                {
                    employeeCredential = db.EmployeeCredential.FirstOrDefault(c => c.Id == employeeCredential.Id);
                }

                //employeeCredential.UserInformation = db.UserInformation.FirstOrDefault(u => u.Id == employeeCredential.UserInformationId);
                //employeeCredential.Credential = db.Credential.FirstOrDefault(u => u.Id == employeeCredential.CredentialId);
                ViewBag.Label = ViewBag.Label + " - Edit";
                return PartialView("Create", employeeCredential);
            }
            catch (AuthorizationException ex)
            {
                Exception exception = new Exception(ex.ErrorMessage);
                HandleErrorInfo handleErrorInfo = new HandleErrorInfo(exception, "City", "Index");
                return PartialView("~/Views/Shared/Error.cshtml", handleErrorInfo);
            }
        }
        public JsonResult GetEmployeeCredential(int id)
        {
            var record = db.EmployeeCredential.FirstOrDefault(e => e.Id == id);
            if (record != null)
                return Json(record);
            return Json("");
        }
        // POST: EmployeeCredential/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.

        public virtual ActionResult CreateRecord(int id, int userInformationId, int? credentialId, bool IsRequired)
        {
            try
            {
                AllowAdd();
                //AddDropDowns();

                EmployeeCredential employeeCredential = null;
                if (id != 0)
                {
                    employeeCredential = db.EmployeeCredential.FirstOrDefault(i => i.Id == id);
                    ViewBag.CredentialId = new SelectList(db.GetAll<Credential>(SessionHelper.SelectedClientId), "Id", "CredentialName", employeeCredential.CredentialId);
                }
                if (employeeCredential == null)
                {
                    ViewBag.CredentialId = new SelectList(db.GetAll<Credential>(SessionHelper.SelectedClientId), "Id", "CredentialName");
                    employeeCredential = new EmployeeCredential();
                }
                employeeCredential.UserInformationId = userInformationId;
                return PartialView(employeeCredential);
            }
            catch (AuthorizationException ex)
            {
                Exception exception = new Exception(ex.ErrorMessage);
                HandleErrorInfo handleErrorInfo = new HandleErrorInfo(exception, "City", "Index");
                return PartialView("~/Views/Shared/Error.cshtml", handleErrorInfo);
            }
        }

        [HttpPost]
        public ActionResult Create(EmployeeCredential employeeCredential)
        {
            if(ModelState.IsValid==true && employeeCredential.ExpirationDate != null)
            {
                if(employeeCredential.IssueDate> employeeCredential.ExpirationDate)
                {
                    ModelState.AddModelError("ExpirationDate", "Expiry Date shouldn't be prior to issue date.");
                }
            }

            if (ModelState.IsValid)
            {
                try
                {
                    if (employeeCredential.Id > 0)
                    {
                        var dbObject = (new TimeAideContext()).EmployeeCredential.Find(employeeCredential.Id);
                        employeeCredential.SetUpdated<EmployeeCredential>();
                        db.Entry(employeeCredential).State = EntityState.Modified;

                        if (string.IsNullOrEmpty(employeeCredential.DocumentName))
                        {
                            employeeCredential.DocumentName = dbObject.DocumentName;
                            employeeCredential.DocumentPath = dbObject.DocumentPath;
                            employeeCredential.DocumentFile = dbObject.DocumentFile;
                        }
                        var employeeCredentialDb = db.EmployeeCredential.FirstOrDefault(ec => ec.Id == employeeCredential.Id);
                        if (employeeCredentialDb.ExpirationDate != employeeCredential.ExpirationDate)
                            db.NotificationLog.Where(l => l.EmployeeCredentialId == employeeCredential.Id).ToList().ForEach(c => c.DataEntryStatus = 0);
                    }
                    else
                    {
                        db.EmployeeCredential.Add(employeeCredential);
                    }
                    db.SaveChanges();
                }
                catch (Exception ex)
                {
                }
                ViewBag.UserInformationId = employeeCredential.UserInformationId;
                return Json(employeeCredential);
            }

            return GetErrors();
        }

        // POST: EmployeeCredential/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        //[ValidateAntiForgeryToken]
        public ActionResult Edit(EmployeeCredential employeeCredential)
        {
            if (ModelState.IsValid)
            {
                var employeeCredentialDb = db.EmployeeCredential.FirstOrDefault(ec => ec.Id == employeeCredential.Id);
                db.Entry(employeeCredential).State = EntityState.Modified;
                if (!string.IsNullOrEmpty(employeeCredentialDb.DocumentName))
                    employeeCredential.DocumentName = employeeCredentialDb.DocumentName;
                if (!string.IsNullOrEmpty(employeeCredentialDb.DocumentPath))
                    employeeCredential.DocumentPath = employeeCredentialDb.DocumentPath;
                db.SaveChanges();
                return RedirectToAction("IndexByUser", new { id = employeeCredential.UserInformationId });
            }

            return GetErrors();
        }

        public ActionResult UploadDocument(int? id)
        {
            var model = db.EmployeeCredential.Find(id);
            return PartialView(model);
        }
        [HttpPost]
        public JsonResult UploadDocument()
        {
            string status = "Success";
            string message = "EmployeeCredential document is Successfully uploaded!";
            // var model = db.EmployeeCredential.Find(id);
            if (Request.Files.Count > 0)
            {
                try
                {
                    HttpPostedFileBase docFile = Request.Files[0];
                    string employeeCredentialId = Request.Form["employeeCredentialId"];
                    var employeeCredential = db.EmployeeCredential.Find(int.Parse(employeeCredentialId));
                    if (employeeCredential != null)
                    {
                        var fileName = Path.GetFileName(docFile.FileName);
                        string docName = "" + employeeCredential.UserInformationId + "_" + employeeCredentialId + "_" + DateTime.Now.ToString("yyyyMMddHHmmss") + "-" + fileName;


                        FilePathHelper filePathHelper = new FilePathHelper();
                        string serverFilePath = filePathHelper.GetPath("EmployeeCredentialDocs", docName);
                        docFile.SaveAs(serverFilePath);
                        employeeCredential.DocumentPath = filePathHelper.RelativePath;
                        employeeCredential.DocumentName = docName;
                        employeeCredential.ModifiedDate = DateTime.Now;
                        db.SaveChanges();
                        //retResult = new { status = "Success", message = "CV is successfully Uploaded!" };
                        //status = "Success";
                        //message = "CV is successfully Uploaded!";
                    }
                    else
                    {
                        //retResult = new { status = "Error", message = "Invalid record data" };
                        status = "Error";
                        message = "Invalid EmployeeCredential record data!";
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


        public JsonResult AjaxGetEmployeeCredential(int id)
        {
            //List<string> selectedClientList = (model.SelectedClientId ?? "").Split(',').ToList();
            var stateList = db.EmployeeCredential
                                .FirstOrDefault(w => w.Id == id);
            JsonResult jsonResult = new JsonResult()
            {
                Data = new { Id = stateList.Id, UserInformationId = stateList.UserInformationId, EmployeeCredentialName = stateList.EmployeeCredentialName, EmployeeCredentialDescription = stateList.EmployeeCredentialDescription, IssueDate = stateList.IssueDate, ExpirationDate = stateList.ExpirationDate, Note = stateList.Note, CredentialTypeId = stateList.CredentialTypeId },
                JsonRequestBehavior = JsonRequestBehavior.AllowGet

            };

            return jsonResult;
        }

        public JsonResult AjaxGetCredentialList(int? id, bool isAllMasterData)
        {
            dynamic credentialList = db.Credential.Where(w => w.Id == -1).Select(s => new { id = s.Id, text = s.CredentialDDLName });
            if (isAllMasterData == false)
            {
                var employmentHistory = db.EmploymentHistory.Where(w => w.UserInformationId == id && w.DataEntryStatus == 1 && w.EndDate == null).OrderByDescending(o => o.Id).FirstOrDefault();
                if (employmentHistory != null)
                {
                    var positionCredentialList = db.PositionCredential.Where(w => w.DataEntryStatus == 1 && w.PositionId == employmentHistory.PositionId).Select(s => s.Credential).ToList();
                    credentialList = positionCredentialList.Select(s => new { id = s.Id, text = s.CredentialDDLName });
                }
            }
            else
            {
                credentialList = db.GetAll<Credential>(SessionHelper.SelectedClientId).Select(s => new { id = s.Id, text = s.CredentialDDLName });
            }

            JsonResult jsonResult = new JsonResult()
            {
                Data = credentialList,
                JsonRequestBehavior = JsonRequestBehavior.AllowGet

            };

            return jsonResult;

        }

        public JsonResult AjaxGetAutoCompleteData(string term, string fieldName)
        {
            IList<string> autoCompleteDataList = null;
            switch (fieldName)
            {
                case "Type":
                    autoCompleteDataList = db.EmployeeCredential
                                           //.Where(w => w.Credential.Contains(term))
                                           .Select(s => s.Credential.CredentialName).Distinct()
                                           .ToList();
                    break;

            }

            return Json(autoCompleteDataList, JsonRequestBehavior.AllowGet);
        }


        public ActionResult CredentialSelectionPopUp(int? id)
        {
            string positionName = "";
            var credentialSelectionList = new SelectList(db.Credential.Where(w => w.Id == -1), "Id", "CredentialDDLName");

            var employmentHistory = db.EmploymentHistory.Where(w => w.UserInformationId == id && w.DataEntryStatus == 1 && w.EndDate == null).OrderByDescending(o => o.Id).FirstOrDefault();
            if (employmentHistory != null)
            {
                var positionTrainingList = db.PositionCredential.Where(w => w.DataEntryStatus == 1 && w.PositionId == employmentHistory.PositionId).Select(s => s.Credential);
                credentialSelectionList = new SelectList(positionTrainingList.Where(w => w.DataEntryStatus == 1), "Id", "CredentialDDLName");
                if (employmentHistory.Position != null)
                    positionName = employmentHistory.Position.PositionName;
            }
            ViewBag.PositionName = (positionName == "" || positionName == null) ? "Not Available" : positionName;
            ViewBag.CredentialSelectionId = credentialSelectionList;
            return PartialView();
        }

        [HttpPost]
        public JsonResult AjaxCheckCredential(int userID)
        {
            dynamic retResult = null;
            string status = "Success";
            string message = "";
            string relativeFilePath = "";
            string serverFilePath = "";
            FileInfo downloadCVFile;
            // serverFilePath = Path.Combine(Server.MapPath("~/Content/doc/resumes"), profilePictureName);
            //retResult = new { status = "Error", message = "No file is selected" };

            if (userID > 0)
            {
                try
                {

                    var user = db.EmployeeCredential.Find(userID);
                    if (user != null)
                    {
                        if (!string.IsNullOrEmpty(user.DocumentPath))
                        {
                            relativeFilePath = user.DocumentPath.Replace("\\\\", "\\");
                            var tempPath = "~" + relativeFilePath;
                            serverFilePath = Server.MapPath(relativeFilePath);
                            downloadCVFile = new FileInfo(serverFilePath);
                            if (!downloadCVFile.Exists)
                            {
                                status = "Error";
                                message = "Credential is not yet Uploaded!";
                            }
                        }
                        else
                        {
                            status = "Error";
                            message = "Credential is not yet Uploaded!";
                        }
                    }
                    else
                    {
                        //retResult = new { status = "Error", message = "Invalid record data" };
                        status = "Error";
                        message = "Invalid User record data!";
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
            else
            {
                status = "Error";
                message = "Invalid User record data!";
            }
            retResult = new { status = status, message = message };
            return Json(retResult);
        }
        public ActionResult DownloadCredential(int id)
        {
            string relativeFilePath = "", serverFilePath = "";
            FileInfo downloadCVFile = null;
            byte[] fileBytes;
            var user = db.EmployeeCredential.Find(id);
            if (user != null)
            {
                if (!string.IsNullOrEmpty(user.DocumentPath))
                {
                    relativeFilePath = user.DocumentPath.Replace("\\\\", "\\"); ;
                    var tempPath = "~" + relativeFilePath;
                    serverFilePath = Server.MapPath(tempPath);
                    downloadCVFile = new FileInfo(serverFilePath);

                    if (downloadCVFile.Exists)
                    {
                        fileBytes = System.IO.File.ReadAllBytes(serverFilePath);
                        return File(fileBytes, System.Net.Mime.MediaTypeNames.Application.Octet, downloadCVFile.Name);
                    }
                }

            }

            return null;

        }
        public ActionResult EmployeeCredentialUploadHistory(int id)
        {
            var employeeCredential = db.EmployeeCredential.FirstOrDefault(c => c.Id == id);
            string title = employeeCredential.EmployeeCredentialName + "(" + employeeCredential.Credential.CredentialName + ")";
            ViewBag.CredentialTitle = title;
            List<int> employeeCredentialIds = new List<int>();
            //SelfServiceCredentials(id, employeeCredentialIds);

            List<EmployeeCredential> employeeCredentials = new List<EmployeeCredential>();
            FilePathHelper filePathHelper = new FilePathHelper();
            string serverFilePath = filePathHelper.GetPath("EmployeeCredentialDocs");
            string[] files = Directory.GetFiles(serverFilePath, "*.*", SearchOption.AllDirectories);
            //if (employeeCredentialIds.Count() > 0)
            {
                int userId = employeeCredential.UserInformationId ?? 0;
                foreach (string s in files)
                {
                    //foreach (int documentId in employeeCredentialIds)
                    //{
                        string documentName = Path.GetFileName(s);
                        if (s.Contains(userId.ToString() + "_" + id.ToString() + "_"))
                        {
                            string[] fileNameParts = s.Split('_');
                            DateTime documentUploadDate = DateTime.Now;
                            if (fileNameParts.Length >= 3)
                            {
                                try
                                {
                                    documentUploadDate = DateTime.ParseExact(fileNameParts[2], "yyyyMMddHHmmss", CultureInfo.InvariantCulture);
                                    //documentUploadDate = Convert.ToDateTime(fileNameParts[2]);
                                }
                                catch { }
                            }

                            employeeCredentials.Add(new EmployeeCredential() { Id = id, DocumentName = documentName, DocumentPath = documentName + "\\" + documentName, CreatedDate = documentUploadDate });
                        //}
                    }
                }
            }

            return PartialView(employeeCredentials);
        }

        //public bool SelfServiceCredentials(int employeeCredentialId, List<int> employeeCredentialIds)
        //{
        //    employeeCredentialIds.Add(employeeCredentialId);
        //    var selfServiceCredentials = db.SelfServiceEmployeeCredential.FirstOrDefault(c => c.EmployeeCredentialId == employeeCredentialId);
        //    if (selfServiceCredentials != null && selfServiceCredentials.NewEmployeeCredentialId.HasValue)
        //        SelfServiceCredentials(selfServiceCredentials.NewEmployeeCredentialId.Value, employeeCredentialIds);
        //    return false;
        //}
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
