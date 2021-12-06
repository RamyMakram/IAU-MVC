using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.WebSockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;

namespace AdminPanel
{
    public class MvcApplication : System.Web.HttpApplication
    {
        protected void Application_Start()
        {
            AreaRegistration.RegisterAllAreas();
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            BundleConfig.RegisterBundles(BundleTable.Bundles);
            MvcHandler.DisableMvcResponseHeader = true;
            //Task.Run(async () =>
            //{
            //    do
            //    {
            //        using (var socket = new ClientWebSocket())
            //            try
            //            {
            //                await socket.ConnectAsync(new Uri("wss://localhost:44344/WSHandler.ashx?Name=35"), CancellationToken.None);
            //                await Receive(socket);

            //            }
            //            catch (Exception ex)
            //            {
            //                Console.WriteLine($"ERROR - {ex.Message}");
            //            }
            //    } while (true);
            //});
        }
        //static async Task Receive(ClientWebSocket socket)
        //{
        //    var buffer = new ArraySegment<byte>(new byte[2048]);
        //    do
        //    {
        //        WebSocketReceiveResult result;
        //        using (var ms = new MemoryStream())
        //        {
        //            do
        //            {
        //                result = await socket.ReceiveAsync(buffer, CancellationToken.None);
        //                ms.Write(buffer.Array, buffer.Offset, result.Count);
        //            } while (!result.EndOfMessage);

        //            if (result.MessageType == WebSocketMessageType.Close)
        //                break;

        //            ms.Seek(0, SeekOrigin.Begin);
        //            using (var reader = new StreamReader(ms, Encoding.UTF8))
        //            {
        //                var data = await reader.ReadToEndAsync();
        //                Console.WriteLine(data);
        //            }
        //        }
        //    } while (true);
        //}
        protected void Application_Error()
        {
            //HttpContext.Current.Response.Redirect(Request.Url.ToString().Replace(Request.Url.PathAndQuery, "") + "/" + "Error");
        }
    }
}
