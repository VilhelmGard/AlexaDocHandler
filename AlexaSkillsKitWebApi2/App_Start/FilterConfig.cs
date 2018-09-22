using System.Web;
using System.Web.Mvc;

namespace AlexaSkillsKitWebApi2
{
    public class FilterConfig
    {
        public static void RegisterGlobalFilters(GlobalFilterCollection filters)
        {
            filters.Add(new HandleErrorAttribute());
        }
    }
}
