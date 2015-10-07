using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MvcBreadCrumbs
{
    public class BraedCrumbs
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

    }

}
