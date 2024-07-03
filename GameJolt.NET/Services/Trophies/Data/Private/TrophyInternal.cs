#nullable enable

using System;
using Hertzole.GameJolt.Serialization.Shared;
#if NET6_0_OR_GREATER || FORCE_SYSTEM_JSON
using JsonProperty = System.Text.Json.Serialization.JsonPropertyNameAttribute;
using System.Text.Json.Serialization;
using Hertzole.GameJolt.Serialization.System;
#else
using Newtonsoft.Json;
using Hertzole.GameJolt.Serialization.Newtonsoft;
#endif

namespace Hertzole.GameJolt
{
	internal readonly struct TrophyInternal : IEquatable<TrophyInternal>
	{
		[JsonProperty("id")]
		[JsonConverter(typeof(GameJoltIntConverter))]
		public readonly int id;
		[JsonProperty("title")]
		public readonly string title;
		[JsonProperty("description")]
		public readonly string description;
		[JsonProperty("difficulty")]
		[JsonConverter(typeof(GameJoltTrophyDifficultyConverter))]
		public readonly TrophyDifficulty difficulty;
		[JsonProperty("image_url")]
		public readonly string imageUrl;
		[JsonProperty("achieved")]
		[JsonConverter(typeof(BooleanOrDateConverter))]
		public readonly bool achieved;

		[JsonConstructor]
		public TrophyInternal(int id, string? title, string? description, TrophyDifficulty difficulty, string? imageUrl, bool achieved)
		{
			this.id = id;
			this.title = title ?? string.Empty;
			this.description = description ?? string.Empty;
			this.difficulty = difficulty;
			this.imageUrl = imageUrl ?? string.Empty;
			this.achieved = achieved;
		}

		public bool Equals(TrophyInternal other)
		{
			return id == other.id && difficulty == other.difficulty && achieved == other.achieved &&
			       EqualityHelper.StringEquals(title, other.title) &&
			       EqualityHelper.StringEquals(description, other.description) &&
			       EqualityHelper.StringEquals(imageUrl, other.imageUrl);
		}

		public override bool Equals(object? obj)
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
				hashCode = (hashCode * 397) ^ (!string.IsNullOrEmpty(title) ? title.GetHashCode() : 0);
				hashCode = (hashCode * 397) ^ (!string.IsNullOrEmpty(description) ? description.GetHashCode() : 0);
				hashCode = (hashCode * 397) ^ (!string.IsNullOrEmpty(imageUrl) ? imageUrl.GetHashCode() : 0);
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

		public override string ToString()
		{
			return
				$"{nameof(TrophyInternal)} ({nameof(id)}: {id}, {nameof(title)}: {title}, {nameof(description)}: {description}, {nameof(difficulty)}: {difficulty}, {nameof(imageUrl)}: {imageUrl}, {nameof(achieved)}: {achieved})";
		}
	}
}