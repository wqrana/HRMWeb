using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using TimeAide.Services;
using TimeAide.Web.Models;


namespace TimeAide.Web.Extensions
{
    public static class UserManagmentService
    {
        public static string GetUserRole(int userInformationId)
        {
            TimeAideContext db = new TimeAideContext();
            var role = db.UserInformationRole.FirstOrDefault(r => r.UserInformationId == userInformationId);
            if (role != null)
                return role.Role.RoleName;
            return "";
        }

        public static string GetUserEmployeeGroups(int userInformationId)
        {
            TimeAideContext db = new TimeAideContext();
            var role = db.UserEmployeeGroup.Where(r => r.UserInformationId == userInformationId);
            if (role != null)
                return string.Join<string>(",", role.Select(r => r.EmployeeGroup.EmployeeGroupName));
            return "";
        }

        public static string Create(string email, string password)
        {
            ApplicationDbContext _identityContext = new ApplicationDbContext();
            var UserManager = new UserManager<ApplicationUser>(new UserStore<ApplicationUser>(_identityContext));

            ApplicationUser user = CreateUser(email, password, UserManager);
            return user.Id;
        }

        public static ApplicationUser CreateUser(string email, string password, UserManager<ApplicationUser> userManager)
        {
            userManager.UserLockoutEnabledByDefault = true;
            GetUserLockoutSettings(userManager);
            var user = new ApplicationUser { UserName = email, Email = email };
            var result = userManager.Create(user, password);
            return user;
        }

        public static ApplicationUser UpdateUser(ApplicationUser user, UserManager<ApplicationUser> userManager)
        {
            var result = userManager.Update(user);
            return user;
        }

        public static bool IsInvalid(this UserInformationActivation userInformationActivation)
        {
            if (userInformationActivation.ActivationCodeDays() < 0)
            {
                return true;
            }
            return false;
        }

        public static bool IsActive(this UserInformationActivation userInformationActivation)
        {
            if (userInformationActivation != null)
            {
                ApplicationConfigurationService applicationConfigurationService = new ApplicationConfigurationService(userInformationActivation.ClientId ?? 0);
                int activationCodeDays = 0;
                if (applicationConfigurationService != null)
                    activationCodeDays = applicationConfigurationService.UserActivationCodeDays;

                if ((userInformationActivation.DataEntryStatus == 1) && (userInformationActivation.ActivationCodeDays() > activationCodeDays || userInformationActivation.ActivationCodeDays() < 0))
                {
                    return false;
                }
                return true;
            }
            return false;
        }
        public static bool IsUserRegistered(UserInformation each)
        {
            return each.ActiveUserContactInformation != null &&
                   !String.IsNullOrEmpty(each.ActiveUserContactInformation.LoginEmail) &&
                   IsActive(each.UserInformationActivation.OrderByDescending(a => a.Id).FirstOrDefault());
        }
        public static double ActivationCodeDays(this UserInformationActivation userInformationActivation)
        {
            return (DateTime.Now - userInformationActivation.CreatedDate).TotalDays;
        }

        /// <summary>
        /// Generates a Random Password
        /// respecting the given strength requirements.
        /// </summary>
        /// <param name="opts">A valid PasswordOptions object
        /// containing the password strength requirements.</param>
        /// <returns>A random password</returns>
        public static string GenerateRandomPassword(PasswordValidator opts = null)
        {
            if (opts == null) opts = new PasswordValidator()
            {
                RequiredLength = 6,
                RequireNonLetterOrDigit = true,
                RequireDigit = true,
                RequireLowercase = true,
                RequireUppercase = true,
            };

            string[] randomChars = new[] {
            "ABCDEFGHJKLMNOPQRSTUVWXYZ",    // uppercase 
            "abcdefghijkmnopqrstuvwxyz",    // lowercase
            "0123456789",                   // digits
            "!@$?_-"                        // non-alphanumeric
        };

            Random rand = new Random(Environment.TickCount);
            List<char> chars = new List<char>();

            if (opts.RequireUppercase)
                chars.Insert(rand.Next(0, chars.Count),
                    randomChars[0][rand.Next(0, randomChars[0].Length)]);

            if (opts.RequireLowercase)
                chars.Insert(rand.Next(0, chars.Count),
                    randomChars[1][rand.Next(0, randomChars[1].Length)]);

            if (opts.RequireDigit)
                chars.Insert(rand.Next(0, chars.Count),
                    randomChars[2][rand.Next(0, randomChars[2].Length)]);

            for (int i = chars.Count; i < opts.RequiredLength; i++)
            {
                string rcs = randomChars[rand.Next(0, randomChars.Length)];
                chars.Insert(rand.Next(0, chars.Count),
                    rcs[rand.Next(0, rcs.Length)]);
            }

            return new string(chars.ToArray());
        }

