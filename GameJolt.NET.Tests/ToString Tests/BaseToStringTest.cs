#if !DISABLE_GAMEJOLT // Disables all GameJolt-related code

#nullable enable

using System;
using Bogus;
using GameJolt.NET.Tests.Enums;

namespace GameJolt.NET.Tests.ToString
{
	public abstract class BaseToStringTest
	{
		protected readonly Faker faker = new Faker();
	}
}
#endif // DISABLE_GAMEJOLT