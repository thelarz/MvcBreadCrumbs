using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MvcBreadCrumbs
{
    public interface IProvideBreadCrumbsSession
    {
        string SessionId { get; }
    }
}
