#if !DISABLE_GAMEJOLT
#nullable enable

using System;
using System.Diagnostics.CodeAnalysis;
using System.Runtime.CompilerServices;

namespace Hertzole.GameJolt
{
	internal static partial class Guard
	{
		/// <exception cref="ArgumentNullException">Thrown if <paramref name="value" /> is <see langword="null" />.</exception>
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static void IsNotNull<T>([NotNull] T? value, string paramName) where T : class
		{
			if (value is not null)
			{
				return;
			}

			ThrowHelper.ThrowArgumentNullExceptionForIsNotNull<T>(paramName);
		}

		/// <exception cref="ArgumentNullException">Thrown if <paramref name="value" /> is <see langword="null" />.</exception>
		/// <exception cref="ArgumentException">Thrown if <paramref name="value" /> is whitespace.</exception>
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static void IsNotNullOrWhiteSpace([NotNull] string? value, string paramName)
		{
			if (!string.IsNullOrWhiteSpace(value))
			{
				return;
			}

			ThrowHelper.ThrowArgumentExceptionForIsNotNullOrWhiteSpace(value, paramName);
		}
	}
}
#endif