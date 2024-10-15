using System;
using System.Configuration;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.AspNet.SignalR;
using Microsoft.Owin;
using Microsoft.Owin.Security.Cookies;
using Microsoft.Owin.Security.Google;
using Owin;
using SignalRChat;
using TimeAide.Web.Controllers;
using TimeAide.Web.Models;

namespace TimeAide.Web
{
    public partial class Startup
    {
        // For more information on configuring authentication, please visit https://go.microsoft.com/fwlink/?LinkId=301864
        public void ConfigureAuth(IAppBuilder app)
        {
            // Configure the db context, user manager and signin manager to use a single instance per request
            app.CreatePerOwinContext(ApplicationDbContext.Create);
            app.CreatePerOwinContext<ApplicationUserManager>(ApplicationUserManager.Create);
            app.CreatePerOwinContext<ApplicationSignInManager>(ApplicationSignInManager.Create);

            // Enable the application to use a cookie to store information for the signed in user
            // and to use a cookie to temporarily store information about a user logging in with a third party login provider
            // Configure the sign in cookie
            app.UseCookieAuthentication(new CookieAuthenticationOptions
            {
                AuthenticationType = DefaultAuthenticationTypes.ApplicationCookie,
                LoginPath = new PathString("/Account/LogOff"),
                Provider = new CookieAuthenticationProvider
                {
                    // Enables the application to validate the security stamp when the user logs in.
                    // This is a security feature which is used when you change a password or add an external login to your account.  
                    OnValidateIdentity = SecurityStampValidator.OnValidateIdentity<ApplicationUserManager, ApplicationUser>(
                    validateInterval: TimeSpan.FromMinutes(24*60),
                    regenerateIdentity: (manager, user) => user.GenerateUserIdentityAsync(manager))
                },
                SlidingExpiration = true,
                ExpireTimeSpan = TimeSpan.FromMinutes(24*60) //Set the session timeout at here
            });


            //CookieAuthenticationOptions options = new CookieAuthenticationOptions
            //{
            //    //AuthenticationType = DefaultAuthenticationTypes.ApplicationCookie,
            //    //CookieName = "TimeAideChat",
            //    //CookieSecure = CookieSecureOption.Always,
            //    //ExpireTimeSpan = TimeSpan.FromHours(8),
            //    //SlidingExpiration = true
            //};

            app.UseExternalSignInCookie(DefaultAuthenticationTypes.ExternalCookie);

            #region SignalR
            //GlobalHost.Configuration.ConnectionTimeout = TimeSpan.FromSeconds(50);
            //GlobalHost.Configuration.DisconnectTimeout = TimeSpan.FromSeconds(20);
            //GlobalHost.Configuration.KeepAlive = TimeSpan.FromSeconds(5);

            GlobalHost.Configuration.ConnectionTimeout = TimeSpan.FromMinutes(60 * 24);
            GlobalHost.Configuration.DisconnectTimeout = TimeSpan.FromMinutes(60 * 24);
            GlobalHost.Configuration.KeepAlive = TimeSpan.FromMinutes(60 * 6);
            app.MapSignalR();
            #endregion
            //GlobalHost.HubPipeline.RequireAuthentication();

            // Enables the application to temporarily store user information when they are verifying the second factor in the two-factor authentication process.
            app.UseTwoFactorSignInCookie(DefaultAuthenticationTypes.TwoFactorCookie, TimeSpan.FromMinutes(24 * 60));

            // Enables the application to remember the second login verification factor such as phone or email.
            // Once you check this option, your second step of verification during the login process will be remembered on the device where you logged in from.
            // This is similar to the RememberMe option when you log in.
            app.UseTwoFactorRememberBrowserCookie(DefaultAuthenticationTypes.TwoFactorRememberBrowserCookie);

            // Uncomment the following lines to enable logging in with third party login providers
            //app.UseMicrosoftAccountAuthentication(
            //    clientId: "",
            //    clientSecret: "");

            //app.UseTwitterAuthentication(
            //   consumerKey: "",
            //   consumerSecret: "");

            //app.UseFacebookAuthentication(
            //   appId: "",
            //   appSecret: "");

            app.UseGoogleAuthentication(new GoogleOAuth2AuthenticationOptions()
            {
                ClientId = ConfigurationManager.AppSettings["GoogleClientId"],
                ClientSecret = ConfigurationManager.AppSettings["GoogleClientSecret"]

                //ClientId = "39712937321-55uks6u2nodbue4e7d99lavdgc3q6vmh.apps.googleusercon tent.com",
                //  ClientSecret = "FvLm7KYou0ua4wwWY_hZe1cq"

                //ClientId = "39712937321-ejhqrplcire6a7m1pq4vccilvsblq6qm.apps.googleusercontent.com",
                //ClientSecret = "Bvl4rxNFPZAzJWeSzGI37cAA"
            });
            //GlobalHost.DependencyResolver.Register(typeof(AccountController),() => new AccountController(new AuthHub()));
            //app.MapSignalR();
        }
    }


    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {



            ConfigureAuth(app);
        }
    }
}