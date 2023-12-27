#if UNITY_2021_1_OR_NEWER || !NET6_0_OR_GREATER
namespace Hertzole.GameJolt
{
	internal sealed class AuthResponseConverter : SimpleResponseConverter<AuthResponse>
	{
		protected override AuthResponse CreateResponse(bool success, string message)
		{
			return new AuthResponse(success, message);
		}
	}
}
#endif