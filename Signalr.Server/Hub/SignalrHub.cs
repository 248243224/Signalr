using System;
using System.Collections.Generic;
using System.Web;
using Microsoft.AspNet.SignalR;
using Microsoft.AspNet.SignalR.Hubs;

namespace Signalr.Server.Hub
{
    [HubName("SignalrHub")]
    public class SignalrHub : Microsoft.AspNet.SignalR.Hub
    {
        internal static void SendMsg(MsgModel model)
        {
            var _context = GlobalHost.ConnectionManager.GetHubContext<SignalrHub>();
            _context.Clients.All.recieveMsg(model);
        }
    }

    public class MsgModel
    {
        public string name { get; set; }
        public string msg { get; set; }
    }
}