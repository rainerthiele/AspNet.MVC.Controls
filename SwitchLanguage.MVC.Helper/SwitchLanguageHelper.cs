using SwitchLanguage.MVC.Helper.Filters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;
using System.Web.Routing;

namespace SwitchLanguage.MVC.Helper
{
    /// <summary>
    /// An url helper extension to create links to switch the language.
    /// </summary>
    public static class SwitchLanguageHelper
    {
        /// <summary>
        /// Generates a url that points to the current page and also appends the language parameter as a query string.
        /// The name of the parameter is set via the properties of the  <see cref="SwitchLanguageFilter"/>.
        /// <param name="cultureName">
        /// Contains the language to be switched to. This can either be a valid CultureName (e.g. "en-US") or a valid <c>TwoLetterISOLanguageName</c> (e.g. "en"). This CultureName 
        /// is set via the <c>SwitchLanguageFilter</c> in the <c>CurrentUICulture</c> of the CurrentThread.
        /// </param>
        /// </summary>
        public static string SwitchLanguageUrl(this UrlHelper url, string cultureName)
        {
            RouteValueDictionary routeValues = CreateLanguageRouteValues(url, cultureName);

            return url.RouteUrl(routeValues);
        }

        private static RouteValueDictionary CreateLanguageRouteValues(UrlHelper helper, String cultureName)
        {
            string languageParameterName = "language";
            var filter = GlobalFilters.Filters.Where(f => f.Instance.GetType() == typeof(SwitchLanguageFilter)).FirstOrDefault();
            if (filter != null)
            {
                languageParameterName = ((SwitchLanguageFilter)filter.Instance).LanguageParameterName;
            }

            // retrieve the route values from the view context
            RouteValueDictionary routeValues = new RouteValueDictionary(helper.RequestContext.RouteData.Values);

            // copy the query strings into the route values to generate the link
            var queryString = helper.RequestContext.HttpContext.Request.QueryString;

            foreach (string key in queryString)
            {
                if (queryString[key] != null && !string.IsNullOrWhiteSpace(key))
                {
                    if (routeValues.ContainsKey(key))
                    {
                        routeValues[key] = queryString[key];
                    }
                    else
                    {
                        routeValues.Add(key, queryString[key]);
                    }
                }
            }

            // set the language into route values
            routeValues[languageParameterName] = cultureName.ToLower();

            return routeValues;
        }

    }
}
