#if !DISABLE_GAMEJOLT // Disables all GameJolt-related code

#nullable enable

using System;
using System.Diagnostics;
using System.Globalization;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
#if NETSTANDARD2_1_OR_GREATER || NET5_0_OR_GREATER || UNITY_2021_3_OR_NEWER
using GameJoltResultTask = System.Threading.Tasks.ValueTask<Hertzole.GameJolt.GameJoltResult>;
using StringIntTask = System.Threading.Tasks.ValueTask<Hertzole.GameJolt.GameJoltResult<(string stringValue, int intValue)>>;
using GameJoltStringTask = System.Threading.Tasks.ValueTask<Hertzole.GameJolt.GameJoltResult<string>>;
using GameJoltStringArrayTask = System.Threading.Tasks.ValueTask<Hertzole.GameJolt.GameJoltResult<string[]>>;
#else
using GameJoltResultTask = System.Threading.Tasks.Task<Hertzole.GameJolt.GameJoltResult>;
using StringIntTask = System.Threading.Tasks.Task<Hertzole.GameJolt.GameJoltResult<(string stringValue, int intValue)>>;
using GameJoltStringTask = System.Threading.Tasks.Task<Hertzole.GameJolt.GameJoltResult<string>>;
using GameJoltStringArrayTask = System.Threading.Tasks.Task<Hertzole.GameJolt.GameJoltResult<string[]>>;
#endif

namespace Hertzole.GameJolt
{
	/// <summary>
	///     A cloud-based data storage system. It's completely up to you what you use this for. The more inventive the better!
	/// </summary>
	public sealed class GameJoltDataStore
	{
		private readonly IGameJoltWebClient webClient;
		private readonly IGameJoltSerializer serializer;
		private readonly GameJoltUsers users;

		internal GameJoltDataStore(IGameJoltWebClient webClient, IGameJoltSerializer serializer, GameJoltUsers users)
		{
			this.webClient = webClient;
			this.serializer = serializer;
			this.users = users;
		}

		private const string ENDPOINT = "data-store/";
		internal const string SET_ENDPOINT = ENDPOINT + "set/";
		internal const string REMOVE_ENDPOINT = ENDPOINT + "remove/";
		internal const string UPDATE_ENDPOINT = ENDPOINT + "update/";
		internal const string FETCH_ENDPOINT = ENDPOINT;
		internal const string FETCH_KEYS_ENDPOINT = ENDPOINT + "get-keys/";

		/// <summary>
		///     Set data in the data store. The data will only be accessible to everyone.
		/// </summary>
		/// <param name="key">The key of the data item you'd like to set.</param>
		/// <param name="data">The data you'd like to set.</param>
		/// <param name="cancellationToken">Optional cancellation token for stopping this task.</param>
		/// <returns>The result of the operation.</returns>
		/// <exception cref="ArgumentException">Returned if <paramref name="data" /> is null, empty, or whitespace.</exception>
		public async Task<GameJoltResult> SetAsync(string key, string data, CancellationToken cancellationToken = default)
		{
			return await SetInternalAsync(key, data, null, null, cancellationToken);
		}

		/// <summary>
		///     Set data in the data store. The data will only be accessible to everyone.
		/// </summary>
		/// <param name="key">The key of the data item you'd like to set.</param>
		/// <param name="data">The data you'd like to set.</param>
		/// <param name="cancellationToken">Optional cancellation token for stopping this task.</param>
		/// <returns>The result of the operation.</returns>
		/// <exception cref="ArgumentException">Returned if <paramref name="data" /> is null, empty, or whitespace.</exception>
		public async Task<GameJoltResult> SetAsync(string key, int data, CancellationToken cancellationToken = default)
		{
			return await SetInternalAsync(key, data.ToString(CultureInfo.InvariantCulture), null, null, cancellationToken);
		}

		/// <summary>
		///     Set data in the data store as a Base64 string. The data will only be accessible to everyone.
		/// </summary>
		/// <param name="key">The key of the data item you'd like to set.</param>
		/// <param name="data">The data you'd like to set.</param>
		/// <param name="cancellationToken">Optional cancellation token for stopping this task.</param>
		/// <returns>The result of the operation.</returns>
		/// <exception cref="ArgumentException">Returned if <paramref name="data" /> is null, empty, or whitespace.</exception>
		public async Task<GameJoltResult> SetAsync(string key, byte[] data, CancellationToken cancellationToken = default)
		{
			return await SetInternalAsync(key, Convert.ToBase64String(data), null, null, cancellationToken);
		}

