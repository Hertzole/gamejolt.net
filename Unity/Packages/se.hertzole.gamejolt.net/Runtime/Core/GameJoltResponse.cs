#if NET6_0_OR_GREATER
using System.Text.Json.Serialization;
#endif

namespace Hertzole.GameJolt
{
	internal readonly struct GameJoltResponse<T>
	{
		public readonly T response;

#if NET6_0_OR_GREATER
		[JsonConstructor]
#endif
		public GameJoltResponse(T response)
		{
			this.response = response;
		}
	}
}