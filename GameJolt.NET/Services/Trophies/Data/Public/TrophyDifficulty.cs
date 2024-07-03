#if !DISABLE_GAMEJOLT // Disables all GameJolt-related code

namespace Hertzole.GameJolt
{
	/// <summary>
	///     The difficulty of a trophy.
	/// </summary>
	public enum TrophyDifficulty
	{
		/// <summary>
		///     The trophy is bronze, the lowest difficulty.
		/// </summary>
		Bronze = 0,
		/// <summary>
		///     The trophy is silver, the second lowest difficulty.
		/// </summary>
		Silver = 1,
		/// <summary>
		///     The trophy is gold, the second highest difficulty.
		/// </summary>
		Gold = 2,
		/// <summary>
		///     The trophy is platinum, the highest difficulty.
		/// </summary>
		Platinum = 3
	}
}
#endif // DISABLE_GAMEJOLT