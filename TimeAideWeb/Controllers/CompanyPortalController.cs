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
using System.Web.Script.Serialization;
namespace TimeAide.Web.Controllers
{
    public class CompanyPortalController : TimeAideWebBaseControllers
    {
        // GET: CompanyPortal
        private TimeAideContext db = new TimeAideContext();
        // GET: UserDashboard
        public ActionResult Index()
        {
            var model = new CompanyPortalViewModel();
            model.UserId = SessionHelper.LoginId;
            var companyInfo = db.Company.Find(SessionHelper.SelectedCompanyId);
            var defaultPortalPictureCMP = db.GetAll<Company>(SessionHelper.SelectedClientId).
                                            Where(w => w.IsDefaultPortalPicture == true).FirstOrDefault();
            var defaultPortalStatementCMP = db.GetAll<Company>(SessionHelper.SelectedClientId).
                                            Where(w => w.IsDefaultPortalStatement == true).FirstOrDefault();
       
            model.CompanyId = companyInfo.Id;
            model.CompanyName = companyInfo.CompanyName;
            model.CompanyProfilePicture = defaultPortalPictureCMP==null? companyInfo.PortalPictureFilePath: defaultPortalPictureCMP.PortalPictureFilePath;
            model.CompanyProfilePicture = string.IsNullOrEmpty(model.CompanyProfilePicture) ? "/Images/no-profile-image.jpg" : model.CompanyProfilePicture;
            model.CompanyProfilePicture += "?timestamp=" + DateTime.Now.ToLongTimeString();
            model.IsDefaultCompanyProfilePicture = defaultPortalPictureCMP == null ? false : defaultPortalPictureCMP.IsDefaultPortalPicture ?? false;
            model.DefaultProfilePictureCompanyId = defaultPortalPictureCMP == null ? null : (int?)defaultPortalPictureCMP.Id;

            model.PortalWelcomeStatement = defaultPortalStatementCMP == null ?
                                           companyInfo.PortalWelcomeStatement == null ? "Welcome to the Company Portal!" : companyInfo.PortalWelcomeStatement
                                           : defaultPortalStatementCMP.PortalWelcomeStatement == null ? "Welcome to the Company Portal!" : defaultPortalStatementCMP.PortalWelcomeStatement;
            model.IsDefaultCompanyPortalStatement = defaultPortalStatementCMP == null ? false : defaultPortalStatementCMP.IsDefaultPortalStatement ?? false;
            model.DefaultPortalStatementCompanyId = defaultPortalStatementCMP == null ? null: (int?)defaultPortalStatementCMP.Id;

            var defaultMedia = db.GetAllByCompany<CompanyMedia>(SessionHelper.SelectedCompanyId, SessionHelper.SelectedClientId).
                                      Where(w => w.IsDefaultMedia == true).Select(s => new { MediaPath = s.MediaFilePath, MediaTitle= s.Name,MediaType=s.MediaType ,MediaPoster=s.PosterFilePath }).FirstOrDefault();
            if (defaultMedia != null) {
                model.CompanyMediaType = defaultMedia.MediaType;
                model.CompanyDefaultMediaPath = defaultMedia.MediaPath;
                model.CompanyDefaultMediaTitle = defaultMedia.MediaTitle;
                model.CompanyDefaultMediaPosterPath = defaultMedia.MediaPoster;
                    }
            model.IsPortalAdmin = CheckCompanyPortalAdmin();
           
            //var connStr = ClientDBConnectionHelper.GetClientConnection();
            //var efConnStr= DataHelper.ConvertToEFConnectionString(connStr);
            //if (connStr != "")
            //{
            //    var dbContext = new TimeAideWindowContext(efConnStr);
            //    var compList= dbContext.tCompanies.ToList();
            //}
            //var timeAideWindowContext = DataHelper.GetSelectedClientTAWinEFContext();
            //if (timeAideWindowContext != null)
            //{
            //    var tuser = timeAideWindowContext.tusers.ToList();
            //}
            
            return View(model);
        }
        [HttpPost]
        public JsonResult AjaxUploadCompanyPortalPicture()
        {
            string status = "Success";
            string message = "Successfully Updated!";
            string retPath = "";
            dynamic retResult = null;
            string serverFilePath = "";
            string relativeFilePath = "";
            var companyInfo = db.Company.Find(SessionHelper.SelectedCompanyId);
            Company updateDefaultCompany = null;
            
            var isDefaultProfile = bool.Parse(Request.Form["IsDefaultProfile"]);
            int defaultCompanyId;
            int.TryParse(Request.Form["DefaultCompanyId"],out defaultCompanyId);
            if(defaultCompanyId>0 && isDefaultProfile==true)
            {
                companyInfo = db.Company.Find(defaultCompanyId);
            }
            else if(defaultCompanyId > 0 && isDefaultProfile == false)
            {
                updateDefaultCompany = db.Company.Find(defaultCompanyId);
                updateDefaultCompany.IsDefaultPortalPicture = false; 
            }
            else if(defaultCompanyId == 0 && isDefaultProfile == true)
            {
                companyInfo.IsDefaultPortalPicture = true;
            }
            //var profilePictureName = companyInfo.Id + "_" + companyInfo.CompanyName + '.' + "jpg";
            //Call function to remove special char in company name.
            var profilePictureName = companyInfo.Id + "_" + FilePathHelper.RemoveSpecialCharacters(companyInfo.CompanyName) + '.' + "jpg";
            //FilePathHelper.RemoveSpecialCharacters(
            FilePathHelper filePathHelper = new FilePathHelper(companyInfo.Id);
            serverFilePath = filePathHelper.GetPath("CompanyPortal\\Images", profilePictureName);
          
            FileInfo checkFile = null;
            if (Request.Files.Count > 0)
            {
                try
                {
                    HttpPostedFileBase pictureFile = Request.Files[0];

                    if (companyInfo != null)
                    {
                        pictureFile.SaveAs(serverFilePath);
                        relativeFilePath = filePathHelper.RelativePath;
                        companyInfo.PortalPictureFilePath = relativeFilePath;                        
                        retPath = relativeFilePath ;
                        db.SaveChanges();
                    }
                    else
                    {
                        status = "Error";
                        message = "Invalid record data";
                        
                    }
                   
                }
                catch (Exception ex)
                {
                    Helpers.ErrorLogHelper.InsertLog(Helpers.ErrorLogType.Error, ex, this.ControllerContext);
                    status = "Error";
                    message = ex.Message;
                }
            }
            else
            {                
                db.SaveChanges();
                retPath = companyInfo.PortalPictureFilePath;
            }
            return Json(new { status = status, message = message, picturePath = retPath });
        }
        [HttpPost]
        public JsonResult AjaxDeleteCompanyPortalPicture(int? defaultCompanyId)
        {
            string status = "Success";
            string message = "Successfully Deleted!";
            string retPath = "";
            dynamic retResult = null;
            int id = defaultCompanyId == null ? SessionHelper.SelectedCompanyId : defaultCompanyId.Value;
            var companyInfo = db.Company.Find(id);           
            
                try
                {
                  
                    if (companyInfo != null)
                    {
                                             
                        companyInfo.PortalPictureFilePath = "";
                        retPath = "/Images/no-profile-image.jpg";// no image
                        db.SaveChanges();
                    }
                    else
                    {
                        status = "Error";
                        message = "Invalid record data";

                    }

                }
                catch (Exception ex)
                {
                    Helpers.ErrorLogHelper.InsertLog(Helpers.ErrorLogType.Error, ex, this.ControllerContext);
                    status = "Error";
                    message = ex.Message;
                }
                        
            return Json(new { status = status, message = message, picturePath = retPath });
        }

