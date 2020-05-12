using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Web;
using System.Web.Routing;
using WebApplication5.HtmlExtensions.Infrastructure;

namespace DataTables.MVC.Control.Models
{
    /// <summary>
    /// 
    /// </summary>
    public class TableConfiguration
    {
        /// <summary>
        /// A parameter that determines whether the table is sortable. If false, the column titles are not linked for sorting and no sort direction is displayed.
        /// Default value: true
        /// </summary>
        [Html5Data("ordering")]
        public bool HasOrdering { get; set; }

        /// <summary>
        /// A parameter that determines whether the data is displayed page by page. 
        /// Default value: true
        /// </summary>
        [Html5Data("paging")]
        public bool HasPaging { get; set; }

        /// <summary>
        /// A parameter that determines whether the table provides a search field.
        /// Default value: true
        /// </summary>
        [Html5Data("searching")]
        public bool HasSearching { get; set; }

        /// <summary>
        /// A parameter that determines whether a hint should be displayed while data is loading. The content of this note can be customized via a language file if necessary.
        /// Default value: true
        /// </summary>
        [Html5Data("processing")]
        public bool ShowProcessingIndicator { get; set; }

        /// <summary>
        /// A parameter that determines whether to display a drop-down menu for selecting the number of rows displayed.
        /// Default value: true
        /// </summary>
        [Html5Data("length-change")]
        public bool HasPageLengthMenu { get; set; }

        /// <summary>
        /// A parameter that determines whether the data should be filtered and sorted on the server side or on the client side. 
        /// Default value: false
        /// </summary>
        [Html5Data("server-side")]
        public bool ServerSide { get; set; }

        /// <summary>
        /// The url from which to read the data for the table.
        /// </summary>
        [Html5Data("ajaxurl")]
        public string AjaxReadUrl { get; set; }

        /// <summary>
        ///  With this parameter, the initial sorting of the table is determined. The parameter is of type OrderConfiguration, which contains a column number (zero-based) and
        /// a sort order.
        /// Default value: [0, Ascending]
        /// </summary>
        [Html5Data("order")]
        public OrderConfiguration Order { get; set; }

        /// <summary>
        /// This parameter determines how many data records are initially displayed on each page.
        /// Default value: 10
        /// </summary>
        [Html5Data("page-length")]
        public int InitalPageLength { get; set; }

        /// <summary>
        /// A parameter that populates the drop down menu values for changing the page length.
        /// Default value: [10, 20, 30, 40, 50]
        /// </summary>
        [Html5Data("length-menu")]
        public int[] PageLengthMenu { get; set; }

        /// <summary>
        /// A url under which a language file for the standard texts of the data table can be loaded. By default, all texts are in English.
        /// (optional)
        /// </summary>
        public string LanguageFileUrl { get; set; }

        /// <summary>
        /// A string that defines the order in which the controls (search field, pager, etc.) should be arranged around the table and whether additional div elements should be added.
        /// (optional)
        /// </summary>
        [Html5Data("dom")]
        public string ControlDefinition { get; set; }

        /// <summary>
        /// The name of a Javascript function that is called after the table has been rendered (draw event).
        /// (optional)
        /// </summary>
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

        /// <summary>
        /// Converts the settings into Html data attributes. This method is only for internal purposes.
        /// </summary>
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