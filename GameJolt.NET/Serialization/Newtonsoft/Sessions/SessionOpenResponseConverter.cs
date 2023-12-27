#if UNITY_2021_1_OR_NEWER || !NET6_0_OR_GREATER
namespace Hertzole.GameJolt
{
	internal sealed class SessionOpenResponseConverter : SimpleResponseConverter<SessionOpenResponse>
	{
		protected override SessionOpenResponse CreateResponse(bool success, string message)
		{
			return new SessionOpenResponse(success, message);
		}
	}
}
#endif