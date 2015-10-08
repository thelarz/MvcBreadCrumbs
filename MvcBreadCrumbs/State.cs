using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Emit;
using System.Runtime.CompilerServices;
using System.Web.Mvc;
using Microsoft.SqlServer.Server;

namespace MvcBreadCrumbs
{

    public class State
    {
        
        public string SessionCookie { get; set; }
        public List<StateEntry> Crumbs { get; set; }
        public StateEntry Current { get; set; }
       

        public void Push(ActionExecutingContext context, string label)
        {

            var key =
                context.HttpContext.Request.Url.ToString()
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
                .WithLabel(label);
                
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
            Label = Label ?? (string) context.RouteData.Values["action"];
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