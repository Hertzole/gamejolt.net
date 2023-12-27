#nullable enable

#if NET6_0_OR_GREATER
using JsonName = System.Text.Json.Serialization.JsonPropertyNameAttribute;
using JsonConverter = System.Text.Json.Serialization.JsonConverterAttribute;
using JsonConstructor = System.Text.Json.Serialization.JsonConstructorAttribute;
using JsonIgnore = System.Text.Json.Serialization.JsonIgnoreAttribute;
#endif

namespace Hertzole.GameJolt
{
	internal readonly struct GetTablesResponse : IResponse
	{
		[JsonName("success")]
		[JsonConverter(typeof(GameJoltBooleanConverter))]
		public readonly bool success;
		[JsonName("message")]
		public readonly string? message;
		[JsonName("tables")]
		public readonly TableInternal[] tables;

		[JsonIgnore]
		public bool Success
		{
			get { return success; }
		}
		[JsonIgnore]
		public string? Message
		{
			get { return message; }
		}

		[JsonConstructor]
		public GetTablesResponse(bool success, string? message, TableInternal[] tables)
		{
			this.success = success;
			this.message = message;
			this.tables = tables;
		}

		public override string ToString()
		{
			return
				$"{nameof(success)}: {success}, {nameof(message)}: {message}, {nameof(tables)}: {(tables == null ? "" : tables.ToCommaSeparatedString())}";
		}
	}
}