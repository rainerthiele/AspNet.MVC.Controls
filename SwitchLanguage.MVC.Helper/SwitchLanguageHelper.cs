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
    public static class SwitchLanguageHelper
    {
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
