using System;
using System.Globalization;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.Owin.Security;
using TimeAide.Web.Models;
using TimeAide.Common.Helpers;
using System.Web.Security;
using System.Text;
using TimeAide.Services.Helpers;
using TimeAide.Web.Extensions;
using System.Net;
using System.Collections.Generic;
using Newtonsoft.Json;
using TimeAide.Services;
using SignalRChat;
using Microsoft.Office.Interop.Excel;
using DocumentFormat.OpenXml.ExtendedProperties;
using System.Linq.Expressions;
using DocumentFormat.OpenXml.Office2010.Excel;
using Microsoft.ReportingServices.ReportProcessing.ReportObjectModel;
using TimeAide.Models.Models;
using DocumentFormat.OpenXml.EMMA;
using DocumentFormat.OpenXml.Wordprocessing;

namespace TimeAide.Web.Controllers
{
    [Authorize]
    public class AccountController : Controller
    {
        private TimeAideContext db = new TimeAideContext();
        private ApplicationSignInManager _signInManager;
        private ApplicationUserManager _userManager;

        public AccountController()
        {

        }
        public AccountController(ApplicationUserManager userManager, ApplicationSignInManager signInManager)
        {
            UserManager = userManager;
            SignInManager = signInManager;
        }

        public ApplicationSignInManager SignInManager
        {
            get
            {
                return _signInManager ?? HttpContext.GetOwinContext().Get<ApplicationSignInManager>();
            }
            private set
            {
                _signInManager = value;
            }
        }

        public ApplicationUserManager UserManager
        {
            get
            {
                return _userManager ?? HttpContext.GetOwinContext().GetUserManager<ApplicationUserManager>();
            }
            private set
            {
                _userManager = value;
            }
        }
        [HttpGet]
        public JsonResult GetLoginEmail()
        {

            return Json(SessionHelper.LoginEmail, JsonRequestBehavior.AllowGet);
        }

        //
        // GET: /Account/Login
        [AllowAnonymous]
        public ActionResult Login(string company, string returnUrl)
        {
            KillSession(true);
            ViewBag.ReturnUrl = returnUrl;
            ViewBag.LogoPath = TimeAide.Services.CompanyService.GetCompanyLogo(company);
            return View();
        }


        [AllowAnonymous]
        public ActionResult Login1()
        {
            return View("Login");
        }
        private void KillSession(bool isLogin)
        {
            string currentSessionUser = "";
            //if (isLogin && !string.IsNullOrEmpty(SessionHelper.LoginEmail))
            currentSessionUser = SessionHelper.LoginEmail;
            FormsAuthentication.SignOut();
            AuthenticationManager.SignOut(DefaultAuthenticationTypes.ApplicationCookie);
            Session.Clear();
            Session.Abandon();
     
            SessionHelper.PreviousLoginEmail = currentSessionUser;
        }

