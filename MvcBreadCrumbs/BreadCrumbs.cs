using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using System.Web.Mvc.Html;
using System.Web.Routing;
using System.Web.UI.HtmlControls;

namespace MvcBreadCrumbs
{
    public class BreadCrumbs
    {

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

        public static void SetLabel(string label)
        {
            var state = StateManager.GetState(SessionProvider.SessionId);
            state.Current.Label = label;
        }

        public static string Display()
        {
            
            var state = StateManager.GetState(SessionProvider.SessionId);
            //var ctx = state.Current.Context.RequestContext;
            
            state.Crumbs.ForEach(x =>
            {
                Trace.WriteLine(x.Url);
                //Trace.WriteLine("<a href=\"" + new UrlHelper(ctx).Action(x.Action, x.Controller) + "\">" + x.Label + "</a>");
            });

            return MvcHtmlString.Create(string.Join(" > ",
                state.Crumbs.Select(x => "<a href=\"" + x.Url + "\">" + x.Label + "</a>"))).ToHtmlString();

        }

    }

}
