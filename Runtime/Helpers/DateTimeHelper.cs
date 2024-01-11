using System;

namespace Hertzole.GameJolt
{
	internal static class DateTimeHelper
	{
		public static DateTime FromUnixTimestamp(long timestamp)
		{
			return DateTimeOffset.FromUnixTimeSeconds(timestamp).DateTime;
		}

		public static long ToUnixTimestamp(DateTime time)
		{
			return new DateTimeOffset(time).ToUnixTimeSeconds();
		}
	}
}