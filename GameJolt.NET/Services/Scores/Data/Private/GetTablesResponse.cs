#nullable enable

using System;
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
	internal readonly struct GetTablesResponse : IResponse, IEquatable<GetTablesResponse>
	{
		[JsonProperty("success")]
		[JsonConverter(typeof(GameJoltBooleanConverter))]
		public readonly bool success;
		[JsonProperty("message")]
		public readonly string? message;
		[JsonProperty("tables")]
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
		public GetTablesResponse(bool success, string? message, TableInternal[]? tables)
		{
			this.success = success;
			this.message = message;
			this.tables = tables ?? Array.Empty<TableInternal>();
		}

		public bool Equals(GetTablesResponse other)
		{
			return EqualityHelper.ResponseEquals(this, other) && EqualityHelper.ArrayEquals(tables, other.tables);
		}

		public override bool Equals(object? obj)
		{
			return obj is GetTablesResponse other && Equals(other);
		}

		public override int GetHashCode()
		{
			unchecked
			{
				int hashCode = EqualityHelper.ResponseHashCode(0, this);
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
				$"{nameof(GetTablesResponse)} ({nameof(Success)}: {Success}, {nameof(Message)}: {Message}, {nameof(tables)}: {tables.ToCommaSeparatedString()})";
		}
	}
}