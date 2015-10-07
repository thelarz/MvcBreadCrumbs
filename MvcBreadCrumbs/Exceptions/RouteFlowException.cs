using System;

namespace MvcBreadCrumbs.Exceptions
{
    public class BreadCrumbException : Exception
    {
        public BreadCrumbException()
            : base()
        {
        }
        public BreadCrumbException(string message)
            : base(message)
        {
        }
        public BreadCrumbException(string message, Exception inner)
            : base(message, inner)
        {
        }
    }
}
