#nullable enable

using System;
using System.Text;
using Bogus;
using Hertzole.GameJolt;

namespace GameJolt.NET.Tests.Serialization.Converters
{
	public abstract class BaseSerializationTest
	{
		protected readonly Faker faker = new Faker();

		protected string Serialize<T>(T data)
		{
			return GameJoltAPI.serializer.Serialize(data);
		}

		protected T Deserialize<T>(string json)
		{
			return GameJoltAPI.serializer.Deserialize<T>(json);
		}

		protected string WriteExpectedResponse(bool success, string? message, Action<StringBuilder> writeExpected)
		{
			using (StringBuilderPool.Rent(out StringBuilder? sb))
			{
				sb.Append("{\"success\":");
				sb.Append(success.ToString().ToLower());
				sb.Append(",\"message\":");
				sb.Append(message == null ? "null" : $"\"{message}\"");

				writeExpected.Invoke(sb);

				sb.Append("}");

				return sb.ToString();
			}
		}
	}
}