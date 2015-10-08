using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Linq;
using System.Net.Http;
using System.Reflection;
using System.Web;
using System.Web.Mvc;

namespace MvcBreadCrumbs
{
   
    [AttributeUsage(AttributeTargets.Method | AttributeTargets.Class, AllowMultiple = false)]
    public class BreadCrumbAttribute : ActionFilterAttribute
    {

        public bool Clear { get; set; }
        public static IProvideBreadCrumbsSession _SessionProvider { get; set; }

        public static IProvideBreadCrumbsSession SessionProvider
        {
            get
            {
                if (_SessionProvider != null)
                {
                    return _SessionProvider;
                }
                return new HttpSessionProvider();
            }
        }

        public override void OnActionExecuting(ActionExecutingContext filterContext)
        {

            if (filterContext.IsChildAction)
                return;

            if (filterContext.HttpContext.Request.HttpMethod != "GET")
                return;

            if (Clear)
            {
                StateManager.RemoveState(SessionProvider.SessionId);
            }

            var state = StateManager.GetState(SessionProvider.SessionId);
            state.Push(filterContext);

        }

      

    }

    //[AttributeUsage(AttributeTargets.Method, AllowMultiple = true)]
    //public class RouteFlowSetCorrelation : ActionFilterAttribute
    //{
    //    public Type Path { get; set; }
    //    public string As { get; set; }
    //    public string Value { get; set; }

    //    public override void OnActionExecuting(ActionExecutingContext filterContext)
    //    {
    //        if (!RouteFlow.OnPath(Path.Name))
    //            return;

    //        var actionValues = filterContext.ActionParameters;
    //        if (actionValues[Value] == null)
    //        {
    //            // Attempting to set a correlation with a null value (maybe due to restarting a routeflow step)
    //            var value = RouteFlow.GetCorrelationId(As);
    //            if (value != null)
    //            {
    //                actionValues[Value] = RouteFlow.GetCorrelationId(As);
    //                return;
    //            }
    //        }
    //        RouteFlow.SetCorrelationId(As, actionValues[Value]);
    //    }
    //}

    //[AttributeUsage(AttributeTargets.Method, AllowMultiple = true)]
    //public class RouteFlowGetCorrelation : ActionFilterAttribute
    //{
    //    public Type Path { get; set; }
    //    public string Name { get; set; }
    //    public string AssignTo { get; set; }

    //    public override void OnActionExecuting(ActionExecutingContext filterContext)
    //    {
    //        if (!RouteFlow.OnPath(Path.Name))
    //            return;

    //        var actionValues = filterContext.ActionParameters;
    //        if (actionValues.ContainsKey(AssignTo))
    //            actionValues[AssignTo] = RouteFlow.GetCorrelationId(Name);
    //    }
    //}

    
}