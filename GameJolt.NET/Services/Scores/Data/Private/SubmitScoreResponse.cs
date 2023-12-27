#nullable enable

#if NET6_0_OR_GREATER
using JsonName = System.Text.Json.Serialization.JsonPropertyNameAttribute;
using JsonConstructor = System.Text.Json.Serialization.JsonConstructorAttribute;
using JsonConverter = System.Text.Json.Serialization.JsonConverterAttribute;
#endif

namespace Hertzole.GameJolt
{
	internal readonly struct SubmitScoreResponse : IResponse
	{
		[JsonName("success")]
		[JsonConverter(typeof(GameJoltBooleanConverter))]
		public bool Success { get; }
		[JsonName("message")]
		public string? Message { get; }

		[JsonConstructor]
		public SubmitScoreResponse(bool success, string? message)
		{
			Success = success;
			Message = message;
		}
	}
}