		/// <summary>
		///     Set data in the data store. The data will only be accessible to everyone.
		/// </summary>
		/// <param name="key">The key of the data item you'd like to set.</param>
		/// <param name="data">The data you'd like to set.</param>
		/// <param name="cancellationToken">Optional cancellation token for stopping this task.</param>
		/// <returns>The result of the operation.</returns>
		/// <exception cref="ArgumentException">Returned if <paramref name="data" /> is null, empty, or whitespace.</exception>
		public async Task<GameJoltResult> SetAsync(string key, bool data, CancellationToken cancellationToken = default)
		{
			return await SetInternalAsync(key, data ? "true" : "false", null, null, cancellationToken);
		}

		/// <summary>
		///     Set data in the data store as the current user. The data will only be accessible for this user. This requires the
		///     user to be authenticated.
		/// </summary>
		/// <param name="key">The key of the data item you'd like to set.</param>
		/// <param name="data">The data you'd like to set.</param>
		/// <param name="cancellationToken">Optional cancellation token for stopping this task.</param>
		/// <returns>The result of the operation.</returns>
		/// <exception cref="ArgumentException">Thrown if <paramref name="data" /> is null, empty, or whitespace.</exception>
		/// <exception cref="GameJoltAuthorizedException">Returned if the user is not authenticated.</exception>
		public async Task<GameJoltResult> SetAsCurrentUserAsync(string key, string data, CancellationToken cancellationToken = default)
		{
			if (!users.IsAuthenticatedInternal(out GameJoltResult authResult))
			{
				return authResult;
			}

			return await SetInternalAsync(key, data, users.myUsername, users.myToken, cancellationToken);
		}

		/// <summary>
		///     Set data in the data store as the current user. The data will only be accessible for this user. This requires the
		///     user to be authenticated.
		/// </summary>
		/// <param name="key">The key of the data item you'd like to set.</param>
		/// <param name="data">The data you'd like to set.</param>
		/// <param name="cancellationToken">Optional cancellation token for stopping this task.</param>
		/// <returns>The result of the operation.</returns>
		/// <exception cref="ArgumentException">Thrown if <paramref name="data" /> is null, empty, or whitespace.</exception>
		/// <exception cref="GameJoltAuthorizedException">Returned if the user is not authenticated.</exception>
		public async Task<GameJoltResult> SetAsCurrentUserAsync(string key, int data, CancellationToken cancellationToken = default)
		{
			if (!users.IsAuthenticatedInternal(out GameJoltResult authResult))
			{
				return authResult;
			}

			return await SetInternalAsync(key, data.ToString(CultureInfo.InvariantCulture), users.myUsername, users.myToken, cancellationToken);
		}

		/// <summary>
		///     Set data in the data store as a Base64 string as the current user. The data will only be accessible for this user.
		///     This requires the user to be authenticated.
		/// </summary>
		/// <param name="key">The key of the data item you'd like to set.</param>
		/// <param name="data">The data you'd like to set.</param>
		/// <param name="cancellationToken">Optional cancellation token for stopping this task.</param>
		/// <returns>The result of the operation.</returns>
		/// <exception cref="ArgumentException">Thrown if <paramref name="data" /> is null, empty, or whitespace.</exception>
		/// <exception cref="GameJoltAuthorizedException">Returned if the user is not authenticated.</exception>
		public async Task<GameJoltResult> SetAsCurrentUserAsync(string key, byte[] data, CancellationToken cancellationToken = default)
		{
			if (!users.IsAuthenticatedInternal(out GameJoltResult authResult))
			{
				return authResult;
			}

			return await SetInternalAsync(key, Convert.ToBase64String(data), users.myUsername, users.myToken, cancellationToken);
		}

