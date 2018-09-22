using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Microsoft.AspNet.SignalR;
using AlexaSkillsKitWebApi.Controllers;
using System.Threading.Tasks;

namespace AlexaSkillsKitWebApi2.Hubs
{
    public class ChatHub : Hub
    {
        public void Send(string title, string message)
        {
            Clients.All.addNewMessageToPage(title, message);
        }
    }
}