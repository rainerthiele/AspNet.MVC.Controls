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
    public static class AjaxDataTable
    {
        public static TableBuilder<TModel> Table<TModel>(this HtmlHelper htmlHelper, TableConfiguration config, object htmlAttributes) where TModel : class, new()
        {
            var attributes = HtmlHelper.AnonymousObjectToHtmlAttributes(htmlAttributes);
            return Table<TModel>(htmlHelper, config, attributes);
        }

        public static TableBuilder<TModel> Table<TModel>(this HtmlHelper htmlHelper, string readUrl, object htmlAttributes) where TModel : class, new()
        {
            var attributes = HtmlHelper.AnonymousObjectToHtmlAttributes(htmlAttributes);
            return Table<TModel>(htmlHelper, new TableConfiguration() { AjaxReadUrl = readUrl }, attributes);
        }

        public static TableBuilder<TModel> Table<TModel>(this HtmlHelper htmlHelper, TableConfiguration config) where TModel : class, new()
        {
            return Table<TModel>(htmlHelper, config, null);
        }

        public static TableBuilder<TModel> Table<TModel>(this HtmlHelper htmlHelper, string readUrl) where TModel : class, new()
        {
            return Table<TModel>(htmlHelper, new TableConfiguration() { AjaxReadUrl = readUrl }, null);
        }


        private static TableBuilder<TModel> Table<TModel>(HtmlHelper htmlHelper, TableConfiguration config, RouteValueDictionary htmlAttributes) where TModel : class, new()
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
