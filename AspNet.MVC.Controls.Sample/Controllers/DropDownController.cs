using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using AjaxDropDowns.MVC.Control.ActionResults;

namespace AspNet.MVC.Controls.Sample.Controllers
{
    public class DropDownController : Controller
    {
        // GET: DropDown
        public ActionResult Index()
        {
            return View();
        }

        public JsonResult Options()
        {
            List<SelectListItem> result = new List<SelectListItem>(){
                new SelectListItem(){Text = "Item 1",Value = "1"},
                new SelectListItem(){Text = "Item 2",Value = "2"}
            };

            return Json(result, JsonRequestBehavior.AllowGet);
        }

        public ActionResult Options2()
        {
            List<SelectListItem> result = new List<SelectListItem>(){
                new SelectListItem(){Text = "Item 1",Value = "1"},
                new SelectListItem(){Text = "Item 2",Value = "2"}
            };

            return new ScriptDropDownResult(result);
        }

        public ActionResult Options3()
        {
            List<SelectListItem> result = new List<SelectListItem>(){
                new SelectListItem(){Text = "Item A",Value = "1"},
                new SelectListItem(){Text = "Item B",Value = "2"}
            };

            return new ScriptDropDownResult(result);
        }
    }
}