using System.Net.Http;
using System;
using Microsoft.Extensions.Configuration;

namespace MustafidApp.Helpers
{
    public class HttpClientAdminBackend
    {
        public static HttpResponseMessage getDataAdmin(string apiName, IConfiguration configuration)
        {
            HttpClient h = new HttpClient();

            h.BaseAddress = new Uri(configuration["AdminPanel_BE_URL"]);

            h.DefaultRequestHeaders.Add("IsTwasul_OC", "true");
            h.DefaultRequestHeaders.Add("crd", "dkvkk45523g2ejieiisncbgey@jn#Wuhuhe6&&*bhjbde4w7ee7@k309m$.f,dkks");

            var res = h.GetAsync("/api/" + apiName);
            return res.Result;
        }
    }
}
