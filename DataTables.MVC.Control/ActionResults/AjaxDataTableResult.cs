using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace DataTables.MVC.Control.ActionResults
{
    /// <summary>
    /// The generic class <c>AjaxDataTableResult</c> inherits from <c>JsonResult</c>. 
    /// It creates a Json object that corresponds to the Json requirements of the datatables.net plugin.
    /// </summary>
    /// <typeparam name="T">Type of the data used for the datatable.</typeparam>
    public class AjaxDataTableResult<T> : JsonResult where T : class
    {
       /// <summary>
       /// To be used for data tables with client side processing.
       /// </summary>
       /// <param name="data">An Enumerable with the records to be shown in the data table.
       /// With client side processing the Enumerable must contain all records of all pages, unfiltered and can be unsorted.</param>
        public AjaxDataTableResult(IEnumerable<T> data)
        {
            this.Data = new { data = data };
            this.JsonRequestBehavior = JsonRequestBehavior.AllowGet;
        }

        /// <summary>
        /// To be used for data tables with server side processing.
        /// </summary>
        /// <param name="data">An Enumerable with the records to be shown in the data table.
        /// With server side processing the Enumerable must contain only records of the page to display, filtered and sorted accordingly.</param>
        /// <param name="draw">An integer value that counts the number of requests. This means that every time you page, sort, etc., this value is increased by one. It can be taken from the 
        /// <see cref="Models.AjaxDataTableRequestModel"/>.</param>
        /// <param name="recordsTotal">The total number of records that the data source would deliver without filtering.</param>
        /// <param name="recordsFiltered">The number of records that the data source provides after filtering.</param>
        public AjaxDataTableResult(IEnumerable<T> data, int draw, int recordsTotal, int recordsFiltered)
        {
            this.Data = new { data = data, draw = draw, recordsTotal = recordsTotal, recordsFiltered = recordsFiltered };
            this.JsonRequestBehavior = JsonRequestBehavior.AllowGet;
        }
    }
}
