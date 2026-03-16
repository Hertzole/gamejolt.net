#if !DISABLE_GAMEJOLT
#nullable enable

using System;
using System.Diagnostics.CodeAnalysis;
using System.Runtime.CompilerServices;

namespace Hertzole.GameJolt
{
	internal static partial class Guard
	{
		private static class ThrowHelper
		{
			[DoesNotReturn]
			public static void ThrowArgumentNullExceptionForIsNotNull<T>(string name)
			{
				throw new ArgumentNullException(name, $"Parameter {name} ({typeof(T).FullName}) must not be null.");
			}

			[DoesNotReturn]
			public static void ThrowArgumentExceptionForIsNotNullOrWhiteSpace(string? text, string name)
			{
				throw GetException(text, name);

				[MethodImpl(MethodImplOptions.NoInlining)]
				static Exception GetException(string? text, string name)
				{
					if (text is null)
					{
						return new ArgumentNullException(name, $"Parameter {name} (string) must not be null.");
					}

					return new ArgumentException($"Parameter {name} (string) must not be empty or whitespace.", name);
				}
			}
		}
	}
}
#endif