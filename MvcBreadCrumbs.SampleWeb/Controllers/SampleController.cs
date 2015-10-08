using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace MvcBreadCrumbs.SampleWeb.Controllers
{
    [BreadCrumb]
    public class SampleController : Controller
    {
        // GET: Sample
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult Get(int id)
        {
            BreadCrumbs.SetLabel("Sample" + id);
            return View(id);
        }
    }
}