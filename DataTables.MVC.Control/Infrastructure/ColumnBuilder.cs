using DataTables.MVC.Control.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.Mvc;

namespace DataTables.MVC.Control.Infrastructure
{
    public class ColumnBuilder
    {
        private TagBuilder tagbuilder;

        public string RenderFunction { get; private set; }

        public ColumnBuilder(ColumnConfiguration configuration, string dataProperty, string renderFunction = null)
        {
            tagbuilder = new TagBuilder("th");
            RenderFunction = null;

            if (!String.IsNullOrEmpty(renderFunction))
            {
                RenderFunction = "function(data, type, row, meta) { return " + renderFunction + "(data, type, row, meta); }";
            }
            tagbuilder.MergeAttributes(configuration.ToHtml5Data());
            tagbuilder.MergeAttribute("data-data", dataProperty);
        }

        public ColumnBuilder(ColumnConfiguration configuration, string dataProperty, NumericColumnConfiguration numericConfiguration)
        {
            tagbuilder = new TagBuilder("th");
            RenderFunction = $"$.fn.dataTable.render.number('{numericConfiguration.ThousandsSeperator}', '{numericConfiguration.DecimalSign}', {numericConfiguration.DecimalPlaces}, '{numericConfiguration.ValuePrependix}','{numericConfiguration.ValueAppendix}')";

            tagbuilder.MergeAttributes(configuration.ToHtml5Data());
            tagbuilder.MergeAttribute("data-data", dataProperty);
        }

        public ColumnBuilder(ColumnConfiguration configuration, string dataProperty, DateTimeColumnConfiguration dateTimeConfiguration)
        {
            tagbuilder = new TagBuilder("th");
            RenderFunction = "function(d, t) { return t === 'sort' ? moment(d).format('YYYYMMDDHHmmss') :  moment(d).format('" + dateTimeConfiguration.DateTimeFormat + "'); }";

            tagbuilder.MergeAttributes(configuration.ToHtml5Data());
            tagbuilder.MergeAttribute("data-data", dataProperty);
        }

        public ColumnBuilder(ColumnConfiguration configuration, string dataProperty, LinkConfiguration linkConfiguration)
        {
            TagBuilder link = (linkConfiguration.TagType == TagType.Anchor) ? new TagBuilder("a") : new TagBuilder("button");

            if (!string.IsNullOrEmpty(linkConfiguration.CssClass))
                link.Attributes.Add("class", linkConfiguration.CssClass);

            if (linkConfiguration.LinkType == LinkType.Script)
            {
                if (linkConfiguration.TagType == TagType.Anchor)
                    link.Attributes.Add("href", "#");
                link.Attributes.Add("onclick", $"{linkConfiguration.ResolvedTarget("r")}; return false;");
            }
            else
            {
                if (linkConfiguration.TagType == TagType.Anchor)
                    link.Attributes.Add("href", linkConfiguration.ResolvedTarget("r"));
                else
                    link.Attributes.Add("onclick", $"window.location.href=\\'{linkConfiguration.ResolvedTarget("r")}\\';");
            }

            StringBuilder function = new StringBuilder("function (d, t, r) { return t === 'sort' ? d : ");

            function.Append($"'{link.ToString(TagRenderMode.StartTag).Replace("&#39;","'")}' +");

            function.Append($"'{linkConfiguration.ResolvedInnerHtml("r")}'");

            function.Append($" + '{link.ToString(TagRenderMode.EndTag)}';}}");

            tagbuilder = new TagBuilder("th");
            RenderFunction = function.ToString();

            tagbuilder.MergeAttributes(configuration.ToHtml5Data());
            tagbuilder.MergeAttribute("data-data", dataProperty);
        }


        public IHtmlString Render()
        {
            tagbuilder.InnerHtml = "\n";
            return MvcHtmlString.Create(tagbuilder.ToString());
        }

    }


}