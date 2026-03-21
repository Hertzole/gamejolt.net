#if !NET9_0_OR_GREATER
namespace System
{
	internal static class MemoryShims
	{
		// Provides a shim for ReadOnlySpan<T>.EndsWith(T) which is only available in .NET 9.0 and later.
		public static bool EndsWith<T>(this ReadOnlySpan<T> span, T value) where T : IEquatable<T>
		{
			return !span.IsEmpty && span[span.Length - 1].Equals(value);
		}
	}
}
#endif