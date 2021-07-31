using System;
using System.Collections.Generic;
using System.Linq;

namespace HITUOISR.Toolkit.Settings
{
    /// <summary>
    /// <seealso cref="ISettingsRoot"/> 扩展方法。
    /// </summary>
    internal static class SettingsRootExtensions
    {
        internal static IEnumerable<ISettingsSection> GetChildrenImpl(this ISettingsRoot root, string? section)
        {
            return root.Keys
                .Select(key => SettingsPath.GetSubsection(key.Path, section))
                .OfType<string>()
                .Distinct(StringComparer.OrdinalIgnoreCase)
                .Select(subsection => root.GetSection(subsection));
        }
    }
}
