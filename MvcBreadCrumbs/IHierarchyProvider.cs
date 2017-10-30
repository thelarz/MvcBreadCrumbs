namespace MvcBreadCrumbs
{
	internal interface IHierarchyProvider
	{
		int GetLevel();
		int GetLevel(string url);
	}
}