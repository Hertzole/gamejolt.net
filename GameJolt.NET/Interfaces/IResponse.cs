namespace Hertzole.GameJolt
{
	internal interface IResponse
	{
		bool Success { get; }
		
		string? Message { get; }
	}
}