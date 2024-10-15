using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;

namespace TimeAide.Web.Extensions
{
    /// <summary>
    /// Summary description for Handler1
    /// </summary>
    public class DenyDirectAccessHandler : IHttpHandler
    {

        public void ProcessRequest(HttpContext context)
        {
            //write your handler implementation here.

            //Http
            HttpRequest request = context.Request;
            //HttpResponse response = context.Response;
            bool isRequiredAuthentication = true;
            if (context.Request.UrlReferrer != null)
            {
                if (
                    context.Request.UrlReferrer.AbsolutePath.EndsWith("/") ||
                    context.Request.UrlReferrer.AbsolutePath.Contains("Account") 
                   )
                {
                    isRequiredAuthentication = false;
                }
            }
            if (!request.IsAuthenticated && isRequiredAuthentication==true)
            {
                
                context.Response.Redirect("/Account/Login");
            }
            else
            {
               // Check if URL and URL Referrer are null
                if (context.Request.Url != null && context.Request.UrlReferrer != null)
                {
                    //Check image types
                    if (!context.Request.Url.AbsolutePath.Contains(".bmp")  ||
                        !context.Request.Url.AbsolutePath.Contains(".jpg")  ||
                        !context.Request.Url.AbsolutePath.Contains(".jpeg") ||
                        !context.Request.Url.AbsolutePath.Contains(".png")  ||
                        !context.Request.Url.AbsolutePath.Contains(".mp4"))
                    {
                       //Get bytes from file
                       byte[] MyBytes = File.ReadAllBytes(context.Request.PhysicalPath);                          
                       context.Response.OutputStream.Write(MyBytes, 0, MyBytes.Length);
                       context.Response.Flush();                     

                    }
                  
                }
                else
                    //Redirect                
                    context.Response.Redirect("/Account/Login");
            }
        }

        public bool IsReusable
        {
            get
            {
                return false;
            }
        }
    }
}