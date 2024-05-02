#nullable enable

using System;
#if NET6_0_OR_GREATER
using JsonName = System.Text.Json.Serialization.JsonPropertyNameAttribute;
using JsonConverter = System.Text.Json.Serialization.JsonConverterAttribute;
using JsonConstructor = System.Text.Json.Serialization.JsonConstructorAttribute;
using Hertzole.GameJolt.Serialization.System;
#else
using JsonName = Newtonsoft.Json.JsonPropertyAttribute;
using JsonConverter = Newtonsoft.Json.JsonConverterAttribute;
using JsonConstructor = Newtonsoft.Json.JsonConstructorAttribute;
using Hertzole.GameJolt.Serialization.Newtonsoft;
#endif

namespace Hertzole.GameJolt
{
	internal readonly struct FetchTimeResponse : IResponse, IEquatable<FetchTimeResponse>
	{
		[JsonName("timestamp")]
		[JsonConverter(typeof(GameJoltLongConverter))]
		public readonly long timestamp;
		[JsonName("timezone")]
		public readonly string timezone;
		[JsonName("year")]
		[JsonConverter(typeof(GameJoltIntConverter))]
		public readonly int year;
		[JsonName("month")]
		[JsonConverter(typeof(GameJoltIntConverter))]
		public readonly int month;
		[JsonName("day")]
		[JsonConverter(typeof(GameJoltIntConverter))]
		public readonly int day;
		[JsonName("hour")]
		[JsonConverter(typeof(GameJoltIntConverter))]
		public readonly int hour;
		[JsonName("minute")]
		[JsonConverter(typeof(GameJoltIntConverter))]
		public readonly int minute;
		[JsonName("second")]
		[JsonConverter(typeof(GameJoltIntConverter))]
		public readonly int second;
		[JsonName("success")]
		[JsonConverter(typeof(GameJoltBooleanConverter))]
		public bool Success { get; }
		[JsonName("message")]
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