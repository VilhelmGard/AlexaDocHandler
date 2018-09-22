using AlexaSkillsKitWebApi2.Models;
using System.Collections.Generic;
using System.Web.Mvc;

namespace AlexaSkillsKitWebApi2.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Chat()
        {
            List<Document> DocList = SharePointService.DocsInList();

            return View(DocList);
        }
    }
}
