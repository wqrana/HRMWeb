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
using TimeAide.Models.ViewModel;
using TimeAide.Services.Helpers;
using TimeAide.Web.Models;

namespace TimeAide.Web.Controllers
{
    public class ChatConversationController : TimeAideWebControllers<ChatConversation>
    {

        public ActionResult ChatConversation(int id)
        {
            ChatConversation model = db.ChatConversation.FirstOrDefault(c => c.Id == id);
            return View(model);
        }

        [HttpPost]
        public ActionResult ChatConversation(ChatConversation conversation)
        {
            var participant = db.ChatConversationParticipant.FirstOrDefault(c => c.ChatConversationId == conversation.Id && c.ParticipantId == SessionHelper.LoginId);
            var message = new ChatMessage();
            message.ChatConversationParticipantId = participant.Id;
            message.ChatConversationId = conversation.Id;
            message.ChatMessageText = conversation.ChatMessageText;

            //TimeAide.Services.Helpers.UtilityHelper.SendEmail(toEmails, "", "", null, message, "Notification alert on " + DateTime.Now.ToString("dd-MMM-yyyy"));

            db.ChatMessage.Add(message);
            if (participant != null)
                participant.LastMessageReadTime = DateTime.Now;
            db.SaveChanges();

            return Json("success");
        }
        public ActionResult HideConversation(ChatConversation conversation)
        {
            var participant = db.ChatConversationParticipant.FirstOrDefault(c => c.ChatConversationId == conversation.Id && c.ParticipantId == SessionHelper.LoginId);
            participant.DataEntryStatus = 0;
            participant.LastMessageReadTime = DateTime.Now;
            db.SaveChanges();

            return Json("success");
        }

        [HttpPost]
        public JsonResult UploadDocument()
        {
            if (Request.Files.Count > 0)
            {
                try
                {


                    //TimeAide.Services.Helpers.UtilityHelper.SendEmail(toEmails, "", "", null, message, "Notification alert on " + DateTime.Now.ToString("dd-MMM-yyyy"));



                    HttpPostedFileBase chatFile = Request.Files[0];
                    int conversationId = Convert.ToInt32(Request.Form["eduDocRecordID"]);

                    var participant = db.ChatConversationParticipant.FirstOrDefault(c => c.ChatConversationId == conversationId && c.ParticipantId == SessionHelper.LoginId);


                    var message = new ChatMessage();
                    message.ChatMessageText = "";
                    message.ChatConversationParticipantId = participant.Id;
                    message.ChatConversationId = conversationId;
                    db.ChatMessage.Add(message);
                    db.SaveChanges();
                    var fileName = Path.GetFileName(chatFile.FileName);
                    var fileExt = Path.GetExtension(chatFile.FileName);
                    string docName = message.Id + "-" + fileName;

                    FilePathHelper filePathHelper = new FilePathHelper();
                    string serverFilePath = filePathHelper.GetPath("ChatDocuments\\ChatConversation_" + participant.ChatConversationId + "\\" + "Participant_" + participant.ParticipantId + "\\", docName);
                    chatFile.SaveAs(serverFilePath);
                    message.ChatDocumentFilePath = filePathHelper.RelativePath;


                    if (participant != null)
                        participant.LastMessageReadTime = DateTime.Now;
                    db.SaveChanges();
                }
                catch (Exception ex)
                {
                    Helpers.ErrorLogHelper.InsertLog(Helpers.ErrorLogType.Error, ex, this.ControllerContext);
                    //retResult = new { status = "Error", message = ex.Message };
                    //status = "Error";
                    //message = ex.Message;
                }
            }
            return Json(new { status = "", message = "success" });
        }

