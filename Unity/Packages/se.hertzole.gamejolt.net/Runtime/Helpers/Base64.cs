#if !DISABLE_GAMEJOLT
#nullable enable

using System;
using System.Buffers;

namespace Hertzole.GameJolt
{
	internal static class Base64
	{
		/// <summary>
		///     Tries to convert the provided <paramref name="data" /> to a byte array.
		/// </summary>
		/// <param name="data"></param>
		/// <param name="result"></param>
		/// <returns>
		///     <see langword="true" /> if the <paramref name="data" /> is a Base64 string and was successfully decoded into a
		///     <see langword="byte" /> array; otherwise <see langword="false" />.
		/// </returns>
		internal static bool TryConvertBase64ToBytes(string data, out MemoryOwner<byte> result)
		{
#if NETSTANDARD2_1_OR_GREATER || NET5_0_OR_GREATER
			// Estimate the buffer size from the Base64 result string.
			int estimatedBufferSize = data.Length * 3 / 4;
			if (estimatedBufferSize <= 0)
			{
				result = MemoryOwner<byte>.Empty;
				return false;
			}

			byte[] buffer = ArrayPool<byte>.Shared.Rent(estimatedBufferSize);

			if (Convert.TryFromBase64String(data, buffer, out int bytesWritten))
			{
				result = new MemoryOwner<byte>(buffer, bytesWritten);
				return true;
			}

			result = MemoryOwner<byte>.Empty;
			return false;
#else
			try
			{
				byte[] bytes = Convert.FromBase64String(data);
				// Use a buffer here just to be consistent with newer implementations.
				byte[] buffer = ArrayPool<byte>.Shared.Rent(bytes.Length);
				bytes.CopyTo(buffer, 0);
				result = new MemoryOwner<byte>(buffer, bytes.Length);
				return true;
			}
			catch (FormatException) // If it failed, it was not a valid Base64 string.
			{
				result = MemoryOwner<byte>.Empty;
				return false;
			}
#endif
		}
	}
}
#endif // !DISABLE_GAMEJOLT