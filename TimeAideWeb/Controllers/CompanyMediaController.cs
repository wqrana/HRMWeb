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
using TimeAide.Data;

namespace TimeAide.Web.Controllers
{
    public class CompanyMediaController : TimeAideWebBaseControllers
    {
        // GET: CompanyPortal
        private TimeAideContext db = new TimeAideContext();
        // GET: UserDashboard
        public ActionResult Index()
        {
            ViewBag.IsMediaAdmin = CheckCompanyMediaAdmin();
            return PartialView();
        }
        [HttpPost]
        public ActionResult IndexByGridView(UserFilterViewModel filter)
        {
            ViewBag.IsMediaAdmin = CheckCompanyMediaAdmin();
            var mediaList=  db.GetAllByCompany<CompanyMedia>(SessionHelper.SelectedCompanyId, SessionHelper.SelectedClientId)
                 .Where(w => w.Name.ToLower().Contains(filter.SearchText==null?"": filter.SearchText.ToLower()))
                 .OrderByDescending(o=>o.CreatedDate).ToList<CompanyMedia>();
            var mediaApplyPageList = ApplyMediaGridViewPaging(mediaList, filter);
            return PartialView(mediaApplyPageList);
        }
        private List<CompanyMedia> ApplyMediaGridViewPaging(List<CompanyMedia> mediaList, UserFilterViewModel userFilter)
        {
            int totalRecords, pageRecordFrom, pageRecordTo, totalPages = 0;
            int pageSize = userFilter.PageSize ?? 0;
            int pageNo = userFilter.PageNo ?? 0;
            int skipRecords = (pageNo - 1) * pageSize;
            pageRecordFrom = skipRecords + 1;
            totalRecords = mediaList.Count();
            double tempTotalRec = totalRecords * 1.0;
            totalPages = (int)Math.Ceiling((tempTotalRec / pageSize));
            mediaList = mediaList.Skip(skipRecords).Take(pageSize).ToList<CompanyMedia>();
            pageRecordTo = skipRecords + mediaList.Count();
            pageRecordFrom = pageRecordTo > 0 ? pageRecordFrom : 0;
            ViewBag.GridPagingObject = new PagingViewModel
            {
                TotolRecords = totalRecords,
                TotalPages = totalPages,
                StartRecord = pageRecordFrom,
                EndRecord = pageRecordTo,
                CurrentPage = pageNo
            };

            return mediaList;
        }
        public ActionResult AddEditCompanyMedia(int? id)
        {
            CompanyMedia model = null;
            if (id == 0)
            {
                model = new CompanyMedia() { IsAllCompanies=true,MediaType="F" };
            }
            else
            {
                model = db.CompanyMedia.Find(id);
                model.IsAllCompanies = (model.CompanyId == null);
                model.MediaType = string.IsNullOrEmpty(model.MediaType) ? "F" : model.MediaType;
            }
             var mediaList= DataHelper.MediaType.
                                Select(w => new SelectListItem() { Text = w.Value, Value = w.Key.ToString() }).ToList();
            ViewBag.MediaType = new SelectList(mediaList, "Value", "Text", model.MediaType);
            return PartialView(model);
        }
        [HttpPost]
       
