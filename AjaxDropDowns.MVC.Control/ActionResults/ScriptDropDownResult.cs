﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

namespace AjaxDropDowns.MVC.Control.ActionResults
{
    /// <summary>
    /// The <c>ScriptDropDownResult</c> generates a JavaScript code including the
    /// script tags from a list of <c>SelectListItem</c>, which is an array with the
    /// Contains text / value pairs that appear as options in the DropDownList.
    /// </summary>
    public class ScriptDropDownResult : ActionResult
    {
        private IEnumerable<SelectListItem> _items;

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="items">A list of <c>SelectListItem</c>, which contains the options of the DropDownList.</param>
        public ScriptDropDownResult(IEnumerable<SelectListItem> items)
        {
            _items = items;
        }

        public override void ExecuteResult(ControllerContext context)
        {
            HttpResponseBase response = context.HttpContext.Response;
            response.ContentType = "text/html";
            List<string> array = new List<string>();
            StringBuilder sb = new StringBuilder("<script>\n");

            foreach (var item in _items)
            {
                array.Add(String.Format("['{0}', '{1}']", item.Value, item.Text));
            }

            string dataname = context.RouteData.Values["dataname"].ToString();

            sb.AppendFormat("var {0} = [{1}];", dataname, string.Join(", ", array.ToArray()));
            sb.Append("\n</script>");

            response.Write(sb.ToString());
        }
    }
}
