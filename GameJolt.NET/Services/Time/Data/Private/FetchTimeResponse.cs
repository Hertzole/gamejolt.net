#nullable enable

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
	internal readonly struct FetchTimeResponse : IResponse
	{
		[JsonName("success")]
		[JsonConverter(typeof(GameJoltBooleanConverter))]
		public bool Success { get; }
		[JsonName("message")]
		public string? Message { get; }
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

		[JsonConstructor]
		public FetchTimeResponse(long timestamp,
			string timezone,
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
			this.timezone = timezone;
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
			timezone = "";
			year = time.Year;
			month = time.Month;
			day = time.Day;
			hour = time.Hour;
			minute = time.Minute;
			second = time.Second;
			Success = success;
			Message = message;
		}
	}
}