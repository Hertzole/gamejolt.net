#if UNITY_2021_1_OR_NEWER || !NET6_0_OR_GREATER
namespace Hertzole.GameJolt
{
	internal sealed class SessionCloseResponseConverter : SimpleResponseConverter<SessionCloseResponse>
	{
		protected override SessionCloseResponse CreateResponse(bool success, string message)
		{
			return new SessionCloseResponse(success, message);
		}
	}
}
#endif