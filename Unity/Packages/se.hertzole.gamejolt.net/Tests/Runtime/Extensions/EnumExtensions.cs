#if !DISABLE_GAMEJOLT // Disables all GameJolt-related code

#nullable enable

using System;
using System.Text;
using Bogus;
using GameJolt.NET.Tests.Enums;

namespace GameJolt.NET.Tests.Extensions
{
	public static class EnumExtensions
	{
		private static readonly Faker faker = new Faker();
		
		public static T[]? CreateArray<T>(this ArrayInitialization arrayInitialization, Func<Faker, T> createElement, int min = 5, int max = 10)
		{
			T[]? array;

			switch (arrayInitialization)
			{
				default:
				case ArrayInitialization.CreateArray:
					array = new T[faker.Random.Int(min, max)];
					break;
				case ArrayInitialization.EmptyArray:
					array = Array.Empty<T>();
					break;
				case ArrayInitialization.NullArray:
					array = null;
					break;
			}

			if (array != null)
			{
				for (int i = 0; i < array.Length; i++)
				{
					array[i] = createElement(faker);
				}
			}

			return array;
		}

		public static void AppendToBuilder(this StringInitialization stringInitialization, StringBuilder builder, string? value)
		{
			switch (stringInitialization)
			{
				case StringInitialization.Empty:
					builder.Append("\"\"");
					break;
				case StringInitialization.Null:
					builder.Append("null");
					break;
				default:
				case StringInitialization.Normal:
					if (string.IsNullOrEmpty(value))
					{
						throw new ArgumentNullException(nameof(value));
					}
					
					builder.AppendStringValue(value!);
					break;
			}
		}
		
		public static string? GetData(this StringInitialization stringInitialization)
		{
			switch (stringInitialization)
			{
				case StringInitialization.Empty:
					return string.Empty;
				case StringInitialization.Null:
					return null;
				default:
				case StringInitialization.Normal:
					return faker.Lorem.Sentence();
			}
		}
	}
}
#endif // DISABLE_GAMEJOLT