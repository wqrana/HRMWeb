
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
    public class TAWinTimeOffDocumentController : BaseTAWindowRoleRightsController<TAWindowTimeOffTransaction>
    {
    
        // GET: TAWinTimeOffDocument
        public ActionResult Index()
        {
            var model = getTransDefTimeOff();
            return PartialView(model);
        }
        public ActionResult IndexByDocumentList(int? transId)
        {
            ViewBag.TransId = 0;
            ViewBag.TransName = "";
            List<TAWinTimeOffDocument> model = null;
            var transDef = timeAideWindowContext.tTransDef.Find(transId);
            model = timeAideWindowContext.tblSS_TransdefTimeOffDocument
                                 .Where(w => w.intTransId == transId).
                                 Select(s => new TAWinTimeOffDocument() { Id = s.Id ,sDocumentName = s.sDocumentName, intDocumentType = s.intDocumentType ?? 0, intSubmissionType = s.intSubmissionType ?? 0, intTimeOffDays = s.intTimeOffDays ?? 0 })
                                 .ToList();
            //if (model == null) {
            //    model = new List<TAWinTimeOffDocument>();
            //   }
            if (transDef != null)
            {
                ViewBag.TransId = transDef.ID;
                ViewBag.TransName = transDef.Name;
            }
           return PartialView(model);
        }
          [HttpPost]
        public JsonResult CreateEditTransDefTimeOff(TAWinTransDefTimeOff model)
        {
            string status = "Success";
            string message = "Successfully Updated!";
            
                try
                {
                    if (model.IsSickInFamilyTrans)
                    {
                        if ((model.MinimumAccrualTypeBalance??0) <= 0)
                        {
                            throw new Exception("Minimum Accrual Type Balance should be greater than 0");
                        }
                        if ((model.MaximumYearlyTaken??0) <= 0)
                        {
                            throw new Exception("Maximum Yearly Taken should be greater than 0");
                        }
                    }
                    var transDefTimeOffEntity=timeAideWindowContext.tblSS_TransdefTimeOffRequest
                                          .Where(w => w.strTransName == model.TransName)
                                          .FirstOrDefault();
                    var transDefEntity = timeAideWindowContext.tTransDef.Where(w=>w.Name == model.TransName).FirstOrDefault();
                    if (transDefEntity != null)
                    {
                        if (transDefTimeOffEntity == null)
                        {
                            transDefTimeOffEntity = new tblSS_TransdefTimeOffRequest()
                            {
                                strTransName = model.TransName,
                                intTimeOffRequest = model.IsTimeOffTrans ? -1 : 0,

                            };
                            timeAideWindowContext.tblSS_TransdefTimeOffRequest.Add(transDefTimeOffEntity);
                        }
                        else
                        {
                            transDefTimeOffEntity.intTimeOffRequest = model.IsTimeOffTrans ? -1 : 0;
                        }
                        transDefEntity.boolUseSickInFamily = model.IsSickInFamilyTrans;
                        transDefEntity.decMinimumAccrualTypeBalance = model.MinimumAccrualTypeBalance;
                        transDefEntity.decMaximumYearlyTaken = model.MaximumYearlyTaken;
                       
                        timeAideWindowContext.SaveChanges();
                    }
                }
                catch (Exception ex)
                {
                    status = "Error";
                    message = ex.Message;
                }

            return Json(new { status = status, message = message });
        }         

            
      
        private List<TAWinTransDefTimeOff> getTransDefTimeOff()
        {

            var transDefTimeOffList = timeAideWindowContext.spSS_TransDefTimeOff()
                .Select(s => new TAWinTransDefTimeOff
                {
                    TransId = s.TransId,
                    TransName = s.TransName,
                    Description = s.Description,
                    AccrualType = s.AccrualType,
                    IsTimeOffTrans = s.IsTimeOffTrans == 0 ? false : true,
                    IsSickInFamilyTrans = s.boolUseSickInFamily,
                    MinimumAccrualTypeBalance = s.decMinimumAccrualTypeBalance,
                    MaximumYearlyTaken = s.decMaximumYearlyTaken
                    
                })
                .OrderByDescending(o => o.IsTimeOffTrans).ToList<TAWinTransDefTimeOff>();
            return transDefTimeOffList;
            
        }

        public ActionResult CreateOrEditTimeOffDocument(int id)
        {
            TAWinTimeOffDocument model = null;
            if (id == 0)
            {
                model = new TAWinTimeOffDocument();
            }
            else
            {
                model = timeAideWindowContext.tblSS_TransdefTimeOffDocument
                                 .Where(w => w.Id == id).
                                 Select(s => new TAWinTimeOffDocument() { Id = s.Id, sDocumentName = s.sDocumentName, intDocumentType = s.intDocumentType ?? 0, intSubmissionType = s.intSubmissionType ?? 0, intTimeOffDays = s.intTimeOffDays ?? 0 })
                                 .FirstOrDefault();
            }

            return PartialView(model);
        }
        [HttpPost]
        public JsonResult CreateOrEditTimeOffDocument(TAWinTimeOffDocument model)
        {
            string status = "Success";
            string message = "Successfully Updated!";
            try
            {
                var transDefTimeOffDocEntity = timeAideWindowContext.tblSS_TransdefTimeOffDocument
                                      .Where(w => w.Id == model.Id)
                                      .FirstOrDefault();
                if (transDefTimeOffDocEntity == null)
                {
                    transDefTimeOffDocEntity = new tblSS_TransdefTimeOffDocument();                   
                    timeAideWindowContext.tblSS_TransdefTimeOffDocument.Add(transDefTimeOffDocEntity);
                }
                transDefTimeOffDocEntity.intTransId = model.intTransId;
                transDefTimeOffDocEntity.sDocumentName = model.sDocumentName;
                transDefTimeOffDocEntity.intDocumentType = model.intDocumentType;
                transDefTimeOffDocEntity.intSubmissionType = model.intSubmissionType;
                transDefTimeOffDocEntity.intTimeOffDays = model.intTimeOffDays;

                timeAideWindowContext.SaveChanges();
            }
            catch (Exception ex)
            {
                status = "Error";
                message = ex.Message;
            }

            return Json(new { status = status, message = message });
        }
        public ActionResult Delete(int id)
        {
            try
            {
                AllowDelete();
                var entity = timeAideWindowContext.tblSS_TransdefTimeOffDocument.Find(id);
                var model = new TAWinTimeOffDocument() { Id = entity.Id, sDocumentName = entity.sDocumentName };
                return PartialView(model);
            }
            catch (AuthorizationException ex)
            {
                Exception exception = new Exception(ex.ErrorMessage);
                HandleErrorInfo handleErrorInfo = new HandleErrorInfo(exception, "TAWinTimeOffDocument", "Delete");
                return View("~/Views/Shared/Error.cshtml", handleErrorInfo);
            }
        }

        [HttpPost]
        public ActionResult ConfirmDelete(int id)
        {           
         string status = "Success";
         string message = "Successfully Deleted!";
         var timeOffDocEntity = timeAideWindowContext.tblSS_TransdefTimeOffDocument.Find(id); ;
         try
            {
               timeAideWindowContext.tblSS_TransdefTimeOffDocument.Remove(timeOffDocEntity);
               timeAideWindowContext.SaveChanges();                       
           }
           catch (Exception ex)
             {                       
                Helpers.ErrorLogHelper.InsertLog(Helpers.ErrorLogType.Error, ex, this.ControllerContext);
                status = "Error";
                message = ex.Message;
              }
                
            return Json(new { status = status, message = message });
          }
        }
}