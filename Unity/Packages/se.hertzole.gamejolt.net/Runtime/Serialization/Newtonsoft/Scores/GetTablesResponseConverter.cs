#if !DISABLE_GAMEJOLT // Disables all GameJolt-related code

#if !NET6_0_OR_GREATER && !FORCE_SYSTEM_JSON
#nullable enable

using System;
using Newtonsoft.Json;

namespace Hertzole.GameJolt.Serialization.Newtonsoft
{
	internal sealed class GetTablesResponseConverter : ResponseConverter<GetTablesResponse>
	{
		protected override void WriteResponseJson(JsonWriter writer, GetTablesResponse value, JsonSerializer serializer)
		{
			writer.WritePropertyName("tables");
			serializer.Serialize(writer, value.tables);
		}

		protected override GetTablesResponse ReadResponseJson(JsonReader reader, JsonSerializer serializer)
		{
			TableInternal[]? tables = null;

			while (reader.TokenType != JsonToken.EndObject)
			{
				// Skip unknown types.
				if (reader.TokenType != JsonToken.PropertyName)
				{
					reader.Skip();
					reader.Read();
					continue;
				}

				// Read the property name.
				string propertyName = (string) reader.Value!;

				if (propertyName.Equals("tables", StringComparison.OrdinalIgnoreCase))
				{
					reader.Read();

					if (reader.TokenType == JsonToken.Null)
					{
						tables = Array.Empty<TableInternal>();
					}
					else
					{
						tables = serializer.Deserialize<TableInternal[]>(reader);
					}

					break;
				}

				// Read the next property name.
				reader.Read();
			}

			return new GetTablesResponse(true, string.Empty, tables);
		}

		protected override GetTablesResponse CreateResponse(bool success, string? message, GetTablesResponse existingData)
		{
			return new GetTablesResponse(success, message, existingData.tables);
		}
	}
}
#endif
#endif // DISABLE_GAMEJOLT