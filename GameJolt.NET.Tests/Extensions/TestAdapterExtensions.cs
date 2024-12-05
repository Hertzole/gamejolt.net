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
				Type? type = FindType(test.ClassName);

				return type?.GetCustomAttribute<SkipInitializationAttribute>() != null;
			}

			return false;
		}

		private static Type? FindType(string name)
		{
			// Try the executing assembly for quickest results.
			Type? type = Assembly.GetExecutingAssembly().GetType(name);
			if (type != null)
			{
				return type;
			}

			// Try the entry assembly if it's not the same as the executing assembly.
			Assembly? entryAssembly = Assembly.GetEntryAssembly();
			if (entryAssembly != null && entryAssembly != Assembly.GetExecutingAssembly())
			{
				type = entryAssembly.GetType(name);
				if (type != null)
				{
					return type;
				}
			}

			// Try all loaded assemblies.
			foreach (Assembly assembly in AppDomain.CurrentDomain.GetAssemblies())
			{
				type = assembly.GetType(name);
				if (type != null)
				{
					return type;
				}
			}

			// No type was found.
			return null;
		}
	}
}
#endif // DISABLE_GAMEJOLT