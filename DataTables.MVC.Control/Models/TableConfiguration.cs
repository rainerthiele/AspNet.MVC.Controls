using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Web;
using System.Web.Routing;
using WebApplication5.HtmlExtensions.Infrastructure;

namespace DataTables.MVC.Control.Models
{
    public class TableConfiguration
    {
        [Html5Data("ordering")]
        public bool HasOrdering { get; set; }

        [Html5Data("paging")]
        public bool HasPaging { get; set; }

        [Html5Data("searching")]
        public bool HasSearching { get; set; }

        [Html5Data("processing")]
        public bool ShowProcessingIndicator { get; set; }

        [Html5Data("length-change")]
        public bool HasPageLengthMenu { get; set; }

        [Html5Data("server-side")]
        public bool ServerSide { get; set; }

        [Html5Data("ajaxurl")]
        public string AjaxReadUrl { get; set; }

        [Html5Data("order")]
        public OrderConfiguration Order { get; set; }

        [Html5Data("page-length")]
        public int InitalPageLength { get; set; }

        [Html5Data("length-menu")]
        public int[] PageLengthMenu { get; set; }

        public string LanguageFileUrl { get; set; }

        [Html5Data("dom")]
        public string ControlDefinition { get; set; }

        public string DrawCallbackFunction { get; set; }

        public TableConfiguration()
        {
            HasOrdering = true;
            HasPaging = true;
            HasPageLengthMenu = true;
            HasSearching = true;
            InitalPageLength = 10;
            PageLengthMenu = new int[] { 10, 20, 30, 40, 50 };
            Order = new OrderConfiguration() { ColumnNumber = 0, OrderType = OrderType.Ascending };
            ShowProcessingIndicator = true;
            ServerSide = false;
        }

        public RouteValueDictionary ToHtml5Data()
        {
            RouteValueDictionary result = new RouteValueDictionary();
            Type t = typeof(TableConfiguration);

            foreach (PropertyInfo p in t.GetProperties())
            {
                try
                {
                    object attr = p.GetCustomAttributes(typeof(Html5DataAttribute), false).FirstOrDefault();
                    if (attr != null)
                    {
                        Html5DataAttribute html5Data = attr as Html5DataAttribute;
                        object value = p.GetValue(this);

                        if (p.Name == nameof(Order))
                        {
                            string direction = Order.OrderType == OrderType.Ascending ? "asc" : "desc";
                            result.Add("data-" + html5Data.Html5DataAttributeName, string.Format("[[{0}, '{1}']]", Order.ColumnNumber, direction));
                        }
                        else if (p.Name == nameof(PageLengthMenu))
                        {
                            result.Add("data-" + html5Data.Html5DataAttributeName, string.Format("[{0}]", string.Join(", ", PageLengthMenu)));
                        }
                        else if (p.Name == nameof(ControlDefinition))
                        {
                            if (!String.IsNullOrEmpty(ControlDefinition))
                                result.Add("data-" + html5Data.Html5DataAttributeName, value);
                        }
                        else if (p.PropertyType == typeof(Boolean))
                        {
                            result.Add("data-" + html5Data.Html5DataAttributeName, value.ToString().ToLower());
                        }
                        else
                        {
                            result.Add("data-" + html5Data.Html5DataAttributeName, value);
                        }
                    }
                }
                catch (Exception ex)
                {
                    throw ex;
                }
            }

            if (!String.IsNullOrEmpty(LanguageFileUrl))
            {
                result.Add("data-languagefile", LanguageFileUrl);
            }

            return result;
        }
    }


    public enum OrderType
    {
        Ascending,
        Descending,
    }

    public class OrderConfiguration
    {
        public int ColumnNumber { get; set; }
        public OrderType OrderType { get; set; }
    }
}