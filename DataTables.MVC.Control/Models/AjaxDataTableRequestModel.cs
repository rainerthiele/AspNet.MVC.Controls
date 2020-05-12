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
    /// <summary>
    /// Represents an ajax request from the data table to the server if the parameter <c>ServerSide = true</c> 
                /// is in the <see cref="TableConfiguration"/>.
    /// </summary>
    public class AjaxDataTableRequestModel
    {
        /// <summary>
        /// An integer value that counts the number of requests. I. e., each time you page, sort, etc., this value is increased by one. 
        /// </summary>
        public int Draw { get; set; }

        /// <summary>
        /// The zero-based index of the first record to return.
        /// </summary>
        public int Start { get; set; }

        /// <summary>
        /// The number of records to return.
        /// </summary>
        public int Length { get; set; }

        /// <summary>
        /// The search term for which the data should be searched.
        /// </summary>
        public string SearchValue { get; set; }

        /// <summary>
        /// The names of the columns to search for the search term. The list contains all columns that have the parameter <c>Searchable = true</c>  
        /// according to their <see cref="ColumnConfiguration"/>.
        /// </summary>
        public string[] SearchColumns { get; set; }

        /// <summary>
        /// The name of the column by which to sort the result.
        /// </summary>
        public string OrderColumn { get; set; }

        /// <summary>
        /// The sort direction by which the result should be sorted.
        /// </summary>
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
