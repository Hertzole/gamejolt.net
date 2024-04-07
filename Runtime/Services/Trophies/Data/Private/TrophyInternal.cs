using System;
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
	internal readonly struct TrophyInternal : IEquatable<TrophyInternal>
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

		public bool Equals(TrophyInternal other)
		{
			return id == other.id && difficulty == other.difficulty && achieved == other.achieved &&
			       EqualityHelper.StringEquals(title, other.title) &&
			       EqualityHelper.StringEquals(description, other.description) &&
			       EqualityHelper.StringEquals(imageUrl, other.imageUrl);
		}

		public override bool Equals(object obj)
		{
			return obj is TrophyInternal other && Equals(other);
		}

		public override int GetHashCode()
		{
			unchecked
			{
				int hashCode = id;
				hashCode = (hashCode * 397) ^ (int) difficulty;
				hashCode = (hashCode * 397) ^ achieved.GetHashCode();
				hashCode = (hashCode * 397) ^ (title != null ? title.GetHashCode() : 0);
				hashCode = (hashCode * 397) ^ (description != null ? description.GetHashCode() : 0);
				hashCode = (hashCode * 397) ^ (imageUrl != null ? imageUrl.GetHashCode() : 0);
				return hashCode;
			}
		}

		public static bool operator ==(TrophyInternal left, TrophyInternal right)
		{
			return left.Equals(right);
		}

		public static bool operator !=(TrophyInternal left, TrophyInternal right)
		{
			return !left.Equals(right);
		}

		public GameJoltTrophy ToPublicTrophy()
		{
			return new GameJoltTrophy(id, title, description, difficulty, imageUrl, achieved);
		}
	}
}