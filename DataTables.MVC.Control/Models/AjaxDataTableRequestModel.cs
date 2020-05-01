using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

namespace DataTables.MVC.Control.Models
{
    public class AjaxDataTableRequestModel
    {
        public int Draw { get; set; }
        public int Start { get; set; }
        public int Length { get; set; }

        public string SearchValue { get; set; }
        public string[] SearchColumns { get; set; }
        public string OrderColumn { get; set; }
        public OrderType OrderDirection { get; set; }
    }

    public class AjaxDataTableRequestModelBinder : IModelBinder
    {
        public object BindModel(ControllerContext controllerContext, ModelBindingContext bindingContext)
        {
            HttpRequestBase request = controllerContext.HttpContext.Request;

            AjaxDataTableRequestModel result = new AjaxDataTableRequestModel();
            List<string> searchColumns = new List<string>();

            NameValueCollection collection;

            if (request.HttpMethod == "POST")
                collection = request.Form;
            else
                collection = request.QueryString;

            int i = 0;

            //"columns[0][data]"
            //"columns[0][searchable]"

            if (collection.AllKeys.Contains("draw") && int.TryParse(collection["draw"], out i))
                result.Draw = i;

            if (collection.AllKeys.Contains("length") && int.TryParse(collection["length"], out i))
                result.Length = i;

            if (collection.AllKeys.Contains("start") && int.TryParse(collection["start"], out i))
                result.Start = i;

            if (collection.AllKeys.Contains("order[0][column]") && int.TryParse(collection["order[0][column]"], out i))
            {
                if (collection.AllKeys.Contains($"columns[{i}][data]"))
                    result.OrderColumn = collection[$"columns[{i}][data]"];
            }

            if (collection.AllKeys.Contains($"order[0][dir]") && collection[$"order[0][dir]"].ToLower().StartsWith("d"))
                result.OrderDirection = OrderType.Descending;
            else
                result.OrderDirection = OrderType.Ascending;

            if (collection.AllKeys.Contains("search[value]"))
                result.SearchValue = collection["search[value]"];

            foreach (string key in collection.AllKeys.Where(k => k.StartsWith("columns") && k.EndsWith("[searchable]")))
            {
                string dataKey = key.Replace("searchable", "data");
                if (collection[key].ToLower().Equals("true"))
                    searchColumns.Add(collection[dataKey]);
            }

            result.SearchColumns = searchColumns.ToArray();

            return result;
        }
    }
}
