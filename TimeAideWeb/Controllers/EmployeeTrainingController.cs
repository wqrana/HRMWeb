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
using TimeAide.Web.Models;


namespace TimeAide.Web.Controllers
{
    public class EmployeeTrainingController : TimeAideWebControllers<EmployeeTraining>
    {
        [HttpGet]
        // GET: EmployeeTraining
        //public ActionResult IndexByUser(int id)
        //{
        //    var model = db.EmployeeTraining.Where(w => w.DataEntryStatus == 1 && w.UserInformationId == id).OrderByDescending(o => o.Id);
        //    return PartialView("Index", model);
        //}
        public ActionResult IndexByUser(int id)
        {
            var empTrainingList = new List<EmployeeTraining>();
            var model = db.EmployeeTraining.Where(w => w.DataEntryStatus == 1 && w.UserInformationId == id)
                                            .ToList();
            

            var employmentHistoryEntity = db.EmploymentHistory.Where(w => w.UserInformationId == id && w.DataEntryStatus == 1 && w.EndDate == null).OrderByDescending(o => o.Id).FirstOrDefault();
            if (employmentHistoryEntity != null)
            {
                var positionTrainingList = db.PositionTraining.Where(w => w.DataEntryStatus == 1 && w.PositionId == employmentHistoryEntity.PositionId).Select(s => s.Training).ToList();
                foreach(var positionTraining in positionTrainingList)
                {
                    model = model ?? empTrainingList;

                    var isExist = model.Where(w => w.TrainingId == positionTraining.Id).Count();
                    if (isExist == 0)
                    {
                        model.Add(new EmployeeTraining() { UserInformationId = id, TrainingId = positionTraining.Id, Training = positionTraining });
                    }
                }
            }
            //model=model.OrderByDescending(o => o.Id);

            return PartialView("Index", model.OrderByDescending(o => o.Id));
        }
        public override ActionResult Create()
        {
            
            ViewBag.TrainingId = new SelectList(db.GetAll<Training>(SessionHelper.SelectedClientId), "Id", "TrainingName");
            return PartialView();
        }
        public ActionResult SubmitTraining(int trainingId)
        {

            ViewBag.TrainingId = new SelectList(db.GetAll<Training>(SessionHelper.SelectedClientId), "Id", "TrainingName", trainingId);
            return PartialView("Create");
        }
        public override ActionResult Edit(int? id)
        {
            var model = db.EmployeeTraining.Where(w => w.Id == id).FirstOrDefault();
            ViewBag.TrainingId = new SelectList(db.GetAll<Training>(SessionHelper.SelectedClientId), "Id", "TrainingName", model.TrainingId);
            return PartialView(model);
        }
        public  ActionResult TrainingSelectionPopUp(int? id)
        { string positionName = "";
            var trainingSelectionList = new SelectList(db.Training.Where(w => w.Id == -1).ToList(), "Id", "TrainingName");

            var employmentHistoryEntity = db.EmploymentHistory.Where(w => w.UserInformationId == id && w.DataEntryStatus == 1 && w.EndDate == null).OrderByDescending(o => o.Id).FirstOrDefault();
            if (employmentHistoryEntity != null) {
                var positionTrainingList = db.PositionTraining.Where(w => w.DataEntryStatus == 1 && w.PositionId == employmentHistoryEntity.PositionId).Select(s=>s.Training).ToList();
                trainingSelectionList = new SelectList(positionTrainingList.Where(w => w.DataEntryStatus == 1), "Id", "NameAndDescription");
                positionName = employmentHistoryEntity.Position==null?"": employmentHistoryEntity.Position.PositionName;
            }
            ViewBag.PositionName = (positionName==""|| positionName==null)?"Not Available": positionName;
            ViewBag.TrainingSelectionId = trainingSelectionList;
            return PartialView();
        }
        public JsonResult AjaxGetTrainingList(int? id, bool isAllMasterData)
        {
            dynamic trainingList = db.Training.Where(w => w.Id == -1).Select(s => new { id = s.Id, text = s.NameAndDescription });
            if (isAllMasterData == false)
            {
                var employmentHistoryEntity = db.EmploymentHistory.Where(w => w.UserInformationId == id && w.DataEntryStatus == 1 && w.EndDate == null).OrderByDescending(o => o.Id).FirstOrDefault();
                if (employmentHistoryEntity != null)
                {
                    var positionTrainingList = db.PositionTraining.Where(w => w.DataEntryStatus == 1 && w.PositionId == employmentHistoryEntity.PositionId).Select(s => s.Training).ToList();
                    trainingList = positionTrainingList.Where(w => w.DataEntryStatus == 1).Select(s => new { id = s.Id, text = s.NameAndDescription });
                }
            }
            else
            {
                trainingList = db.GetAll<Training>(SessionHelper.SelectedClientId).Select(s => new { id = s.Id, text = s.NameAndDescription });
            }
            
                JsonResult jsonResult = new JsonResult()
                {
                    Data = trainingList,
                    JsonRequestBehavior = JsonRequestBehavior.AllowGet

                };

                return jsonResult;
             
         }
        public ActionResult UploadDocument(int? id)
        {
            var model = db.EmployeeTraining.Find(id);
            return PartialView(model);
        }
        [HttpPost]
        public JsonResult UploadDocument()
        {
            string status = "Success";
            string message = "Training document is Successfully uploaded!";
            
            if (Request.Files.Count > 0)
            {
                try
                {
                    HttpPostedFileBase docFile = Request.Files[0];
                    string trainingID = Request.Form["trainingDocRecordID"];
                    var employeeTrainingEntity = db.EmployeeTraining.Find(int.Parse(trainingID));
                    if (employeeTrainingEntity != null)
                    {
                        var fileName = Path.GetFileName(docFile.FileName);
                        var fileExt = Path.GetExtension(docFile.FileName);
                        string docName = "" + employeeTrainingEntity.UserInformationId + "_" + trainingID + "-" + fileName;

                        FilePathHelper filePathHelper = new FilePathHelper();
                        string serverFilePath = filePathHelper.GetPath("trainingDocs", docName);
                        docFile.SaveAs(serverFilePath);
                        employeeTrainingEntity.DocFilePath = filePathHelper.RelativePath;
                        employeeTrainingEntity.DocName = docName;
                        employeeTrainingEntity.ModifiedDate = DateTime.Now;
                        db.SaveChanges();
                        //retResult = new { status = "Success", message = "CV is successfully Uploaded!" };
                        //status = "Success";
                        //message = "CV is successfully Uploaded!";
                    }
                    else
                    {
                        //retResult = new { status = "Error", message = "Invalid record data" };
                        status = "Error";
                        message = "Invalid Training record data!";
                    }

                }
                catch (Exception ex)
                {
                    //retResult = new { status = "Error", message = ex.Message };
                    Helpers.ErrorLogHelper.InsertLog(Helpers.ErrorLogType.Error, ex, this.ControllerContext);
                    status = "Error";
                    message = ex.Message;
                }
            }
            return Json(new { status = status, message = message });
        }

