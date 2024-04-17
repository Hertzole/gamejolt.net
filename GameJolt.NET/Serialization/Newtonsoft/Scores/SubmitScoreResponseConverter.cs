#if !NET6_0_OR_GREATER
#nullable enable

namespace Hertzole.GameJolt.Serialization.Newtonsoft
{
	internal sealed class SubmitScoreResponseConverter : ResponseConverter<SubmitScoreResponse>
	{
		protected override SubmitScoreResponse CreateResponse(bool success, string? message, SubmitScoreResponse existingData)
		{
			return new SubmitScoreResponse(success, message);
		}
	}
}
#endif