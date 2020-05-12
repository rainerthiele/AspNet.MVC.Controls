using DataTables.MVC.Control.Infrastructure;
using DataTables.MVC.Control.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;
using System.Web.Routing;

namespace DataTables.MVC.Control
{
    /// <summary>
    /// Html helper to create data tables wich load their data via ajax
    /// </summary>
    public static class AjaxDataTableHelper
    {
        /// <summary>
        /// An extension method to create a data table which loads the data via ajax
        /// </summary>
        /// <typeparam name="TModel">The data type of the data shown in the table.</typeparam>
        /// <param name="htmlHelper">A reference to the html helper</param>
        /// <param name="config"><see cref="TableConfiguration"/></param>
        /// <param name="htmlAttributes">Html attributes for the out table tag.</param>
        /// <returns><see cref="TableBuilder{TModel}"/></returns>
        public static TableBuilder<TModel> AjaxDataTable<TModel>(this HtmlHelper htmlHelper, TableConfiguration config, object htmlAttributes) where TModel : class, new()
        {
            var attributes = HtmlHelper.AnonymousObjectToHtmlAttributes(htmlAttributes);
            return AjaxDataTable<TModel>(htmlHelper, config, attributes);
        }

        /// <summary>
        /// An extension method to create a data table which loads the data via ajax
        /// </summary>
        /// <typeparam name="TModel">The data type of the data shown in the table.</typeparam>
        /// <param name="htmlHelper">A reference to the html helper</param>
        /// <param name="readUrl">The url to read the data.</param>
        /// <param name="htmlAttributes">Html attributes for the out table tag.</param>
        /// <returns><see cref="TableBuilder{TModel}"/></returns>
        public static TableBuilder<TModel> AjaxDataTable<TModel>(this HtmlHelper htmlHelper, string readUrl, object htmlAttributes) where TModel : class, new()
        {
            var attributes = HtmlHelper.AnonymousObjectToHtmlAttributes(htmlAttributes);
            return AjaxDataTable<TModel>(htmlHelper, new TableConfiguration() { AjaxReadUrl = readUrl }, attributes);
        }

        /// <summary>
        /// <see cref="AjaxDataTable{TModel}(HtmlHelper, TableConfiguration, object)"/>
        /// </summary>
        public static TableBuilder<TModel> AjaxDataTable<TModel>(this HtmlHelper htmlHelper, TableConfiguration config) where TModel : class, new()
        {
            return AjaxDataTable<TModel>(htmlHelper, config, null);
        }

        /// <summary>
        /// <see cref="AjaxDataTable{TModel}(HtmlHelper, string, object)"/>
        /// </summary>
        public static TableBuilder<TModel> AjaxDataTable<TModel>(this HtmlHelper htmlHelper, string readUrl) where TModel : class, new()
        {
            return AjaxDataTable<TModel>(htmlHelper, new TableConfiguration() { AjaxReadUrl = readUrl }, null);
        }

        private static TableBuilder<TModel> AjaxDataTable<TModel>(HtmlHelper htmlHelper, TableConfiguration config, RouteValueDictionary htmlAttributes) where TModel : class, new()
        {
            HtmlHelper<TModel> modelHtmlHelper = GetHtmlHelperForModelType<TModel>(htmlHelper.ViewContext, htmlHelper.ViewDataContainer.ViewData, htmlHelper.RouteCollection);
            TableBuilder<TModel> tableBuilder = new TableBuilder<TModel>(modelHtmlHelper, config, htmlAttributes);

            return tableBuilder;
        }

        private static HtmlHelper<TModel> GetHtmlHelperForModelType<TModel>(ViewContext viewContext, ViewDataDictionary viewData, RouteCollection routeCollection) where TModel : class, new()
        {
            TModel model = new TModel();
            var newViewData = new ViewDataDictionary(viewData) { Model = model };
            ViewContext newViewContext = new ViewContext(
                viewContext.Controller.ControllerContext,
                viewContext.View,
                newViewData,
                viewContext.TempData,
                viewContext.Writer);
            var viewDataContainer = new ViewDataContainer(newViewContext.ViewData);
            return new HtmlHelper<TModel>(newViewContext, viewDataContainer, routeCollection);
        }

        private class ViewDataContainer : System.Web.Mvc.IViewDataContainer
        {
            public System.Web.Mvc.ViewDataDictionary ViewData { get; set; }

            public ViewDataContainer(System.Web.Mvc.ViewDataDictionary viewData)
            {
                ViewData = viewData;
            }
        }
    }

}