        public JsonResult AjaxCheckDocument(int id)
        {
            dynamic retResult = null;
            string status = "Success";
            string message = "";
            string relativeFilePath = "";
            string serverFilePath = "";
            FileInfo downloadDocFile;


            if (id > 0)
            {
                try
                {

                    var employeeTrainingEntity = db.EmployeeTraining.Find(id);
                    if (employeeTrainingEntity != null)
                    {
                        if (!string.IsNullOrEmpty(employeeTrainingEntity.DocFilePath))
                        {
                            relativeFilePath = employeeTrainingEntity.DocFilePath;
                            var tempPath = "~" + relativeFilePath;
                            serverFilePath = Server.MapPath(tempPath);
                            downloadDocFile = new FileInfo(serverFilePath);
                            if (!downloadDocFile.Exists)
                            {
                                status = "Error";
                                message = "Document is not yet Uploaded!";
                            }
                        }
                        else
                        {
                            status = "Error";
                            message = "Document is not yet Uploaded!";
                        }
                    }
                    else
                    {
                        //retResult = new { status = "Error", message = "Invalid record data" };
                        status = "Error";
                        message = "Invalid Training record data!";
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
                message = "Invalid Training record data!";
            }
            retResult = new { status = status, message = message };
            return Json(retResult, JsonRequestBehavior.AllowGet);
        }
        public ActionResult DownloadDocument(int id)
        {
            string relativeFilePath = "", serverFilePath = "";
            FileInfo downloadDocFile = null;
            byte[] fileBytes;
            var userTraining = db.EmployeeTraining.Find(id);
            if (userTraining != null)
            {
                if (!string.IsNullOrEmpty(userTraining.DocFilePath))
                {
                    relativeFilePath = userTraining.DocFilePath;
                    var tempPath = "~" + relativeFilePath;
                    serverFilePath = Server.MapPath(tempPath);
                    downloadDocFile = new FileInfo(serverFilePath);

                    if (downloadDocFile.Exists)
                    {
                        fileBytes = System.IO.File.ReadAllBytes(serverFilePath);
                        return File(fileBytes, System.Net.Mime.MediaTypeNames.Application.Octet, downloadDocFile.Name);
                    }
                }

            }

            return null;

        }

        [HttpPost]
        public JsonResult CreateEdit(EmployeeTraining model)
        {
            string status = "Success";
            string message = "Successfully Added/Updated!";
            EmployeeTraining employeeTrainingEntity = null;
            try
            {
                if (model.Id == 0)
                {
                    employeeTrainingEntity = new EmployeeTraining();
                    employeeTrainingEntity.UserInformationId = model.UserInformationId;
                    db.EmployeeTraining.Add(employeeTrainingEntity);
                }
                else
                {
                    employeeTrainingEntity = db.EmployeeTraining.Find(model.Id);
                    employeeTrainingEntity.ModifiedBy = SessionHelper.LoginId;
                    employeeTrainingEntity.ModifiedDate = DateTime.Now;
                }
                employeeTrainingEntity.TrainingId = model.TrainingId;
                employeeTrainingEntity.TrainingTypeId = model.TrainingTypeId;
                employeeTrainingEntity.TrainingDate = model.TrainingDate;
                employeeTrainingEntity.ExpiryDate = model.ExpiryDate;
                employeeTrainingEntity.Note = model.Note;

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

        [HttpPost]
        public JsonResult ConfirmDelete(int id)
        {
            string status = "Success";
            string message = "Successfully Deleted!";
            var employeeTrainingEntity = db.EmployeeTraining.Find(id);
            try
            {
                employeeTrainingEntity.ModifiedBy = SessionHelper.LoginId;
                employeeTrainingEntity.ModifiedDate = DateTime.Now;
                employeeTrainingEntity.DataEntryStatus = 0;
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
        public JsonResult AjaxGetAutoCompleteData(string term, string fieldName)
        {
            IList<string> autoCompleteDataList = null;
            switch (fieldName)
            {
                case "Type":
                    autoCompleteDataList = db.EmployeeTraining
                                           .Where(w => w.TrainingTypeId.HasValue && w.TrainingType!=null && w.TrainingType.TrainingTypeName.ToLower().Contains(term.ToLower()))
                                           .Select(s => s.TrainingType.TrainingTypeName).Distinct()
                                           .ToList();
                    break;
               
            }

            return Json(autoCompleteDataList, JsonRequestBehavior.AllowGet);
        }
    }
}