		/// <summary>
		///     Set data in the data store as the current user. The data will only be accessible for this user. This requires the
		///     user to be authenticated.
		/// </summary>
		/// <param name="key">The key of the data item you'd like to set.</param>
		/// <param name="data">The data you'd like to set.</param>
		/// <param name="cancellationToken">Optional cancellation token for stopping this task.</param>
		/// <returns>The result of the operation.</returns>
		/// <exception cref="ArgumentException">Thrown if <paramref name="data" /> is null, empty, or whitespace.</exception>
		/// <exception cref="GameJoltAuthorizedException">Returned if the user is not authenticated.</exception>
		public async Task<GameJoltResult> SetAsCurrentUserAsync(string key, bool data, CancellationToken cancellationToken = default)
		{
			if (!users.IsAuthenticatedInternal(out GameJoltResult authResult))
			{
				return authResult;
			}

			return await SetInternalAsync(key, data ? "true" : "false", users.myUsername, users.myToken, cancellationToken);
		}

		/// <summary>
		///     Sets data in the data store.
		/// </summary>
		/// <param name="key">The key of the data item you'd like to set.</param>
		/// <param name="data">The data you'd like to set.</param>
		/// <param name="username">Optional username for the target user.</param>
		/// <param name="token">Optional user token for the target user.</param>
		/// <param name="cancellationToken">Optional cancellation token for stopping this task.</param>
		/// <returns>The result of the operation.</returns>
		/// <exception cref="ArgumentException">Thrown if <paramref name="data" /> is null, empty, or whitespace.</exception>
		private async GameJoltResultTask SetInternalAsync(string key, string data, string? username, string? token, CancellationToken cancellationToken)
		{
			if (string.IsNullOrWhiteSpace(data))
			{
				return GameJoltResult.Error(new ArgumentException("Data cannot be null, empty, or whitespace.", nameof(data)));
			}

			using (StringBuilderPool.Rent(out StringBuilder sb))
			{
				sb.Append(SET_ENDPOINT + "?key=");
				sb.Append(key);
				sb.Append("&data=");
				sb.Append(data);

				if (!string.IsNullOrEmpty(username))
				{
					sb.Append("&username=");
					sb.Append(username);
				}

				if (!string.IsNullOrEmpty(token))
				{
					sb.Append("&user_token=");
					sb.Append(token);
				}

				string json = await webClient.GetStringAsync(GameJoltUrlBuilder.BuildUrl(sb), cancellationToken);
				Response result = serializer.DeserializeResponse<Response>(json);

				if (result.TryGetException(out Exception? exception))
				{
					return GameJoltResult.Error(exception!);
				}

				Debug.Assert(result.Success, "Result was successful, but the success flag was false.");

				return GameJoltResult.Success();
			}
		}

		/// <summary>
		///     Removes data items from the data store. This will only remove data items that are accessible to everyone.
		/// </summary>
		/// <param name="key">The key of the data item you'd like to remove.</param>
		/// <param name="cancellationToken">Optional cancellation token for stopping this task.</param>
		/// <returns>The result of the operation.</returns>
		public async Task<GameJoltResult> RemoveAsync(string key, CancellationToken cancellationToken = default)
		{
			return await RemoveInternalAsync(key, null, null, cancellationToken);
		}

		/// <summary>
		///     Removes data items from the data store. This will only remove data items that is accessible to the current user.
		/// </summary>
		/// <param name="key">The key of the data item you'd like to remove.</param>
		/// <param name="cancellationToken">Optional cancellation token for stopping this task.</param>
		/// <exception cref="GameJoltAuthorizedException">Returned if the user is not authenticated.</exception>
		/// <returns>The result of the operation.</returns>
		public async Task<GameJoltResult> RemoveAsCurrentUserAsync(string key, CancellationToken cancellationToken = default)
		{
			if (!users.IsAuthenticatedInternal(out GameJoltResult authResult))
			{
				return authResult;
			}

			return await RemoveInternalAsync(key, users.myUsername, users.myToken, cancellationToken);
		}

		private async GameJoltResultTask RemoveInternalAsync(string key, string? username, string? token, CancellationToken cancellationToken)
		{
			using (StringBuilderPool.Rent(out StringBuilder sb))
			{
				sb.Append(REMOVE_ENDPOINT + "?key=");
				sb.Append(key);

				if (!string.IsNullOrEmpty(username))
				{
					sb.Append("&username=");
					sb.Append(username);
				}

				if (!string.IsNullOrEmpty(token))
				{
					sb.Append("&user_token=");
					sb.Append(token);
				}

				string json = await webClient.GetStringAsync(GameJoltUrlBuilder.BuildUrl(sb), cancellationToken);
				Response result = serializer.DeserializeResponse<Response>(json);

				if (result.TryGetException(out Exception? exception))
				{
					return GameJoltResult.Error(exception!);
				}

				Debug.Assert(result.Success, "Result was successful, but the success flag was false.");

				return GameJoltResult.Success();
			}
		}

