#if !NET6_0_OR_GREATER
namespace Hertzole.GameJolt
{
	internal sealed class TrophyResponseConverter : ResponseConverter<TrophyResponse>
	{
		protected override TrophyResponse CreateResponse(bool success, string? message, TrophyResponse existingData)
		{
			return new TrophyResponse(success, message);
		}
	}
}
#endif