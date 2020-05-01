using AspNet.MVC.Controls.Sample.Models;
using AspNet.MVC.Controls.Sample.Repositories;
using DataTables.MVC.Control.ActionResults;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace AspNet.MVC.Controls.Sample.Controllers
{
    public class DataTableController : Controller
    {
        // GET: DataTable
        public ActionResult Index()
        {
            return View();
        }

        public JsonResult List()
        {
            return new AjaxDataTableResult<ArticleViewModel>(ArticleRepository.GetArticles());
        }
    }
}