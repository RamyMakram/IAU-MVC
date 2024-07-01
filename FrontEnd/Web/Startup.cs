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
using Microsoft.IdentityModel.Logging;
using System.IO;
using Newtonsoft.Json;
using Microsoft.Owin.Security.DataHandler.Encoder;
using Microsoft.Owin.Security.Jwt;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.Owin.Security.OAuth;
using System.IdentityModel.Tokens.Jwt;
using System.Web;

[assembly: OwinStartup(typeof(Web.Startup))]

namespace Web
{
    public class Startup
    {
        private static string realm = ConfigurationManager.AppSettings["ida:Wtrealm"];
        private static string adfsMetadata = ConfigurationManager.AppSettings["ida:ADFSMetadata"];
        private static string adfsWreply = ConfigurationManager.AppSettings["ida:Wreply"];
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
        public void ConfigureServices(IServiceCollection services)
        {
            //// ...

            //services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
            //    .AddJwtBearer(options =>
            //    {
            //        options.Authority = "https://iauauth.iau.edu.sa";
            //        options.TokenValidationParameters = new TokenValidationParameters
            //        {
            //            ValidateIssuer = true,
            //            ValidIssuer = "https://iauauth.iau.edu.sa/", // The issuer of the ADFS token
            //            ValidateAudience = true,
            //            ValidAudience = "https://mustafid.iau.edu.sa/", // The audience (client app) of the ADFS token
            //            ValidateLifetime = true,
            //            ValidateIssuerSigningKey = true,
            //            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes("your-signing-key")) // The signing key used by ADFS
            //        };
            //    });

            //// ...
        }

        public void ConfigureAuth(IAppBuilder app)
        { // Configure the db context, user manager and signin manager to use a single instance per request
            var issuer = "https://iauauth.iau.edu.sa/";
            var audienceId = "https://mustafid.iau.edu.sa/";
            var audienceSecret = "wsws k jhqbwswqw22";
            var path = HttpContext.Current.Server.MapPath("~");


            // Api controllers with an [Authorize] attribute will be validated with JWT
            app.UseJwtBearerAuthentication(new JwtBearerAuthenticationOptions
            {
                AuthenticationMode = AuthenticationMode.Active,
                AllowedAudiences = new[] { audienceId },
                IssuerSecurityKeyProviders = new IIssuerSecurityKeyProvider[]
                                   {
                                       new SymmetricKeyIssuerSecurityKeyProvider(issuer, audienceSecret)
                                   }
            });
            IdentityModelEventSource.ShowPII = true;
            ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;
            app.SetDefaultSignInAsAuthenticationType(CookieAuthenticationDefaults.AuthenticationType);
            app.UseCookieAuthentication(new CookieAuthenticationOptions { CookieName = "MostafeedSystem" });
            app.UseCookieAuthentication(new CookieAuthenticationOptions());

            app.UseWsFederationAuthentication(
                new WsFederationAuthenticationOptions
                {
                    Wtrealm = realm,

                    MetadataAddress = adfsMetadata,
                    TokenValidationParameters = new TokenValidationParameters
                    {
                        ValidateIssuerSigningKey = false,
                        ValidAudience = "https://mustafid.iau.edu.sa/",
                        ValidateAudience = false,
                    },
                    Notifications = new WsFederationAuthenticationNotifications()
                    {
                        // this method will be invoked after login succes , for the first login
                        SecurityTokenValidated = context =>
                        {
                            //File.WriteAllText(Path.Combine(path, $"{DateTime.Now.Ticks}.json"), JsonConvert.SerializeObject(context, new JsonSerializerSettings { ReferenceLoopHandling = ReferenceLoopHandling.Ignore }));
                            ClaimsIdentity identity = context.AuthenticationTicket.Identity;
                            try
                            {
                                File.WriteAllText(Path.Combine(path, $"{DateTime.Now.Ticks}_identity.json"), JsonConvert.SerializeObject(identity, new JsonSerializerSettings { ReferenceLoopHandling = ReferenceLoopHandling.Ignore }));
                            }
                            catch (Exception)
                            {
                            }
                            return Task.FromResult(0);
                        },
                        AuthenticationFailed = context =>
                        {
                            File.WriteAllText(Path.Combine(path, $"Failed_{DateTime.Now.Ticks}.json"), JsonConvert.SerializeObject(new { Exception = context.Exception, context.State }));
                            //ClaimsIdentity identity = context.AuthenticationTicket.Identity;
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
