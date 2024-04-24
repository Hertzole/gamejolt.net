#nullable enable

using System;
using System.Text;

namespace GameJolt.NET.Tests.Extensions
{
	public static class StringBuilderExtensions
	{
		public static void AppendJsonPropertyName(this StringBuilder sb, string name)
		{
			sb.Append("\"");
			sb.Append(name);
			sb.Append("\":");
		}

		public static void AppendStringValue(this StringBuilder sb, string value)
		{
			sb.Append("\"");
			sb.Append(value);
			sb.Append("\"");
		}

		public static void AppendArray<T>(this StringBuilder sb, T[]? array, Action<StringBuilder, T> appendAction, bool dontWriteNull = false)
		{
			if (array == null)
			{
				sb.Append(dontWriteNull ? "[]" : "null");

				return;
			}

			sb.Append("[");
			for (int i = 0; i < array.Length; i++)
			{
				if (i > 0)
				{
					sb.Append(",");
				}

				appendAction(sb, array[i]);
			}

			sb.Append("]");
		}
	}
}