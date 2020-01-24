using System;
using System.Collections.Generic;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace Ballast.Atlantis.Utility {
    public class DictionaryStringPrimitiveJsonConverter : JsonConverter<IDictionary<string, object?>> {
        public override bool CanConvert(Type typeToConvert) {
            return typeof(IDictionary<string, object?>).IsAssignableFrom(typeToConvert);
        }

        public override IDictionary<string, object?> Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options) {
            if (reader.TokenType != JsonTokenType.StartObject)
                throw new JsonException($"Unexpected token. Expected object start, got {reader.TokenType}");

            Dictionary<string, object?> dictionary = new Dictionary<string, object?>();
            while (reader.Read()) {
                if (reader.TokenType == JsonTokenType.EndObject)
                    return dictionary;

                if (reader.TokenType != JsonTokenType.PropertyName)
                    throw new JsonException($"Unexpected token. Expected property name, got {reader.TokenType}");

                string key = reader.GetString();
                reader.Read();

                object? value;
                switch (reader.TokenType) {
                    case JsonTokenType.String:
                        value = reader.GetString();
                        break;
                    case JsonTokenType.Number:
                        if (reader.TryGetInt32(out int intValue)) {
                            value = intValue;
                        }
                        else if (reader.TryGetUInt32(out uint uintValue)) {
                            value = uintValue;
                        }
                        else if (reader.TryGetInt64(out long longValue)) {
                            value = longValue;
                        }
                        else if (reader.TryGetUInt64(out ulong ulongValue)) {
                            value = ulongValue;
                        }
                        else {
                            value = reader.GetDouble();
                        }

                        break;
                    case JsonTokenType.True:
                        value = true;
                        break;
                    case JsonTokenType.False:
                        value = false;
                        break;
                    case JsonTokenType.Null:
                        value = null;
                        break;
                    default:
                        throw new JsonException($"Unexpected token. Expected a value, got {reader.TokenType}");
                }

                dictionary.Add(key, value);
            }

            throw new JsonException($"Unexpected token. Expected object end, got {reader.TokenType}");
        }

        public override void Write(Utf8JsonWriter writer, IDictionary<string, object?> value, JsonSerializerOptions options) {
            writer.WriteStartObject();
            foreach (KeyValuePair<string, object?> pair in value) {
                writer.WritePropertyName(pair.Key);
                switch (pair.Value) {
                    case null:
                        writer.WriteNullValue();
                        break;
                    case bool pairValue:
                        writer.WriteBooleanValue(pairValue);
                        break;
                    case string pairValue:
                        writer.WriteStringValue(pairValue);
                        break;
                    case sbyte pairValue:
                        writer.WriteNumberValue(pairValue);
                        break;
                    case byte pairValue:
                        writer.WriteNumberValue(pairValue);
                        break;
                    case short pairValue:
                        writer.WriteNumberValue(pairValue);
                        break;
                    case ushort pairValue:
                        writer.WriteNumberValue(pairValue);
                        break;
                    case int pairValue:
                        writer.WriteNumberValue(pairValue);
                        break;
                    case uint pairValue:
                        writer.WriteNumberValue(pairValue);
                        break;
                    case long pairValue:
                        writer.WriteNumberValue(pairValue);
                        break;
                    case ulong pairValue:
                        writer.WriteNumberValue(pairValue);
                        break;
                    case float pairValue:
                        writer.WriteNumberValue(pairValue);
                        break;
                    case double pairValue:
                        writer.WriteNumberValue(pairValue);
                        break;
                    case decimal pairValue:
                        writer.WriteNumberValue(pairValue);
                        break;
                    default:
                        throw new JsonException($"Unexpected value type {pair.Value.GetType().Name}. Only primitive built-in types are supported.");
                }
            }

            writer.WriteEndObject();
        }
    }
}
