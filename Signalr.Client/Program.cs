using Microsoft.AspNet.SignalR.Client;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace Signalr.Client
{
    class Program
    {
        static void Main(string[] args)
        {
            var hubConnection = new HubConnection("https://localhost:8089/");
            //hubConnection.TraceLevel = TraceLevels.All;
            //hubConnection.TraceWriter = Console.Out;
            IHubProxy hubProxy = hubConnection.CreateHubProxy("SignalrHub");
            hubProxy.On("recieveMsg", msgModel =>
             {
                 Console.WriteLine("UserName: {1} ,Incoming msg: {0}", msgModel.msg, msgModel.name);
             });
            //hubProxy.On<string, string>("recieveMsg", (name, msg) =>
            //  {
            //      Console.WriteLine("UserName: {1} ,Incoming msg: {0}", msg, name);
            //  });
            ServicePointManager.DefaultConnectionLimit = 10;
            hubConnection.Start().Wait();
            Console.ReadLine();
        }

    }
}
