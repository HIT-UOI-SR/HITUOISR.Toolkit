using System;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace HITUOISR.Toolkit.Common.Compatibility
{
    /// <summary>
    /// 双精度浮点数值JSON转换器。
    /// </summary>
    /// <remarks>
    /// 支持 <see cref="double.NaN"/> 等特殊值。
    /// </remarks>
    public sealed class DoubleJsonConverter : JsonConverter<double>
    {
        /// <inheritdoc/>
        public override double Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
        {
            return reader.TokenType switch
            {
                JsonTokenType.Number => reader.GetDouble(),
                JsonTokenType.String => double.Parse(reader.GetString() ?? string.Empty),
                _ => throw new JsonException(),
            };
        }

        /// <inheritdoc/>
        public override void Write(Utf8JsonWriter writer, double value, JsonSerializerOptions options)
        {
            if (double.IsFinite(value))
            {
                writer.WriteNumberValue(value);
            }
            else
            {
                writer.WriteStringValue(value.ToString("G"));
            }
        }
    }
}
