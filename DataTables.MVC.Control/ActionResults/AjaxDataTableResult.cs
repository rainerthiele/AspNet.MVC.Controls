using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace DataTables.MVC.Control.ActionResults
{
    public class AjaxDataTableResult<T> : JsonResult where T : class
    {
        public AjaxDataTableResult(IEnumerable<T> data)
        {
            this.Data = new { data = data };
            this.JsonRequestBehavior = JsonRequestBehavior.AllowGet;
        }

        public AjaxDataTableResult(IEnumerable<T> data, int draw, int recordsTotal, int recordsFiltered)
        {
            this.Data = new { data = data, draw = draw, recordsTotal = recordsTotal, recordsFiltered = recordsFiltered };
            this.JsonRequestBehavior = JsonRequestBehavior.AllowGet;
        }
    }
}
