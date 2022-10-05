using Hangfire;
using IAUBackEnd.Admin.Models;
using Microsoft.Owin;
using Owin;
using System;
using System.Threading.Tasks;

[assembly: OwinStartup(typeof(IAUBackEnd.Admin.Startup))]

namespace IAUBackEnd.Admin
{
    public class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            var context = new TasahelEntities();

            GlobalConfiguration.Configuration
                .UseSqlServerStorage(context.Database.Connection.ConnectionString);

            app.UseHangfireDashboard();
            app.UseHangfireServer();

            RecurringJob.AddOrUpdate("Delayed Request", () => Helper.DelayedRequest(), Cron.Daily(1));

        }
    }
}