        //public static IdentityResult ChangePassword(string email, string newPassword,string userId)
        //{
        //    try
        //    {
        //        ApplicationDbContext context = new ApplicationDbContext();
        //        var roleManager = new RoleManager<IdentityRole>(new RoleStore<IdentityRole>(context));
        //        var UserManager = new UserManager<ApplicationUser>(new UserStore<ApplicationUser>(context));
        //        var UserManager = HttpContext.GetOwinContext().GetUserManager<ApplicationUserManager>();

        //        ApplicationUser user = UserManager.FindById(userId);
        //        UserManager.UserTokenProvider = new TokenProviderDescriptor(typeof(IUserTwoFactorTokenProvider<User>))
        //        var token = UserManager.GeneratePasswordResetToken(userId);
        //        return UserManager.ResetPassword(userId, token, newPassword);
        //    }
        //    catch (Exception ex)
        //    {
        //        throw ex;
        //    }
        //}

        public static bool SetLockout(string aspNetUserId)
        {
            ApplicationDbContext context = new ApplicationDbContext();
            var userManager = new UserManager<ApplicationUser>(new UserStore<ApplicationUser>(context));
            ApplicationUser user = userManager.FindByEmail(aspNetUserId);
            if (user != null)
            {
                IdentityResult result = userManager.SetLockoutEndDate(user.Id, DateTime.Now.AddYears(200));
                return result.Succeeded;
            }
            return true;
        }

        public static bool SetUnLockout(string aspNetUserId)
        {
            ApplicationDbContext context = new ApplicationDbContext();
            var userManager = new UserManager<ApplicationUser>(new UserStore<ApplicationUser>(context));
            ApplicationUser user = userManager.FindByEmail(aspNetUserId);
            if (user != null)
            {
                user.LockoutEndDateUtc = null;
                IdentityResult result = userManager.Update(user);
                return result.Succeeded;
            }
            return true;
        }
        public static bool SetLockout(string aspNetUserId,out string message)
        {
            message = "";
            ApplicationDbContext context = new ApplicationDbContext();
            var userManager = new UserManager<ApplicationUser>(new UserStore<ApplicationUser>(context));
            ApplicationUser user = userManager.FindByEmail(aspNetUserId);
            if (user != null)
            {
                IdentityResult result = userManager.SetLockoutEndDate(user.Id, DateTime.Now.AddYears(200));
                message = string.Join(",", result.Errors);
                return result.Succeeded;
            }

            return true;
        }
        public static bool SetLockout(string aspNetUserId,DateTime lockEndDate)
        {
            ApplicationDbContext context = new ApplicationDbContext();
            var userManager = new UserManager<ApplicationUser>(new UserStore<ApplicationUser>(context));
            ApplicationUser user = userManager.FindByEmail(aspNetUserId);
            if (user != null)
            {
                IdentityResult result = userManager.SetLockoutEndDate(user.Id, lockEndDate);
                return result.Succeeded;
            }
            return true;
        }

