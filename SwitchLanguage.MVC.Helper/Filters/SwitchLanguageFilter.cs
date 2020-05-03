using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

namespace SwitchLanguage.MVC.Helper.Filters
{
    public class SwitchLanguageFilter : IActionFilter
    {
        public string LanguageParameterName { get; set; }
        public string LanguageCookieName { get; set; }
        public int LanguageCookieExpirationDays { get; set; }

        public SwitchLanguageFilter()
        {
            LanguageParameterName = "language";
            LanguageCookieName = "CurrentUICulture";
            LanguageCookieExpirationDays = 0;
        }

        public virtual void OnActionExecuting(ActionExecutingContext filterContext)
        {
            // load the culture info from the cookie
            var cookie = filterContext.HttpContext.Request.Cookies[LanguageCookieName];

            if ((filterContext.HttpContext.Request.QueryString[LanguageParameterName] != null) && !string.IsNullOrWhiteSpace(filterContext.HttpContext.Request.QueryString[LanguageParameterName].ToString()))
            {
                // set the culture from the route data (url)
                Thread.CurrentThread.CurrentUICulture = CultureInfo.CreateSpecificCulture(filterContext.HttpContext.Request.QueryString[LanguageParameterName].ToString());
            }
            else if ((cookie != null))
            {
                // set the culture by the cookie content
                Thread.CurrentThread.CurrentUICulture = CultureInfo.CreateSpecificCulture(cookie.Value);
            }
            else if (filterContext.HttpContext.Request.UserLanguages != null && filterContext.HttpContext.Request.UserLanguages.Count() > 0)
            {
                CultureInfo culture = CultureInfo.CreateSpecificCulture(filterContext.HttpContext.Request.UserLanguages[0]);
            }


            // save the location into cookie
            cookie = new HttpCookie(LanguageCookieName, Thread.CurrentThread.CurrentUICulture.Name);
            if (LanguageCookieExpirationDays > 0)
                cookie.Expires = DateTime.Now.AddDays(LanguageCookieExpirationDays);
            filterContext.HttpContext.Response.SetCookie(cookie);
        }

        public virtual void OnActionExecuted(ActionExecutedContext filterContext)
        {
        }
    }
}
