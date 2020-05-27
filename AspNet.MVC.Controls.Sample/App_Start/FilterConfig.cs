using SwitchLanguage.MVC.Helper.Filters;
using System.Web;
using System.Web.Mvc;

namespace AspNet.MVC.Controls.Sample
{
    public class FilterConfig
    {
        public static void RegisterGlobalFilters(GlobalFilterCollection filters)
        {
            filters.Add(new HandleErrorAttribute());
            filters.Add(new SwitchLanguageFilter()
            {
                LanguageCookieExpirationDays = 30,
                 LanguageParameterType= LanguageParameterType.RouteValue
            });
        }
    }
}
