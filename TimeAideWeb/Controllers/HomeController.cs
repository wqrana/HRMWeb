using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using TimeAide.Web.Models;

namespace TimeAide.Web.Controllers
{
    public class HomeController : TimeAideWebBaseControllers
    {
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }

        public ActionResult RequestApproval(string param1, string param2, int Id)
        {
            ViewBag.Message = "Your contact page.";
            if (!string.IsNullOrEmpty(param1) && !param1.StartsWith("/") && !param1.StartsWith("\\"))
            {
                param1 = "/" + param1;
            }
            ViewBag.ApprovalUrl = "" + param1 + "/" + param2 + "?Id=" + Id;
            return View("Index");
        }


    }
}