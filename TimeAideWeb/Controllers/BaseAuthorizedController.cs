//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Web;
//using System.Web.Mvc;
//using TimeAide.Common.Helpers;

//namespace TimeAide.Web.Controllers
//{
//    public class BaseAuthorizedController : BaseController
//    {
//        protected override void OnActionExecuting(ActionExecutingContext filterContext)
//        {
//            base.OnActionExecuting(filterContext);
//            if (String.IsNullOrWhiteSpace(SessionHelper.LoginEmail))
//            {
//                filterContext.Result = new RedirectToRouteResult(new System.Web.Routing.RouteValueDictionary { { "controller", "Account" }, { "action", "Login" } });
//            }
//        }
//        protected override void OnException(ExceptionContext filterContext)
//        {
//            filterContext.ExceptionHandled = true;
//            //Log the error!!
//            Helpers.ErrorLogHelper.InsertLog(Helpers.ErrorLogType.Error, filterContext.Exception, this.ControllerContext);
//            //Redirect to action
//            //filterContext.Result = RedirectToAction("Error", "InternalError");

//            // OR return specific view
//            filterContext.Result = new ViewResult
//            {
//                ViewName = "~/Views/Shared/_InternalError.cshtml"
//            };
//        }
//    }
//}