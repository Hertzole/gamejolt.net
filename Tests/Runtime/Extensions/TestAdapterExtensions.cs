#if !DISABLE_GAMEJOLT // Disables all GameJolt-related code

#nullable enable

using System;
using System.Reflection;
using GameJolt.NET.Tests.Attributes;
using NUnit.Framework;

namespace GameJolt.NET.Tests.Extensions
{
	internal static class TestAdapterExtensions
	{
		private static readonly string skipInitialization = nameof(SkipInitializationAttribute).Substring(0, nameof(SkipInitializationAttribute).Length - 9);

		public static bool HasSkipInitializationAttribute(this TestContext.TestAdapter test)
		{
			if (test.Properties.ContainsKey(skipInitialization))
			{
				return true;
			}

			if (!string.IsNullOrEmpty(test.ClassName))
			{
				Type? type = Assembly.GetExecutingAssembly().GetType(test.ClassName);
				if (type == null)
				{
					return false;
				}
				
				return type.GetCustomAttribute<SkipInitializationAttribute>() != null;
			}

			return false;
		}
	}
}
#endif // DISABLE_GAMEJOLT