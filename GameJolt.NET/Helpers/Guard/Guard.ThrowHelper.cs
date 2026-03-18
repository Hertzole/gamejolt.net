#if !DISABLE_GAMEJOLT
#nullable enable

using System;
using System.Collections.Generic;
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
				throw new ArgumentNullException(name, $"Parameter {name} ({typeof(T).ToTypeString()}) must not be null.");
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

			[DoesNotReturn]
			public static void ThrowArgumentExceptionForIsNotNullOrEmptyNullable<T>(T? list, string name)
			{
				throw GetException(list, name);

				[MethodImpl(MethodImplOptions.NoInlining)]
				static Exception GetException(T? list, string name)
				{
					if (list is null)
					{
						return new ArgumentNullException(name, $"Parameter {name} ({typeof(T).ToTypeString()}) must not be null.");
					}

					return new ArgumentException($"Parameter {name} ({typeof(T).ToTypeString()}) must not be empty.", name);
				}
			}

			[DoesNotReturn]
			public static void ThrowArgumentExceptionForHasSizeGreaterThanOrEqualTo<T>(IReadOnlyCollection<T> list, int size, string name)
			{
				throw new ArgumentException(
					$"Parameter {name} ({typeof(IReadOnlyCollection<T>).ToTypeString()}) must have a size of at least {size}, had a size of {list.Count}.",
					name);
			}
		}
	}
}
#endif