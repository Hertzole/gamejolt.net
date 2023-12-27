#if UNITY_2021_1_OR_NEWER || !NET6_0_OR_GREATER
namespace Hertzole.GameJolt
{
	internal sealed class SessionPingResponseConverter : SimpleResponseConverter<SessionPingResponse>
	{
		protected override SessionPingResponse CreateResponse(bool success, string message)
		{
			return new SessionPingResponse(success, message);
		}
	}
}
#endif