        //
        // POST: /Account/Login
        [HttpPost]
        [AllowAnonymous]
        //[ValidateAntiForgeryToken]
        public async Task<ActionResult> Login(LoginViewModel model, string returnUrl)
        {
            if (!string.IsNullOrEmpty(SessionHelper.LoginEmail))
            {
                if (SessionHelper.LoginEmail != model.Email)
                {
                    SessionHelper.PreviousLoginEmail = SessionHelper.LoginEmail;
                }
            }

            if (!ModelState.IsValid)
            {
                return View(model);
            }
            var UserContactInformation = db.UserContactInformation.FirstOrDefault(a => a.LoginEmail.ToLower() == model.Email.ToLower());
            UserInformation UserInformation = null;
            bool validLoginAttempt = true;
            if (UserContactInformation == null)
            {
                validLoginAttempt = false;
            }
            else
            {
                UserInformation = db.UserInformation.FirstOrDefault(a => a.Id == UserContactInformation.UserInformationId);
                if (UserInformation.UserInformationRole.Count > 0 && UserInformation.UserInformationRole.FirstOrDefault().Role.RoleTypeId == 1)
                {
                    validLoginAttempt = true;
                }
                else if (UserInformation.EmployeeStatus == null)
                {
                    validLoginAttempt = false;
                }
                else if (UserInformation.EmployeeStatus.Id == (int)EmployeeStatusNames.Closed)
                {
                    var configuration = ApplicationConfigurationService.GetApplicationConfiguration("ClosedEmployeesAccessAllow");
                    if (configuration == null || !configuration.ValueAsBoolean)
                        validLoginAttempt = false;

                }
                else if (UserInformation.EmployeeStatus.Id == (int)EmployeeStatusNames.Inactive)
                {
                    var configuration = ApplicationConfigurationService.GetApplicationConfiguration("InactiveEmployeesAccessAllow");
                    if (configuration == null || !configuration.ValueAsBoolean)
                        validLoginAttempt = false;
                }
            }
            if (!validLoginAttempt)
            {
                ModelState.AddModelError("", "Invalid login attempt, Please contact system admin");
                return View(model);
            }



            SessionHelper.SelectedClientId = SessionHelper.ClientId = UserInformation.ClientId ?? 0;
            SessionHelper.SelectedCompanyId = SessionHelper.CompanyId = UserInformation.CompanyId ?? 0;

            var unknowndeviceloginalert = ApplicationConfigurationService.GetApplicationConfiguration("2-StepVerification");
            if (unknowndeviceloginalert != null && unknowndeviceloginalert.ValueAsBoolean)
            {
                var devicedetail = UtilityHelper.DeviceValidate(UserInformation);


                if (devicedetail != null && devicedetail.Id > 0)
                {
                    ApplicationUser user = await UserManager.FindByEmailAsync(model.Email);
                    if (user != null)
                    {
                        var Isvalidepassword = UserManager.CheckPassword(user, model.Password);
                        if (Isvalidepassword)
                        {

                           

                            GenerateAndEmailSecurityCode(UserInformation.Id, UserInformation.ClientId ?? 0, model.Email, UserContactInformation.UserInformation.FirstLastName ?? "", devicedetail);

                            TempData["loginviewmodel"] = model;
                            return RedirectToAction("VerifySecurityCode", new { userinfoid = Encryption.EncryptURLParm(UserInformation.Id.ToString()), cltid = Encryption.EncryptURLParm(UserInformation.ClientId.ToString()), flname = Encryption.EncryptURLParm(UserContactInformation.UserInformation.FirstLastName ?? ""), email = Encryption.EncryptURLParm(model.Email), deviceid = Encryption.EncryptURLParm(devicedetail.Id.ToString()) });

                        }
                    }
                    else
                    {
                        ModelState.AddModelError("", "Invalid login attempt.");
                        return View(model);
                    }




                }

            }

               

            // This doesn't count login failures towards account lockout
            // To enable password failures to trigger account lockout, change to shouldLockout: true
            var result = await SignInManager.PasswordSignInAsync(model.Email, model.Password, model.RememberMe, shouldLockout: true);
            switch (result)
            {
                case SignInStatus.Success:

                    TimeAideContext db = new TimeAideContext();
                    if (UserSessionLogEventService.UserSessionLogEvent == null)
                    {
                        UserSessionLogEventService.UserSessionLogEvent = db.UserSessionLogEvent.Where(u => u.DataEntryStatus == 1).ToList();
                    }
                    UtilityHelper.SetSessionVariables(UserInformation, model.Email);
                    UtilityHelper.LogUserActivity();

                    var configuration = ApplicationConfigurationService.GetApplicationConfiguration("SessionTimeOut");
                    int sessionTimeout = 30;
                    if (configuration == null)
                        configuration = db.ApplicationConfiguration.FirstOrDefault(c => c.ApplicationConfigurationName == "SessionTimeOut" && c.DataEntryStatus == 1);

                    if (configuration != null && configuration.ValueAsInt.HasValue)
                        sessionTimeout = configuration.ValueAsInt.Value;
                    if (!SignalRService.CompanySessionTimeout.ContainsKey(SessionHelper.SelectedCompanyId))
                        SignalRService.CompanySessionTimeout.Add(SessionHelper.SelectedCompanyId, sessionTimeout);
                    else
                        SignalRService.CompanySessionTimeout[SessionHelper.SelectedCompanyId] = sessionTimeout;

                    if (model.RememberMe)
                    {
                        // They do, so let's create an authentication cookie
                        var cookie = FormsAuthentication.GetAuthCookie(model.Email.ToLower(), model.RememberMe);

                        // Since they want to be remembered, set the expiration for 30 days
                        cookie.Expires = DateTime.Now.AddDays(30);
                        // Store the cookie in the Response
                        Response.Cookies.Add(cookie);
                    }
                    else
                    {
                        // Otherwise set the cookie as normal
                        FormsAuthentication.SetAuthCookie(model.Email.ToLower(), model.RememberMe); // <- true/false
                    }



                    ApplicationUser user = await UserManager.FindByEmailAsync(model.Email);
                    if (user.ChangePassword)
                    {
                        //AuthenticationManager.SignOut(DefaultAuthenticationTypes.ApplicationCookie);
                        //Session.Clear();
                        //return View("ChangePassword");
                        return RedirectToAction("ChangePassword");

                    }
                    if (!string.IsNullOrEmpty(returnUrl))
                        return RedirectToLocal(returnUrl);
                    // return RedirectToAction("Index", "Home");
                    //return RedirectToAction("Index", "CompanyPortal");
                    return RedirectToAction("Home", new { ReturnUrl = returnUrl });
                case SignInStatus.LockedOut:
                    return View("Lockout");
                case SignInStatus.RequiresVerification:
                    return RedirectToAction("SendCode", new { ReturnUrl = returnUrl, RememberMe = model.RememberMe });
                case SignInStatus.Failure:
                default:
                    ModelState.AddModelError("", "Invalid login attempt.");
                    return View(model);
            }
        }



    
        [AllowAnonymous]
        //[ValidateAntiForgeryToken]
        public async Task<ActionResult> VerifiedLogin(LoginViewModel model, string returnUrl)
        {
            model = (LoginViewModel)TempData["loginviewmodel"];

            if (!string.IsNullOrEmpty(SessionHelper.LoginEmail))
            {
                if (SessionHelper.LoginEmail != model.Email)
                {
                    SessionHelper.PreviousLoginEmail = SessionHelper.LoginEmail;
                }
            }

            var UserContactInformation = db.UserContactInformation.FirstOrDefault(a => a.LoginEmail.ToLower() == model.Email.ToLower());
            UserInformation UserInformation = null;
            bool validLoginAttempt = true;
            if (UserContactInformation == null)
            {
                validLoginAttempt = false;
            }
            else
            {
                UserInformation = db.UserInformation.FirstOrDefault(a => a.Id == UserContactInformation.UserInformationId);
                if (UserInformation.UserInformationRole.Count > 0 && UserInformation.UserInformationRole.FirstOrDefault().Role.RoleTypeId == 1)
                {
                    validLoginAttempt = true;
                }
                else if (UserInformation.EmployeeStatus == null)
                {
                    validLoginAttempt = false;
                }
                else if (UserInformation.EmployeeStatus.Id == (int)EmployeeStatusNames.Closed)
                {
                    var configuration = ApplicationConfigurationService.GetApplicationConfiguration("ClosedEmployeesAccessAllow");
                    if (configuration == null || !configuration.ValueAsBoolean)
                        validLoginAttempt = false;

                }
                else if (UserInformation.EmployeeStatus.Id == (int)EmployeeStatusNames.Inactive)
                {
                    var configuration = ApplicationConfigurationService.GetApplicationConfiguration("InactiveEmployeesAccessAllow");
                    if (configuration == null || !configuration.ValueAsBoolean)
                        validLoginAttempt = false;
                }
            }
            if (!validLoginAttempt)
            {
                ModelState.AddModelError("", "Invalid login attempt, Please contact system admin");
                return View(model);
            }


       

            // This doesn't count login failures towards account lockout
            // To enable password failures to trigger account lockout, change to shouldLockout: true
            var result = await SignInManager.PasswordSignInAsync(model.Email, model.Password, model.RememberMe, shouldLockout: true);
            switch (result)
            {
                case SignInStatus.Success:

                    TimeAideContext db = new TimeAideContext();
                    if (UserSessionLogEventService.UserSessionLogEvent == null)
                    {
                        UserSessionLogEventService.UserSessionLogEvent = db.UserSessionLogEvent.Where(u => u.DataEntryStatus == 1).ToList();
                    }
                    UtilityHelper.SetSessionVariables(UserInformation, model.Email);
                    UtilityHelper.LogUserActivity();

                    var configuration = ApplicationConfigurationService.GetApplicationConfiguration("SessionTimeOut");
                    int sessionTimeout = 30;
                    if (configuration == null)
                        configuration = db.ApplicationConfiguration.FirstOrDefault(c => c.ApplicationConfigurationName == "SessionTimeOut" && c.DataEntryStatus == 1);

                    if (configuration != null && configuration.ValueAsInt.HasValue)
                        sessionTimeout = configuration.ValueAsInt.Value;
                    if (!SignalRService.CompanySessionTimeout.ContainsKey(SessionHelper.SelectedCompanyId))
                        SignalRService.CompanySessionTimeout.Add(SessionHelper.SelectedCompanyId, sessionTimeout);
                    else
                        SignalRService.CompanySessionTimeout[SessionHelper.SelectedCompanyId] = sessionTimeout;

                    if (model.RememberMe)
                    {
                        // They do, so let's create an authentication cookie
                        var cookie = FormsAuthentication.GetAuthCookie(model.Email.ToLower(), model.RememberMe);

                        // Since they want to be remembered, set the expiration for 30 days
                        cookie.Expires = DateTime.Now.AddDays(30);
                        // Store the cookie in the Response
                        Response.Cookies.Add(cookie);
                    }
                    else
                    {
                        // Otherwise set the cookie as normal
                        FormsAuthentication.SetAuthCookie(model.Email.ToLower(), model.RememberMe); // <- true/false
                    }



                    ApplicationUser user = await UserManager.FindByEmailAsync(model.Email);
                    if (user.ChangePassword)
                    {
                        //AuthenticationManager.SignOut(DefaultAuthenticationTypes.ApplicationCookie);
                        //Session.Clear();
                        //return View("ChangePassword");
                        return RedirectToAction("ChangePassword");

                    }
                    if (!string.IsNullOrEmpty(returnUrl))
                        return RedirectToLocal(returnUrl);
                    // return RedirectToAction("Index", "Home");
                    return RedirectToAction("Index", "CompanyPortal");
                case SignInStatus.LockedOut:
                    return View("Lockout");
                case SignInStatus.RequiresVerification:
                    return RedirectToAction("SendCode", new { ReturnUrl = returnUrl, RememberMe = model.RememberMe });
                case SignInStatus.Failure:
                default:
                    ModelState.AddModelError("", "Invalid login attempt.");
                    return View("Login",model);
            }
        }




