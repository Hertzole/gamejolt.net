namespace GameJolt.NET.Tests.Extensions
{
	public static class StringExtensions
	{
		public static string ReplaceWithUnicode(this string str)
		{
			string result = str;
			result = result.Replace("'", "\\u0027");

			return result;
		}
	}
}