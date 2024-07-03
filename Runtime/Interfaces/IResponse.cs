#if !DISABLE_GAMEJOLT // Disables all GameJolt-related code

#nullable enable

namespace Hertzole.GameJolt
{
	internal interface IResponse
	{
		bool Success { get; }

		string? Message { get; }
	}
}
#endif // DISABLE_GAMEJOLT