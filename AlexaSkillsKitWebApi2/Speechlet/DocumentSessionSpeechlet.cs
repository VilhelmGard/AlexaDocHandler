// Copyright 2018 Stefan Negritoiu (FreeBusy) and contributors. See LICENSE file for more information.

// The code below is from the AlexaSkillsKit.Net repository (link to the original: https://github.com/AreYouFreeBusy/AlexaSkillsKit.NET/blob/master/AlexaSkillsKit.Sample/Speechlet/SampleSessionSpeechlet.cs)

using AlexaSkillsKit.Slu;
using AlexaSkillsKit.Speechlet;
using AlexaSkillsKit.UI;
using AlexaSkillsKitWebApi2.Hubs;
using AlexaSkillsKitWebApi2.Models;
using Microsoft.AspNet.SignalR;
using NLog;
using System.Linq;
using System;
using System.Collections.Generic;

namespace AlexaSkillsKitWebApi.Controllers
{

    public class DocumentSessionSpeechlet : SpeechletBase, ISpeechletWithContext
    {
        private static Logger Log = LogManager.GetCurrentClassLogger();

        private const string NAME_KEY = "document";
        private const string NAME_SLOT = "Document";

        

        public void OnSessionStarted(SessionStartedRequest request, Session session, Context context)
        {
            Log.Info("OnSessionStarted requestId={0}, sessionId={1}", request.RequestId, session.SessionId);
        }


        public SpeechletResponse OnLaunch(LaunchRequest request, Session session, Context context)
        {
            Log.Info("OnLaunch requestId={0}, sessionId={1}", request.RequestId, session.SessionId);
            return GetWelcomeResponse();
        }


        public SpeechletResponse OnIntent(IntentRequest request, Session session, Context context)
        {
            Log.Info("OnIntent requestId={0}, sessionId={1}", request.RequestId, session.SessionId);

            Intent intent = request.Intent;
            string intentName = (intent != null) ? intent.Name : null;

            if ("GetDocumentIntent".Equals(intentName))
            {
                return SetNameInSessionAndSayHello(intent, session);
            }
            else if ("ReadDocumentIntent".Equals(intentName))
            {
                return GetNameFromSessionAndSayHello(intent, session);
            }
            else if ("AMAZON.CancelIntent".Equals(intentName))
            {
                return CloseSession(intent, session);
            }
            else
            {
                throw new SpeechletException("Invalid Intent");
            }
        }



        public void OnSessionEnded(SessionEndedRequest request, Session session, Context context)
        {
            Log.Info("OnSessionEnded requestId={0}, sessionId={1}", request.RequestId, session.SessionId);
        }

        public SpeechletResponse GetWelcomeResponse()
        {

            string speechOutput = "Welcome to the Evry document handler, get documents by saying, get me document, then the document i.d.";

            return BuildSpeechletResponse("Welcome", speechOutput, false);
        }


        public SpeechletResponse SetNameInSessionAndSayHello(Intent intent, Session session)
        {
            bool shouldEndSession = false;

            IDictionary<string, Slot> slots = intent.Slots;
            
            Slot nameSlot = slots[NAME_SLOT];
            string speechOutput = "";
            
            if (nameSlot != null)
            {
                string name = nameSlot.Value;

                string docName = SPDocList().Where(d => d.Id.ToString() == name).SingleOrDefault()?.Name;

                session.Attributes[NAME_KEY] = name;
                speechOutput = String.Format(
                    "{0} selected, you can get the content by saying, show me the content", docName);

                if (String.IsNullOrEmpty(docName))
                {
                    speechOutput = String.Format(
                        "I'm not sure what document you want, please try again");
                    shouldEndSession = true;
                    docName = "Document could not be ";
                }

                var context = GlobalHost.ConnectionManager.GetHubContext<ChatHub>();

                context.Clients.All.addNewMessageToPage(" Alexa", docName);
            }
            else
            {
                speechOutput = "I'm not sure what document you want, please try again";
            }
            
            return BuildSpeechletResponse(intent.Name, speechOutput, shouldEndSession);
        }

        public SpeechletResponse GetNameFromSessionAndSayHello(Intent intent, Session session)
        {
            string speechOutput = "";
            bool shouldEndSession = false;

            string name = (String)session.Attributes[NAME_KEY];

            if (!String.IsNullOrEmpty(name))
            {
                foreach (Document doc in SPDocList().Where(d => d.Id.ToString() == name))
                {
                    speechOutput = String.Format("You should see the content on the screen now");


                    var context = GlobalHost.ConnectionManager.GetHubContext<ChatHub>();

                    context.Clients.All.addNewMessageToPage(" Alexa", doc.Content);

                    shouldEndSession = true;
                }
            }
            else
            {
                speechOutput = ("I could not get the content of the document, get back to coding");
            }

            return BuildSpeechletResponse(intent.Name, speechOutput, shouldEndSession);
        }

        private SpeechletResponse CloseSession(Intent intent, Session session)
        {
            string speechOutput = "Closing session";

            bool shouldEndSession = true;

            return BuildSpeechletResponse(intent.Name, speechOutput, shouldEndSession);
        }

        public SpeechletResponse BuildSpeechletResponse(string title, string output, bool shouldEndSession)
        {


            SimpleCard card = new SimpleCard
            {
                Title = String.Format("Documenthandler - {0}", title),
                Content = String.Format("Documenthandler - {0}", output)
            };

            PlainTextOutputSpeech speech = new PlainTextOutputSpeech
            {
                Text = output
            };

            SpeechletResponse response = new SpeechletResponse
            {
                ShouldEndSession = shouldEndSession,
                OutputSpeech = speech,
                Card = card
            };

            return response;
        }

        public List<Document> SPDocList()
        {
            return SharePointService.DocsInList();
        }    
    }
}
