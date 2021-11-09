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
            string name = this.WebSocketContext.QueryString["Name"];
            Mapped[name] = new WebSocketCollection() { this };
            clients.Add(this);
        }
        public static void SendTo(string Name, string Message)
        {
            Mapped[Name].Broadcast(Message);
        }
        public static void SendLogout(string Name)
        {
            Mapped[Name].Broadcast("Out");
            var ws_User = Mapped[Name];
            Mapped.Remove(Name);
        }
        public static void SendToMulti(int[] Names, string Message)
        {
            foreach (var i in Names)
                if (Mapped.ContainsKey(i.ToString()))
                    Mapped[i.ToString()].Broadcast(Message);
        }

        public override void OnMessage(byte[] message)
        {
            //clients.Broadcast(string.Format("{0} said: {1}", name, message));
        }
        public override void OnClose()
        {
            clients.Remove(this);
            var WillBeRemoved = Mapped.Where(q => q.Value.Contains(this)).Select(q => q.Key);
            foreach (var i in WillBeRemoved)
                Mapped[i].Remove(this);
        }
    }
}