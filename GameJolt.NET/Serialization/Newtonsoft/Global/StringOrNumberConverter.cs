#if !NET6_0_OR_GREATER
#nullable enable

using System;
using System.Globalization;
using Newtonsoft.Json;

namespace Hertzole.GameJolt.Serialization.Newtonsoft
{
	internal sealed class StringOrNumberConverter : JsonConverter<string>
	{
		public static readonly StringOrNumberConverter Instance = new StringOrNumberConverter();

		public override void WriteJson(JsonWriter writer, string? value, JsonSerializer serializer)
		{
			writer.WriteValue(value);
		}

		public override string? ReadJson(JsonReader reader, Type objectType, string? existingValue, bool hasExistingValue, JsonSerializer serializer)
		{
			reader.Read();

			if (reader.TokenType == JsonToken.Integer)
			{
				long lValue = (long) reader.Value!;
				return lValue.ToString(CultureInfo.InvariantCulture);
			}

			if (reader.TokenType == JsonToken.Float)
			{
				double dValue = (double) reader.Value!;
				return dValue.ToString(CultureInfo.InvariantCulture);
			}

			string? stringValue = reader.Value as string;

			if (string.IsNullOrEmpty(stringValue))
			{
				return string.Empty;
			}

			if (long.TryParse(stringValue, NumberStyles.Integer, CultureInfo.InvariantCulture, out long longValue))
			{
				return longValue.ToString(CultureInfo.InvariantCulture);
			}

			if (double.TryParse(stringValue, NumberStyles.Float, CultureInfo.InvariantCulture, out double doubleValue))
			{
				return doubleValue.ToString(CultureInfo.InvariantCulture);
			}

			return reader.Value as string;
		}
	}
}
#endif