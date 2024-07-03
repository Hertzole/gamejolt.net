#if !DISABLE_GAMEJOLT // Disables all GameJolt-related code

namespace Hertzole.GameJolt
{
	/// <summary>
	///     A session status used in <see cref="GameJoltSessions.PingAsync" />.
	/// </summary>
	public enum SessionStatus
	{
		/// <summary>
		///     The session is idle.
		/// </summary>
		Idle = 0,
		/// <summary>
		///     The session is active.
		/// </summary>
		Active = 1
	}
}
#endif // DISABLE_GAMEJOLT