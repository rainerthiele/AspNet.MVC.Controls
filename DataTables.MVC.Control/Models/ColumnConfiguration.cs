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
    public class ColumnConfiguration
    {
        [Html5Data("class-name")]
        public string ClassName { get; set; }

        [Html5Data("orderable")]
        public bool Orderable { get; set; }

        [Html5Data("searchable")]
        public bool Searchable { get; set; }

        [Html5Data("title")]
        public string Title { get; set; }

        [Html5Data("visible")]
        public bool Visible { get; set; }

        public ColumnConfiguration()
        {
            Orderable = true;
            Searchable = true;
            Visible = true;
        }

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

    public class NumericColumnConfiguration
    {
        public string ThousandsSeperator { get; set; }
        public string DecimalSign { get; set; }
        public int DecimalPlaces { get; set; }
        public string ValueAppendix { get; set; }
        public string ValuePrependix { get; set; }

        public NumericColumnConfiguration()
        {
            ThousandsSeperator = ".";
            DecimalSign = ",";
        }
    }

    public class DateTimeColumnConfiguration
    {
        public string DateTimeFormat { get; set; }
        public DateTimeColumnConfiguration(string dateTimeFormat)
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

    public class LinkConfiguration
    {
        public string InnerHtml { private get; set; }
        public string CssClass { get; set; }
        public string Target { get; set; }
        public LinkType LinkType { get; set; }
        public TagType TagType { get; set; }

        public string ResolvedTarget(string rowPrefix)
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

        public string ResolvedInnerHtml(string rowPrefix)
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