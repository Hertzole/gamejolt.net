#if UNITY_2021_1_OR_NEWER || !NET6_0_OR_GREATER
using System;
using Newtonsoft.Json;

namespace Hertzole.GameJolt
{
	internal static class JsonReaderExtensions
	{
		public static bool ReadAsBooleanWithGameJolt(this JsonReader reader)
		{
			if (reader.Read())
			{
				if (reader.TokenType == JsonToken.Boolean)
				{
					return (bool) reader.Value!;
				}

				if (reader.TokenType == JsonToken.String)
				{
					string value = (string) reader.Value;
					if (value!.Equals("1", StringComparison.OrdinalIgnoreCase))
					{
						return true;
					}

					if (value!.Equals("0", StringComparison.OrdinalIgnoreCase))
					{
						return false;
					}
				}
			}

			throw new JsonSerializationException("Unexpected token type: " + reader.TokenType);
		}
	}
}
#endif