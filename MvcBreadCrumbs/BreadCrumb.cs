using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Web.Mvc;

namespace MvcBreadCrumbs
{
	public class BreadCrumb
	{
		private static IProvideBreadCrumbsSession _sessionProvider { get; set; }

		private static IProvideBreadCrumbsSession SessionProvider
		{
			get
			{
				if (_sessionProvider != null)
				{
					return _sessionProvider;
				}
				return new HttpSessionProvider();
			}
		}

		private static IHierarchyProvider _hierarchyProvider { get; set; }

		internal static IHierarchyProvider HierarchyProvider
		{
			get
			{
				if (_hierarchyProvider != null)
					return _hierarchyProvider;

				return new DefaultHierarchyProvider();
			}
		}


		public static void Add(string url, string label)
		{
			var state = StateManager.GetState(SessionProvider.SessionId);
			state.Push(url, label);
		}

		public static void SetLabel(string label)
		{
			var state = StateManager.GetState(SessionProvider.SessionId);
			state.SetCurrentLabel(label);
		}

		public static void Clear()
		{
			StateManager.RemoveState(SessionProvider.SessionId);
		}

		/// <summary>
		/// Get the currently active URL from the BreadCrumb
		/// </summary>
		/// <returns>The currently active URL from the BreadCrumb</returns>
		public static string GetCurrentUrl()
		{
			return StateManager.GetState(SessionProvider.SessionId).Current.Url;
		}

		/// <summary>
		/// Get the URL of the preceeding item from the BreadCrumb
		/// </summary>
		/// <returns>The URL of the preceeding item in the breadcrumb</returns>
		public static string GetPreviousUrl()
		{
			var previousPage = StateManager.GetState(SessionProvider.SessionId).Crumbs;
			var updatedList = new SortedSet<StateEntry>(previousPage, new StateEntryComparer());
			updatedList.Reverse();

			if (updatedList.Count > 1)
				return updatedList.Skip(1).First().Url;

			return null;
		}

		/// <summary>
		/// Get the full list of URL currently in the breadcrumb. Index 0 being the farthest page.
		/// </summary>
		/// <returns>The full list of URL currently in the breadcrumb</returns>
		public static IEnumerable<string> GetOrderedUrls()
		{
			return StateManager.GetState(SessionProvider.SessionId).Crumbs.Select(s => s.Url);
		}

		/// <summary>
		/// Redirects
		/// </summary>
		/// <returns></returns>
		public static RedirectResult RedirectToPreviousUrl()
		{
			var previousPage = StateManager.GetState(SessionProvider.SessionId).Crumbs;
			var updatedList = new SortedSet<StateEntry>(previousPage, new StateEntryComparer());
			updatedList.Reverse();

			if (updatedList.Count > 1)
			{
				if (string.IsNullOrEmpty(updatedList.Skip(1).First().Url))
					return new RedirectResult(updatedList.Skip(1).First().Url);
			}

			return null;
		}

		/// <summary>
		/// Get the full list of <see cref="RedirectResult"/> currently in the breadcrumb. Index 0 being the farthest page.
		/// </summary>
		/// <returns>The full list of <see cref="RedirectResult"/> currently in the breadcrumb</returns>
		public static IEnumerable<RedirectResult> GetOrderedRedirections()
		{
			return StateManager.GetState(SessionProvider.SessionId).Crumbs.Select(s => new RedirectResult(s.Url));
		}

		public static string Display(string cssClassOverride = "breadcrumb")
		{

			var state = StateManager.GetState(SessionProvider.SessionId);

			if (state.Crumbs != null && !state.Crumbs.Any())
				return "<!-- BreadCrumbs stack is empty -->";

			StringBuilder sb = new StringBuilder();
			sb.Append("<ol class=\"");
			sb.Append(cssClassOverride);
			sb.Append("\">");
			state.Crumbs.Select(x => new { Entry = x, IsCurrent = IsCurrentPage(x.Key) }).OrderBy(x => x.IsCurrent).ToList().ForEach(x =>
			{
				string label = string.IsNullOrWhiteSpace(x.Entry.Label) ? x.Entry.Action : x.Entry.Label;

				if (x.IsCurrent)
				{
					sb.Append("<li class='active'>" + label + "</li>");
				}
				else
				{
					sb.Append("<li><a href=\"" + x.Entry.Url + "\">" + label + "</a></li>");
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

			// don't allow blank labels to propagate outside
			state.Crumbs.ToList().ForEach(x => { x.Label = string.IsNullOrWhiteSpace(x.Label) ? x.Action : x.Label; });

			return string.Join(" > ",
				state.Crumbs.Select(x => "<a href=\"" + x.Url + "\">" + x.Label + "</a>").ToArray());

		}

		private static bool IsCurrentPage(int compareKey)
		{
			var key =
				System.Web.HttpContext.Current.Request.Url
				.LocalPath
				.ToLowerInvariant()
				.GetHashCode();

			return key == compareKey;
		}

	}

}
