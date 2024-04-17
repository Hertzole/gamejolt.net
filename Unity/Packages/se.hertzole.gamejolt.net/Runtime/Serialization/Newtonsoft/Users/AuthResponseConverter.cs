#if !NET6_0_OR_GREATER
#nullable enable

namespace Hertzole.GameJolt.Serialization.Newtonsoft
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