#nullable enable

using System;
using Bogus;
using GameJolt.NET.Tests.Enums;

namespace GameJolt.NET.Tests.Extensions
{
	public static class ArrayInitializationExtensions
	{
		public static T[]? CreateArray<T>(this ArrayInitialization arrayInitialization, Func<Faker, T> createElement, int min = 5, int max = 10)
		{
			T[]? array;
			Faker faker = new Faker();

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
	}
}