# MvcBreadCrumbs
MvcBreadCrumbs is a [NuGet](http://nuget.org/) package that allows you to specify custom bread crumb definitions at the controller and route level. 

## Integrating MvcRouteFlow into your web project

It's simple: 

1. Install the Nuget package MvcBreadCrumbs. 
2. Modify your _layout.cshtml to display the breadcrumb links
3. Start adding the BreadCrumb attribute to your controllers.

By default MvcBreadCrumbs ignores all Http Methods except for GET.

## Sample _Layout.cshtml snipet ##

By default, the MvcBreadCrumbs will display as a ul/li/anchor with the appropriate class name to have BootStrap style the breadcrumb for you.  You can thank me later.

    @Html.Raw(BreadCrumb.Display())

## Sample BreadCrumbs ##

This sample will display bread crumbs for all routes using the Action name as the BreadCrumb label.

    [BreadCrumb]
    public class SampleController : Controller
    {
        public ActionResult Index()
        {
            return View();
        }
    }

This sample demostrates how MvcBreadCrumb will clear the bread crumb stack and add a new entry with the label "Widgets" instead of the default label "Index".

    public class SampleController : Controller
    {
        [BreadCrumb(Clear = true, Label = "Widgets")]
        public ActionResult Index()
        {
            return View();
        }
    }

This sample shows how you can control the bread crumb label directly with data from your model.

    public class SampleController : Controller
    {
        [BreadCrumb]
        public ActionResult GetProduct(int id)
        {
            var model = db.GetProduct(id);
            BreadCrumb.SetLabel("Product " + model.ProductName);
            return View(model);
        }
    }

This sample demostrates how to clear the current breadcrumb stack with code and also how to push a custom URL onto the stack with code.

    public class SampleController : Controller
    {
        public ActionResult ClearIt()
        {
            BreadCrumb.Clear();
            BreadCrumb.Add(Url.Action("SomeAction", "Home"), "NewRoot");
            return View();
        }
    }





