using System;
using System.Web.Mvc;

namespace MvcBreadCrumbs
{

	[AttributeUsage(AttributeTargets.Method | AttributeTargets.Class, AllowMultiple = false)]
	public class BreadCrumbAttribute : ActionFilterAttribute
	{

		public bool Clear { get; set; }

		public string Label { get; set; }

		public Type ResourceType { get; set; }

		/// <summary>
		/// If set to true, prevents the <see cref="BreadCrumbAttribute"/> at the controller level to process the action.
		/// Use when you want to add dynamic breadcrumb trails in code 
		/// but still allows other actions to use the attribute at the controller level.
		/// </summary>
		public bool Manual { get; set; }

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

		public override void OnActionExecuting(ActionExecutingContext filterContext)
		{

			if (filterContext.IsChildAction)
				return;

			if (filterContext.HttpContext.Request.HttpMethod != "GET")
				return;

			if (Clear)
			{
				StateManager.RemoveState(SessionProvider.SessionId);
			}

			if (Manual)
				return;

			var state = StateManager.GetState(SessionProvider.SessionId);
			state.Push(filterContext, Label, ResourceType);

		}

	}

}