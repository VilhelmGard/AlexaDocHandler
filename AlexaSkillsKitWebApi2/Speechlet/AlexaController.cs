using AlexaSkillsKitWebApi.Controllers;
using System.Net.Http;
using System.Web.Http;

namespace Sample.Controllers
{
    public class AlexaController : ApiController
    {
        [Route("alexa/documentHandler-session")]
        [HttpPost]
        public HttpResponseMessage SampleSession()
        {
            var speechlet = new DocumentSessionSpeechlet();
            
            return speechlet.GetResponse(Request);
        }
    }
}