        public void GenerateAndEmailSecurityCode(int UserInformationid,int clientid,string email,string firstlastname,DeviceMetaData devicedetail)
        {

            
            db.SP_MarkInvalidateSecurityCode(UserInformationid, clientid);
            Dictionary<string, string> toEmails = new Dictionary<string, string>();
            toEmails.Add(email, firstlastname);
            Random rnd = new Random();
            int code = rnd.Next(999999);
            SecurityCode securityCode = new SecurityCode()
            {
                Code = code,
                UserInformationId = UserInformationid,
                ClientId = clientid,
                DeviceMetaDataId = devicedetail.Id,
                IsValid = true

            };
            db.SecurityCode.Add(securityCode);
            db.SaveChanges();

            UtilityHelper.SendSecurityAlertEmail(code, toEmails, devicedetail);
        }

        [HttpPost]
        [AllowAnonymous]
        public  JsonResult ResendSecurityCode(VerifySecurityCodeViewModel verifySecurityCodeView)
        {
            string status = "";
            string message = "";

            if (verifySecurityCodeView.Userinformationid == 0 || verifySecurityCodeView.Clientid == 0 || verifySecurityCodeView.loginView.Email == null)
            {
                message = "Invalid resend code request Please try to login again.";
                status = "Error";
                return Json(new { id = 0, status = status, message = message });
            }

              var devicedetail= db.DeviceMetaData.Where(c => c.Id == verifySecurityCodeView.DeviceMetaDataid).FirstOrDefault();

            if (devicedetail != null)
            {
                GenerateAndEmailSecurityCode(verifySecurityCodeView.Userinformationid, verifySecurityCodeView.Clientid, verifySecurityCodeView.loginView.Email, verifySecurityCodeView.FisrtLastName, devicedetail);

                message = "Secuirty code sent successfully.";
                status = "Succes";
                  }
            else
            {
                message = "Invalid resend code request Please try to login again.";
                status = "Error";
            }
     
            return Json(new { status = status, message = message });
        }

