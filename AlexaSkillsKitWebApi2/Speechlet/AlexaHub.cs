using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Microsoft.AspNet.SignalR;
using AlexaSkillsKitWebApi.Controllers;

namespace AlexaSkillsKitWebApi2.Speechlet
{
    public class AlexaHub : Hub
    {
        public void Hello()
        {
            var speechlet = new DocumentSessionSpeechlet();

            string docNam = speechlet.docNam;
            string docCon = speechlet.docCon;

            Clients.All.hello();
        }
    }
}