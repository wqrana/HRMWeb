
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
using TimeAide.Services;
using TimeAide.Data;
namespace TimeAide.Web.Controllers
{
    public class EmployeeTimeOffRequestDocumentController : BaseTAWindowRoleRightsController<EmployeeTimeOffRequest>
    {
        // GET: EmployeeTimeOffRequestDocument
        public ActionResult GetTimeOffTypeDocument(string timeOffTypeName)
        {
            try
            {
                var showTimeOffDoc = ApplicationConfigurationService.GetConfigurationStatus("ShowTimeOffDocument");
                IList<EmployeeTimeOffRequestDocumentViewModel> documentList = new List<EmployeeTimeOffRequestDocumentViewModel>();
                if (showTimeOffDoc) {
                 var transId = timeAideWindowContext.tTransDef.Where(w => w.Name == timeOffTypeName).FirstOrDefault().ID;
                    //documentList = timeAideWindowContext.tblSS_TransdefTimeOffDocument.Where(w => w.intTransId == transId)
                    //                                        .Select(s=>new EmployeeTimeOffRequestDocumentViewModel() { DocumentName=s.sDocumentName, DocumentType=s.intDocumentType==1?"Request":"Supporting",SubmissionType=s.intSubmissionType==1? "Optional": "Mandatory",Status="NA" })
                    //                                        .ToList();
                    documentList = timeAideWindowContext.tblSS_TransdefTimeOffDocument.Where(w => w.intTransId == transId)
                                                            .AsEnumerable()
                                                            .Select(s => new EmployeeTimeOffRequestDocumentViewModel() { DocumentName = s.sDocumentName, DocumentType = DataHelper.TimeOffDocumentType[s.intDocumentType.Value], SubmissionType = DataHelper.TimeOffSubmissionType[s.intSubmissionType.Value],TimeoffDays=s.intTimeOffDays , Status = "NA" })
                                                            .ToList();
                }
                return PartialView(documentList);
            }
            catch (Exception ex)
            {
                Helpers.ErrorLogHelper.InsertLog(Helpers.ErrorLogType.Error, ex, this.ControllerContext);
                HandleErrorInfo handleErrorInfo = new HandleErrorInfo(ex, "TimeOffRequestDocument", "GetTimeOffTypeDocument");
                return PartialView("~/Views/Shared/Error.cshtml", handleErrorInfo);
            }
            return PartialView();
        }
        public ActionResult GetTimeOffRequestDocument(int id)
        {
            try
            {
                var documentList = timeAideWebContext.EmployeeTimeOffRequestDocument.Where(w => w.EmployeeTimeOffRequestId == id
                                                        && w.DataEntryStatus == 1).ToList();

                return PartialView(documentList);
            }
            catch (Exception ex)
            {
                Helpers.ErrorLogHelper.InsertLog(Helpers.ErrorLogType.Error, ex, this.ControllerContext);
                HandleErrorInfo handleErrorInfo = new HandleErrorInfo(ex, "TimeOffRequestDocument", "GetTimeOffRequestDocument");
                return PartialView("~/Views/Shared/Error.cshtml", handleErrorInfo);
            }
            return PartialView();
        }
        public ActionResult PostSubmitRequestDocumentView()
        {
            return PartialView("_PostSubmitRequestDocument");
        }
        public ActionResult UploadDocumentFiles()
        {
            return PartialView();
        }
        public ActionResult EditUploadDocumentFiles(int id)
        {
            var model = timeAideWebContext.EmployeeTimeOffRequestDocument.Find(id);
            return PartialView(model);
           
        }
        public ActionResult DownloadDocumentFiles(int id)
        {
            var model = timeAideWebContext.EmployeeTimeOffRequestDocument.Find(id);
            return PartialView(model);
        }
        public ActionResult DownloadDocumentFile(int id, int fileSeq)
        {

            byte[] fileBytes = null;
            string fileName = "";
            var model = timeAideWebContext.EmployeeTimeOffRequestDocument.Find(id);
            if (model != null)
            {
                switch (fileSeq)
                {
                    case 1: 
                        fileBytes = model.DocumentFile1;
                        fileName = model.DocumentFile1Name+model.DocumentFile1Ext;
                        break;
                    case 2:
                        fileBytes = model.DocumentFile2;
                        fileName = model.DocumentFile2Name + model.DocumentFile2Ext;
                        break;
                    case 3:
                        fileBytes = model.DocumentFile3;
                        fileName = model.DocumentFile3Name + model.DocumentFile3Ext;
                        break;
                   
                }
                if (fileBytes != null)
                {
                    return File(fileBytes, System.Net.Mime.MediaTypeNames.Application.Octet, fileName);
                }
            }
            return null;
        }
        [HttpPost]
        public JsonResult CreateTimeOffRequestDocuments(List<EmployeeTimeOffRequestDocumentViewModel> model)
        {
            string status = "Success";
            string message = "Time-Off Document(s) are Successfully saved!";
            try
            {
                foreach(var timeOffDoc in model)
                {
                    var timeOffDocumentEntity = new EmployeeTimeOffRequestDocument();
                    timeOffDocumentEntity.EmployeeTimeOffRequestId = timeOffDoc.EmployeeTimeOffRequestId;
                    timeOffDocumentEntity.DocumentName = timeOffDoc.DocumentName;
                    timeOffDocumentEntity.DocumentType = timeOffDoc.DocumentType;
                    timeOffDocumentEntity.SubmissionType = timeOffDoc.SubmissionType;
                    timeOffDocumentEntity.TimeoffDays = timeOffDoc.TimeoffDays;
                    timeOffDocumentEntity.Status = timeOffDoc.Status;
                    if (timeOffDoc.Status == "Submitted")
                    {
                        if (timeOffDoc.DocumentFile1 != null)
                        {
                            byte[] fileData = null;
                            var extFile1 = Path.GetExtension(timeOffDoc.DocumentFile1.FileName);
                            using (var binaryReader = new BinaryReader(timeOffDoc.DocumentFile1.InputStream))
                            {
                                fileData = binaryReader.ReadBytes(timeOffDoc.DocumentFile1.ContentLength);
                            }
                            timeOffDocumentEntity.DocumentFile1Name = timeOffDoc.DocumentFile1Name;
                            timeOffDocumentEntity.DocumentFile1 = fileData;
                            timeOffDocumentEntity.DocumentFile1Ext = extFile1;
                        }
                        if (timeOffDoc.DocumentFile2 != null)
                        {
                            byte[] fileData = null;
                            var extFile2 = Path.GetExtension(timeOffDoc.DocumentFile2.FileName);
                            using (var binaryReader = new BinaryReader(timeOffDoc.DocumentFile2.InputStream))
                            {
                                fileData = binaryReader.ReadBytes(timeOffDoc.DocumentFile2.ContentLength);
                            }
                            timeOffDocumentEntity.DocumentFile2Name = timeOffDoc.DocumentFile2Name;
                            timeOffDocumentEntity.DocumentFile2 = fileData;
                            timeOffDocumentEntity.DocumentFile2Ext = extFile2;
                        }
                        if (timeOffDoc.DocumentFile3 != null)
                        {
                            byte[] fileData = null;
                            var extFile3 = Path.GetExtension(timeOffDoc.DocumentFile3.FileName);
                            using (var binaryReader = new BinaryReader(timeOffDoc.DocumentFile3.InputStream))
                            {
                                fileData = binaryReader.ReadBytes(timeOffDoc.DocumentFile3.ContentLength);
                            }
                            timeOffDocumentEntity.DocumentFile3Name = timeOffDoc.DocumentFile3Name;
                            timeOffDocumentEntity.DocumentFile3 = fileData;
                            timeOffDocumentEntity.DocumentFile3Ext = extFile3;
                        }
                    }
                    timeAideWebContext.EmployeeTimeOffRequestDocument.Add(timeOffDocumentEntity);
                }
                timeAideWebContext.SaveChanges();
            }
            catch(Exception ex)
            {
                Helpers.ErrorLogHelper.InsertLog(Helpers.ErrorLogType.Error, ex, this.ControllerContext);
                //retResult = new { status = "Error", message = ex.Message };
                status = "Error";
                message = ex.Message;
            }

            return Json(new { status = status, message = message });
        }
        public JsonResult EditTimeOffRequestDocument(EmployeeTimeOffRequestDocumentViewModel model)
        {
            string status = "Success";
            string message = "Time-Off Document is Successfully saved!";
            try
            {
               var timeOffDocumentEntity= timeAideWebContext.EmployeeTimeOffRequestDocument.Find(model.EmployeeTimeOffDocumentId);

                if(timeOffDocumentEntity!=null)
                {              
                  if (model.Status == "Submitted")
                    {
                        timeOffDocumentEntity.Status = model.Status;
                        if (model.DocumentFile1 != null && 
                                (model.DocumentFile1Action=="Edit"|| model.DocumentFile1Action == "New"))
                        {
                            byte[] fileData = null;
                            var extFile1 = Path.GetExtension(model.DocumentFile1.FileName);
                            using (var binaryReader = new BinaryReader(model.DocumentFile1.InputStream))
                            {
                                fileData = binaryReader.ReadBytes(model.DocumentFile1.ContentLength);
                            }
                            timeOffDocumentEntity.DocumentFile1Name = model.DocumentFile1Name;
                            timeOffDocumentEntity.DocumentFile1 = fileData;
                            timeOffDocumentEntity.DocumentFile1Ext = extFile1;
                        }
                        if (model.DocumentFile2 != null &&
                                (model.DocumentFile2Action == "Edit" || model.DocumentFile2Action == "New"))                                                        
                        {
                            byte[] fileData = null;
                            var extFile2 = Path.GetExtension(model.DocumentFile2.FileName);
                            using (var binaryReader = new BinaryReader(model.DocumentFile2.InputStream))
                            {
                                fileData = binaryReader.ReadBytes(model.DocumentFile2.ContentLength);
                            }
                            timeOffDocumentEntity.DocumentFile2Name = model.DocumentFile2Name;
                            timeOffDocumentEntity.DocumentFile2 = fileData;
                            timeOffDocumentEntity.DocumentFile2Ext = extFile2;
                        }
                        if (model.DocumentFile3 != null
                            && (model.DocumentFile3Action == "Edit" || model.DocumentFile3Action == "New"))
                        {
                            byte[] fileData = null;
                            var extFile3 = Path.GetExtension(model.DocumentFile3.FileName);
                            using (var binaryReader = new BinaryReader(model.DocumentFile3.InputStream))
                            {
                                fileData = binaryReader.ReadBytes(model.DocumentFile3.ContentLength);
                            }
                            timeOffDocumentEntity.DocumentFile3Name = model.DocumentFile3Name;
                            timeOffDocumentEntity.DocumentFile3 = fileData;
                            timeOffDocumentEntity.DocumentFile3Ext = extFile3;
                        }
                        timeAideWebContext.SaveChanges();
                    }
                    
                }
              
            }
            catch (Exception ex)
            {
                Helpers.ErrorLogHelper.InsertLog(Helpers.ErrorLogType.Error, ex, this.ControllerContext);
                //retResult = new { status = "Error", message = ex.Message };
                status = "Error";
                message = ex.Message;
            }

            return Json(new { status = status, message = message });
        }
    }
}