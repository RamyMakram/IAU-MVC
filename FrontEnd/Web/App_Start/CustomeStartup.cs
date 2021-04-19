using Microsoft.Owin.Extensions;
using Microsoft.Owin.Security;
using Microsoft.Owin.Security.Cookies;
using Microsoft.Owin.Security.WsFederation;
using Owin;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using System.Web;

namespace Web.App_Start
{
    public partial class Startup
    {
        private static string realm = ConfigurationManager.AppSettings["ida:Wtrealm"];
        private static string adfsMetadata = ConfigurationManager.AppSettings["ida:ADFSMetadata"];
        private static string adfsWreply = ConfigurationManager.AppSettings["ida:Wreply"];

        public void ConfigureAuth(IAppBuilder app)
        {
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