#if !NET6_0_OR_GREATER
#nullable enable

using System;
using Newtonsoft.Json;

namespace Hertzole.GameJolt
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
			TableInternal[] tables = Array.Empty<TableInternal>();
			
			while (reader.TokenType != JsonToken.EndObject)
			{
				// Read the property name.
				string propertyName = (string) reader.Value!;

				if (propertyName.Equals("tables", StringComparison.OrdinalIgnoreCase))
				{
					reader.Read();
					tables = serializer.Deserialize<TableInternal[]>(reader)! ?? Array.Empty<TableInternal>();
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