        //
        // GET: /Account/VerifyCode
        [AllowAnonymous]
        public  ActionResult VerifySecurityCode(string userinfoid,string cltid,string flname,string email,string deviceid)
        {
            ViewBag.LogoPath = TimeAide.Services.CompanyService.GetCompanyLogo("");
            var Userinformationdetaid = userinfoid == null ? 0 : int.Parse(Encryption.DecryptURLParm(userinfoid));
            var ClientId = cltid == null ? 0 : int.Parse(Encryption.DecryptURLParm(cltid));
            var DeviceId = deviceid == null ? 0 : int.Parse(Encryption.DecryptURLParm(deviceid));
            var FisrtLastName =  Encryption.DecryptURLParm(flname);
            var Email = Encryption.DecryptURLParm(email);


            VerifySecurityCodeViewModel verifySecurityCodeView = new VerifySecurityCodeViewModel()
            {
                Userinformationid = Userinformationdetaid,
                Clientid = ClientId,
                DeviceMetaDataid = DeviceId,
                FisrtLastName = FisrtLastName,
                Email = Email

            };
            var loginviewmodel = (LoginViewModel)TempData["loginviewmodel"];
            TempData["loginviewmodel"] = loginviewmodel;
            verifySecurityCodeView.loginView = loginviewmodel;

            return View(verifySecurityCodeView);
        }

        [HttpPost]
        [AllowAnonymous]
        public ActionResult VerifySecurityCode(VerifySecurityCodeViewModel verifySecurityCodeView)
        {
          
            if (!ModelState.IsValid)
            {
                return View(verifySecurityCodeView);
            }
            if (verifySecurityCodeView.Userinformationid == 0 || verifySecurityCodeView.Clientid == 0 || verifySecurityCodeView.loginView.Email==null || verifySecurityCodeView.loginView.Password==null)
            {
                ModelState.AddModelError("", "Invalid code request Please try to login again.");
                return View(model: verifySecurityCodeView);
            }

            var securitycode = db.GetAllByUser<SecurityCode>(verifySecurityCodeView.Userinformationid, verifySecurityCodeView.Clientid).Where(c => c.IsUsed == false).FirstOrDefault();


           if(securitycode.CreatedDate.IsSecuirtyCodeExpired())
            {

                ModelState.AddModelError("", "Secuirty code is expired Please resend or login again.");
                return View(model: verifySecurityCodeView);

            }
            if (securitycode.Code == verifySecurityCodeView.Code)
            {
                var devicemetadata = db.GetAllByUser<DeviceMetaData>(verifySecurityCodeView.Userinformationid, verifySecurityCodeView.Clientid).Where(c => c.IsVerified == false).FirstOrDefault();


                devicemetadata.IsVerified = true;

                securitycode.IsUsed = true;
                securitycode.DataEntryStatus = 2;
                securitycode.ModifiedDate = DateTime.Now;
                
                db.SaveChanges();
                TempData["loginviewmodel"] = verifySecurityCodeView.loginView;
                return RedirectToAction("VerifiedLogin");

            }
            else
            {
                ModelState.AddModelError("", "Invalid security code.");
                return View(model: verifySecurityCodeView);
            }
            
        }


        // GET: /Account/VerifyCode
        [AllowAnonymous]
        public async Task<ActionResult> VerifyCode(string provider, string returnUrl, bool rememberMe)
        {
            // Require that the user has already logged in via username/password or external login
            if (!await SignInManager.HasBeenVerifiedAsync())
            {
                return View("Error");
            }
            return View(new VerifyCodeViewModel { Provider = provider, ReturnUrl = returnUrl, RememberMe = rememberMe });
        }

        //
        // POST: /Account/VerifyCode
        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> VerifyCode(VerifyCodeViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            // The following code protects for brute force attacks against the two factor codes. 
            // If a user enters incorrect codes for a specified amount of time then the user account 
            // will be locked out for a specified amount of time. 
            // You can configure the account lockout settings in IdentityConfig
            var result = await SignInManager.TwoFactorSignInAsync(model.Provider, model.Code, isPersistent: model.RememberMe, rememberBrowser: model.RememberBrowser);
            switch (result)
            {
                case SignInStatus.Success:
                    return RedirectToLocal(model.ReturnUrl);
                case SignInStatus.LockedOut:
                    return View("Lockout");
                case SignInStatus.Failure:
                default:
                    ModelState.AddModelError("", "Invalid code.");
                    return View(model);
            }
        }

        //
        // GET: /Account/Register
        [AllowAnonymous]
        public ActionResult Register(string activationCode)
        {
            TimeAideContext db = new TimeAideContext();
            var UserInformationActivation = db.UserInformationActivation.FirstOrDefault(a => a.ActivationCode.ToString() == activationCode);
            if (UserInformationActivation == null)
            {
                ViewBag.ExceptionMessage = "Invalid activation code";
                return View();
            }
            else if (!UserManagmentService.IsActive(UserInformationActivation))
            {
                ViewBag.ExceptionMessage = "Activation code is expired";
                return View();
            }
            else if (UserInformationActivation.DataEntryStatus != 1)
            {
                ViewBag.ExceptionMessage = "Activation code already used";
                return View();
            }

            SessionHelper.LoginId = UserInformationActivation.UserInformationId;
            SessionHelper.ActivationCode = activationCode;

            var UserContactInformation = db.UserContactInformation.FirstOrDefault(a => a.UserInformationId == SessionHelper.LoginId);
            RegisterViewModel registerViewModel = new RegisterViewModel();
            registerViewModel.Email = UserContactInformation.LoginEmail;
            registerViewModel.ActivationCode = activationCode;
            TimeAideContext context = new TimeAideContext();
            UserInformationService.AddUserRegistrationLog("Account Registration", "Activation Link clicked", UserContactInformation.UserInformation.Id, UserContactInformation.UserInformation, "Registration Email", context);
            context.SaveChanges();
            return View(registerViewModel);
        }

