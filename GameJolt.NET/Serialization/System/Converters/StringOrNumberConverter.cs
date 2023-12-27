using System;
using System.Globalization;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace Hertzole.GameJolt
{
	internal sealed class StringOrNumberConverter : JsonConverter<string>
	{
		public override string? Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
		{
			if (reader.TokenType == JsonTokenType.Number)
			{
				return reader.TryGetInt32(out int value)
					? value.ToString(CultureInfo.InvariantCulture)
					: reader.GetDouble().ToString(CultureInfo.InvariantCulture);
			}

			string? stringValue = reader.GetString();

			if (string.IsNullOrEmpty(stringValue))
			{
				return string.Empty;
			}

			if (int.TryParse(stringValue, out int intValue))
			{
				return intValue.ToString(CultureInfo.InvariantCulture);
			}

			if (double.TryParse(stringValue, out double doubleValue))
			{
				return doubleValue.ToString(CultureInfo.InvariantCulture);
			}

			return reader.GetString();
		}

		public override void Write(Utf8JsonWriter writer, string value, JsonSerializerOptions options)
		{
			writer.WriteStringValue(value);
		}
	}
}