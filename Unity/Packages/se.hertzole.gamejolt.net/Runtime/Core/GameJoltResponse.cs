#if !DISABLE_GAMEJOLT // Disables all GameJolt-related code

using System;
using System.Collections.Generic;
#if NET6_0_OR_GREATER || FORCE_SYSTEM_JSON
using System.Text.Json.Serialization;
#endif

namespace Hertzole.GameJolt
{
	internal readonly struct GameJoltResponse<T> : IEquatable<GameJoltResponse<T>>
	{
		public readonly T response;

#if NET6_0_OR_GREATER || FORCE_SYSTEM_JSON
		[JsonConstructor]
#endif
		public GameJoltResponse(T response)
		{
			this.response = response;
		}

		public bool Equals(GameJoltResponse<T> other)
		{
			return EqualityComparer<T>.Default.Equals(response, other.response);
		}

		public override bool Equals(object obj)
		{
			return obj is GameJoltResponse<T> other && Equals(other);
		}

		public override int GetHashCode()
		{
			return EqualityComparer<T>.Default.GetHashCode(response);
		}

		public static bool operator ==(GameJoltResponse<T> left, GameJoltResponse<T> right)
		{
			return left.Equals(right);
		}

		public static bool operator !=(GameJoltResponse<T> left, GameJoltResponse<T> right)
		{
			return !left.Equals(right);
		}
	}
}
#endif // DISABLE_GAMEJOLT