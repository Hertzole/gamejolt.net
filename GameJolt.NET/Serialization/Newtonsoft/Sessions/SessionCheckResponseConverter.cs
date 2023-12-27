#if UNITY_2021_1_OR_NEWER || !NET6_0_OR_GREATER
namespace Hertzole.GameJolt
{
	internal sealed class SessionCheckResponseConverter : SimpleResponseConverter<SessionCheckResponse>
	{
		protected override SessionCheckResponse CreateResponse(bool success, string message)
		{
			return new SessionCheckResponse(success, message);
		}
	}
}
#endif