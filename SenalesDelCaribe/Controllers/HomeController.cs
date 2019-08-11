using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using DataSenalesCaribe;

namespace SenalesDelCaribe.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            ViewBag.Message = "Welcome to ASP.NET MVC!";

            var db = new ContentDataContext(Comun.GetConnString());
            var tmpData = db.Categories
                            .Where(c => c.IsMenu == true && c.IDParent == -1)
                            .OrderBy(c => c.OrderBy).FirstOrDefault();

            if (tmpData != null)
            {
                return RedirectToAction("Index", "GeneralPage", new { id = tmpData.IDCategory });
            }
            else
            {
                return View();
            }
        }

        public ActionResult About()
        {
            return PartialView("_About");
        }

        public ActionResult TwoColumnMenu()
        {
            return PartialView("_TwoColumnMenu");
        }
    }
}
