using System;
using System.Linq;
using System.Text;

namespace MvcBreadCrumbs
{
    public class BreadCrumb
    {

        private static IProvideBreadCrumbsSession _SessionProvider { get; set; }

        private static IProvideBreadCrumbsSession SessionProvider
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

            StringBuilder sb = new StringBuilder();
            sb.Append("<ul class=\"breadcrumb\">");
            state.Crumbs.ForEach(x =>
            {
                sb.Append("<li><a href=\"" + x.Url + "\">" + x.Label + "</a></li>");
            });
            sb.Append("</ul>");
            return sb.ToString();

        }
        public static string DisplayRaw()
        {

            var state = StateManager.GetState(SessionProvider.SessionId);

            return string.Join(" > ",
                state.Crumbs.Select(x => "<a href=\"" + x.Url + "\">" + x.Label + "</a>").ToArray());

        }

    }

}
