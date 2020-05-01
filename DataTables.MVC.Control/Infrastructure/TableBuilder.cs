using DataTables.MVC.Control.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Web;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;

namespace DataTables.MVC.Control.Infrastructure
{
    public class TableBuilder<TModel>
    {
        private List<ColumnBuilder> columnBuilders;
        private HtmlHelper<TModel> htmlHelper;
        private TableConfiguration config;
        private RouteValueDictionary htmlAttributes;

        public TableBuilder(HtmlHelper<TModel> htmlHelper, TableConfiguration config, RouteValueDictionary htmlAttributes)
        {
            this.htmlHelper = htmlHelper;
            this.config = config;
            this.htmlAttributes = htmlAttributes;
            this.columnBuilders = new List<ColumnBuilder>();
        }

        public TableBuilder<TModel> LinkColumn<TProperty>(Expression<Func<TModel, TProperty>> expression, LinkConfiguration linkConfiguration)
        {
            columnBuilders.Add(new ColumnBuilder(new ColumnConfiguration() { Orderable = false }, GetMemberName(expression), linkConfiguration));

            return this;
        }

        public TableBuilder<TModel> LinkColumn<TProperty>(Expression<Func<TModel, TProperty>> expression, ColumnConfiguration configuration, LinkConfiguration linkConfiguration)
        {
            columnBuilders.Add(new ColumnBuilder(configuration, GetMemberName(expression), linkConfiguration));

            return this;
        }

        public TableBuilder<TModel> DateTimeColumn<TProperty>(Expression<Func<TModel, TProperty>> expression, DateTimeColumnConfiguration dateTimeConfiguration)
        {
            columnBuilders.Add(new ColumnBuilder(SetTitleToConfiguration(expression, new ColumnConfiguration()), GetMemberName(expression), dateTimeConfiguration));

            return this;
        }

        public TableBuilder<TModel> DateTimeColumn<TProperty>(Expression<Func<TModel, TProperty>> expression, ColumnConfiguration configuration, DateTimeColumnConfiguration dateTimeConfiguration)
        {
            columnBuilders.Add(new ColumnBuilder(SetTitleToConfiguration(expression, configuration), GetMemberName(expression), dateTimeConfiguration));

            return this;
        }

        public TableBuilder<TModel> NumericColumn<TProperty>(Expression<Func<TModel, TProperty>> expression)
        {
            columnBuilders.Add(new ColumnBuilder(SetTitleToConfiguration(expression, new ColumnConfiguration()), GetMemberName(expression), new NumericColumnConfiguration()));

            return this;
        }

        public TableBuilder<TModel> NumericColumn<TProperty>(Expression<Func<TModel, TProperty>> expression, ColumnConfiguration configuration)
        {
            columnBuilders.Add(new ColumnBuilder(SetTitleToConfiguration(expression, configuration), GetMemberName(expression), new NumericColumnConfiguration()));

            return this;
        }

        public TableBuilder<TModel> NumericColumn<TProperty>(Expression<Func<TModel, TProperty>> expression, NumericColumnConfiguration numericConfiguration)
        {
            columnBuilders.Add(new ColumnBuilder(SetTitleToConfiguration(expression, new ColumnConfiguration()), GetMemberName(expression), numericConfiguration));

            return this;
        }

        public TableBuilder<TModel> NumericColumn<TProperty>(Expression<Func<TModel, TProperty>> expression, ColumnConfiguration configuration, NumericColumnConfiguration numericConfiguration)
        {
            columnBuilders.Add(new ColumnBuilder(SetTitleToConfiguration(expression, configuration), GetMemberName(expression), numericConfiguration));

            return this;
        }


        public TableBuilder<TModel> Column<TProperty>(Expression<Func<TModel, TProperty>> expression, string renderFunction = null)
        {
            columnBuilders.Add(new ColumnBuilder(SetTitleToConfiguration(expression, new ColumnConfiguration()), GetMemberName(expression), renderFunction));

            return this;
        }

        public TableBuilder<TModel> Column<TProperty>(Expression<Func<TModel, TProperty>> expression, ColumnConfiguration configuration, string renderFunction = null)
        {
            columnBuilders.Add(new ColumnBuilder(SetTitleToConfiguration(expression, configuration), GetMemberName(expression), renderFunction));

            return this;
        }

