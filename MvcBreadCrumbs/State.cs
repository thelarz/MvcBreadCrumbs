using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Reflection;
using System.Web.Mvc;

namespace MvcBreadCrumbs
{
    public class State
    {
        public string SessionCookie { get; set; }
        public List<StateEntry> Crumbs { get; set; }
        public StateEntry Current { get; set; }

        public void Push(ActionExecutingContext context, string label, Type resourceType )
        {
            var key =
                context.HttpContext.Request.Url.LocalPath
                .ToLower()
                .GetHashCode();

            if (Crumbs.Any(x => x.Key == key))
            {
                var newCrumbs = new List<StateEntry>();
                var remove = false;
                // We've seen this route before, maybe user clicked on a breadcrumb
                foreach (var crumb in Crumbs)
                {
                    if (crumb.Key == key)
                    {
                        remove = true;
                    }
                    if (!remove)
                    {
                        newCrumbs.Add(crumb);
                    }
                }
                Crumbs = newCrumbs;
            }

            Current = new StateEntry().WithKey(key)
                .SetContext(context)
                .WithUrl(context.HttpContext.Request.Url.ToString())
                .WithLabel(ResourceHelper.GetResourceLookup(resourceType, label));
                
            Crumbs.Add(Current);
        }
        
        public State(string cookie)
        {
            SessionCookie = cookie;
            Crumbs = new List<StateEntry>();
        }
    }

    public class StateEntry
    {
        public ActionExecutingContext Context { get; private set; }
        public string Label { get; set; }
        public int Key { get; set; }
        public string Url { get; set; }

        public StateEntry WithKey(int key)
        {
            Key = key;
            return this;
        }

        public StateEntry WithUrl(string url)
        {
            Url = url;
            return this;
        }

        public StateEntry WithLabel(string label)
        {
            Label = label ?? Label;
            return this;
        }


        public StateEntry SetContext(ActionExecutingContext context)
        {
            Context = context;
            var type = Context.Controller.GetType();
            var actionName = (string)Context.RouteData.Values["Action"];
            var labelQuery =
                from m in type.FindMembers(MemberTypes.Method, BindingFlags.Public | BindingFlags.Instance, (memberInfo, _) => memberInfo.Name == actionName, null)
                let atts = m.GetCustomAttributes(typeof(DisplayAttribute), false)
                where atts.Length > 0
                select ((DisplayAttribute)atts[0]).GetName();
            Label = labelQuery.FirstOrDefault() ?? (string)context.RouteData.Values["Action"];
            return this;
        }

        public string Controller 
        { 
            get
            {
                return (string) Context.RouteData.Values["controller"];
            } 
        }

        public string Action
        {
            get
            {
                return (string)Context.RouteData.Values["action"];
            }
        }
    }
}