		/// <summary>
		///     Updates data in the data store with various functions. This will only update data items that are accessible to
		///     everyone.
		/// </summary>
		/// <param name="key">The key of the data item you'd like to update.</param>
		/// <param name="data">The value you'd like to apply to the data store item.</param>
		/// <param name="operation">The operation you'd like to perform.</param>
		/// <param name="cancellationToken">Optional cancellation token for stopping this task.</param>
		/// <returns>The result of the operation and the new value.</returns>
		/// <exception cref="GameJoltInvalidDataStoreKeyException">Returned if the key doesn't exist on the cloud.</exception>
		public async Task<GameJoltResult<string>> UpdateAsync(string key, string data, StringOperation operation, CancellationToken cancellationToken = default)
		{
			GameJoltResult<(string stringValue, int intValue)> result =
				await UpdateInternalAsync(key, GetStringOperation(operation), data, null, null, cancellationToken);

			if (result.HasError)
			{
				return GameJoltResult<string>.Error(result.Exception!);
			}

			return GameJoltResult<string>.Success(result.Value.stringValue);
		}

		/// <summary>
		///     Updates data in the data store with various functions. This will only update data items that are accessible to
		///     everyone.
		/// </summary>
		/// <param name="key">The key of the data item you'd like to update.</param>
		/// <param name="data">The value you'd like to apply to the data store item.</param>
		/// <param name="operation">The operation you'd like to perform.</param>
		/// <param name="cancellationToken">Optional cancellation token for stopping this task.</param>
		/// <returns>The result of the operation and the new value.</returns>
		/// <exception cref="GameJoltInvalidDataStoreKeyException">Returned if the key doesn't exist on the cloud.</exception>
		public async Task<GameJoltResult<int>> UpdateAsync(string key, int data, NumericOperation operation, CancellationToken cancellationToken = default)
		{
			GameJoltResult<(string stringValue, int intValue)> result =
				await UpdateInternalAsync(key, GetNumberOperation(operation), data.ToString(CultureInfo.InvariantCulture), null, null, cancellationToken);

			if (result.HasError)
			{
				return GameJoltResult<int>.Error(result.Exception!);
			}

			return GameJoltResult<int>.Success(result.Value.intValue);
		}

		/// <summary>
		///     Updates data in the data store with various functions. This will only update data items that are accessible to the
		///     current user.
		/// </summary>
		/// <param name="key">The key of the data item you'd like to update.</param>
		/// <param name="data">The value you'd like to apply to the data store item.</param>
		/// <param name="operation">The operation you'd like to perform.</param>
		/// <param name="cancellationToken">Optional cancellation token for stopping this task.</param>
		/// <returns>The result of the operation and the new value.</returns>
		/// <exception cref="GameJoltInvalidDataStoreKeyException">Returned if the key doesn't exist on the cloud.</exception>
		public async Task<GameJoltResult<string>> UpdateAsCurrentUserAsync(string key,
			string data,
			StringOperation operation,
			CancellationToken cancellationToken = default)
		{
			if (!users.IsAuthenticatedInternal(out GameJoltResult authResult))
			{
				return GameJoltResult<string>.Error(authResult.Exception!);
			}

			GameJoltResult<(string stringValue, int intValue)> result =
				await UpdateInternalAsync(key, GetStringOperation(operation), data, users.myUsername, users.myToken, cancellationToken);

			if (result.HasError)
			{
				return GameJoltResult<string>.Error(result.Exception!);
			}

			return GameJoltResult<string>.Success(result.Value.stringValue);
		}

