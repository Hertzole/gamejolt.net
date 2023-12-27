#nullable enable

#if NET6_0_OR_GREATER
using JsonName = System.Text.Json.Serialization.JsonPropertyNameAttribute;
using JsonConverter = System.Text.Json.Serialization.JsonConverterAttribute;
using JsonConstructor = System.Text.Json.Serialization.JsonConstructorAttribute;
#endif

namespace Hertzole.GameJolt
{
	internal readonly struct UpdateDataResponse : IResponse
	{
		[JsonName("data")]
		[JsonConverter(typeof(StringOrNumberConverter))]
		public readonly string data;
		[JsonName("success")]
		[JsonConverter(typeof(GameJoltBooleanConverter))]
		public bool Success { get; }
		[JsonName("message")]
		public string? Message { get; }

		[JsonConstructor]
		public UpdateDataResponse(bool success, string? message, string data)
		{
			this.data = data;
			Success = success;
			Message = message;
		}
	}
}