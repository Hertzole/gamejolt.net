#if UNITY_2021_1_OR_NEWER || !NET6_0_OR_GREATER
namespace Hertzole.GameJolt
{
	internal sealed class SubmitScoresResponseConverter : SimpleResponseConverter<SubmitScoreResponse> 
	{
		protected override SubmitScoreResponse CreateResponse(bool success, string message)
		{
			return new SubmitScoreResponse(success, message);
		}
	}
}
#endif