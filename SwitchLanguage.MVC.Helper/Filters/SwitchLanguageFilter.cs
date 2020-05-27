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
    public enum LanguageParameterType
    {
        QueryString,
        RouteValue
    }

    /// <summary>
    /// The class SwitchLanguageFilter implements the interface <c>IActionFilter</c>; in particular the method <c>OnActionExecuting</c>. That means that the filter
    /// every time an action is called.
    /// <para>The filter searches for information about the selected language in three stages: in the QueryString after the language parameter.If not found,
    /// in the language cookie.If not found,  the browser language. </para>
    /// <para>The language found is then saved in a cookie. This ensures that the language is retained the next time an action is called. Furthermore
    /// it is written into the <c>CurrentThread.CurrentUICulture</c>.This means that it is available throughout the processing of the request.</para>
    /// <para>The filter must be added to the <c>GlobalFilterCollection</c>. This is usually done in the static method <c> RegisterGlobalFilters</c> in
    /// the class <c>FilterConfig</c> (to be found in the "App_Start" folder of the web project).</para>>
    /// </summary>
    public class SwitchLanguageFilter : IActionFilter
    {
        /// <value>
        /// The name of the language parameter in the QueryString.
        /// </value>
        public string LanguageParameterName { get; set; }
        
        /// <value>
        /// The name of the cookie in which the language for the next request and possibly for the next session (depending on the setting of the <c>LanguageCookieExpirationDays</c> value)
        /// is saved.
        /// </value>
        public string LanguageCookieName { get; set; }

        /// <value>
        /// The duration of the language cookie in days. By default, zero days are given here. I. e. the cookie is discarded at the end of the session. A higher value means that the user will 
        /// find his or her selected language when visiting the page again.
        /// </value>
        public int LanguageCookieExpirationDays { get; set; }

        /// <value>
        /// Sets where the language parameter should be determined.
        /// </value>
        public LanguageParameterType LanguageParameterType { get; set; }

        public SwitchLanguageFilter()
        {
            LanguageParameterName = "language";
            LanguageCookieName = "CurrentUICulture";
            LanguageCookieExpirationDays = 0;
            LanguageParameterType = LanguageParameterType.QueryString;
        }

        public virtual void OnActionExecuting(ActionExecutingContext filterContext)
        {
            // load the culture info from the cookie
            var cookie = filterContext.HttpContext.Request.Cookies[LanguageCookieName];

            if ((LanguageParameterType == LanguageParameterType.QueryString) && (filterContext.HttpContext.Request.QueryString[LanguageParameterName] != null) && !string.IsNullOrWhiteSpace(filterContext.HttpContext.Request.QueryString[LanguageParameterName].ToString()))
            {
                // set the culture from the query string
                Thread.CurrentThread.CurrentUICulture = CultureInfo.CreateSpecificCulture(filterContext.HttpContext.Request.QueryString[LanguageParameterName].ToString());
            }
            else if ((LanguageParameterType == LanguageParameterType.RouteValue) && (filterContext.RouteData.Values[LanguageParameterName] != null) && !string.IsNullOrWhiteSpace(filterContext.RouteData.Values[LanguageParameterName].ToString()))
            {
                // set the culture from the route data (url)
                Thread.CurrentThread.CurrentUICulture = CultureInfo.CreateSpecificCulture(filterContext.RouteData.Values[LanguageParameterName].ToString());
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
