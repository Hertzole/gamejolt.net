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
	internal readonly struct FetchTimeResponse : IResponse, IEquatable<FetchTimeResponse>
	{
		[JsonProperty("timestamp")]
		[JsonConverter(typeof(GameJoltLongConverter))]
		public readonly long timestamp;
		[JsonProperty("timezone")]
		public readonly string timezone;
		[JsonProperty("year")]
		[JsonConverter(typeof(GameJoltIntConverter))]
		public readonly int year;
		[JsonProperty("month")]
		[JsonConverter(typeof(GameJoltIntConverter))]
		public readonly int month;
		[JsonProperty("day")]
		[JsonConverter(typeof(GameJoltIntConverter))]
		public readonly int day;
		[JsonProperty("hour")]
		[JsonConverter(typeof(GameJoltIntConverter))]
		public readonly int hour;
		[JsonProperty("minute")]
		[JsonConverter(typeof(GameJoltIntConverter))]
		public readonly int minute;
		[JsonProperty("second")]
		[JsonConverter(typeof(GameJoltIntConverter))]
		public readonly int second;
		[JsonProperty("success")]
		[JsonConverter(typeof(GameJoltBooleanConverter))]
		public bool Success { get; }
		[JsonProperty("message")]
		public string? Message { get; }

		[JsonConstructor]
		public FetchTimeResponse(long timestamp,
			string? timezone,
			int year,
			int month,
			int day,
			int hour,
			int minute,
			int second,
			bool success,
			string? message)
		{
			this.timestamp = timestamp;
			this.timezone = timezone ?? string.Empty;
			this.year = year;
			this.month = month;
			this.day = day;
			this.hour = hour;
			this.minute = minute;
			this.second = second;
			Success = success;
			Message = message;
		}

		public FetchTimeResponse(bool success, string? message, DateTime time)
		{
			timestamp = DateTimeHelper.ToUnixTimestamp(time);
			timezone = string.Empty;
			year = time.Year;
			month = time.Month;
			day = time.Day;
			hour = time.Hour;
			minute = time.Minute;
			second = time.Second;
			Success = success;
			Message = message;
		}

		public bool Equals(FetchTimeResponse other)
		{
			return timestamp == other.timestamp && year == other.year && month == other.month && day == other.day &&
			       hour == other.hour && minute == other.minute && second == other.second &&
			       EqualityHelper.StringEquals(timezone, other.timezone) &&
			       EqualityHelper.ResponseEquals(this, other);
		}

		public override bool Equals(object? obj)
		{
			return obj is FetchTimeResponse other && Equals(other);
		}

		public override int GetHashCode()
		{
			unchecked
			{
				int hashCode = timestamp.GetHashCode();
				hashCode = (hashCode * 397) ^ year;
				hashCode = (hashCode * 397) ^ month;
				hashCode = (hashCode * 397) ^ day;
				hashCode = (hashCode * 397) ^ hour;
				hashCode = (hashCode * 397) ^ minute;
				hashCode = (hashCode * 397) ^ second;
				hashCode = (hashCode * 397) ^ timezone.GetHashCode();
				hashCode = EqualityHelper.ResponseHashCode(hashCode, this);
				return hashCode;
			}
		}

		public static bool operator ==(FetchTimeResponse left, FetchTimeResponse right)
		{
			return left.Equals(right);
		}

		public static bool operator !=(FetchTimeResponse left, FetchTimeResponse right)
		{
			return !left.Equals(right);
		}

		public override string ToString()
		{
			return
				$"{nameof(FetchTimeResponse)} ({nameof(Success)}: {Success}, {nameof(Message)}: {Message}, {nameof(timestamp)}: {timestamp}, {nameof(timezone)}: {timezone}, {nameof(year)}: {year}, {nameof(month)}: {month}, {nameof(day)}: {day}, {nameof(hour)}: {hour}, {nameof(minute)}: {minute}, {nameof(second)}: {second})";
		}
	}
}