        //
        // POST: /Account/Register
        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Register(RegisterViewModel model)
        {
            if (ModelState.IsValid)
            {
                try 
                {
                    //var user = new ApplicationUser { UserName = model.Email, Email = model.Email };
                    UserManager.PasswordValidator = new PasswordValidator
                    {
                        RequiredLength = 6,
                        RequireNonLetterOrDigit = true,
                        RequireDigit = true,
                        RequireLowercase = true,
                        RequireUppercase = true,
                    };
                    //var result = await UserManager.CreateAsync(user, model.Password);
                    ApplicationUser user = await UserManager.FindByEmailAsync(model.Email);

                    var token = await UserManager.GeneratePasswordResetTokenAsync(user.Id);
                   

                    var result = await UserManager.ResetPasswordAsync(user.Id, token, model.Password);

                
                    if (result.Succeeded)
                    {
                        var UserInformationActivation = db.UserInformationActivation.FirstOrDefault(a => a.ActivationCode.ToString() == model.ActivationCode);
                        UserInformationActivation.DataEntryStatus = 2;

                        var UserInformation = db.UserInformation.FirstOrDefault(a => a.Id == UserInformationActivation.UserInformationId);
                        var UserContactInformation = db.UserContactInformation.FirstOrDefault(a => a.UserInformationId == UserInformationActivation.UserInformationId);

                        UtilityHelper.SetSessionVariables(UserInformation, UserContactInformation.LoginEmail);
                        UtilityHelper.LogUserActivity();

                        UserInformationService.AddUserRegistrationLog("Account Registration", "Registration Completed", UserContactInformation.UserInformation.Id, UserContactInformation.UserInformation, "Registration Email", db);
                        db.SaveChanges();

                        await SignInManager.SignInAsync(user, isPersistent: false, rememberBrowser: false);

                        UserInformationService.AddUserRegistrationLog("Account Registration", "User SignedIn", UserContactInformation.UserInformation.Id, UserContactInformation.UserInformation, "Registration Email", db);
                        db.SaveChanges();
                        // For more information on how to enable account confirmation and password reset please visit https://go.microsoft.com/fwlink/?LinkID=320771
                        // Send an email with this link
                        // string code = await UserManager.GenerateEmailConfirmationTokenAsync(user.Id);
                        // var callbackUrl = Url.Action("ConfirmEmail", "Account", new { userId = user.Id, code = code }, protocol: Request.Url.Scheme);
                        // await UserManager.SendEmailAsync(user.Id, "Confirm your account", "Please confirm your account by clicking <a href=\"" + callbackUrl + "\">here</a>");

                        //return RedirectToAction("Index", "Home");
                        return RedirectToAction("Index", "CompanyPortal");
                    }
                    else
                    {
                        var UserInformationActivation = db.UserInformationActivation.FirstOrDefault(a => a.ActivationCode.ToString() == model.ActivationCode);
                        var UserContactInformation = db.UserContactInformation.FirstOrDefault(a => a.UserInformationId == UserInformationActivation.UserInformationId);
                        UserInformationService.AddUserRegistrationLog("Account Registration ", "Problem With Password Reset step", UserContactInformation.UserInformation.Id, new UserInformation(), "Password Reset  problem step", db);
                        db.SaveChanges();
                    }

                    AddErrors(result);
                    // If we got this far, something failed, redisplay form
                    return View(model);

                }
                catch(Exception ex)
                {

                    Helpers.ErrorLogHelper.InsertLog(Helpers.ErrorLogType.Error, ex, this.ControllerContext);

                    
                     
                }

               

            }
            return View(model);


        }

        //
        // GET: /Account/ConfirmEmail
        [AllowAnonymous]
        public async Task<ActionResult> ConfirmEmail(string userId, string code)
        {
            if (userId == null || code == null)
            {
                return View("Error");
            }
            var result = await UserManager.ConfirmEmailAsync(userId, code);
            return View(result.Succeeded ? "ConfirmEmail" : "Error");
        }

        //
        // GET: /Account/ForgotPassword
        [AllowAnonymous]
        public ActionResult ForgotPassword()
        {
            return View();
        }

        //
        // POST: /Account/ForgotPassword
        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> ForgotPassword(ForgotPasswordViewModel model)
        {
            if (ModelState.IsValid)
            {
                var user = await UserManager.FindByNameAsync(model.Email.Trim());
                //if (user == null || !(await UserManager.IsEmailConfirmedAsync(user.Id)))
                //{
                //    // Don't reveal that the user does not exist or is not confirmed
                //    return View("ForgotPasswordConfirmation");
                //}


                if (user != null)
                {
                    var userInformation = db.UserContactInformation.FirstOrDefault(u => !String.IsNullOrEmpty(u.LoginEmail) && u.LoginEmail.ToLower() == model.Email.Trim().ToLower());
                    int clientId = 0;
                    int companyId = 0;
                    if (userInformation != null)
                    {
                        var employment = EmploymentService.GetActiveEmployment(userInformation.UserInformationId);

                        if (employment != null)
                        {
                            var employmentHistory = EmploymentHistoryService.GetActiveEmploymentHistory(userInformation.UserInformationId, employment.Id);
                            if (employmentHistory == null)
                            {
                                ModelState.AddModelError("BirthDate", "There is an issue with user configuration, please contact admin.");
                                return View(model);
                            }
                            clientId = employmentHistory.ClientId ?? 0;
                            companyId = employmentHistory.CompanyId ?? 0;
                        }
                    }
                    if (userInformation.UserInformation.BirthDate != model.BirthDate)
                    {
                        ModelState.AddModelError("BirthDate", "Birth Date does not match. ");
                        return View(model);
                    }
                    PassswordResetCode userInformationActivation = new PassswordResetCode()
                    {
                        ResetCode = await UserManager.GeneratePasswordResetTokenAsync(user.Id),
                        ActivationCode = Guid.NewGuid(),
                        UserInformationId = userInformation.UserInformation.Id,
                        CreatedBy = userInformation.Id
                    };
                    db.PassswordResetCode.Add(userInformationActivation);
                    db.SaveChanges();
                    if (!String.IsNullOrEmpty(userInformation.LoginEmail))
                    {
                        var emailTemplate = db.GetAllByCompany<EmailTemplate>(companyId, clientId).FirstOrDefault(u => u.EmailType.EmailTypeName == "ForgotPassword");
                        string emailBody = "";
                        string emailSubject = "";

                        if (emailTemplate != null)
                        {
                            emailBody = UtilityHelper.ReplaceMessagePlaceholders(userInformation.UserInformation, null, emailTemplate.EmailBody, "", "");
                            emailSubject = emailTemplate.EmailSubject;
                        }
                        UtilityHelper.ForgotPasswordEmail(userInformation.LoginEmail, userInformation.UserInformation.FirstLastName, emailSubject, emailBody, userInformationActivation.ActivationCode.ToString());
                        return View("ForgotPasswordConfirmation");
                    }
                }
                else
                {
                    ModelState.AddModelError("Email", "Email does not exist in the system.");
                }
                // For more information on how to enable account confirmation and password reset please visit https://go.microsoft.com/fwlink/?LinkID=320771
                // Send an email with this link
                // string code = await UserManager.GeneratePasswordResetTokenAsync(user.Id);
                // var callbackUrl = Url.Action("ResetPassword", "Account", new { userId = user.Id, code = code }, protocol: Request.Url.Scheme);		
                // await UserManager.SendEmailAsync(user.Id, "Reset Password", "Please reset your password by clicking <a href=\"" + callbackUrl + "\">here</a>");
                // return RedirectToAction("ForgotPasswordConfirmation", "Account");
            }

            // If we got this far, something failed, redisplay form
            return View(model);
        }



