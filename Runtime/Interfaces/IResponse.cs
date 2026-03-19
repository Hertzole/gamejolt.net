#if !DISABLE_GAMEJOLT // Disables all GameJolt-related code
#nullable enable

using System.Diagnostics.CodeAnalysis;

namespace Hertzole.GameJolt
{
	internal interface IResponse
	{
		[MemberNotNullWhen(true, nameof(Message))]
		bool Success { get; }

		string? Message { get; }
	}
}
#endif // DISABLE_GAMEJOLT