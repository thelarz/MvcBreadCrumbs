using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Emit;
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

        [HttpGet]
        public ActionResult Get(int id)
        {
            BreadCrumb.SetLabel("Sample" + id);
            return View(id);
        }
        
        [HttpPost]
        public ActionResult Post()
        {
            return RedirectToAction("Complete");
        }

        [HttpGet,BreadCrumb(Label="Congratulations!")]
        public ActionResult Complete()
        {
            return View();
        }

        [HttpGet]
        public ActionResult ChildAction()
        {
            return PartialView();
        }

    }
}