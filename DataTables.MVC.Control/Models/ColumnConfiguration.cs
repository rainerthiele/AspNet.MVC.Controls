using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text.RegularExpressions;
using System.Web;
using System.Web.Routing;
using WebApplication5.HtmlExtensions.Infrastructure;

namespace DataTables.MVC.Control.Models
{
    /// <summary>
    /// Contains the configuration for the display and behavior of a column.
    /// </summary>
    public class ColumnConfiguration
    {
        /// <summary>
        /// A name of a CSS clas which is added to the table cells (td tags).
        /// (optional)
        /// </summary>
        [Html5Data("class-name")]
        public string ClassName { get; set; }

        /// <summary>
        /// A parameter that determines whether this column is sortable.
        /// Default value: true
        /// </summary>
        [Html5Data("orderable")]
        public bool Orderable { get; set; }

        /// <summary>
        /// A parameter that determines whether this column is searchable.
        /// Default value: true
        /// </summary>
        [Html5Data("searchable")]
        public bool Searchable { get; set; }

        /// <summary>
        /// Sets the column heading. If nothing is specified here, the heading is determined via the display attribute of the property of the model.
        /// (optional)
        /// </summary>
        [Html5Data("title")]
        public string Title { get; set; }

        /// <summary>
        /// A parameter that determines whether this column is visible.
        /// Default value: true
        /// </summary>
        [Html5Data("visible")]
        public bool Visible { get; set; }

        public ColumnConfiguration()
        {
            Orderable = true;
            Searchable = true;
            Visible = true;
        }

        /// <summary>
        /// Converts the settings into Html data attributes. This method is only for internal purposes.
        /// </summary>
        public RouteValueDictionary ToHtml5Data()
        {
            RouteValueDictionary result = new RouteValueDictionary();
            Type t = typeof(ColumnConfiguration);

            foreach (PropertyInfo p in t.GetProperties())
            {
                try
                {
                    object attr = p.GetCustomAttributes(typeof(Html5DataAttribute), false).FirstOrDefault();
                    if (attr != null)
                    {
                        Html5DataAttribute html5Data = attr as Html5DataAttribute;
                        object value = p.GetValue(this);

                        if (p.PropertyType == typeof(Boolean))
                        {
                            result.Add("data-" + html5Data.Html5DataAttributeName, value.ToString().ToLower());
                        }
                        else
                            result.Add("data-" + html5Data.Html5DataAttributeName, value);
                    }
                }
                catch (Exception ex)
                {
                    throw ex;
                }
            }

            return result;
        }

    }

    /// <summary>
    /// Contains the configuration for the display of a column with numerical values.
    /// </summary>
    public class NumericConfiguration
    {
        /// <summary>
        /// Sets the thousands separator.
        /// Default value: . (dot)
        /// </summary>
        public string ThousandsSeperator { get; set; }

        /// <summary>
        /// Sets the decimal separator.
        /// Default value: , (comma)
        /// </summary>
        public string DecimalSign { get; set; }

        /// <summary>
        /// Specifies the number of decimal places.
        /// Default value: 0 (no decimal places)
        /// </summary>
        public int DecimalPlaces { get; set; }

        /// <summary>
        /// A value that is inserted after the actual data value (e.g. a currency symbol).
        /// </summary>
        public string ValueAppendix { get; set; }

        /// <summary>
        /// A value that is inserted before the actual data value (e.g. a currency symbol).
        /// </summary>
        public string ValuePrependix { get; set; }

        public NumericConfiguration()
        {
            ThousandsSeperator = ".";
            DecimalSign = ",";
        }
    }

    /// <summary>
    /// Contains the configuration for the display of a column with date or time values.
    /// </summary>
    public class DateTimeConfiguration
    {
        /// <summary>
        /// Specifies the date and time format of the column. Note: This entry must be in a format valid for moment.js.
        /// </summary>
        public string DateTimeFormat { get; set; }
        public DateTimeConfiguration(string dateTimeFormat)
        {
            DateTimeFormat = dateTimeFormat;
        }

    }

    public enum LinkType
    {
        Script,
        Url
    }

    public enum TagType
    {
        Anchor,
        Button
    }


    /// <summary>
    /// Contains the configuration for the display and the content of a column with a link.
    /// </summary>
    public class LinkConfiguration
    {
        /// <summary>
        /// Specifies the text to be linked. In this text, placeholders in curly brackets can be used to reference values from the data record.
        /// </summary>
        public string InnerHtml { private get; set; }

        /// <summary>
        /// A Css class name which is added to the link (to the button resp. a tag, optional).
        /// </summary>
        public string CssClass { get; set; }

        /// <summary>
        /// The javascript function or url to be linked. In this text, placeholders in curly brackets can be used to reference values from the data record.
        /// </summary>
        public string Target { get; set; }

        /// <summary>
        /// A parameter that determines whether the link should be a script or a url.
        /// </summary>
        public LinkType LinkType { get; set; }

        /// <summary>
        /// A parameter that determines whether the link is created as button or a tag.
        /// </summary>
        public TagType TagType { get; set; }

        /// <summary>
        /// Replaces the placeholders in the link (Target). This method is for internal purposes only.
        /// </summary>
        public string ResolveTarget(string rowPrefix)
        {
            Dictionary<string, string> replacements = GetReplacements(Target, rowPrefix);

            string result = Target;

            foreach (var key in replacements.Keys)
            {
                if (LinkType == LinkType.Script)
                    result = result.Replace(key, $"\\''+{replacements[key]}+'\\'");
                else
                {
                    if (TagType == TagType.Anchor)
                        result = result.Replace(key, $"'+encodeURI({replacements[key]})+'");
                    else
                        result = result.Replace(key, $"'+encodeURI({replacements[key]})+'");
                }
            }

            return result;
        }

        /// <summary>
        /// Replaces the placeholders in the linked text (InnerHtml). This method is for internal purposes only.
        /// </summary>
        public string ResolveInnerHtml(string rowPrefix)
        {
            Dictionary<string, string> replacements = GetReplacements(InnerHtml, rowPrefix);

            string result = InnerHtml;

            foreach (var key in replacements.Keys)
                result = result.Replace(key, $"'+{replacements[key]}+'");

            return result;

        }

        private Dictionary<string, string> GetReplacements(string value, string rowPrefix)
        {
            Regex regEx = new Regex("{\\w+}");
            Dictionary<string, string> replacements = new Dictionary<string, string>();

            foreach (Match m in regEx.Matches(value))
                replacements.Add(m.Value, rowPrefix + "." + m.Value.Substring(1, m.Value.Length - 2));

            return replacements;
        }
    }
}