        //
        // GET: /Account/ForgotPasswordConfirmation
        [AllowAnonymous]
        public ActionResult ForgotPasswordConfirmation()
        {
            return View();
        }

        //
        // GET: /Account/ResetPassword
        [AllowAnonymous]
        public ActionResult ResetPassword(string activationCode)
        {
            UserManager.PasswordValidator = new PasswordValidator
            {
                RequiredLength = 6,
                RequireNonLetterOrDigit = true,
                RequireDigit = true,
                RequireLowercase = true,
                RequireUppercase = true,
            };
            //activationCode == null ? View("Error") :
            var UserInformationActivation = db.PassswordResetCode.FirstOrDefault(a => a.ActivationCode.ToString() == activationCode);
            if (UserInformationActivation == null)
            {
                Exception exception = new Exception("Invalid Reset code");
                HandleErrorInfo handleErrorInfo = new HandleErrorInfo(exception, "Account", "ResetPassword");
                return View("~/Views/Shared/Error.cshtml", handleErrorInfo);
            }
            else if (UserInformationActivation.DataEntryStatus != 1)
            {
                Exception exception = new Exception("Reset code is expired");
                HandleErrorInfo handleErrorInfo = new HandleErrorInfo(exception, "Account", "ResetPassword");
                return View("~/Views/Shared/Error.cshtml", handleErrorInfo);
            }

            //SessionHelper.LoginId = UserInformationActivation.UserInformationId;
            //SessionHelper.ActivationCode = activationCode;

            var UserContactInformation = db.UserContactInformation.FirstOrDefault(a => a.UserInformationId == UserInformationActivation.UserInformationId);
            ResetPasswordViewModel resetmodel = new ResetPasswordViewModel();
            resetmodel.Email = UserContactInformation.LoginEmail;
            resetmodel.Code = UserInformationActivation.ResetCode;
            return View(resetmodel);
        }

        //
        // POST: /Account/ResetPassword
        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> ResetPassword(ResetPasswordViewModel model)
        {
            UserManager.PasswordValidator = new PasswordValidator
            {
                RequiredLength = 6,
                RequireNonLetterOrDigit = true,
                RequireDigit = true,
                RequireLowercase = true,
                RequireUppercase = true,
            };
            if (!ModelState.IsValid)
            {
                return View(model);
            }
            var user = await UserManager.FindByNameAsync(model.Email);

            if (user == null)
            {
                // Don't reveal that the user does not exist
                return RedirectToAction("ResetPasswordConfirmation", "Account");
            }
            var result = await UserManager.ResetPasswordAsync(user.Id, model.Code, model.Password);
            if (result.Succeeded)
            {
                return RedirectToAction("ResetPasswordConfirmation", "Account");
            }
            AddErrors(result);
            return View();
        }

        //
        // GET: /Account/ResetPasswordConfirmation
        [AllowAnonymous]
        public ActionResult ResetPasswordConfirmation()
        {
            return View();
        }


        public ActionResult ChangePassword()
        {
            return View();
        }

        //
        //Save Details of New Password using Identity
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> ChangePassword(ChangePasswordViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            UserManager.PasswordValidator = new PasswordValidator
            {
                RequiredLength = 6,
                RequireNonLetterOrDigit = true,
                RequireDigit = true,
                RequireLowercase = true,
                RequireUppercase = true,
            };

            var user = await UserManager.FindByNameAsync(SessionHelper.LoginEmail);
           var NewpasswordMatch = UserManager.PasswordHasher.VerifyHashedPassword(user.PasswordHash, model.NewPassword);
            if (NewpasswordMatch == PasswordVerificationResult.Success)
            {
                AddErrors(IdentityResult.Failed("New password can not be same as the previous one."));
                return View(model);
            }
            var CurrentpasswordMatch = UserManager.PasswordHasher.VerifyHashedPassword(user.PasswordHash, model.OldPassword);
            if (CurrentpasswordMatch == PasswordVerificationResult.Failed)
            {
                AddErrors(IdentityResult.Failed("Current password is incorrect."));
                return View(model);
            }
            var result = await UserManager.ChangePasswordAsync(user.Id, model.OldPassword, model.NewPassword);
            if (result.Succeeded)
            {
                user.ChangePassword = false;
                var result1 = UserManager.Update(user);

                //var user = await UserManager.FindByIdAsync(User.Identity.GetUserId<int>());
                if (user != null && result1.Succeeded)
                {
                    await SignInManager.SignInAsync(user, isPersistent: false, rememberBrowser: false);
                }
                return View("ChangePasswordConfirmation");
            }
            AddErrors(result);
            return View(model);
        }


