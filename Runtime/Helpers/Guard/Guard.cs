#if !DISABLE_GAMEJOLT
#nullable enable

using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using JetBrains.Annotations;
using NotNullAttribute = System.Diagnostics.CodeAnalysis.NotNullAttribute;

namespace Hertzole.GameJolt
{
	internal static partial class Guard
	{
		/// <exception cref="ArgumentNullException">Thrown if <paramref name="value" /> is <see langword="null" />.</exception>
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static void IsNotNull<T>([NotNull][NoEnumeration] T? value, string paramName) where T : class
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

		/// <exception cref="ArgumentNullException">Thrown if <paramref name="value" /> is <see langword="null" />.</exception>
		/// <exception cref="ArgumentException">Thrown if <paramref name="value" /> is empty.</exception>
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static void IsNotNullOrEmpty<T>([NotNull] IReadOnlyCollection<T>? value, string paramName)
		{
			if (value is not null && value.Count > 0)
			{
				return;
			}

			ThrowHelper.ThrowArgumentExceptionForIsNotNullOrEmptyNullable(value, paramName);
		}

		/// <exception cref="ArgumentException">Thrown if the size of <paramref name="value" /> &lt; <paramref name="size" />.</exception>
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static void HasSizeGreaterThanOrEqualTo<T>(IReadOnlyCollection<T> value, int size, string paramName)
		{
			if (value.Count >= size)
			{
				return;
			}

			ThrowHelper.ThrowArgumentExceptionForHasSizeGreaterThanOrEqualTo(value, size, paramName);
		}
	}
}
#endif