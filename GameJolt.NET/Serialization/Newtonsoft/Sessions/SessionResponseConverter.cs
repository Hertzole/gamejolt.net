#if !NET6_0_OR_GREATER
namespace Hertzole.GameJolt
{
	internal sealed class SessionResponseConverter : ResponseConverter<SessionResponse>
	{
		protected override SessionResponse CreateResponse(bool success, string? message, SessionResponse existingData)
		{
			return new SessionResponse(success, message);
		}
	}
}
#endif