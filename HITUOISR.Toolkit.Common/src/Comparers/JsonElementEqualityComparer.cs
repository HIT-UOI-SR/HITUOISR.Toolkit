using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Text.Json;

namespace HITUOISR.Toolkit.Common.Comparers
{
    /// <summary>
    /// <see cref="JsonElement"/> 相等性比较器。
    /// </summary>
    public class JsonElementEqualityComparer : EqualityComparer<JsonElement>
    {
        private readonly JsonPropertyEqualityComparer PropertyComparer;

        /// <summary>
        /// 初始化 <see cref="JsonElement"/> 相等性比较器。
        /// </summary>
        /// <param name="propertyNameComparer">属性名称比较器。</param>
        public JsonElementEqualityComparer(IEqualityComparer<string> propertyNameComparer) =>
            PropertyComparer = new JsonPropertyEqualityComparer(propertyNameComparer, this);

        /// <inheritdoc/>
        public override bool Equals(JsonElement x, JsonElement y) => x.ValueKind == y.ValueKind && 
            x.ValueKind switch
            {
                JsonValueKind.Object => x.EnumerateObject().SequenceEqual(y.EnumerateObject(), PropertyComparer),
                JsonValueKind.Array => x.EnumerateArray().SequenceEqual(y.EnumerateArray(), this),
                JsonValueKind.String => x.ValueEquals(y.GetString()),
                JsonValueKind.Number => x.GetRawText() == y.GetRawText(),
                _ => true,
            };

        /// <inheritdoc/>
        public override int GetHashCode([DisallowNull] JsonElement obj)
        {
            HashCode hash = new();
            hash.Add(obj.ValueKind);
            switch (obj.ValueKind)
            {
                case JsonValueKind.Object:
                    foreach (var prop in obj.EnumerateObject())
                    {
                        hash.Add(prop, PropertyComparer);
                    }
                    break;
                case JsonValueKind.Array:
                    foreach (var val in obj.EnumerateArray())
                    {
                        hash.Add(val, this);
                    }
                    break;
                case JsonValueKind.String:
                    hash.Add(obj.GetString());
                    break;
                case JsonValueKind.Number:
                    hash.Add(obj.GetRawText());
                    break;
                default:
                    break;
            }
            return hash.ToHashCode();
        }
    }
}
