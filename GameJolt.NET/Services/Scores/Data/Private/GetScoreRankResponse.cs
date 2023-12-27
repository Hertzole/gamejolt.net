#nullable enable

#if NET6_0_OR_GREATER
using JsonName = System.Text.Json.Serialization.JsonPropertyNameAttribute;
using JsonConverter = System.Text.Json.Serialization.JsonConverterAttribute;
using JsonConstructor = System.Text.Json.Serialization.JsonConstructorAttribute;
using JsonIgnore = System.Text.Json.Serialization.JsonIgnoreAttribute;
#endif

namespace Hertzole.GameJolt
{
	internal readonly struct GetScoreRankResponse : IResponse
	{
		[JsonName("success")]
		[JsonConverter(typeof(GameJoltBooleanConverter))]
		public readonly bool success;
		[JsonName("message")]
		public readonly string? message;
		[JsonName("rank")]
		[JsonConverter(typeof(GameJoltIntConverter))]
		public readonly int rank;

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
		public GetScoreRankResponse(bool success, string? message, int rank)
		{
			this.success = success;
			this.message = message;
			this.rank = rank;
		}
	}
}