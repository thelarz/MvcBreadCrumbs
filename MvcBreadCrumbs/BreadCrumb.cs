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

        public static void Clear()
        {
            StateManager.RemoveState(SessionProvider.SessionId);
        }

        public static string Display()
        {
            
            var state = StateManager.GetState(SessionProvider.SessionId);

            if (state.Crumbs != null && !state.Crumbs.Any())
                return "<!-- BreadCrumbs stack is empty -->";

            StringBuilder sb = new StringBuilder();
            sb.Append("<ol class=\"breadcrumb\">");
            state.Crumbs.ForEach(x =>
            {
                if (IsCurrentPage(x.Key))
                {
                    sb.Append("<li class='active'>" + x.Label + "</li>");
                }
                else
                {
                    sb.Append("<li><a href=\"" + x.Url + "\">" + x.Label + "</a></li>");
                }
            });
            sb.Append("</ol>");
            return sb.ToString();

        }
        public static string DisplayRaw()
        {

            var state = StateManager.GetState(SessionProvider.SessionId);

            if (state.Crumbs != null && !state.Crumbs.Any())
                return "<!-- BreadCrumbs stack is empty -->";

            return string.Join(" > ",
                state.Crumbs.Select(x => "<a href=\"" + x.Url + "\">" + x.Label + "</a>").ToArray());

        }

        private static bool IsCurrentPage(int compareKey)
        {
            var key =
                System.Web.HttpContext.Current.Request.Url.ToString()
                .ToLower()
                .GetHashCode();
            return key == compareKey;
        }

    }

}
