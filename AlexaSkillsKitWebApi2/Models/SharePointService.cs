using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using SP = Microsoft.SharePoint.Client;
using System.IO;
using DocumentFormat.OpenXml.Packaging;
using DocumentFormat.OpenXml.Wordprocessing;
using System.Security;

namespace AlexaSkillsKitWebApi2.Models
{
    public class SharePointService
    {
        public static SecureString ToSecureString(string Source)
        {
            if (string.IsNullOrWhiteSpace(Source))
                return null;
            else
            {
                SecureString Result = new SecureString();
                foreach (char c in Source.ToCharArray())
                    Result.AppendChar(c);
                return Result;
            }
        }

        static private void CopyStream(Stream source, Stream destination)
        {
            byte[] buffer = new byte[32768];
            int bytesRead;
            do
            {
                bytesRead = source.Read(buffer, 0, buffer.Length);
                destination.Write(buffer, 0, bytesRead);
            }
            while (bytesRead != 0);
        }

        public static List<Models.Document> DocsInList()
        {
            string webSPOUrl = "https://yourdevsite";
            string userName = "your@username";
            string password = "password";

            //TODO: set up Azure AD login

            int count = 0;

            string text = "";

            List<Models.Document> docsInList = new List<Models.Document>();

            using (var clientContext = new SP.ClientContext(webSPOUrl))
            {
                clientContext.Credentials = new SP.SharePointOnlineCredentials(userName, ToSecureString(password));

                SP.Web web = clientContext.Web;

                SP.List sharedDocumentsList = clientContext.Web.Lists.GetByTitle("Dokument");

                SP.CamlQuery camlQuery = new SP.CamlQuery();

                SP.ListItemCollection listItems = sharedDocumentsList.GetItems(camlQuery);

                clientContext.Load(sharedDocumentsList);

                clientContext.Load(listItems);

                clientContext.ExecuteQuery();

                if (listItems.Count != 0)
                {
                    foreach (SP.ListItem item in listItems)
                    {
                        count++;

                        string fileName = (String)item["FileLeafRef"];

                        SP.FileInformation fileInformation = SP.File.OpenBinaryDirect(clientContext, (string)item["FileRef"]);

                        using (MemoryStream memoryStream = new MemoryStream())
                        {
                            CopyStream(fileInformation.Stream, memoryStream);

                            using (WordprocessingDocument doc = WordprocessingDocument.Open(memoryStream, false))
                            {
                                var firstParagraph = doc.MainDocumentPart.Document.Body;

                                if (firstParagraph != null)
                                {
                                    text = firstParagraph.InnerText;
                                }
                                else
                                    text = "Document doesn't contain any paragraphs.";
                            }
                        }
                        docsInList.Add(new Models.Document(count, fileName, text));
                    }
                }
                else
                    text = "Document not found.";
            }
            return docsInList;
        }
        
    }
}