        public ActionResult ChangePasswordByAdmin()
        {
            return PartialView();
        }
        [HttpPost]
        public ActionResult ChangePasswordByAdmin(SetPasswordViewModel model)
        {
            UserManager.PasswordValidator = UserManagmentService.GetPasswordValidator();

            var userContactInformation = db.UserContactInformation.FirstOrDefault(c => c.Id == model.UserInformationId);
            var activationToken = db.UserInformationActivation.Where(t => t.UserInformationId == userContactInformation.UserInformationId).OrderByDescending(t => t.CreatedDate).FirstOrDefault();
            if (activationToken != null)
            {
                activationToken.DataEntryStatus = 2;
            }

            //string pass = UserManagmentService.GenerateRandomPassword(null);
            ApplicationUser user = UserManager.FindByEmail(userContactInformation.LoginEmail);
            if (user != null)
            {
                UserManager.Delete(user);
            }

            user = new ApplicationUser { UserName = userContactInformation.LoginEmail, Email = userContactInformation.LoginEmail };
            UserManagmentService.GetUserLockoutSettings(UserManager);
            var result = UserManager.Create(user, model.NewPassword);
            if (!result.Succeeded)
            {
                foreach (var each in result.Errors)
                    ModelState.AddModelError("NewPassword", each);
                return GetErrors();
            }
            userContactInformation.UserInformation.AspNetUserId = user.Id;
            ChangePasswordByAdminReason reason = new ChangePasswordByAdminReason();
            reason.Reason = model.Reason = model.Reason;
            reason.ChangeByUserId = SessionHelper.LoginId;
            reason.CompanyId = SessionHelper.SelectedCompanyId;
            reason.UserInformationId = userContactInformation.UserInformationId;
            db.ChangePasswordByAdminReason.Add(reason);
            db.SaveChanges();
            return Json(model);
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

        //
        // POST: /Account/ExternalLogin
        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public ActionResult ExternalLogin(string provider, string returnUrl)
        {
            // Request a redirect to the external login provider
            return new ChallengeResult(provider, Url.Action("ExternalLoginCallback", "Account", new { ReturnUrl = returnUrl }));
        }

        //
        // GET: /Account/SendCode
        [AllowAnonymous]
        public async Task<ActionResult> SendCode(string returnUrl, bool rememberMe)
        {
            var userId = await SignInManager.GetVerifiedUserIdAsync();
            if (userId == null)
            {
                return View("Error");
            }
            var userFactors = await UserManager.GetValidTwoFactorProvidersAsync(userId);
            var factorOptions = userFactors.Select(purpose => new SelectListItem { Text = purpose, Value = purpose }).ToList();
            return View(new SendCodeViewModel { Providers = factorOptions, ReturnUrl = returnUrl, RememberMe = rememberMe });
        }

        //
        // POST: /Account/SendCode
        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> SendCode(SendCodeViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View();
            }

            // Generate the token and send it
            if (!await SignInManager.SendTwoFactorCodeAsync(model.SelectedProvider))
            {
                return View("Error");
            }
            return RedirectToAction("VerifyCode", new { Provider = model.SelectedProvider, ReturnUrl = model.ReturnUrl, RememberMe = model.RememberMe });
        }

        //
        // GET: /Account/ExternalLoginCallback
        [AllowAnonymous]
        public async Task<ActionResult> ExternalLoginCallback(string returnUrl)
        {
            var loginInfo = await AuthenticationManager.GetExternalLoginInfoAsync();
            if (loginInfo == null)
            {
                return RedirectToAction("Login");
            }

            // Sign in the user with this external login provider if the user already has a login
            var result = await SignInManager.ExternalSignInAsync(loginInfo, isPersistent: false);
            switch (result)
            {
                case SignInStatus.Success:
                    {
                        var userContact = db.UserContactInformation.Where(u => u.LoginEmail.ToLower() == loginInfo.Email.ToLower()).FirstOrDefault();
                        if (userContact == null)
                        {
                            //Exception exception = new Exception("User is not registred, please contact system admin to get register.");
                            //HandleErrorInfo handleErrorInfo = new HandleErrorInfo(exception, "Account", "Register");
                            //return View("~/Views/Shared/Error.cshtml", handleErrorInfo);
                            goto case SignInStatus.Failure;
                        }

                        //SessionHelper.ClientId = user.ClientId ?? 0;
                        //var UserContactInformation = db.UserContactInformation.FirstOrDefault(a => a.LoginEmail.ToLower() == loginInfo.Email.ToLower());
                        //var userInformation = db.UserInformation.FirstOrDefault(a => a.UserId == UserContactInformation.UserID);
                        var UserContactInformation = db.UserContactInformation.FirstOrDefault(a => a.LoginEmail.ToLower() == loginInfo.Email.ToLower());
                        var UserInformation = db.UserInformation.FirstOrDefault(a => a.Id == UserContactInformation.UserInformationId);
                        UtilityHelper.SetSessionVariables(UserInformation, UserContactInformation.LoginEmail);
                        UtilityHelper.LogUserActivity();

                        return RedirectToLocal(returnUrl);
                    }
                case SignInStatus.LockedOut:
                    return View("Lockout");
                case SignInStatus.RequiresVerification:
                    return RedirectToAction("SendCode", new { ReturnUrl = returnUrl, RememberMe = false });
                case SignInStatus.Failure:
                default:
                    // If the user does not have an account, then prompt the user to create an account

                    //TimeAideContext db = new TimeAideContext();
                    var userContactInformation = db.UserContactInformation.FirstOrDefault(a => a.LoginEmail.ToLower() == loginInfo.Email.ToLower());
                    UserInformationActivation userInformationActivation = null;
                    if (userContactInformation != null)
                    {
                        userInformationActivation = db.UserInformationActivation.Where(a => a.UserInformationId == userContactInformation.UserInformationId).OrderByDescending(o => o.Id).FirstOrDefault();
                    }

                    if (userContactInformation == null || userInformationActivation == null)
                    {
                        StringBuilder stringBuilder = new StringBuilder();
                        stringBuilder.Append("You are trying to login to TimeAide using ");
                        stringBuilder.Append(loginInfo.Email);
                        stringBuilder.Append(".<br>");
                        stringBuilder.Append("Invalid Registration attempt, please contact system admin to get valid invitation to register.");
                        Exception exception = new Exception(stringBuilder.ToString());
                        HandleErrorInfo handleErrorInfo = new HandleErrorInfo(exception, "Account", "Register");
                        return View("~/Views/Shared/Error.cshtml", handleErrorInfo);
                    }
                    else if (userInformationActivation.DataEntryStatus != 1)
                    {
                        Exception exception = new Exception("Activation code is expired");
                        HandleErrorInfo handleErrorInfo = new HandleErrorInfo(exception, "Account", "Register");
                        return View("~/Views/Shared/Error.cshtml", handleErrorInfo);
                    }
                    SessionHelper.LoginId = userInformationActivation.UserInformationId;
                    SessionHelper.ActivationCode = userInformationActivation.ActivationCode.ToString();

                    ////////////////////////////
                    ViewBag.ReturnUrl = returnUrl;
                    ViewBag.LoginProvider = loginInfo.Login.LoginProvider;
                    return View("ExternalLoginConfirmation", new ExternalLoginConfirmationViewModel { Email = loginInfo.Email });
            }
        }

