using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace MvcBreadCrumbs.SampleWeb.Controllers
{
    [BreadCrumb]

    public class HomeController : Controller
    {

        [BreadCrumb(Clear = true)]
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";
            BreadCrumbs.SetLabel("About BreadCrumbs");
            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }
    }
}