		/// <summary>
		///     Updates data in the data store with various functions. This will only update data items that are accessible to the
		///     current user.
		/// </summary>
		/// <param name="key">The key of the data item you'd like to update.</param>
		/// <param name="data">The value you'd like to apply to the data store item.</param>
		/// <param name="operation">The operation you'd like to perform.</param>
		/// <param name="cancellationToken">Optional cancellation token for stopping this task.</param>
		/// <returns>The result of the operation and the new value.</returns>
		/// <exception cref="GameJoltInvalidDataStoreKeyException">Returned if the key doesn't exist on the cloud.</exception>
		public async Task<GameJoltResult<int>> UpdateAsCurrentUserAsync(string key,
			int data,
			NumericOperation operation,
			CancellationToken cancellationToken = default)
		{
			if (!users.IsAuthenticatedInternal(out GameJoltResult authResult))
			{
				return GameJoltResult<int>.Error(authResult.Exception!);
			}

			GameJoltResult<(string stringValue, int intValue)> result =
				await UpdateInternalAsync(key, GetNumberOperation(operation), data.ToString(CultureInfo.InvariantCulture), users.myUsername, users.myToken,
					cancellationToken);

			if (result.HasError)
			{
				return GameJoltResult<int>.Error(result.Exception!);
			}

			return GameJoltResult<int>.Success(result.Value.intValue);
		}

		private async StringIntTask UpdateInternalAsync(string key,
			string operation,
			string value,
			string? username,
			string? token,
			CancellationToken cancellationToken)
		{
			using (StringBuilderPool.Rent(out StringBuilder sb))
			{
				sb.Append(UPDATE_ENDPOINT + "?key=");
				sb.Append(key);

				if (!string.IsNullOrEmpty(username))
				{
					sb.Append("&username=");
					sb.Append(username);
				}

				if (!string.IsNullOrEmpty(token))
				{
					sb.Append("&user_token=");
					sb.Append(token);
				}

				sb.Append("&operation=");
				sb.Append(operation);
				sb.Append("&value=");
				sb.Append(value);

				string json = await webClient.GetStringAsync(GameJoltUrlBuilder.BuildUrl(sb), cancellationToken);
				UpdateDataResponse response = serializer.DeserializeResponse<UpdateDataResponse>(json);

				if (response.TryGetException(out Exception? exception))
				{
					return GameJoltResult<(string, int)>.Error(exception!);
				}

				Debug.Assert(response.Success, "Result was successful, but the success flag was false.");

				if (!int.TryParse(response.data, NumberStyles.Integer, CultureInfo.InvariantCulture, out int intValue))
				{
					intValue = 0;
				}

				return GameJoltResult<(string, int)>.Success((response.data, intValue));
			}
		}

		/// <summary>
		///     Fetches data from the data store as a <c>string</c>. This will only fetch data items that are accessible to
		///     everyone.
		/// </summary>
		/// <param name="key">The key of the data item you'd like to fetch.</param>
		/// <param name="cancellationToken">Optional cancellation token for stopping this task.</param>
		/// <returns>The result of the operation and the value.</returns>
		/// <exception cref="GameJoltInvalidDataStoreKeyException">Returned if the key doesn't exist on the cloud.</exception>
		public async Task<GameJoltResult<string>> GetValueAsStringAsync(string key, CancellationToken cancellationToken = default)
		{
			return await GetValueInternalAsync(key, null, null, cancellationToken);
		}

		/// <summary>
		///     Fetches data from the data store as an <c>int</c>. This will only fetch data items that are accessible to everyone.
		/// </summary>
		/// <param name="key">The key of the data item you'd like to fetch.</param>
		/// <param name="cancellationToken">Optional cancellation token for stopping this task.</param>
		/// <returns>The result of the operation and the value.</returns>
		/// <exception cref="GameJoltInvalidDataStoreKeyException">Returned if the key doesn't exist on the cloud.</exception>
		/// <exception cref="GameJoltInvalidDataStoreValueException">Returned if the value is not an integer.</exception>
		public async Task<GameJoltResult<int>> GetValueAsIntAsync(string key, CancellationToken cancellationToken = default)
		{
			GameJoltResult<string> result = await GetValueInternalAsync(key, null, null, cancellationToken);

			if (result.HasError)
			{
				return GameJoltResult<int>.Error(result.Exception!);
			}

			if (!int.TryParse(result.Value, NumberStyles.Integer, CultureInfo.InvariantCulture, out int intValue))
			{
				return GameJoltResult<int>.Error(new GameJoltInvalidDataStoreValueException("The value stored is not an integer."));
			}

			return GameJoltResult<int>.Success(intValue);
		}