        public JsonResult AddEditSaveCompanyMedia()
        {
            string serverFilePath = "";
            string relativeFilePath = "";
            string fullMediaFileName = "";
            string relativePosterFilePath = "";
            string fullMediaPosterName = "";
            string _status = "Error";
            string _message = "Invalid Operation";
            var model = new CompanyMedia();
            var actionType = Request.Form["ActionType"];
            model.Id = int.Parse(Request.Form["Id"]);
            model.Name = Request.Form["Name"];
            model.MediaType = Request.Form["MediaType"];
            model.MediaLink = Request.Form["MediaLink"];
            model.IsDefaultMedia = bool.Parse(Request.Form["IsDefaultMedia"]);
            model.IsAllCompanies = bool.Parse(Request.Form["IsAllCompanies"]);
            
            FilePathHelper filePathHelper = new FilePathHelper();

            
                try
                {                    
                   var companyMediaEntity = new CompanyMedia();
                if (Request.Files.Count > 0 && model.MediaType=="F")
                {
                    HttpPostedFileBase mediaFile = Request.Files[0];
                    HttpPostedFileBase posterFile = Request.Files.Count>1? Request.Files[1]:null;
                    if (actionType == "Poster")
                    {
                        mediaFile = null;
                        posterFile = Request.Files[0];
                    }
                    if (mediaFile != null)
                    {
                        fullMediaFileName = DateTime.Now.ToString("yyyyMMddHHmmss") + "_" + mediaFile.FileName;
                        serverFilePath = filePathHelper.GetPath("Media", fullMediaFileName, true);
                        mediaFile.SaveAs(serverFilePath);
                        relativeFilePath = filePathHelper.RelativePath;
                    }
                    if (posterFile != null)
                    {
                        fullMediaPosterName= DateTime.Now.ToString("yyyyMMddHHmmss") + "_MediaPoster_" + posterFile.FileName;
                        var posterFileServerPath = filePathHelper.GetPath("Media", fullMediaPosterName, true);
                        posterFile.SaveAs(posterFileServerPath);
                        relativePosterFilePath = filePathHelper.RelativePath;
                    }
                }
                    var companyId = model.IsAllCompanies == false ? model.CompanyId : null;
                    if (model.Id == 0)
                    {
                        db.CompanyMedia.Add(companyMediaEntity);
                    }
                    else
                    {
                        companyMediaEntity = db.CompanyMedia.Find(model.Id);
                        if (actionType == "None" && model.MediaType=="F")
                        {
                        relativeFilePath = companyMediaEntity.MediaFilePath;
                        fullMediaFileName = companyMediaEntity.FileName;
                        relativePosterFilePath = companyMediaEntity.PosterFilePath;
                        fullMediaPosterName = companyMediaEntity.PosterFileName;
                        }
                        else if (actionType == "Poster" && model.MediaType == "F")
                        {
                        relativeFilePath = companyMediaEntity.MediaFilePath;
                        fullMediaFileName = companyMediaEntity.FileName;
                        }
                    }
                if (model.MediaType == "L")
                {
                    relativeFilePath = model.MediaLink;
                }
                companyMediaEntity.Name = model.Name;
                companyMediaEntity.IsDefaultMedia = model.IsDefaultMedia;
                companyMediaEntity.MediaType = model.MediaType;
                companyMediaEntity.MediaFilePath = relativeFilePath;
                companyMediaEntity.FileName = fullMediaFileName;
                companyMediaEntity.PosterFilePath = relativePosterFilePath;
                companyMediaEntity.PosterFileName = fullMediaPosterName;
                companyMediaEntity.CompanyId = companyId;
                db.SaveChanges();
                if (model.IsDefaultMedia == true)
                {
                    var defaultMediaList = db.GetAllByCompany<CompanyMedia>(SessionHelper.SelectedCompanyId, SessionHelper.SelectedClientId)
                                                .Where(w => w.Id != companyMediaEntity.Id && w.IsDefaultMedia == true);
                    foreach(var mediaEntity in defaultMediaList)
                    {
                        mediaEntity.IsDefaultMedia = false;
                        mediaEntity.ModifiedBy = SessionHelper.LoginId;
                        mediaEntity.ModifiedDate = DateTime.Now;
                    }
                    db.SaveChanges();
                 }
                 _status = "Success";
                 _message = "Media is added/Edited successfully!";
                    
                    
                }
                catch (Exception ex)
                {
                    Helpers.ErrorLogHelper.InsertLog(Helpers.ErrorLogType.Error, ex, this.ControllerContext);
                    _status = "Error";
                    _message = ex.Message;

                }
           
            return Json(new { status = _status, message = _message });
        }
        
        public ActionResult DeleteCompanyMedia(int id)
        {
            var model = db.CompanyMedia.Find(id);
            return PartialView(model);
        }
        [HttpPost]
        public JsonResult ConfirmDelete(int id)
        {
            string status = "Success";
            string message = "Media record is Successfully Deleted!";
            string relativeFilePath = "", serverFilePath = "";
            FileInfo deleteMediaFile = null;
            var CompanyMediaEntity = db.CompanyMedia.Find(id);
            try
            {
                
                CompanyMediaEntity.DataEntryStatus = 0;
                CompanyMediaEntity.ModifiedBy = SessionHelper.LoginId;
                CompanyMediaEntity.ModifiedDate = DateTime.Now;

                db.SaveChanges();

                if (CompanyMediaEntity.MediaType == "F")
                {
                    relativeFilePath = CompanyMediaEntity.MediaFilePath;
                    var tempPath = "~" + relativeFilePath;
                    serverFilePath = Server.MapPath(tempPath);
                    deleteMediaFile = new FileInfo(serverFilePath);
                    deleteMediaFile.Delete();
                }
               
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
        public JsonResult AjaxDeleteMediaFile(int id)
        {
            string status = "Success";
            string message = "Media File is Successfully Deleted!";
            string relativeFilePath = "", serverFilePath = "";
            FileInfo deleteMediaFile = null;
            var CompanyMediaEntity = db.CompanyMedia.Find(id);
            try
            {
                relativeFilePath = CompanyMediaEntity.MediaFilePath;
                var tempPath = "~" + relativeFilePath;
                serverFilePath = Server.MapPath(tempPath);
                deleteMediaFile = new FileInfo(serverFilePath);

                CompanyMediaEntity.MediaFilePath = null;
                CompanyMediaEntity.FileName = null;
                CompanyMediaEntity.ModifiedBy = SessionHelper.LoginId;
                CompanyMediaEntity.ModifiedDate = DateTime.Now;
                
                db.SaveChanges();
                
                deleteMediaFile.Delete();
            }
            catch (Exception ex)
            {
                Helpers.ErrorLogHelper.InsertLog(Helpers.ErrorLogType.Error, ex, this.ControllerContext);
                status = "Error";
                message = ex.Message;
            }
            return Json(new { status = status, message = message });

        }
        private bool CheckCompanyMediaAdmin()
        {
            var isMediaAdmin = db.UserEmployeeGroup.Where(w => w.UserInformationId == SessionHelper.LoginId && w.EmployeeGroup.EmployeeGroupTypeId == 8).Count();

            return isMediaAdmin > 0 ? true : false;
        }

    }
}