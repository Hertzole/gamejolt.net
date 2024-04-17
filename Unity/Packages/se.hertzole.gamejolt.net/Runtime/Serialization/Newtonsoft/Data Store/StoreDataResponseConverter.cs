#if !NET6_0_OR_GREATER
#nullable enable

namespace Hertzole.GameJolt.Serialization.Newtonsoft
{
	internal sealed class StoreDataResponseConverter : ResponseConverter<StoreDataResponse>
	{
		protected override StoreDataResponse CreateResponse(bool success, string? message, StoreDataResponse existingData)
		{
			return new StoreDataResponse(success, message);
		}
	}
}
#endif