		/// <summary>
		///     Fetches data from the data store as a <c>byte</c> array. This will only fetch data items that are accessible to
		///     everyone.
		/// </summary>
		/// <param name="key">The key of the data item you'd like to fetch.</param>
		/// <param name="cancellationToken">Optional cancellation token for stopping this task.</param>
		/// <returns>The result of the operation and the value.</returns>
		/// <exception cref="GameJoltInvalidDataStoreKeyException">Returned if the key doesn't exist on the cloud.</exception>
		/// <exception cref="FormatException">Returned if the value can't be decoded from a Base64 string.</exception>
		public async Task<GameJoltResult<byte[]>> GetValueAsBytesAsync(string key, CancellationToken cancellationToken = default)
		{
			GameJoltResult<string> result = await GetValueInternalAsync(key, null, null, cancellationToken);

			if (result.HasError)
			{
				return GameJoltResult<byte[]>.Error(result.Exception!);
			}

			if (string.IsNullOrEmpty(result.Value))
			{
				return GameJoltResult<byte[]>.Success(Array.Empty<byte>());
			}

			try
			{
				return GameJoltResult<byte[]>.Success(Convert.FromBase64String(result.Value!));
			}
			catch (FormatException e)
			{
				return GameJoltResult<byte[]>.Error(e);
			}
		}

		/// <summary>
		///     Fetches data from the data store as a <c>bool</c>. This will only fetch data items that are accessible to everyone.
		/// </summary>
		/// <param name="key">The key of the data item you'd like to fetch.</param>
		/// <param name="cancellationToken">Optional cancellation token for stopping this task.</param>
		/// <returns>The result of the operation and the value.</returns>
		/// <exception cref="GameJoltInvalidDataStoreKeyException">Returned if the key doesn't exist on the cloud.</exception>
		/// <exception cref="GameJoltInvalidDataStoreValueException">Returned if the value is not a bool.</exception>
		public async Task<GameJoltResult<bool>> GetValueAsBoolAsync(string key, CancellationToken cancellationToken = default)
		{
			GameJoltResult<string> result = await GetValueInternalAsync(key, null, null, cancellationToken);

			if (result.HasError)
			{
				return GameJoltResult<bool>.Error(result.Exception!);
			}

			if (!bool.TryParse(result.Value, out bool boolValue))
			{
				return GameJoltResult<bool>.Error(new GameJoltInvalidDataStoreValueException("The value stored is not a boolean."));
			}

			return GameJoltResult<bool>.Success(boolValue);
		}

		/// <summary>
		///     Fetches data from the data store as a <c>string</c>. This will only fetch data items that are accessible to the
		///     current user.
		/// </summary>
		/// <param name="key">The key of the data item you'd like to fetch.</param>
		/// <param name="cancellationToken">Optional cancellation token for stopping this task.</param>
		/// <returns>The result of the operation and the value.</returns>
		/// <exception cref="GameJoltAuthorizedException">Returned if the user is not authenticated.</exception>
		/// <exception cref="GameJoltInvalidDataStoreKeyException">Returned if the key doesn't exist on the cloud.</exception>
		public async Task<GameJoltResult<string>> GetValueAsStringAsCurrentUserAsync(string key, CancellationToken cancellationToken = default)
		{
			if (!users.IsAuthenticatedInternal(out GameJoltResult authResult))
			{
				return GameJoltResult<string>.Error(authResult.Exception!);
			}

			return await GetValueInternalAsync(key, users.myUsername, users.myToken, cancellationToken);
		}

		/// <summary>
		///     Fetches data from the data store as an <c>int</c>. This will only fetch data items that are accessible to the
		///     current user.
		/// </summary>
		/// <param name="key">The key of the data item you'd like to fetch.</param>
		/// <param name="cancellationToken">Optional cancellation token for stopping this task.</param>
		/// <returns>The result of the operation and the value.</returns>
		/// <exception cref="GameJoltAuthorizedException">Returned if the user is not authenticated.</exception>
		/// <exception cref="GameJoltInvalidDataStoreKeyException">Returned if the key doesn't exist on the cloud.</exception>
		/// <exception cref="GameJoltInvalidDataStoreValueException">Returned if the value is not an integer.</exception>
		public async Task<GameJoltResult<int>> GetValueAsIntAsCurrentUserAsync(string key, CancellationToken cancellationToken = default)
		{
			if (!users.IsAuthenticatedInternal(out GameJoltResult authResult))
			{
				return GameJoltResult<int>.Error(authResult.Exception!);
			}

			GameJoltResult<string> result = await GetValueInternalAsync(key, users.myUsername, users.myToken, cancellationToken);

			if (result.HasError)
			{
				return GameJoltResult<int>.Error(result.Exception!);
			}

			if (!int.TryParse(result.Value, NumberStyles.Integer, CultureInfo.InvariantCulture, out int intValue))
			{
				return GameJoltResult<int>.Error(new GameJoltInvalidDataStoreValueException("The value stored is not an integer."));
			}

			return GameJoltResult<int>.Success(intValue);
		}

