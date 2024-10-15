using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
//using TimeAide.Web.Extensions;
using TimeAide.Web.Models;

namespace TimeAide.AdminConsole
{
    class Program
    {
        static void Main(string[] args)
        {
            var adminConsoleHelper = new AdminConsoleHelper();
            string loginEmail = ConfigurationManager.AppSettings["DefaultAdminEmail"];
            var userInformation = adminConsoleHelper.DbContext.UserContactInformation.FirstOrDefault(u => u.LoginEmail == loginEmail).UserInformation;
            string userId = ""; //UserManagmentService.Create(loginEmail, ";Identech01");
            userInformation.AspNetUserId = userId;
            adminConsoleHelper.DbContext.SaveChanges();
            return;
        }

        
    }
}
