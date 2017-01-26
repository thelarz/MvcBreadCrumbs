using System;
using System.Reflection;

namespace MvcBreadCrumbs
{
    /// <summary>
    /// Helper class for retrieving the tranlation values
    /// </summary>
    public class ResourceHelper
    {
        /// <summary>
        /// Gets the resource lookup.
        /// </summary>
        /// <param name="resourceType">Type of the resource.</param>
        /// <param name="resourceName">Name of the resource.</param>
        /// <returns></returns>
        public static string GetResourceLookup(Type resourceType, string resourceName)
        {
            if ((resourceType != null) && (resourceName != null))
            {
                var property = resourceType.GetProperty(resourceName, BindingFlags.Public | BindingFlags.Static);
                if (property == null)
                {
                    return resourceName;
                }
                if (property.PropertyType != typeof(string))
                {
                    return resourceName;
                }

                return (string)property.GetValue(null, null);
            }

            return resourceName ?? string.Empty;
        }
    }
}