        [HttpPost]
        public JsonResult EditCompanyPortalStatement(CompanyPortalViewModel model)
        {
            string status = "Success";
            string message = "Successfully Updated!";

            try
            {
                Company updateDefaultCompany = null;
                var companyEntity = db.Company.Find(SessionHelper.SelectedCompanyId);
                int defaultCompanyId = model.DefaultPortalStatementCompanyId??0;
                bool isDefaultStatement = model.IsDefaultCompanyPortalStatement;
                if (defaultCompanyId > 0 && isDefaultStatement == true)
                {
                    companyEntity = db.Company.Find(defaultCompanyId);
                }
                else if (defaultCompanyId > 0 && isDefaultStatement == false)
                {
                    updateDefaultCompany = db.Company.Find(defaultCompanyId);
                    updateDefaultCompany.IsDefaultPortalStatement = false;
                }
                else if (defaultCompanyId == 0 && isDefaultStatement == true)
                {
                    companyEntity.IsDefaultPortalStatement = true;
                }
                companyEntity.PortalWelcomeStatement = model.PortalWelcomeStatement;
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

        public ActionResult CompanyAnnouncementList()
        {
            var isPortalAdmin = CheckCompanyPortalAdmin();

            var model = db.GetAllByCompany<CompanyAnnouncement>(SessionHelper.SelectedCompanyId, SessionHelper.SelectedClientId)
                        .Where(w => w.DataEntryStatus == 1 && (isPortalAdmin ? true : (w.StartDate <= DateTime.Today && w.ExpirationDate >= DateTime.Today)))
                        .OrderByDescending(o => o.StartDate)
                        .Select(s => new CompanyAnnouncementViewModel
                        {
                            CompanyAnnouncementId = s.Id,
                            AnnouncementName = s.Name,
                            AnnouncementMessage = s.Message,
                            AnnouncementStatus = getCompanyAnnouncementStatus(s.StartDate, s.ExpirationDate)
                        });


            ViewBag.IsPortalAdmin = isPortalAdmin;

            return PartialView(model);
        }

        public JsonResult CreateEditCompanyAnnouncement(int id)
        {
            CompanyAnnouncementViewModel viewModel = new CompanyAnnouncementViewModel();
            var model = db.CompanyAnnouncement.Find(id);
            if (model != null)
            {
                viewModel.CompanyAnnouncementId = model.Id;
                viewModel.AnnouncementName = model.Name;
                viewModel.AnnouncementMessage = model.Message;
                viewModel.StartDate = model.StartDate;
                viewModel.ExpirationDate = model.ExpirationDate;
                viewModel.IsAllCompanies = model.CompanyId == null ? true : false;
            }
            return Json(viewModel, JsonRequestBehavior.AllowGet);
        }
        [HttpPost]
        public JsonResult CreateEditCompanyAnnouncement(CompanyAnnouncementViewModel model)
        {
            string status = "Success";
            string message = "Successfully Added/Updated!";
            CompanyAnnouncement companyAnnouncementEntity = null;
            try
            {
                if (model.StartDate <= model.ExpirationDate)
                {
                    if (model.CompanyAnnouncementId == null)
                    {
                        companyAnnouncementEntity = new CompanyAnnouncement();
                        db.CompanyAnnouncement.Add(companyAnnouncementEntity);
                    }
                    else
                    {
                        companyAnnouncementEntity = db.CompanyAnnouncement.Find(model.CompanyAnnouncementId);
                        companyAnnouncementEntity.ModifiedBy = SessionHelper.LoginId;
                        companyAnnouncementEntity.ModifiedDate = DateTime.Now;
                    }

                    companyAnnouncementEntity.Name = model.AnnouncementName;
                    companyAnnouncementEntity.Message = model.AnnouncementMessage;
                    companyAnnouncementEntity.StartDate = model.StartDate.Value;
                    companyAnnouncementEntity.ExpirationDate = model.ExpirationDate.Value;
                    companyAnnouncementEntity.CompanyId = model.IsAllCompanies ? null : (int?)SessionHelper.SelectedCompanyId;

                    db.SaveChanges();
                }
                else
                {
                    status = "Error";
                    message = "Expiration date shouldn't prior to start date";
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

        public ActionResult ViewCompanyAnnouncement(int id)
        {
            var model = db.CompanyAnnouncement.Find(id);
            var viewModel = new CompanyAnnouncementViewModel() { AnnouncementName = model.Name, AnnouncementMessage = model.Message };
            return PartialView(viewModel);
        }

        public ActionResult DeleteCompanyAnnouncement(int id)
        {
            var model = db.CompanyAnnouncement.Find(id);
            var viewModel = new CompanyAnnouncementViewModel() { CompanyAnnouncementId = model.Id, AnnouncementName = model.Name };
            return PartialView(viewModel);
        }
        [HttpPost]
        public JsonResult ConfirmDeleteCompanyAnnouncement(int id)
        {
            string status = "Success";
            string message = "Successfully Deleted!";
            var companyAnnouncementEntity = db.CompanyAnnouncement.Find(id);
            try
            {
                companyAnnouncementEntity.ModifiedBy = SessionHelper.LoginId;
                companyAnnouncementEntity.ModifiedDate = DateTime.Now;
                companyAnnouncementEntity.DataEntryStatus = 0;
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

        private bool CheckCompanyPortalAdmin()
        {
            var isPortalAdmin = db.UserEmployeeGroup.Where(w => w.UserInformationId == SessionHelper.LoginId && w.EmployeeGroup.EmployeeGroupTypeId == 5).Count();

            return isPortalAdmin > 0 ? true : false;
        }

        private string getCompanyAnnouncementStatus(DateTime startDate, DateTime expiryDate)
        {
            string status = "Active";
            DateTime currentDate = DateTime.Today;
            if (currentDate < startDate)
            {
                status = "Pending";
            }
            else if (currentDate > expiryDate)
            {
                status = "Expired";
            }
            return status;
        }
        public JsonResult AddEditCompanyDocument()
        {           
            string serverFilePath = "";
            string relativeFilePath = "";
            string _status = "Error";
            string _message = "Invalid Operation";
            string documentName = Request.Form["DocumentName"];
            bool IsAllCompanies;
            bool.TryParse(Request.Form["IsAllCompanies"], out IsAllCompanies);
            int companyId = IsAllCompanies ? 0 : SessionHelper.SelectedCompanyId;
            FilePathHelper filePathHelper = new FilePathHelper(companyId);     
                      
            if (Request.Files.Count > 0 && documentName!="")
            {
                try
                {
                  var isExist=db.GetAllByCompany<CompanyDocument>(SessionHelper.SelectedCompanyId,SessionHelper.SelectedClientId).
                                Where(w => w.DataEntryStatus == 1 && w.DocumentName.ToLower() == documentName.ToLower()).Count();

                    if (isExist == 0)
                    {
                        var companyDocumentEntity = new CompanyDocument();
                        HttpPostedFileBase documentFile = Request.Files[0];
                        var fullDOcumentName =  documentName + Path.GetExtension(documentFile.FileName);
                        serverFilePath = filePathHelper.GetPath("CompanyPortal\\CompanyDocument", fullDOcumentName);                  
                        
                        documentFile.SaveAs(serverFilePath);
                        relativeFilePath = filePathHelper.RelativePath;
                        companyDocumentEntity.DocumentName = documentName;
                        companyDocumentEntity.DocumentFilePath = relativeFilePath;
                        companyDocumentEntity.CompanyId = IsAllCompanies ? null :(int?) SessionHelper.SelectedCompanyId;
                        db.CompanyDocument.Add(companyDocumentEntity);
                        db.SaveChanges();
                        _status = "Success";
                        _message = "Document is added successfully!";
                    }
                    else
                    {
                        _status = "Error";
                        _message = "Document Name is already added. Please enter another Name.";
                    }

                }
                catch (Exception ex)
                {
                    Helpers.ErrorLogHelper.InsertLog(Helpers.ErrorLogType.Error, ex, this.ControllerContext);
                    _status = "Error";
                    _message = ex.Message;
                   
                }
            }
            else
            {
                _status = "Error";
                _message = "Missing Required field(s)";
            }
            return Json(new { status = _status, message = _message });
        }
        public ActionResult DownloadCompanyDocument(int id)
        {
            string relativeFilePath = "", serverFilePath = "";
            FileInfo downloadDocFile = null;
            byte[] fileBytes;
            var companyDocument = db.CompanyDocument.Find(id);
            if (companyDocument != null)
            {
                if (!string.IsNullOrEmpty(companyDocument.DocumentFilePath))
                {
                    relativeFilePath = companyDocument.DocumentFilePath;
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
        public ActionResult DeleteCompanyDocument(int id)
        {
            var model = db.CompanyDocument.Find(id);         
            return PartialView(model);
        }
        [HttpPost]
        public JsonResult ConfirmDeleteCompanyDocument(int id)
        {
            string status = "Success";
            string message = "Successfully Deleted!";
            string relativeFilePath = "", serverFilePath = "";
            FileInfo deleteDocFile = null;
            var companyDocumentEntity = db.CompanyDocument.Find(id);
            try
            {
                relativeFilePath = companyDocumentEntity.DocumentFilePath;
                var tempPath = "~" + relativeFilePath;
                serverFilePath = Server.MapPath(tempPath);
                deleteDocFile = new FileInfo(serverFilePath);
              

                companyDocumentEntity.ModifiedBy = SessionHelper.LoginId;
                companyDocumentEntity.ModifiedDate = DateTime.Now;
                companyDocumentEntity.DataEntryStatus = 0;
                db.SaveChanges();
                deleteDocFile.Delete();
            }
            catch (Exception ex)
            {
                Helpers.ErrorLogHelper.InsertLog(Helpers.ErrorLogType.Error, ex, this.ControllerContext);
                status = "Error";
                message = ex.Message;
            }
            return Json(new { status = status, message = message });

        }

        public ActionResult CompanyDocumentList()
        {
            var isPortalAdmin = CheckCompanyPortalAdmin();

            var model = db.GetAllByCompany<CompanyDocument>(SessionHelper.SelectedCompanyId, SessionHelper.SelectedClientId)
                        .Where(w => w.DataEntryStatus == 1 )
                        .OrderByDescending(o => o.CreatedDate);

            ViewBag.IsPortalAdmin = isPortalAdmin;

            return PartialView(model);
        }
        public ActionResult AddEditCompanyConfigurableLink(int id){
           
            var model = db.CompanyConfigurableLink.Find(id);
            
            return PartialView(model);
        }
        [HttpPost]
        public ActionResult AddEditCompanyConfigurableLink(CompanyConfigurableLink model)
        {
            string status = "Success";
            string message = "Successfully Added/Updated!";
            CompanyConfigurableLink companyConfigurableLinkEntity = null;
            try
            {               
                if (model.Id == 0){
                        companyConfigurableLinkEntity = new CompanyConfigurableLink();
                        db.CompanyConfigurableLink.Add(companyConfigurableLinkEntity);
                 }
                 else{
                        companyConfigurableLinkEntity = db.CompanyConfigurableLink.Find(model.Id);
                        companyConfigurableLinkEntity.ModifiedBy = SessionHelper.LoginId;
                        companyConfigurableLinkEntity.ModifiedDate = DateTime.Now;
                  }

                    companyConfigurableLinkEntity.LinkName = model.LinkName;
                    companyConfigurableLinkEntity.LinkURL = model.LinkURL;
                    companyConfigurableLinkEntity.CompanyId = model.CompanyId;
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
        public ActionResult CompanyConfigurableLinkList()
        {
            var isPortalAdmin = CheckCompanyPortalAdmin();

            var model = db.GetAllByCompany<CompanyConfigurableLink>(SessionHelper.SelectedCompanyId, SessionHelper.SelectedClientId)
                        .Where(w => w.DataEntryStatus == 1)
                        .OrderByDescending(o => o.CreatedDate);

            ViewBag.IsPortalAdmin = isPortalAdmin;

            return PartialView(model);
        }

        public ActionResult DeleteCompanyConfigurableLink(int id)
        {
            var model = db.CompanyConfigurableLink.Find(id);
            return PartialView(model);
        }
        [HttpPost]
        public JsonResult ConfirmDeleteCompanyConfigurableLink(int id)
        {
            string status = "Success";
            string message = "Successfully Deleted!";
            var configurableLinkEntity = db.CompanyConfigurableLink.Find(id);
            try
            {
                configurableLinkEntity.ModifiedBy = SessionHelper.LoginId;
                configurableLinkEntity.ModifiedDate = DateTime.Now;
                configurableLinkEntity.DataEntryStatus = 0;
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
       
    }
}