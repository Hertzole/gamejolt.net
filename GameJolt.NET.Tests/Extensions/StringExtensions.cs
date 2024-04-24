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

		public static string RandomCapitalize(this string str)
		{
			if (string.IsNullOrEmpty(str))
			{
				return str;
			}

			char[] chars = str.ToCharArray();
			for (int i = 0; i < chars.Length; i++)
			{
				if (i % 2 == 0)
				{
					chars[i] = char.ToUpper(chars[i]);
				}
			}

			return new string(chars);
		}
	}
}