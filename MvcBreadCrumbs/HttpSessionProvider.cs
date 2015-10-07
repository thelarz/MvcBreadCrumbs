using System;
using System.Collections.Generic;       
using System.Linq;
using System.Text;
using System.Web;

namespace MvcBreadCrumbs
{
    public class HttpSessionProvider : IProvideBreadCrumbsSession
    {
        public string SessionId
        {
            get
            {
                var id = HttpContext.Current.Session.SessionID;

                // Apparently you need to actually ad something to session in order to
                // stabilize the SessionID between requests, who knew.  Just adding SessionID,
                // as a dummy.
                HttpContext.Current.Session["SessionId"] = id;
                return id;
            }
        }
    }
}
