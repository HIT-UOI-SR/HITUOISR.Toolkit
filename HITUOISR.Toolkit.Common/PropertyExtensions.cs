using System;
using System.Linq;
using System.Reflection;

namespace HITUOISR.Toolkit.Common
{
    /// <summary>
    /// <seealso cref="PropertyInfo"/> 扩展方法。
    /// </summary>
    public static class PropertyExtensions
    {
        /// <summary>
        /// 检查属性是否是init-only的。
        /// </summary>
        /// <remarks>
        /// 参见：https://docs.microsoft.com/zh-cn/dotnet/csharp/language-reference/proposals/csharp-9.0/init
        /// </remarks>
        /// <param name="property">要检查的属性元信息。</param>
        /// <returns>若该属性的设置器是init-only的，则为<see langword="true"/>；否则，为<see langword="false"/>。</returns>
        public static bool IsInitOnly(this PropertyInfo property)
        {
            MethodInfo? setMethod = property.SetMethod;
            if (setMethod == null)
                return false;
            return setMethod.ReturnParameter
                .GetRequiredCustomModifiers()
                .Contains(typeof(System.Runtime.CompilerServices.IsExternalInit));
        }
    }
}