        public ActionResult DownloadDocument(int id)
        {
            string relativeFilePath = "", serverFilePath = "";
            FileInfo downloadCVFile = null;
            byte[] fileBytes;
            var user = db.ChatMessage.Find(id);
            if (user != null)
            {
                if (!string.IsNullOrEmpty(user.ChatDocumentFilePath))
                {
                    relativeFilePath = user.ChatDocumentFilePath;
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

        [HttpPost]
        public JsonResult AjaxCheckDocument(int userID)
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

                    var user = db.ChatMessage.Find(userID);
                    if (user != null)
                    {
                        if (!string.IsNullOrEmpty(user.ChatDocumentFilePath))
                        {
                            relativeFilePath = user.ChatDocumentFilePath;
                            var tempPath = "~" + relativeFilePath;
                            serverFilePath = Server.MapPath(tempPath);
                            downloadCVFile = new FileInfo(serverFilePath);
                            if (!downloadCVFile.Exists)
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
        public ActionResult Chat()
        {
            return View();
        }

        public ActionResult ChatWindow(int? id)
        {
            IEnumerable<SelectListItem> notificationList = null;
            notificationList = db.SP_UserInformation<UserInformationViewModel>(0, "", 0,
                                         1, SessionHelper.LoginId, SessionHelper.SelectedClientId, SessionHelper.SelectedCompanyId).Select(s => new SelectListItem
                                         {
                                             Text = s.ShortFullName,
                                             Value = s.Id.ToString()
                                         });
            if (SecurityHelper.IsSupervisor)
            {
                var employeeGroupUsers = db.GetAllByCompany<UserEmployeeGroup>(SessionHelper.SelectedCompanyId, SessionHelper.SelectedClientId)
                                           .Where(e => e.EmployeeGroup.EmployeeGroupTypeId == 3 || e.EmployeeGroup.EmployeeGroupTypeId == 4).Select(e => e.UserInformation).Distinct().ToList()
                                           .Select(s => new SelectListItem
                                           {
                                               Text = s.ShortFullName,
                                               Value = s.Id.ToString()
                                           });

                notificationList = notificationList.Concat(employeeGroupUsers).ToList();
            }
            else if (SecurityHelper.IsAdmin)
            {
                notificationList = db.SP_UserInformation<UserInformationViewModel>(0, "", 0,
                                         1, 1, SessionHelper.SelectedClientId, SessionHelper.SelectedCompanyId).Select(s => new SelectListItem
                                         {
                                             Text = s.ShortFullName,
                                             Value = s.Id.ToString()
                                         });
            }
            else if (SecurityHelper.IsUser)
            {
                notificationList = new List<SelectListItem>();
            }
            ViewBag.SupervisorList = notificationList;
            string[] TempData = { };
            ViewBag.SelectedSupervisorList = TempData;
            ViewBag.EmployeeSupervisorId = SessionHelper.LoginId;
            if (id.HasValue)
            {
                var lidt = db.GetAllByCompany<UserInformation>(SessionHelper.SelectedCompanyId, SessionHelper.SelectedClientId);

                //var supervisors = db.EmployeeSupervisor.Where(u => u.SupervisorUserId == userId && u.DataEntryStatus == 1).ToList();
                ChatConversation model = db.ChatConversation.FirstOrDefault(c => c.Id == id);
                var chatConversationParticipant = model.ChatConversationParticipant.ToList().FirstOrDefault(c => c.IsMe);
                if (chatConversationParticipant != null)
                {
                    chatConversationParticipant.LastMessageReadTime = DateTime.Now;
                    db.SaveChanges();
                }
                return View(model);
            }
            return View(new ChatConversation());
        }

        public JsonResult GetChatEmployeeList(string SearchBy, string SearchValue)
        {
            List<UserInformationViewModel> employeeList = new List<UserInformationViewModel>();
            try
            {

                //Calling Db Procedure
                employeeList = db.SP_UserInformation<UserInformationViewModel>(0, SearchValue, 0,
                                         1, SessionHelper.LoginId, SessionHelper.SelectedClientId, SessionHelper.SelectedCompanyId);
                var employeeGroupUsers = db.GetAllByCompany<UserEmployeeGroup>(SessionHelper.SelectedCompanyId, SessionHelper.SelectedClientId)
                                       .Where(e => e.EmployeeGroup.EmployeeGroupTypeId == 3 || e.EmployeeGroup.EmployeeGroupTypeId == 4).Select(e => e.UserInformation).
                                       Select(row => new UserInformationViewModel() { Id = row.Id, FirstLastName = row.FirstLastName, FirstName = row.FirstName, SecondLastName = row.SecondLastName, ShortFullName = row.ShortFullName }).ToList();


                if (SecurityHelper.IsSupervisor)
                {
                    employeeList = employeeList.Concat(employeeGroupUsers).ToList();

                }
                else if (SecurityHelper.IsAdmin)
                {
                    employeeList = db.SP_UserInformation<UserInformationViewModel>(0, "", 0, 1, 1, SessionHelper.SelectedClientId, SessionHelper.SelectedCompanyId)
                                     .Select(row => new UserInformationViewModel() { Id = row.Id, FirstLastName = row.FirstLastName, FirstName = row.FirstName, SecondLastName = row.SecondLastName, ShortFullName = row.ShortFullName }).ToList();

                }
                else if (SecurityHelper.IsUser)
                {
                    employeeList = db.GetAll<EmployeeSupervisor>(SessionHelper.SelectedClientId).Where(e => e.EmployeeUserId == SessionHelper.LoginId).
                                           Select(s => s.SupervisorUser).Distinct().Select(row => new UserInformationViewModel() { Id = row.Id, FirstLastName = row.FirstLastName, FirstName = row.FirstName, SecondLastName = row.SecondLastName, ShortFullName = row.ShortFullName }).ToList();
                    employeeList = employeeList.Concat(employeeGroupUsers).ToList();
                }
            }
            catch (FormatException)
            {
                Console.WriteLine("{0} Is Not A ID ", SearchValue);
            }
            return Json(employeeList, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public ActionResult ChatWindow(ChatConversation model)
        {
            var participant = db.ChatConversationParticipant.FirstOrDefault(c => c.ChatConversationId == model.Id && c.ParticipantId == SessionHelper.LoginId);
            var message = new ChatMessage();
            message.ChatConversationParticipantId = participant.Id;
            message.ChatConversationId = model.Id;
            message.ChatMessageText = model.ChatMessageText;
            db.ChatMessage.Add(message);
            db.SaveChanges();
            String senderName = "";
            Dictionary<string, string> toEmailAddress = new Dictionary<string, string>();
            var conversationObject = db.ChatConversation.FirstOrDefault(c => c.Id == model.Id);
            foreach (var each in conversationObject.ChatConversationParticipant)
            {
                var contact = db.UserContactInformation.FirstOrDefault(u => u.UserInformationId == each.ParticipantId);
                var chatObject = db.ChatUsers.FirstOrDefault(c => c.UserInformationName == contact.LoginEmail);
                if (each.IsMe)
                {
                    senderName = each.Participant.ShortFullName;
                }
                if (chatObject == null)
                {
                    if (!String.IsNullOrEmpty(contact.LoginEmail))
                        toEmailAddress.Add(contact.LoginEmail, contact.LoginEmail);
                }
            }

            if (toEmailAddress.Count > 0)
            {

                TimeAide.Services.Helpers.UtilityHelper.SendEmail(toEmailAddress, "", "", null, senderName + " has sent you messgae, please login to TimeAide Web to read conversation", "New Chat Message from " + senderName);
            }

            return Json("success");
        }

        public ActionResult CreateNewChat(int? id)
        {
            var userInformation = db.UserInformation.FirstOrDefault(u => u.Id == (id ?? 0));
            if (userInformation != null)
            {
                var previousChat = db.ChatConversation.FirstOrDefault(c => c.ChatConversationParticipant.Count==2 && c.ChatConversationParticipant.Any(cc => cc.ParticipantId == id) && c.ChatConversationParticipant.Any(cc => cc.ParticipantId == SessionHelper.LoginId));
                if (previousChat != null)
                {
                    previousChat.MyUserInformation.DataEntryStatus = 1;
                    db.SaveChanges();
                    return RedirectToAction("ChatWindow", new { id = previousChat.Id });
                }
                else
                {
                    ChatConversation newChatConversation = new ChatConversation() { ChatInitiatedById = SessionHelper.LoginId, ChatConversationTitle = userInformation.FullName };
                    newChatConversation.ChatConversationParticipant.Add(new ChatConversationParticipant() { ParticipantId = SessionHelper.LoginId });
                    newChatConversation.ChatConversationParticipant.Add(new ChatConversationParticipant() { ParticipantId = id ?? 0 });
                    db.ChatConversation.Add(newChatConversation);
                    db.SaveChanges();
                    return RedirectToAction("ChatWindow", new { id = newChatConversation.Id });
                }
            }
            return View();
        }

        public ActionResult SendMessage(int? id)
        {
            var userInformation = db.UserInformation.FirstOrDefault(u => u.Id == (id ?? 0));
            if (userInformation != null)
            {
                var conversation = db.ChatConversation.FirstOrDefault(c => c.ChatConversationParticipant.Count == 2 && c.ChatConversationParticipant.Any(i => i.ParticipantId == id) && c.ChatConversationParticipant.Any(i => i.ParticipantId == SessionHelper.LoginId));
                if (conversation == null)
                {
                    ChatConversation newChatConversation = new ChatConversation() { ChatInitiatedById = SessionHelper.LoginId, ChatConversationTitle = userInformation.FullName };
                    newChatConversation.ChatConversationParticipant.Add(new ChatConversationParticipant() { ParticipantId = SessionHelper.LoginId });
                    newChatConversation.ChatConversationParticipant.Add(new ChatConversationParticipant() { ParticipantId = id ?? 0 });
                    db.ChatConversation.Add(newChatConversation);
                    db.SaveChanges();
                    return RedirectToAction("ChatWindow", new { id = newChatConversation.Id });
                }
                return RedirectToAction("ChatWindow", new { id = conversation.Id });
            }
            return View("ChatWindow");
        }

        [HttpPost]
        public ActionResult CreateNewGroupChat(string title, string selectedCredentialIds)
        {
            string status = "Success";
            string message = "Successfully Added/Updated!";
            try
            {
                var selectedCredentialsList = selectedCredentialIds.Split(',').ToList();

                ChatConversation newChatConversation = new ChatConversation() { ChatInitiatedById = SessionHelper.LoginId, ChatConversationTitle = title };
                newChatConversation.ChatConversationParticipant.Add(new ChatConversationParticipant() { ParticipantId = SessionHelper.LoginId });

                foreach (var selectedCredentialId in selectedCredentialsList)
                {
                    if (selectedCredentialId == "") continue;
                    int credentialId = int.Parse(selectedCredentialId);
                    newChatConversation.ChatConversationParticipant.Add(new ChatConversationParticipant() { ParticipantId = credentialId });
                }

                db.ChatConversation.Add(newChatConversation);
                db.SaveChanges();
                return RedirectToAction("ChatWindow", new { id = newChatConversation.Id });

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
