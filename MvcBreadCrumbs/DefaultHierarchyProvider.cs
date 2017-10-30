using System;
using System.Linq;

namespace MvcBreadCrumbs
{
	internal class DefaultHierarchyProvider : IHierarchyProvider
	{
		public int GetLevel()
		{
			throw new NotImplementedException();
		}

		public int GetLevel(string url)
		{
			// this is a simple, naive implementation counting the forward slashes to determine levels 
			// but it works fine for most scenarios
			return url.Count(c => c.Equals('/'));
		}
	}
}