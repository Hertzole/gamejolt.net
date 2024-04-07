#nullable enable

using System;
#if NET6_0_OR_GREATER
using JsonName = System.Text.Json.Serialization.JsonPropertyNameAttribute;
using JsonConverter = System.Text.Json.Serialization.JsonConverterAttribute;
using JsonConstructor = System.Text.Json.Serialization.JsonConstructorAttribute;
using JsonIgnore = System.Text.Json.Serialization.JsonIgnoreAttribute;
#else
using JsonName = Newtonsoft.Json.JsonPropertyAttribute;
using JsonConverter = Newtonsoft.Json.JsonConverterAttribute;
using JsonConstructor = Newtonsoft.Json.JsonConstructorAttribute;
using JsonIgnore = Newtonsoft.Json.JsonIgnoreAttribute;
#endif

namespace Hertzole.GameJolt
{
	internal readonly struct GetTablesResponse : IResponse, IEquatable<GetTablesResponse>
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

		public bool Equals(GetTablesResponse other)
		{
			return success == other.success && EqualityHelper.StringEquals(message, other.message) && EqualityHelper.ArrayEquals(tables, other.tables);
		}

		public override bool Equals(object? obj)
		{
			return obj is GetTablesResponse other && Equals(other);
		}

		public override int GetHashCode()
		{
			unchecked
			{
				int hashCode = success.GetHashCode();
				hashCode = (hashCode * 397) ^ (message != null ? message.GetHashCode() : 0);
				hashCode = (hashCode * 397) ^ tables.GetHashCode();
				return hashCode;
			}
		}

		public static bool operator ==(GetTablesResponse left, GetTablesResponse right)
		{
			return left.Equals(right);
		}

		public static bool operator !=(GetTablesResponse left, GetTablesResponse right)
		{
			return !left.Equals(right);
		}

		public override string ToString()
		{
			return
				$"{nameof(success)}: {success}, {nameof(message)}: {message}, {nameof(tables)}: {(tables == null ? "" : tables.ToCommaSeparatedString())}";
		}
	}
}