        //
        // POST: /Account/ExternalLoginConfirmation
        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> ExternalLoginConfirmation(ExternalLoginConfirmationViewModel model, string returnUrl)
        {
            if (User.Identity.IsAuthenticated)
            {
                return RedirectToAction("Index", "Manage");
            }

            if (ModelState.IsValid)
            {
                // Get the information about the user from the external login provider
                var info = await AuthenticationManager.GetExternalLoginInfoAsync();
                if (info == null)
                {
                    return View("ExternalLoginFailure");
                }
                var user = new ApplicationUser { UserName = model.Email, Email = model.Email };
                var result = await UserManager.CreateAsync(user);
                if (result.Succeeded)
                {
                    result = await UserManager.AddLoginAsync(user.Id, info.Login);
                    if (result.Succeeded)
                    {
                        //var UserInformation = db.UserInformation.FirstOrDefault(a => a.UserId == SessionHelper.LoginId);
                        //var UserInformationActivation = db.UserInformationActivation.FirstOrDefault(a => a.ActivationCode.ToString() == SessionHelper.ActivationCode);
                        //UserInformation.AspNetUserId = user.Id;
                        //UserInformationActivation.DataEntryStatus = 2;
                        //UtilityHelper.SetSessionVariables(UserInformation.UserId, UserInformation.ClientId, UserInformation.SupervisoryLevelId, UserInformation.LoginEmail);
                        //db.SaveChanges();
                        await SignInManager.SignInAsync(user, isPersistent: false, rememberBrowser: false);
                        return RedirectToLocal(returnUrl);
                    }
                }
                AddErrors(result);
            }

            ViewBag.ReturnUrl = returnUrl;
            return View(model);
        }

        //
        // POST: /Account/LogOff
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult LogOff()
        {
            KillSession(false);
            ViewBag.LogoPath = TimeAide.Services.CompanyService.GetCompanyLogo("");
            return RedirectToAction("Login");
        }

        //
        // GET: /Account/ExternalLoginFailure
        [AllowAnonymous]
        public ActionResult ExternalLoginFailure()
        {
            return View();
        }
        public ActionResult Home(string ReturnUrl)
        {
            ViewBag.ReturnUrl = ReturnUrl;
            return PartialView("AccountHome");
        }
        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                if (_userManager != null)
                {
                    _userManager.Dispose();
                    _userManager = null;
                }

                if (_signInManager != null)
                {
                    _signInManager.Dispose();
                    _signInManager = null;
                }
            }

            base.Dispose(disposing);
        }

        #region Helpers
        // Used for XSRF protection when adding external logins
        private const string XsrfKey = "XsrfId";

        private IAuthenticationManager AuthenticationManager
        {
            get
            {
                return HttpContext.GetOwinContext().Authentication;
            }
        }

        private void AddErrors(IdentityResult result)
        {
            foreach (var error in result.Errors)
            {
                ModelState.AddModelError("", error);
            }
        }

        private ActionResult RedirectToLocal(string returnUrl)
        {
            if (Url.IsLocalUrl(returnUrl))
            {
                return Redirect(returnUrl);
            }
            //return RedirectToAction("Index", "Home");
            return RedirectToAction("Index", "UserDashboard");
        }

        internal class ChallengeResult : HttpUnauthorizedResult
        {
            public ChallengeResult(string provider, string redirectUri)
                : this(provider, redirectUri, null)
            {
            }

            public ChallengeResult(string provider, string redirectUri, string userId)
            {
                LoginProvider = provider;
                RedirectUri = redirectUri;
                UserId = userId;
            }

            public string LoginProvider { get; set; }
            public string RedirectUri { get; set; }
            public string UserId { get; set; }

            public override void ExecuteResult(ControllerContext context)
            {
                var properties = new AuthenticationProperties { RedirectUri = RedirectUri };
                if (UserId != null)
                {
                    properties.Dictionary[XsrfKey] = UserId;
                }
                context.HttpContext.GetOwinContext().Authentication.Challenge(properties, LoginProvider);
            }

        }
        #endregion
    }
}