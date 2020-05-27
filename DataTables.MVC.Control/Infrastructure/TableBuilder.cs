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
    /// <summary>
    /// Creates the html code and scripts for the data table. Provides methods to add columns to the table and render the html.
    /// </summary>
    /// <typeparam name="TModel">The type of the data model the tablle is built of.</typeparam>
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

        /// <summary>
        /// <see cref="LinkColumn{TProperty}(Expression{Func{TModel, TProperty}}, ColumnConfiguration, LinkConfiguration)"/>
        /// </summary>
        public TableBuilder<TModel> LinkColumn<TProperty>(Expression<Func<TModel, TProperty>> expression, LinkConfiguration linkConfiguration)
        {
            columnBuilders.Add(new ColumnBuilder(SetTitleToConfiguration(expression, new ColumnConfiguration()), GetMemberName(expression), linkConfiguration));

            return this;
        }

        /// <summary>
        /// Adds a link column to the data table.
        /// </summary>
        /// <typeparam name="TProperty">The data property of the model.</typeparam>
        /// <param name="expression">An expression to identify the column value.</param>
        /// <param name="configuration"><see cref="ColumnConfiguration"/></param>
        /// <param name="linkConfiguration"><see cref="LinkConfiguration"/></param>
        /// <returns></returns>
        public TableBuilder<TModel> LinkColumn<TProperty>(Expression<Func<TModel, TProperty>> expression, ColumnConfiguration configuration, LinkConfiguration linkConfiguration)
        {
            columnBuilders.Add(new ColumnBuilder(SetTitleToConfiguration(expression, configuration), GetMemberName(expression), linkConfiguration));

            return this;
        }

        /// <summary>
        /// <see cref="DateTimeColumn{TProperty}(Expression{Func{TModel, TProperty}}, ColumnConfiguration, DateTimeConfiguration)"/>
        /// </summary>
        public TableBuilder<TModel> DateTimeColumn<TProperty>(Expression<Func<TModel, TProperty>> expression, DateTimeConfiguration dateTimeConfiguration)
        {
            columnBuilders.Add(new ColumnBuilder(SetTitleToConfiguration(expression, new ColumnConfiguration()), GetMemberName(expression), dateTimeConfiguration));

            return this;
        }

        /// <summary>
        /// Adds a link column to the data table.
        /// </summary>
        /// <typeparam name="TProperty">The data property of the model.</typeparam>
        /// <param name="expression">An expression to identify the column value.</param>
        /// <param name="configuration"><see cref="ColumnConfiguration"/></param>
        /// <param name="dateTimeConfiguration"><see cref="DateTimeConfiguration"/></param>
        /// <returns></returns>
        public TableBuilder<TModel> DateTimeColumn<TProperty>(Expression<Func<TModel, TProperty>> expression, ColumnConfiguration configuration, DateTimeConfiguration dateTimeConfiguration)
        {
            columnBuilders.Add(new ColumnBuilder(SetTitleToConfiguration(expression, configuration), GetMemberName(expression), dateTimeConfiguration));

            return this;
        }

        /// <summary>
        /// <see cref="NumericColumn{TProperty}(Expression{Func{TModel, TProperty}}, ColumnConfiguration, NumericConfiguration)"/>
        /// </summary>
        public TableBuilder<TModel> NumericColumn<TProperty>(Expression<Func<TModel, TProperty>> expression)
        {
            columnBuilders.Add(new ColumnBuilder(SetTitleToConfiguration(expression, new ColumnConfiguration()), GetMemberName(expression), new NumericConfiguration()));

            return this;
        }

        /// <summary>
        /// <see cref="NumericColumn{TProperty}(Expression{Func{TModel, TProperty}}, ColumnConfiguration, NumericConfiguration)"/>
        /// </summary>
        public TableBuilder<TModel> NumericColumn<TProperty>(Expression<Func<TModel, TProperty>> expression, ColumnConfiguration configuration)
        {
            columnBuilders.Add(new ColumnBuilder(SetTitleToConfiguration(expression, configuration), GetMemberName(expression), new NumericConfiguration()));

            return this;
        }

        /// <summary>
        /// <see cref="NumericColumn{TProperty}(Expression{Func{TModel, TProperty}}, ColumnConfiguration, NumericConfiguration)"/>
        /// </summary>
        public TableBuilder<TModel> NumericColumn<TProperty>(Expression<Func<TModel, TProperty>> expression, NumericConfiguration numericConfiguration)
        {
            columnBuilders.Add(new ColumnBuilder(SetTitleToConfiguration(expression, new ColumnConfiguration()), GetMemberName(expression), numericConfiguration));

            return this;
        }

        /// <summary>
        /// Adds a link column to the data table.
        /// </summary>
        /// <typeparam name="TProperty">The data property of the model.</typeparam>
        /// <param name="expression">An expression to identify the column value.</param>
        /// <param name="configuration"><see cref="ColumnConfiguration"/></param>
        /// <param name="numericConfiguration"><see cref="NumericConfiguration"/> </param>
        /// <returns></returns>
        public TableBuilder<TModel> NumericColumn<TProperty>(Expression<Func<TModel, TProperty>> expression, ColumnConfiguration configuration, NumericConfiguration numericConfiguration)
        {
            columnBuilders.Add(new ColumnBuilder(SetTitleToConfiguration(expression, configuration), GetMemberName(expression), numericConfiguration));

            return this;
        }

        /// <summary>
        /// <see cref="Column{TProperty}(Expression{Func{TModel, TProperty}}, ColumnConfiguration, string)"/>
        /// </summary>
        public TableBuilder<TModel> Column<TProperty>(Expression<Func<TModel, TProperty>> expression, string renderFunction = null)
        {
            columnBuilders.Add(new ColumnBuilder(SetTitleToConfiguration(expression, new ColumnConfiguration()), GetMemberName(expression), renderFunction));

            return this;
        }

        /// <summary>
        /// Adds a simple text column to the data table.
        /// </summary>
        /// <typeparam name="TProperty">The data property of the model.</typeparam>
        /// <param name="expression">An expression to identify the column value.</param>
        /// <param name="configuration"><see cref="ColumnConfiguration"/></param>
        /// <param name="renderFunction">The name of a JavaScript function that controls the output of the value. If no function is specified, the value is simply output as a string. If a name is given, only the name needs to be 
        /// given, without parentheses.The fragment "(data, type, row, meta)" is always appended to the name, so that these four parameters are available in the JavaScript function.</param>
        /// <returns></returns>
        public TableBuilder<TModel> Column<TProperty>(Expression<Func<TModel, TProperty>> expression, ColumnConfiguration configuration, string renderFunction = null)
        {
            columnBuilders.Add(new ColumnBuilder(SetTitleToConfiguration(expression, configuration), GetMemberName(expression), renderFunction));

            return this;
        }

        /// <summary>
        /// Redenders the data table.
        /// </summary>
        /// <param name="withScript">If set to false, no javascript will be created (optional).</param>
        /// <returns></returns>
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