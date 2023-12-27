#if NET6_0_OR_GREATER
using JsonName = System.Text.Json.Serialization.JsonPropertyNameAttribute;
using JsonConverter = System.Text.Json.Serialization.JsonConverterAttribute;
using JsonConstructor = System.Text.Json.Serialization.JsonConstructorAttribute;
#endif

namespace Hertzole.GameJolt
{
	internal readonly struct TableInternal
	{
		[JsonName("id")]
		[JsonConverter(typeof(GameJoltIntConverter))]
		public readonly int id;
		[JsonName("name")]
		public readonly string name;
		[JsonName("description")]
		public readonly string description;
		[JsonName("primary")]
		[JsonConverter(typeof(GameJoltBooleanConverter))]
		public readonly bool isPrimary;

		[JsonConstructor]
		public TableInternal(int id, string name, string description, bool isPrimary)
		{
			this.id = id;
			this.name = name;
			this.description = description;
			this.isPrimary = isPrimary;
		}

		public GameJoltTable ToPublicTable()
		{
			return new GameJoltTable(id, name, description, isPrimary);
		}
	}
}