        public static bool IsLockout(string aspNetUserId)
        {
            if (!string.IsNullOrEmpty(aspNetUserId))
            {
                ApplicationDbContext context = new ApplicationDbContext();
                var userManager = new UserManager<ApplicationUser>(new UserStore<ApplicationUser>(context));
                ApplicationUser user = userManager.FindByEmail(aspNetUserId);
                if (user != null)
                {
                    var result = userManager.IsLockedOut(user.Id);
                    return result;
                }
            }
            return false;
        }
        public static bool DeleteUser(string email)
        {
            ApplicationDbContext context = new ApplicationDbContext();
            var userManager = new UserManager<ApplicationUser>(new UserStore<ApplicationUser>(context));
            ApplicationUser user = userManager.FindByEmail(email);
            if (user != null)
            {
                IdentityResult result = userManager.Delete(user);
                return result.Succeeded;
            }
            return true;
        }
        public static PasswordValidator GetPasswordValidator()
        {
           return new PasswordValidator
            {
                RequiredLength = 6,
                RequireNonLetterOrDigit = true,
                RequireDigit = true,
                RequireLowercase = true,
                RequireUppercase = true,
            };
        }
        public static void GetUserLockoutSettings(UserManager<ApplicationUser> manager)
        {
            manager.UserLockoutEnabledByDefault = true;
            var configurationLockoutTime = ApplicationConfigurationService.GetApplicationConfiguration("AccountLockoutTimeSpanMinutes", null, 1);
            if (configurationLockoutTime != null && configurationLockoutTime.ValueAsInt.HasValue)
                manager.DefaultAccountLockoutTimeSpan = TimeSpan.FromMinutes(configurationLockoutTime.ValueAsInt.Value);
            //else
            //   manager.DefaultAccountLockoutTimeSpan = TimeSpan.FromMinutes(5);
            var configurationMaxFailedAttempts = ApplicationConfigurationService.GetApplicationConfiguration("MaxFailedAccessAttemptsBeforeLockout", null, 1);
            if (configurationMaxFailedAttempts != null && configurationMaxFailedAttempts.ValueAsInt.HasValue)
                manager.MaxFailedAccessAttemptsBeforeLockout = configurationMaxFailedAttempts.ValueAsInt.Value;
            //else
            //    manager.MaxFailedAccessAttemptsBeforeLockout = 3;
        }
        public static void GetUserLockoutSettings(ApplicationUserManager manager)
        {
            manager.UserLockoutEnabledByDefault = true;
            var configurationLockoutTime = ApplicationConfigurationService.GetApplicationConfiguration("AccountLockoutTimeSpanMinutes", null, 1);
            if (configurationLockoutTime != null && configurationLockoutTime.ValueAsInt.HasValue)
                manager.DefaultAccountLockoutTimeSpan = TimeSpan.FromMinutes(configurationLockoutTime.ValueAsInt.Value);
            //else
            //   manager.DefaultAccountLockoutTimeSpan = TimeSpan.FromMinutes(5);
            var configurationMaxFailedAttempts = ApplicationConfigurationService.GetApplicationConfiguration("MaxFailedAccessAttemptsBeforeLockout", null, 1);
            if (configurationMaxFailedAttempts != null && configurationMaxFailedAttempts.ValueAsInt.HasValue)
                manager.MaxFailedAccessAttemptsBeforeLockout = configurationMaxFailedAttempts.ValueAsInt.Value;
            //else
            //    manager.MaxFailedAccessAttemptsBeforeLockout = 3;
        }
        public static UserValidator<ApplicationUser> GetUserValidator(ApplicationUserManager manager)
        {
            return new UserValidator<ApplicationUser>(manager)
            {
                AllowOnlyAlphanumericUserNames = false,
                RequireUniqueEmail = true
            };
        }

        public static ApplicationUser FindByEmail(string email)
        {
            ApplicationDbContext context = new ApplicationDbContext();
            var userManager = new UserManager<ApplicationUser>(new UserStore<ApplicationUser>(context));
            ApplicationUser user = userManager.FindByEmail(email);
            return user;
        }
        public static ApplicationUser FindByEmployee(UserInformation userInformation)
        {
            ApplicationDbContext context = new ApplicationDbContext();
            var userManager = new UserManager<ApplicationUser>(new UserStore<ApplicationUser>(context));
            ApplicationUser user = userManager.FindById(userInformation.AspNetUserId);
            return user;
        }

      
    }
}
