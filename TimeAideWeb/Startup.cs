using Microsoft.AspNet.Identity;
using Microsoft.AspNet.SignalR;
using Microsoft.Owin;
using Microsoft.Owin.Security.Cookies;
using Owin;
using System;

[assembly: OwinStartupAttribute(typeof(TimeAide.Web.Startup))]
namespace TimeAide.Web
{
    //public partial class Startup
    //{
    //    public void Configuration(IAppBuilder app)
    //    {
    //        CookieAuthenticationOptions options = new CookieAuthenticationOptions
    //        {
    //            AuthenticationType = DefaultAuthenticationTypes.ApplicationCookie,
    //            CookieName = "TimeAideChat",
    //            CookieSecure = CookieSecureOption.Always,
    //            ExpireTimeSpan = TimeSpan.FromHours(8),
    //            SlidingExpiration = true
    //        };
    //        app.UseCookieAuthentication(options);

    //        app.MapSignalR();
    //        GlobalHost.HubPipeline.RequireAuthentication();
    //        ConfigureAuth(app);
    //    }
    //}
}
