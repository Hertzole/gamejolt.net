#if !DISABLE_GAMEJOLT // Disables all GameJolt-related code

#nullable enable

using System;
using System.Reflection;
using NUnit.Framework;

namespace GameJolt.NET.Tests.Extensions
{
	internal static class TestAdapterExtensions
	{
		public static bool HasAttribute<T>(this TestContext.TestAdapter test) where T : Attribute
		{
			string attributeName = GetAttributeName<T>();

			if (test.Properties.ContainsKey(attributeName))
			{
				return true;
			}

			if (!string.IsNullOrEmpty(test.ClassName))
			{
				Type? type = FindType(test.ClassName);

				if (type != null)
				{
					// First check the method. If it has the attribute, then we don't need to check the class. If it doesn't, then we check the class.
					if (TryGetAttribute<T>(FindMethod(type, test.MethodName)))
					{
						return true;
					}

					if (TryGetAttribute<T>(type))
					{
						return true;
					}
				}
			}

			return false;
		}

		private static MethodInfo? FindMethod(Type type, string? name)
		{
			if (string.IsNullOrEmpty(name))
			{
				return null;
			}

			foreach (MethodInfo method in type.GetMethods(BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.Static))
			{
				if (method.Name == name)
				{
					return method;
				}
			}

			return null;
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

		private static bool TryGetAttribute<T>(MemberInfo? type) where T : Attribute
		{
			if (type == null)
			{
				return false;
			}

			T? attribute = type.GetCustomAttribute<T>();
			return attribute != null;
		}

		private static string GetAttributeName<T>() where T : Attribute
		{
			string name = typeof(T).Name;
			if (name.EndsWith("Attribute"))
			{
				name = name.Substring(0, name.Length - 9);
			}

			return name;
		}
	}
}
#endif // DISABLE_GAMEJOLT