﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Microsoft.AspNet.SignalR;

namespace AlexaSkillsKitWebApi2.Speechlet
{
    public class TestHub : Hub
    {
        public void Hello()
        {
            Clients.All.hello();
        }
    }
}