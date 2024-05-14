#nullable enable

using System;
#if NET6_0_OR_GREATER
using JsonProperty = System.Text.Json.Serialization.JsonPropertyNameAttribute;
using System.Text.Json.Serialization;
using Hertzole.GameJolt.Serialization.System;
#else
using Newtonsoft.Json;
using Hertzole.GameJolt.Serialization.Newtonsoft;
#endif

namespace Hertzole.GameJolt
{
	internal readonly struct TableInternal : IEquatable<TableInternal>
	{
		[JsonProperty("id")]
		[JsonConverter(typeof(GameJoltIntConverter))]
		public readonly int id;
		[JsonProperty("name")]
		public readonly string name;
		[JsonProperty("description")]
		public readonly string description;
		[JsonProperty("primary")]
		[JsonConverter(typeof(GameJoltBooleanConverter))]
		public readonly bool isPrimary;

		[JsonConstructor]
		public TableInternal(int id, string? name, string? description, bool isPrimary)
		{
			this.id = id;
			this.name = name ?? string.Empty;
			this.description = description ?? string.Empty;
			this.isPrimary = isPrimary;
		}

		public bool Equals(TableInternal other)
		{
			return id == other.id && isPrimary == other.isPrimary &&
			       EqualityHelper.StringEquals(name, other.name) &&
			       EqualityHelper.StringEquals(description, other.description);
		}

		public override bool Equals(object? obj)
		{
			return obj is TableInternal other && Equals(other);
		}

		public override int GetHashCode()
		{
			unchecked
			{
				int hashCode = id;
				hashCode = (hashCode * 397) ^ isPrimary.GetHashCode();
				hashCode = (hashCode * 397) ^ (!string.IsNullOrEmpty(name) ? name.GetHashCode() : 0);
				hashCode = (hashCode * 397) ^ (!string.IsNullOrEmpty(description) ? description.GetHashCode() : 0);
				return hashCode;
			}
		}

		public static bool operator ==(TableInternal left, TableInternal right)
		{
			return left.Equals(right);
		}

		public static bool operator !=(TableInternal left, TableInternal right)
		{
			return !left.Equals(right);
		}

		public GameJoltTable ToPublicTable()
		{
			return new GameJoltTable(id, name, description, isPrimary);
		}

		public override string ToString()
		{
			return
				$"{nameof(TableInternal)} ({nameof(id)}: {id}, {nameof(name)}: {name}, {nameof(description)}: {description}, {nameof(isPrimary)}: {isPrimary})";
		}
	}
}