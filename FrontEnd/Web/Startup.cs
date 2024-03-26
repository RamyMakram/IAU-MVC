using Microsoft.Owin;
using Microsoft.Owin.Security.Cookies;
using Microsoft.Owin.Security.WsFederation;
using Owin;
using System;
using System.Net.Security;
using System.Net;
using System.Threading.Tasks;
using System.Security.Cryptography.X509Certificates;
using Microsoft.AspNet.Identity;
using System.Configuration;
using Microsoft.Owin.Extensions;
using Microsoft.Owin.Security;
using System.Security.Claims;

[assembly: OwinStartup(typeof(Web.Startup))]

namespace Web
{
    public class Startup
    {
        private static string realm = ConfigurationManager.AppSettings["ida:Wtrealm"];
        private static string adfsMetadata = ConfigurationManager.AppSettings["ida:ADFSMetadata"];
        private static string adfsWreply = ConfigurationManager.AppSettings["ida:Wreply"];

        public void Configuration(IAppBuilder app)
        { // Configure the db context, user manager and signin manager to use a single instance per request
            app.SetDefaultSignInAsAuthenticationType(CookieAuthenticationDefaults.AuthenticationType);
            app.UseCookieAuthentication(new CookieAuthenticationOptions { CookieName = "MostafeedSystem" });
            app.UseCookieAuthentication(new CookieAuthenticationOptions());

            app.UseWsFederationAuthentication(
                new WsFederationAuthenticationOptions
                {
                    Wtrealm = realm,
                    MetadataAddress = adfsMetadata,
                    Notifications = new WsFederationAuthenticationNotifications()
                    {
                        // this method will be invoked after login succes , for the first login
                        SecurityTokenValidated = context =>
                        {
                            ClaimsIdentity identity = context.AuthenticationTicket.Identity;

                            return Task.FromResult(0);
                        },
                        RedirectToIdentityProvider = context =>
                        {
                            context.ProtocolMessage.Wreply = adfsWreply;

                            return Task.FromResult(0);
                        }

                    },
                });
            app.UseStageMarker(PipelineStage.Authenticate);
        }
    }
}