        public IHtmlString Render(bool withScript = true)
        {
            bool mustRenderInitFunction = false;
            string initFunctionName;

            if (htmlHelper.ViewContext.HttpContext.Items["AjaxDataTableInitFunctionName"] == null)
                htmlHelper.ViewContext.HttpContext.Items["AjaxDataTableInitFunctionName"] = "f" + htmlHelper.GetHashCode().ToString("x");
            initFunctionName = htmlHelper.ViewContext.HttpContext.Items["AjaxDataTableInitFunctionName"].ToString();

            if (htmlHelper.ViewContext.HttpContext.Items["MustRenderAjaxDataTableInitFunction"] == null)
            {
                htmlHelper.ViewContext.HttpContext.Items["MustRenderAjaxDataTableInitFunction"] = false;
                mustRenderInitFunction = true;
            }


            var table = new TagBuilder("table");
            table.InnerHtml = "\n";
            if (htmlAttributes != null)
                table.MergeAttributes(htmlAttributes);
            var attr = config.ToHtml5Data();
            table.MergeAttributes(attr);

            if (table.Attributes.Any(a => a.Key == "id") && table.Attributes["id"] == null)
            {
                if (table.Attributes.Any(a => a.Key == "name") && table.Attributes["name"] == null)
                {
                    table.GenerateId(typeof(TModel).Name);
                }
                else
                {
                    table.GenerateId(table.Attributes["name"].ToString());
                }
            }
            else
                table.GenerateId(typeof(TModel).Name);

            string id = table.Attributes["id"];

            var thead = new TagBuilder("thead");
            thead.InnerHtml = "\n";
            foreach (var column in columnBuilders)
            {
                thead.InnerHtml += column.Render() + "\n";
            }
            thead.InnerHtml += "\n";

            string html = table.ToString(TagRenderMode.StartTag).Replace("\"", "'").Replace("&#39;", "\"") +
                thead.ToString(TagRenderMode.Normal) +
                table.ToString(TagRenderMode.EndTag);
            if (withScript && mustRenderInitFunction)
                html = html + GetInitFunctionScript(initFunctionName);
            if (withScript)
                html = html + GetInitCallScript(id, initFunctionName);

            return MvcHtmlString.Create(html);
        }

        private static string GetInitFunctionScript(string initFunctionName)
        {
            var result = BundleTable.EnableOptimizations ? Resources.Scripts.AjaxDataTableInitFunction_es5_min : Resources.Scripts.AjaxDataTableInitFunction;
            result = BundleTable.EnableOptimizations ? result.Replace("initAjaxDataTable", initFunctionName) : result;
            return $"<script>{result}</script>";
        }

        private string GetInitCallScript(string id, string initFunctionName)
        {
            initFunctionName = !BundleTable.EnableOptimizations ? "initAjaxDataTable" : initFunctionName;

            StringBuilder script = new StringBuilder("\n");
            string configName = "c" + script.GetHashCode().ToString("x");
            string tableName = "d" + script.GetHashCode().ToString("x");

            script.AppendLine("<script>");
            script.AppendLine($"var {configName} = [");

            for (var i = 0; i < columnBuilders.Count(); i++)
            {
                if (!string.IsNullOrEmpty(columnBuilders[i].RenderFunction))
                {
                    script.AppendLine($"{{ targets: {i}, render: {columnBuilders[i].RenderFunction} }},");
                }
            }

            script.AppendLine("];");
            script.AppendLine("$(function(){");
            if (String.IsNullOrEmpty(config.DrawCallbackFunction))
                script.AppendLine($"var {tableName} = {initFunctionName}('{id}', {configName});");
            else
                script.AppendLine($"var {tableName} = {initFunctionName}('{id}', {configName}, {config.DrawCallbackFunction});");
            script.AppendLine("});\n</script>");

            return script.ToString();
        }

        private ColumnConfiguration SetTitleToConfiguration<TProperty>(Expression<Func<TModel, TProperty>> expression, ColumnConfiguration configuration)
        {
            if (String.IsNullOrEmpty(configuration.Title))
            {
                var metadata = ModelMetadata.FromLambdaExpression(expression, htmlHelper.ViewData);
                if (String.IsNullOrEmpty(metadata.DisplayName))
                    configuration.Title = metadata.PropertyName;
                else
                    configuration.Title = metadata.DisplayName;
            }

            return configuration;
        }

        private string GetMemberName<TProperty>(Expression<Func<TModel, TProperty>> expression)
        {
            var memberExpression = (MemberExpression)expression.Body;
            return memberExpression.Member.Name;
        }
    }
}