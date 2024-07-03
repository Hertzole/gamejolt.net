#if !DISABLE_GAMEJOLT // Disables all GameJolt-related code

#nullable enable

using System;
using System.Text;

namespace GameJolt.NET.Tests.Extensions
{
	public static class StringBuilderExtensions
	{
		public static void AppendJsonPropertyName(this StringBuilder sb, string name, bool randomCapitalize = false)
		{
			sb.Append('"');
			sb.Append(randomCapitalize ? name.RandomCapitalize() : name);
			sb.Append("\":");
		}

		public static void AppendStringValue(this StringBuilder sb, string? value, bool writeNull = false)
		{
			if(value == null && writeNull)
			{
				sb.Append("null");
				return;
			}
			
			sb.Append('"');
			if (value != null)
			{
				sb.Append(value.ReplaceWithUnicode());
			}
			sb.Append('"');
		}

		public static void AppendArray<T>(this StringBuilder sb, T[]? array, Action<StringBuilder, T> appendAction, bool dontWriteNull = false)
		{
			if (array == null)
			{
				sb.Append(dontWriteNull ? "[]" : "null");

				return;
			}

			sb.Append('[');
			for (int i = 0; i < array.Length; i++)
			{
				if (i > 0)
				{
					sb.Append(',');
				}

				appendAction(sb, array[i]);
			}

			sb.Append(']');
		}
	}
}
#endif // DISABLE_GAMEJOLT