using System;

namespace MvcBreadCrumbs
{
    public interface IProvideBreadCrumbsSession
    {
        string SessionId { get; }
    }
}
