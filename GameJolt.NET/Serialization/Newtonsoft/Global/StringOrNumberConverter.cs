#if !NET6_0_OR_GREATER
#nullable enable

using System;
using System.Globalization;
using Newtonsoft.Json;

namespace Hertzole.GameJolt
{
	internal sealed class StringOrNumberConverter : JsonConverter<string>
	{
		public static readonly StringOrNumberConverter Instance = new StringOrNumberConverter();

		public override bool CanRead
		{
			get { return true; }
		}

		public override bool CanWrite
		{
			get { return true; }
		}

		public override void WriteJson(JsonWriter writer, string? value, JsonSerializer serializer)
		{
			writer.WriteValue(value);
		}

		public override string? ReadJson(JsonReader reader, Type objectType, string? existingValue, bool hasExistingValue, JsonSerializer serializer)
		{
			if (reader.TokenType == JsonToken.Integer)
			{
				return reader.ReadAsInt32()!.Value.ToString(CultureInfo.InvariantCulture);
			}

			if (reader.TokenType == JsonToken.Float)
			{
				return reader.ReadAsDouble()!.Value.ToString(CultureInfo.InvariantCulture);
			}

			string? stringValue = reader.ReadAsString();

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

			return reader.Value as string;
		}
	}
}
#endif