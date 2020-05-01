using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Web;
using System.Web.Mvc;
using System.Web.Mvc.Html;
using System.Web.Routing;
using System.Web.Optimization;

namespace AjaxDopDowns.MVC.Control
{
    // @Html.ScriptDropDownListFor(m => m.Id, "/EmployeeGroup/List", new SelectListItem() { Text = "", Value = "" } )

    public static class ScriptDropDownList
    {
        public static IHtmlString ScriptDropDownListFor<TModel, TProperty>(this HtmlHelper<TModel> htmlHelper, Expression<Func<TModel, TProperty>> expression, string readUrl, object htmlAttributes)
        {
            var attributes = HtmlHelper.AnonymousObjectToHtmlAttributes(htmlAttributes);
            return ScriptDropDownListFor(htmlHelper, expression, readUrl, null, attributes);
        }

        public static IHtmlString ScriptDropDownListFor<TModel, TProperty>(this HtmlHelper<TModel> htmlHelper, Expression<Func<TModel, TProperty>> expression, string readUrl)
        {
            return ScriptDropDownListFor(htmlHelper, expression, readUrl, null, null);
        }

        public static IHtmlString ScriptDropDownListFor<TModel, TProperty>(this HtmlHelper<TModel> htmlHelper, Expression<Func<TModel, TProperty>> expression, string readUrl, SelectListItem optionItem, object htmlAttributes)
        {
            var attributes = HtmlHelper.AnonymousObjectToHtmlAttributes(htmlAttributes);
            return ScriptDropDownListFor(htmlHelper, expression, readUrl, optionItem, attributes);
        }

        public static IHtmlString ScriptDropDownListFor<TModel, TProperty>(this HtmlHelper<TModel> htmlHelper, Expression<Func<TModel, TProperty>> expression, string readUrl, SelectListItem optionItem)
        {
            return ScriptDropDownListFor(htmlHelper, expression, readUrl, optionItem, null);
        }

        public static IHtmlString ScriptDropDownListFor<TModel, TProperty>(this HtmlHelper<TModel> htmlHelper, Expression<Func<TModel, TProperty>> expression, string readUrl, SelectListItem optionItem, RouteValueDictionary htmlAttributes)
        {
            if (htmlHelper.ViewContext.HttpContext.Items["ScriptDropDownListInitReadUrls"] == null)
                htmlHelper.ViewContext.HttpContext.Items["ScriptDropDownListInitReadUrls"] = new Dictionary<string, string>();
            Dictionary<string, string> scriptDropDownListInitReadUrls = (Dictionary<string, string>)htmlHelper.ViewContext.HttpContext.Items["ScriptDropDownListInitReadUrls"];

            var metadata = ModelMetadata.FromLambdaExpression(expression, htmlHelper.ViewData);
            string value = (metadata.Model != null) ? metadata.Model.ToString() : "";

            var select = new TagBuilder("select");
            if (htmlAttributes != null)
                select.MergeAttributes(htmlAttributes);
            select.MergeAttribute("name", htmlHelper.NameFor(expression).ToString());
            select.MergeAttribute("data-value", value);
            if (optionItem != null)
            {
                select.MergeAttribute("data-option-value", optionItem.Value);
                select.MergeAttribute("data-option-text", optionItem.Text);
            }
            select.GenerateId(htmlHelper.NameFor(expression).ToString());
            string id = select.Attributes["id"];
            string dataname;
            bool mustRenderInitFunction = false;
            string initFunctionName;

            if (htmlHelper.ViewContext.HttpContext.Items["ScriptDropDownListInitFunctionName"] == null)
                htmlHelper.ViewContext.HttpContext.Items["ScriptDropDownListInitFunctionName"] = "f" + htmlHelper.GetHashCode().ToString("x");
            initFunctionName = htmlHelper.ViewContext.HttpContext.Items["ScriptDropDownListInitFunctionName"].ToString();

            if (htmlHelper.ViewContext.HttpContext.Items["MustRenderScriptDropDownListInitFunction"] == null)
            {
                htmlHelper.ViewContext.HttpContext.Items["MustRenderScriptDropDownListInitFunction"] = false;
                mustRenderInitFunction = true;
            }

            if (!scriptDropDownListInitReadUrls.ContainsKey(readUrl))
            {
                dataname = "d" + select.GetHashCode().ToString("x");
                scriptDropDownListInitReadUrls.Add(readUrl, dataname);
                RouteValueDictionary routeValues = ParseUrl(htmlHelper, readUrl);
                routeValues.Add("dataname", dataname);

                string controller = routeValues["controller"].ToString();
                string action = routeValues["action"].ToString();
                routeValues.Remove("controller");
                routeValues.Remove("action");

                htmlHelper.RenderAction(action, controller, routeValues);
            }
            else dataname = scriptDropDownListInitReadUrls[readUrl];

            var script = new StringBuilder("\n");
            script.AppendLine("<script>");
            if (mustRenderInitFunction)
                script.AppendLine(GetInitFunctionScript(initFunctionName));
            script.AppendLine(GetInitCallScript(id, dataname, initFunctionName));
            script.AppendLine("</script>");

            var html = select.ToString(TagRenderMode.Normal);

            return MvcHtmlString.Create(html + script.ToString());
        }

        private static RouteValueDictionary ParseUrl(HtmlHelper htmlHelper, string url)
        {
            var originRequest = htmlHelper.ViewContext.RequestContext.HttpContext.Request;

            if (!Uri.IsWellFormedUriString(url, UriKind.Absolute))
            {
                url = new Uri(originRequest.Url, url).AbsoluteUri;
            }

            int queryIdx = url.IndexOf('?');
            string queryString = null;
            string parseUrl = url;

            if (queryIdx != -1)
            {
                parseUrl = url.Substring(0, queryIdx);
                queryString = url.Substring(queryIdx + 1);
            }

            // Extract the data and render the action.    
            RouteValueDictionary result;
            var request = new HttpRequest(null, parseUrl, queryString);
            var response = new HttpResponse(new StringWriter());
            var httpContext = new HttpContext(request, response);
            var routeData = RouteTable.Routes.GetRouteData(new HttpContextWrapper(httpContext));
            var values = routeData.Values;
            if (!string.IsNullOrEmpty(queryString))
            {
                var valuesCollection = HttpUtility.ParseQueryString(queryString);
                result = new RouteValueDictionary(valuesCollection.AllKeys.ToDictionary(k => k, k => (object)valuesCollection[k]));
            }
            else
            {
                result = new RouteValueDictionary();
            }

            foreach (var key in routeData.Values.Keys)
                result.Add(key, routeData.Values[key]);

            return result;
        }

        private static string GetInitCallScript(string id, string dataName, string initFunctionName)
        {
            var result = Resources.Scripts.ScriptDropDownListInitCall.Replace("{id}", id).Replace("'{dataname}'", dataName);
            result = BundleTable.EnableOptimizations ? result.Replace("initScriptDropDownList", initFunctionName) : result;
            return result;
        }
        private static string GetInitFunctionScript(string initFunctionName)
        {
            var result = BundleTable.EnableOptimizations ? Resources.Scripts.ScriptDropDownListInitFunction_es5_min : Resources.Scripts.ScriptDropDownListInitFunction;
            result = BundleTable.EnableOptimizations ? result.Replace("initScriptDropDownList", initFunctionName) : result;
            return result;
        }
    }
}