		/// <summary>
		///     Fetches data from the data store as a <c>byte</c> array. This will only fetch data items that are accessible to the
		///     current user.
		/// </summary>
		/// <param name="key">The key of the data item you'd like to fetch.</param>
		/// <param name="cancellationToken">Optional cancellation token for stopping this task.</param>
		/// <returns>The result of the operation and the value.</returns>
		/// <exception cref="GameJoltAuthorizedException">Returned if the user is not authenticated.</exception>
		/// <exception cref="GameJoltInvalidDataStoreKeyException">Returned if the key doesn't exist on the cloud.</exception>
		/// <exception cref="FormatException">Returned if the value can't be decoded from a Base64 string.</exception>
		public async Task<GameJoltResult<byte[]>> GetValueAsBytesAsCurrentUserAsync(string key, CancellationToken cancellationToken = default)
		{
			if (!users.IsAuthenticatedInternal(out GameJoltResult authResult))
			{
				return GameJoltResult<byte[]>.Error(authResult.Exception!);
			}

			GameJoltResult<string> result = await GetValueInternalAsync(key, users.myUsername, users.myToken, cancellationToken);

			if (result.HasError)
			{
				return GameJoltResult<byte[]>.Error(result.Exception!);
			}

			if (string.IsNullOrEmpty(result.Value))
			{
				return GameJoltResult<byte[]>.Success(Array.Empty<byte>());
			}

			try
			{
				return GameJoltResult<byte[]>.Success(Convert.FromBase64String(result.Value!));
			}
			catch (FormatException e)
			{
				return GameJoltResult<byte[]>.Error(e);
			}
		}

		/// <summary>
		///     Fetches data from the data store as a <c>bool</c>. This will only fetch data items that are accessible to the
		///     current user.
		/// </summary>
		/// <param name="key">The key of the data item you'd like to fetch.</param>
		/// <param name="cancellationToken">Optional cancellation token for stopping this task.</param>
		/// <returns>The result of the operation and the value.</returns>
		/// <exception cref="GameJoltAuthorizedException">Returned if the user is not authenticated.</exception>
		/// <exception cref="GameJoltInvalidDataStoreKeyException">Returned if the key doesn't exist on the cloud.</exception>
		/// <exception cref="GameJoltInvalidDataStoreValueException">Returned if the value is not a bool.</exception>
		public async Task<GameJoltResult<bool>> GetValueAsBoolAsCurrentUserAsync(string key, CancellationToken cancellationToken = default)
		{
			if (!users.IsAuthenticatedInternal(out GameJoltResult authResult))
			{
				return GameJoltResult<bool>.Error(authResult.Exception!);
			}

			GameJoltResult<string> result = await GetValueInternalAsync(key, users.myUsername, users.myToken, cancellationToken);

			if (result.HasError)
			{
				return GameJoltResult<bool>.Error(result.Exception!);
			}

			if (!bool.TryParse(result.Value, out bool boolValue))
			{
				return GameJoltResult<bool>.Error(new GameJoltInvalidDataStoreValueException("The value stored is not a boolean."));
			}

			return GameJoltResult<bool>.Success(boolValue);
		}

		private async GameJoltStringTask GetValueInternalAsync(string key, string? username, string? token, CancellationToken cancellationToken)
		{
			using (StringBuilderPool.Rent(out StringBuilder sb))
			{
				sb.Append(FETCH_ENDPOINT + "?key=");
				sb.Append(key);

				if (!string.IsNullOrEmpty(username))
				{
					sb.Append("&username=");
					sb.Append(username);
				}

				if (!string.IsNullOrEmpty(token))
				{
					sb.Append("&user_token=");
					sb.Append(token);
				}

				string json = await webClient.GetStringAsync(GameJoltUrlBuilder.BuildUrl(sb), cancellationToken);
				GetDataResponse result = serializer.DeserializeResponse<GetDataResponse>(json);

				if (result.TryGetException(out Exception? exception))
				{
					return GameJoltResult<string>.Error(exception!);
				}

				Debug.Assert(result.Success, "Result was successful, but the success flag was false.");

				return GameJoltResult<string>.Success(result.data);
			}
		}

