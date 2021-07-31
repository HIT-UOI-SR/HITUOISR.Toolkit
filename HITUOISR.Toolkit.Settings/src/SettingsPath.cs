using System.Collections.Generic;

namespace HITUOISR.Toolkit.Settings
{
    /// <summary>
    /// 设置路径的辅助方法。
    /// </summary>
    public static class SettingsPath
    {
        /// <summary>
        /// 设置键分隔符。
        /// </summary>
        public static readonly char KeyDelimiter = '.';

        /// <summary>
        /// 拼接路径。
        /// </summary>
        /// <param name="pathSegments">要拼接的路径节。</param>
        /// <returns>拼接的路径。</returns>
        public static string Combine(params string[] pathSegments) => string.Join(KeyDelimiter, pathSegments);

        /// <summary>
        /// 拼接路径。
        /// </summary>
        /// <param name="pathSegments">要拼接的路径节。</param>
        /// <returns>拼接的路径。</returns>
        public static string Combine(IEnumerable<string> pathSegments) => string.Join(KeyDelimiter, pathSegments);

        /// <summary>
        /// 提取路径的最后一个片段（无修饰的键）。
        /// </summary>
        /// <param name="path">路径。</param>
        /// <returns>路径的最后一个片段。</returns>
        public static string GetSectionKey(string path)
        {
            int lastDelimiterIndex = path.LastIndexOf(KeyDelimiter);
            return lastDelimiterIndex == -1 ? path : path[(lastDelimiterIndex + 1)..];
        }

        /// <summary>
        /// 提取路径的父级。
        /// </summary>
        /// <param name="path">路径。</param>
        /// <returns>路径的父级路径。</returns>
        public static string GetParentPath(string path)
        {
            int lastDelimiterIndex = path.LastIndexOf(KeyDelimiter);
            return lastDelimiterIndex == -1 ? string.Empty : path.Substring(0, lastDelimiterIndex);
        }

        /// <summary>
        /// 提取路径中指定节的子节。
        /// </summary>
        /// <param name="path">要提取的路径。</param>
        /// <param name="section">指定的节（若为<see langword="null"/>，则返回顶级子节）。</param>
        /// <returns>
        /// 如果路径中没有指定的节，则为<see langword="null"/>；
        /// 如果路径中有指定的节，则返回它的第一级子节。
        /// </returns>
        public static string? GetSubsection(string path, string? section)
        {
            string start = section == null ? string.Empty : section + KeyDelimiter;
            if (path.StartsWith(start))
            {
                string rest = path[start.Length..];
                int firstDelimiterIndex = rest.IndexOf(KeyDelimiter);
                return firstDelimiterIndex == -1 ? path : path.Substring(0, start.Length + firstDelimiterIndex);
            }
            else
            {
                return null;
            }
        }
    }
}
