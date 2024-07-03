#if !DISABLE_GAMEJOLT // Disables all GameJolt-related code

namespace Hertzole.GameJolt
{
	internal interface IGameJoltSerializer
	{
		/// <summary>
		///     Serializes as a <see cref="GameJoltResponse{T}" /> with the given value.
		/// </summary>
		/// <param name="value">The value to serialize.</param>
		/// <typeparam name="T">The type of the value.</typeparam>
		/// <returns>The JSON value as a response JSON.</returns>
		string SerializeResponse<T>(T value);

		/// <summary>
		///     Serializes the given value to a JSON string.
		/// </summary>
		/// <param name="value">The value to serialize.</param>
		/// <typeparam name="T">The type of the value.</typeparam>
		/// <returns>The JSON value.</returns>
		string Serialize<T>(T value);

		/// <summary>
		///     Deserializes the given JSON string to the specified type.
		/// </summary>
		/// <param name="value">The JSON string to deserialize.</param>
		/// <typeparam name="T">The type to deserialize to.</typeparam>
		/// <returns>The deserialized value.</returns>
		T DeserializeResponse<T>(string value);

		/// <summary>
		///     Deserializes the given JSON string to the specified type.
		/// </summary>
		/// <param name="value">The JSON string to deserialize.</param>
		/// <typeparam name="T">The type to deserialize to.</typeparam>
		/// <returns>The deserialized value.</returns>
		T Deserialize<T>(string value);
	}
}
#endif // DISABLE_GAMEJOLT