		/// <summary>
		///     Fetches keys of data items from the data store. This will only fetch data items that are accessible to everyone.
		/// </summary>
		/// <param name="pattern">Optional pattern to apply to the key names in the data store.</param>
		/// <param name="cancellationToken">Optional cancellation token for stopping this task.</param>
		/// <returns>The result of the operation and the keys.</returns>
		public async Task<GameJoltResult<string[]>> GetKeysAsync(string? pattern = null, CancellationToken cancellationToken = default)
		{
			return await GetKeysInternalAsync(pattern, null, null, cancellationToken);
		}

		/// <summary>
		///     Fetches keys of data items from the data store. This will only fetch data items that are accessible to the current
		///     user.
		/// </summary>
		/// <param name="pattern">Optional pattern to apply to the key names in the data store.</param>
		/// <param name="cancellationToken">Optional cancellation token for stopping this task.</param>
		/// <returns>The result of the operation and the keys.</returns>
		/// <exception cref="GameJoltAuthorizedException">Returned if the user is not authenticated.</exception>
		public async Task<GameJoltResult<string[]>> GetKeysAsCurrentUserAsync(string? pattern = null, CancellationToken cancellationToken = default)
		{
			if (!users.IsAuthenticatedInternal(out GameJoltResult authResult))
			{
				return GameJoltResult<string[]>.Error(authResult.Exception!);
			}

			return await GetKeysInternalAsync(pattern, users.myUsername, users.myToken, cancellationToken);
		}

		private async GameJoltStringArrayTask GetKeysInternalAsync(string? pattern, string? username, string? token, CancellationToken cancellationToken)
		{
			using (StringBuilderPool.Rent(out StringBuilder sb))
			{
				sb.Append(FETCH_KEYS_ENDPOINT);

				if (!string.IsNullOrWhiteSpace(pattern))
				{
					sb.Append("?pattern=");
					sb.Append(pattern);
				}

				if (!string.IsNullOrEmpty(username))
				{
					sb.Append(!string.IsNullOrWhiteSpace(pattern) ? '&' : '?');
					sb.Append("username=");
					sb.Append(username);
				}

				if (!string.IsNullOrEmpty(token))
				{
					sb.Append("&user_token=");
					sb.Append(token);
				}

				string json = await webClient.GetStringAsync(GameJoltUrlBuilder.BuildUrl(sb), cancellationToken);
				GetKeysResponse result = serializer.DeserializeResponse<GetKeysResponse>(json);

				if (result.TryGetException(out Exception? exception))
				{
					return GameJoltResult<string[]>.Error(exception!);
				}

				Debug.Assert(result.Success, "Result was successful, but the success flag was false.");

				string[] keys = result.keys.Length > 0 ? new string[result.keys.Length] : Array.Empty<string>();

				for (int i = 0; i < result.keys.Length; i++)
				{
					keys[i] = result.keys[i].key;
				}

				return GameJoltResult<string[]>.Success(keys);
			}
		}

		internal static string GetStringOperation(StringOperation operation)
		{
			switch (operation)
			{
				case StringOperation.Append:
					return "append";
				case StringOperation.Prepend:
					return "prepend";
				default:
					throw new ArgumentOutOfRangeException(nameof(operation), operation, null);
			}
		}

		internal static string GetNumberOperation(NumericOperation operation)
		{
			switch (operation)
			{
				case NumericOperation.Add:
					return "add";
				case NumericOperation.Subtract:
					return "subtract";
				case NumericOperation.Multiply:
					return "multiply";
				case NumericOperation.Divide:
					return "divide";
				case NumericOperation.Append:
					return "append";
				case NumericOperation.Prepend:
					return "prepend";
				default:
					throw new ArgumentOutOfRangeException(nameof(operation), operation, null);
			}
		}
	}
}
#endif // DISABLE_GAMEJOLT