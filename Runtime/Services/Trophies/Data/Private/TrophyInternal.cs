#if NET6_0_OR_GREATER
using JsonName = System.Text.Json.Serialization.JsonPropertyNameAttribute;
using JsonConverter = System.Text.Json.Serialization.JsonConverterAttribute;
using JsonConstructor = System.Text.Json.Serialization.JsonConstructorAttribute;

#else
using JsonName = Newtonsoft.Json.JsonPropertyAttribute;
using JsonConverter = Newtonsoft.Json.JsonConverterAttribute;
using JsonConstructor = Newtonsoft.Json.JsonConstructorAttribute;
#endif

namespace Hertzole.GameJolt
{
	internal readonly struct TrophyInternal
	{
		[JsonName("id")]
		[JsonConverter(typeof(GameJoltIntConverter))]
		public readonly int id;
		[JsonName("title")]
		public readonly string title;
		[JsonName("description")]
		public readonly string description;
		[JsonName("difficulty")]
		[JsonConverter(typeof(GameJoltTrophyDifficultyConverter))]
		public readonly TrophyDifficulty difficulty;
		[JsonName("image_url")]
		public readonly string imageUrl;
		[JsonName("achieved")]
		[JsonConverter(typeof(BooleanOrDateConverter))]
		public readonly bool achieved;

		[JsonConstructor]
		public TrophyInternal(int id, string title, string description, TrophyDifficulty difficulty, string imageUrl, bool achieved)
		{
			this.id = id;
			this.title = title;
			this.description = description;
			this.difficulty = difficulty;
			this.imageUrl = imageUrl;
			this.achieved = achieved;
		}

		public GameJoltTrophy ToPublicTrophy()
		{
			return new GameJoltTrophy(id, title, description, difficulty, imageUrl, achieved);
		}
	}
}