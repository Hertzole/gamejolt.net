#if NET6_0_OR_GREATER
using JsonName = System.Text.Json.Serialization.JsonPropertyNameAttribute;
using JsonConstructor = System.Text.Json.Serialization.JsonConstructorAttribute;
#endif

namespace Hertzole.GameJolt
{
	internal readonly struct DataKey
	{
		[JsonName("key")]
		public readonly string key;

		[JsonConstructor]
		public DataKey(string key)
		{
			this.key = key;
		}
	}
}