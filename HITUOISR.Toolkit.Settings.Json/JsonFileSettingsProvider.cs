using HITUOISR.Toolkit.Common.Compatibility;
using HITUOISR.Toolkit.Settings.FileBase;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace HITUOISR.Toolkit.Settings.Json
{
    /// <summary>
    /// JSON文件设置提供器。
    /// </summary>
    public sealed class JsonFileSettingsProvider : FileSettingsProvider
    {
        /// <summary>
        /// 初始化。
        /// </summary>
        /// <param name="source">设置源。</param>
        public JsonFileSettingsProvider(JsonFileSettingsSource source) : base(source) { }

        private Dictionary<string, string> RawData { get; } = new(StringComparer.OrdinalIgnoreCase);

        private static readonly JsonDocumentOptions jsonDocumentOptions = new()
        {
            AllowTrailingCommas = true,
            CommentHandling = JsonCommentHandling.Skip,
        };

        private static readonly JsonSerializerOptions jsonSerializerOptions = new()
        {
            AllowTrailingCommas = true,
            IgnoreReadOnlyProperties = true,
            ReadCommentHandling = JsonCommentHandling.Skip,
            PropertyNameCaseInsensitive = true,
            WriteIndented = true,
            Converters = {
                new DoubleJsonConverter(),
                new JsonStringEnumConverter(),
            },
        };

        /// <inheritdoc/>
        protected override void Preload(Stream stream)
        {
            using var document = JsonDocument.Parse(stream, jsonDocumentOptions);
            JsonElement root = document.RootElement;
            if (root.ValueKind != JsonValueKind.Object)
                throw new InvalidDataException("The root element of json settings file is not an object.");
            RawData.Clear();
            foreach (var prop in root.EnumerateObject())
            {
                RawData.TryAdd(prop.Name, prop.Value.GetRawText());
            }
        }

        /// <inheritdoc/>
        protected override void Save(Stream stream)
        {
            UpdateRawData();
            using var writer = new Utf8JsonWriter(stream, options: new()
            {
                Indented = true,
            });
            writer.WriteStartObject();
            foreach (var (name, json) in RawData)
            {
                writer.WritePropertyName(name);
                // 没法直接写入原始JSON，暂且只能先解析再写入。
                // https://github.com/dotnet/runtime/issues/1784#issuecomment-608331125
                using var document = JsonDocument.Parse(json, jsonDocumentOptions);
                document.WriteTo(writer);
            }
            writer.WriteEndObject();
            writer.Flush();
        }

        /// <inheritdoc/>
        protected override bool TryLoadKey(ISettingsKeyInfo key)
        {
            if (RawData.TryGetValue(key.Path, out string? json))
            {
                Data[key.Path] = JsonSerializer.Deserialize(json, key.Type, jsonSerializerOptions);
                return true;
            }
            else
            {
                return false;
            }
        }

        private void UpdateRawData()
        {
            foreach (var (key, value) in Data)
            {
                RawData[key] = JsonSerializer.Serialize(value, value?.GetType() ?? typeof(object), jsonSerializerOptions);
            }
        }
    }
}
