#if !NET6_0_OR_GREATER
#nullable enable

namespace Hertzole.GameJolt
{
	internal sealed class AuthResponseConverter : ResponseConverter<AuthResponse>
	{
		protected override AuthResponse CreateResponse(bool success, string? message, AuthResponse existingData)
		{
			return new AuthResponse(success, message);
		}
	}
}
#endif