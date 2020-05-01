using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Web;
using System.Web.Mvc;
using System.Web.Mvc.Html;
using System.Web.Optimization;
using System.Web.Routing;

namespace AjaxDopDowns.MVC.Control
{
    // @Html.AjaxDropDownListFor(m => m.Id, "1234", "/EmployeeGroup/List", new SelectListItem() { Text = "", Value = "" } )

    public static class AjaxDropDownList
    {
        public static IHtmlString AjaxDropDownListFor<TModel, TProperty>(this HtmlHelper<TModel> htmlHelper, Expression<Func<TModel, TProperty>> expression, string readUrl, object htmlAttributes)
        {
            var attributes = HtmlHelper.AnonymousObjectToHtmlAttributes(htmlAttributes);
            return AjaxDropDownListFor(htmlHelper, expression, readUrl, null, attributes);
        }

        public static IHtmlString AjaxDropDownListFor<TModel, TProperty>(this HtmlHelper<TModel> htmlHelper, Expression<Func<TModel, TProperty>> expression, string readUrl)
        {
            return AjaxDropDownListFor(htmlHelper, expression, readUrl, null, null);
        }

        public static IHtmlString AjaxDropDownListFor<TModel, TProperty>(this HtmlHelper<TModel> htmlHelper, Expression<Func<TModel, TProperty>> expression, string readUrl, SelectListItem optionItem, object htmlAttributes)
        {
            var attributes = HtmlHelper.AnonymousObjectToHtmlAttributes(htmlAttributes);
            return AjaxDropDownListFor(htmlHelper, expression, readUrl, optionItem, attributes);
        }

        public static IHtmlString AjaxDropDownListFor<TModel, TProperty>(this HtmlHelper<TModel> htmlHelper, Expression<Func<TModel, TProperty>> expression, string readUrl, SelectListItem optionItem)
        {
            return AjaxDropDownListFor(htmlHelper, expression, readUrl, optionItem, null);
        }

        public static IHtmlString AjaxDropDownListFor<TModel, TProperty>(this HtmlHelper<TModel> htmlHelper, Expression<Func<TModel, TProperty>> expression, string readUrl, SelectListItem optionItem, RouteValueDictionary htmlAttributes)
        {
            var metadata = ModelMetadata.FromLambdaExpression(expression, htmlHelper.ViewData);
            string value = (metadata.Model != null) ? metadata.Model.ToString() : "";

            var select = new TagBuilder("select");
            if (htmlAttributes != null)
                select.MergeAttributes(htmlAttributes);
            select.MergeAttribute("name", htmlHelper.NameFor(expression).ToString());
            select.MergeAttribute("data-url", readUrl);
            select.MergeAttribute("data-value", value);
            if (optionItem != null)
            {
                select.MergeAttribute("data-option-value", optionItem.Value);
                select.MergeAttribute("data-option-text", optionItem.Text);
            }
            select.GenerateId(htmlHelper.NameFor(expression).ToString());
            string id = select.Attributes["id"];
            bool mustRenderInitFunction = false;
            string initFunctionName;

            if (htmlHelper.ViewContext.HttpContext.Items["AjaxDropDownListInitFunctionName"] == null)
                htmlHelper.ViewContext.HttpContext.Items["AjaxDropDownListInitFunctionName"] = "f" + htmlHelper.GetHashCode().ToString("x");
            initFunctionName = htmlHelper.ViewContext.HttpContext.Items["AjaxDropDownListInitFunctionName"].ToString();

            if (htmlHelper.ViewContext.HttpContext.Items["MustRenderAjaxDropDownListInitFunction"] == null)
            {
                htmlHelper.ViewContext.HttpContext.Items["MustRenderAjaxDropDownListInitFunction"] = false;
                mustRenderInitFunction = true;
            }

            var script = new StringBuilder("\n");
            script.AppendLine("<script>");
            if (mustRenderInitFunction)
                script.AppendLine(GetInitFunctionScript(initFunctionName));
            script.AppendLine(GetInitCallScript(id, initFunctionName));
            script.AppendLine("</script>");

            var html = select.ToString(TagRenderMode.Normal) + script.ToString(); ;

            return MvcHtmlString.Create(html);
        }

        private static string GetInitCallScript(string id, string initFunctionName)
        {
            var result = Resources.Scripts.AjaxDropDownListInitCall.Replace("{id}", id);
            result = BundleTable.EnableOptimizations ? result.Replace("initAjaxDropDownList", initFunctionName) : result;
            return result;
        }
        private static string GetInitFunctionScript(string initFunctionName)
        {
            var result = BundleTable.EnableOptimizations ? Resources.Scripts.AjaxDropDownListInitFunction_es5_min : Resources.Scripts.AjaxDropDownListInitFunction;
            result = BundleTable.EnableOptimizations ? result.Replace("initAjaxDropDownList", initFunctionName) : result;
            return result;
        }
    }
}