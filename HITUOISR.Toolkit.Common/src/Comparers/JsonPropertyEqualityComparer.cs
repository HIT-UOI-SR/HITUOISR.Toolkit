using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Text.Json;

namespace HITUOISR.Toolkit.Common.Comparers
{
    /// <summary>
    /// <see cref="JsonProperty"/> 相等性比较器。
    /// </summary>
    internal class JsonPropertyEqualityComparer : EqualityComparer<JsonProperty>
    {
        private readonly IEqualityComparer<string> NameComparer;
        private readonly IEqualityComparer<JsonElement> ValueComparer;

        /// <summary>
        /// 初始化 <see cref="JsonProperty"/> 相等性比较器。
        /// </summary>
        /// <param name="nameComparer">属性名称比较器。</param>
        /// <param name="valueComparer">属性值比较器。</param>
        public JsonPropertyEqualityComparer(IEqualityComparer<string> nameComparer, IEqualityComparer<JsonElement> valueComparer)
        {
            NameComparer = nameComparer;
            ValueComparer = valueComparer;
        }

        /// <inheritdoc/>
        public override bool Equals(JsonProperty x, JsonProperty y) =>
            NameComparer.Equals(x.Name, y.Name) && ValueComparer.Equals(x.Value, y.Value);

        /// <inheritdoc/>
        public override int GetHashCode([DisallowNull] JsonProperty obj)
        {
            HashCode hash = new();
            hash.Add(obj.Name, NameComparer);
            hash.Add(obj.Value, ValueComparer);
            return hash.ToHashCode();
        }
    }
}
