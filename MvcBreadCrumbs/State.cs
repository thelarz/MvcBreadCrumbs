using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;

namespace MvcBreadCrumbs
{

    public class State
    {
        
        public string SessionCookie { get; set; }
        public List<StateEntry> Crumbs { get; set; }

        public void Push(ActionExecutingContext context)
        {
            Crumbs.Add(new StateEntry().SetContext(context));
        }
        
        public State(string cookie)
        {
            SessionCookie = cookie;
            Crumbs = new List<StateEntry>();
        }

    }

    public class StateEntry
    {
        public string Key { get; set; }
        public ActionExecutingContext Context { get; private set; }

        public StateEntry SetContext(ActionExecutingContext context)
        {
            Context = context;
            return this;
        }

        public string Controller 
        { 
            get
            {
                return (string)Context.RouteData.Values["controller"];
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