using System;
using System.Globalization;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace Ballast.Atlantis.Utility {
    public class DateTimeFormatJsonConverter : DateTimeFormatJsonConverterBase<DateTime> {
    }

    public class NullableDateTimeFormatJsonConverter : DateTimeFormatJsonConverterBase<DateTime?> {
    }

    public class DateTimeFormatJsonConverterBase<T> : JsonConverter<T> {
        private const string DefaultDateTimeFormat = "yyyy'-'MM'-'dd'T'HH':'mm':'ss.FFFFFFFK";

        private DateTimeStyles _dateTimeStyles = DateTimeStyles.RoundtripKind;
        private string? _dateTimeFormat;
        private CultureInfo? _culture;

        /// <summary>
        /// Gets or sets the date time styles used when converting a date to and from JSON.
        /// </summary>
        /// <value>The date time styles used when converting a date to and from JSON.</value>
        public DateTimeStyles DateTimeStyles {
            get => _dateTimeStyles;
            set => _dateTimeStyles = value;
        }

        /// <summary>
        /// Gets or sets the date time format used when converting a date to and from JSON.
        /// </summary>
        /// <value>The date time format used when converting a date to and from JSON.</value>
        public string? DateTimeFormat {
            get => _dateTimeFormat ?? string.Empty;
            set => _dateTimeFormat = String.IsNullOrEmpty(value) ? null : value;
        }

        /// <summary>
        /// Gets or sets the culture used when converting a date to and from JSON.
        /// </summary>
        /// <value>The culture used when converting a date to and from JSON.</value>
        public CultureInfo Culture {
            get => _culture ?? CultureInfo.CurrentCulture;
            set => _culture = value;
        }

        public override T Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options) {
            if (reader.TokenType != JsonTokenType.String)
                throw new JsonException($"Unexpected token parsing date. Expected {nameof(JsonTokenType.String)}, got {reader.TokenType}");

            string dateText = reader.GetString();

            if (String.IsNullOrEmpty(dateText))
                throw new JsonException("Unexpected token parsing date. Got empty String");

            DateTime value;
            bool success;
            if (!String.IsNullOrEmpty(_dateTimeFormat)) {
                success = DateTime.TryParseExact(dateText, _dateTimeFormat, Culture, _dateTimeStyles, out value);
            }
            else {
                success = DateTime.TryParse(dateText, Culture, _dateTimeStyles, out value);
            }

            if (!success)
                throw new JsonException("Value is not in an expected format");

            return (T) (object) value;
        }

        public override void Write(Utf8JsonWriter writer, T value, JsonSerializerOptions options) {
            DateTime dateTimeValue = (DateTime) (object) value!;
            if ((_dateTimeStyles & DateTimeStyles.AdjustToUniversal) == DateTimeStyles.AdjustToUniversal
                || (_dateTimeStyles & DateTimeStyles.AssumeUniversal) == DateTimeStyles.AssumeUniversal) {
                dateTimeValue = dateTimeValue.ToUniversalTime();
            }

            string text = dateTimeValue.ToString(_dateTimeFormat ?? DefaultDateTimeFormat, Culture);
            writer.WriteStringValue(text);
        }
    }
}
