using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Web;
using Microsoft.Web.WebSockets;
namespace IAUBackEnd.Admin
{
    public class WebSocketManager : WebSocketHandler
    {
        private static WebSocketCollection clients = new WebSocketCollection();
        private static Dictionary<string, WebSocketCollection> Mapped = new Dictionary<string, WebSocketCollection>();
        public override void OnOpen()
        {
            try
            {
                var domain = HttpContext.Current.Request.Url.Authority;

                string name = this.WebSocketContext.QueryString["Name"];
                Mapped[domain + "-" + name] = new WebSocketCollection() { this };

                clients.Add(this);
            }
            catch (Exception eee)
            {
            }
        }
        public static void SendTo(string Name, string Message)
        {
            var domain = HttpContext.Current.Request.Url.Authority;
            Mapped[domain + "-" + Name].Broadcast(Message);
        }
        public static void SendLogout(string Name)
        {
            var domain = HttpContext.Current.Request.Url.Authority;

            if (Mapped.ContainsKey(Name))
            {
                Mapped[Name].Broadcast("Out");
                var ws_User = Mapped[domain + "-" + Name];
                Mapped.Remove(domain + "-" + Name);
            }
        }
        public static void SendToMulti(int[] Names, string Message)
        {
            var domain = HttpContext.Current.Request.Url.Authority;

            foreach (var i in Names)
                if (Mapped.ContainsKey(i.ToString()))
                    Mapped[domain + "-" + i.ToString()].Broadcast(Message);
        }

        public override void OnMessage(byte[] message)
        {
            //clients.Broadcast(string.Format("{0} said: {1}", name, message));
        }
        public override void OnClose()
        {
            var domain = HttpContext.Current.Request.Url.Authority;

            clients.Remove(this);
            var WillBeRemoved = Mapped.Where(q => q.Value.Contains(this)).Select(q => q.Key);
            foreach (var i in WillBeRemoved)
                Mapped[domain + "-" + i].Remove(this);
        }
    }
}