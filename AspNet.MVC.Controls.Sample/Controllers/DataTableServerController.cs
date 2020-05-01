using AspNet.MVC.Controls.Sample.Models;
using AspNet.MVC.Controls.Sample.Repositories;
using DataTables.MVC.Control.ActionResults;
using DataTables.MVC.Control.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Dynamic;
using System.Web;
using System.Web.Mvc;

namespace AspNet.MVC.Controls.Sample.Controllers
{
    public class DataTableServerController : Controller
    {
        // GET: DataTable
        public ActionResult Index()
        {
            return View();
        }

        public JsonResult List(AjaxDataTableRequestModel model)
        {
            var articles = ArticleRepository.GetArticles();
            int count = articles.Count();

            if (!String.IsNullOrEmpty(model.SearchValue))
                articles = articles.Where(a => a.Name.Contains(model.SearchValue));

            if (!String.IsNullOrEmpty(model.OrderColumn))
                articles = articles.OrderBy(model.OrderColumn + " " + model.OrderDirection.ToString().ToLower());

            return new AjaxDataTableResult<ArticleViewModel>(articles.Skip(model.Start).Take(model.Length), model.Draw, count, articles.Count());
        }
    }
}