using System;
using System.Diagnostics;
using System.Globalization;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Hertzole.GameJolt
{
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
		private const string SET_ENDPOINT = ENDPOINT + "set/";
		private const string REMOVE_ENDPOINT = ENDPOINT + "remove/";
		private const string UPDATE_ENDPOINT = ENDPOINT + "update/";
		private const string FETCH_ENDPOINT = ENDPOINT;
		internal const string FETCH_KEYS_ENDPOINT = ENDPOINT + "get-keys/";

		public async Task<GameJoltResult> SetAsync(string key, string data, CancellationToken cancellationToken = default)
		{
			return await SetInternalAsync(key, data, null, null, cancellationToken).ConfigureAwait(false);
		}

		public async Task<GameJoltResult> SetAsync(string key, int data, CancellationToken cancellationToken = default)
		{
			return await SetInternalAsync(key, data.ToString(CultureInfo.InvariantCulture), null, null, cancellationToken).ConfigureAwait(false);
		}
		
		public async Task<GameJoltResult> SetAsync(string key, byte[] data, CancellationToken cancellationToken = default)
		{
			return await SetAsync(key, Convert.ToBase64String(data), cancellationToken).ConfigureAwait(false);
		}

		public async Task<GameJoltResult> SetAsyncAsCurrentUser(string key, string data, CancellationToken cancellationToken = default)
		{
			if (!users.IsAuthenticatedInternal(out GameJoltResult result))
			{
				return result;
			}

			return await SetInternalAsync(key, data, users.myUsername, users.myToken, cancellationToken).ConfigureAwait(false);
		}
		
		public async Task<GameJoltResult> SetAsyncAsCurrentUser(string key, int data, CancellationToken cancellationToken = default)
		{
			if (!users.IsAuthenticatedInternal(out GameJoltResult result))
			{
				return result;
			}

			return await SetInternalAsync(key, data.ToString(CultureInfo.InvariantCulture), users.myUsername, users.myToken, cancellationToken).ConfigureAwait(false);
		}

		public async Task<GameJoltResult> SetAsyncAsCurrentUser(string key, byte[] data, CancellationToken cancellationToken = default)
		{
			return await SetAsyncAsCurrentUser(key, Convert.ToBase64String(data), cancellationToken).ConfigureAwait(false);
		}

		private async Task<GameJoltResult> SetInternalAsync(string key, string data, string? username, string? token, CancellationToken cancellationToken)
		{
			if (string.IsNullOrWhiteSpace(data))
			{
				return GameJoltResult.Error(new ArgumentException("Data cannot be null, empty, or whitespace.", nameof(data)));
			}

			using (StringBuilderPool.Rent(out StringBuilder sb))
			{
				sb.Append(SET_ENDPOINT);
				sb.Append("?key=");
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

				string json = await webClient.GetStringAsync(sb.ToString(), cancellationToken).ConfigureAwait(false);
				StoreDataResponse result = serializer.Deserialize<StoreDataResponse>(json);

				if (result.TryGetException(out Exception? exception))
				{
					return GameJoltResult.Error(exception);
				}

				Debug.Assert(result.Success, "Result was successful, but the success flag was false.");

				return GameJoltResult.Success();
			}
		}

		public async Task<GameJoltResult> RemoveAsync(string key, CancellationToken cancellationToken = default)
		{
			return await RemoveInternalAsync(key, null, null, cancellationToken).ConfigureAwait(false);
		}

		public async Task<GameJoltResult> RemoveAsyncAsCurrentUser(string key, CancellationToken cancellationToken = default)
		{
			if (!users.IsAuthenticatedInternal(out GameJoltResult result))
			{
				return result;
			}

			return await RemoveInternalAsync(key, users.myUsername, users.myToken, cancellationToken).ConfigureAwait(false);
		}

		private async Task<GameJoltResult> RemoveInternalAsync(string key, string? username, string? token, CancellationToken cancellationToken)
		{
			using (StringBuilderPool.Rent(out StringBuilder sb))
			{
				sb.Append(REMOVE_ENDPOINT);
				sb.Append("?key=");
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

				string json = await webClient.GetStringAsync(sb.ToString(), cancellationToken).ConfigureAwait(false);
				StoreDataResponse result = serializer.Deserialize<StoreDataResponse>(json);

				if (result.TryGetException(out Exception? exception))
				{
					return GameJoltResult.Error(exception);
				}

				Debug.Assert(result.Success, "Result was successful, but the success flag was false.");

				return GameJoltResult.Success();
			}
		}

		public async Task<GameJoltResult<string>> UpdateAsync(string key, string data, StringOperation operation, CancellationToken cancellationToken = default)
		{
			GameJoltResult<(string stringValue, int intValue)> result =
				await UpdateInternalAsync(key, GetStringOperation(operation), data, null, null, cancellationToken).ConfigureAwait(false);

			if (result.HasError)
			{
				return GameJoltResult<string>.Error(result.Exception!);
			}

			return GameJoltResult<string>.Success(result.Value.stringValue);
		}

		public async Task<GameJoltResult<int>> UpdateAsync(string key, int data, NumericOperation operation, CancellationToken cancellationToken = default)
		{
			GameJoltResult<(string stringValue, int intValue)> result =
				await UpdateInternalAsync(key, GetNumberOperation(operation), data.ToString(), null, null, cancellationToken).ConfigureAwait(false);

			if (result.HasError)
			{
				return GameJoltResult<int>.Error(result.Exception!);
			}

			return GameJoltResult<int>.Success(result.Value.intValue);
		}
		
		public async Task<GameJoltResult<string>> UpdateAsyncAsCurrentUser(string key, string data, StringOperation operation, CancellationToken cancellationToken = default)
		{
			if (!users.IsAuthenticatedInternal(out GameJoltResult result))
			{
				return GameJoltResult<string>.Error(result.Exception!);
			}

			GameJoltResult<(string stringValue, int intValue)> result2 =
				await UpdateInternalAsync(key, GetStringOperation(operation), data, users.myUsername, users.myToken, cancellationToken).ConfigureAwait(false);

			if (result2.HasError)
			{
				return GameJoltResult<string>.Error(result2.Exception!);
			}

			return GameJoltResult<string>.Success(result2.Value.stringValue);
		}
		
		public async Task<GameJoltResult<int>> UpdateAsyncAsCurrentUser(string key, int data, NumericOperation operation, CancellationToken cancellationToken = default)
		{
			if (!users.IsAuthenticatedInternal(out GameJoltResult result))
			{
				return GameJoltResult<int>.Error(result.Exception!);
			}

			GameJoltResult<(string stringValue, int intValue)> result2 =
				await UpdateInternalAsync(key, GetNumberOperation(operation), data.ToString(), users.myUsername, users.myToken, cancellationToken).ConfigureAwait(false);

			if (result2.HasError)
			{
				return GameJoltResult<int>.Error(result2.Exception!);
			}

			return GameJoltResult<int>.Success(result2.Value.intValue);
		}

		private async Task<GameJoltResult<(string stringValue, int intValue)>> UpdateInternalAsync(string key,
			string operation,
			string value,
			string? username,
			string? token,
			CancellationToken cancellationToken)
		{
			using (StringBuilderPool.Rent(out StringBuilder sb))
			{
				sb.Append(UPDATE_ENDPOINT);
				sb.Append("?key=");
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

				string json = await webClient.GetStringAsync(sb.ToString(), cancellationToken).ConfigureAwait(false);
				UpdateDataResponse response = serializer.Deserialize<UpdateDataResponse>(json);

				if (response.TryGetException(out Exception? exception))
				{
					return GameJoltResult<(string, int)>.Error(exception);
				}

				Debug.Assert(response.Success, "Result was successful, but the success flag was false.");

				if(!int.TryParse(response.data, NumberStyles.Integer, CultureInfo.InvariantCulture, out int intValue))
				{
					intValue = 0;
				}
				
				return GameJoltResult<(string, int)>.Success((response.data, intValue));
			}
		}

		public async Task<GameJoltResult<string>> GetValueAsStringAsync(string key, CancellationToken cancellationToken = default)
		{
			return await GetValueInternalAsync(key, null, null, cancellationToken).ConfigureAwait(false);
		}

		public async Task<GameJoltResult<int>> GetValueAsIntAsync(string key, CancellationToken cancellationToken = default)
		{
			GameJoltResult<string> result = await GetValueInternalAsync(key, null, null, cancellationToken).ConfigureAwait(false);

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
		
		public async Task<GameJoltResult<byte[]>> GetValueAsBytesAsync(string key, CancellationToken cancellationToken = default)
		{
			GameJoltResult<string> result = await GetValueInternalAsync(key, null, null, cancellationToken).ConfigureAwait(false);

			if (result.HasError)
			{
				return GameJoltResult<byte[]>.Error(result.Exception!);
			}

			try
			{
				return GameJoltResult<byte[]>.Success(Convert.FromBase64String(result.Value));
			}
			catch (FormatException e)
			{
				return GameJoltResult<byte[]>.Error(e);
			}
		}
		
		public async Task<GameJoltResult<string>> GetValueAsStringAsCurrentUserAsync(string key, CancellationToken cancellationToken = default)
		{
			if (!users.IsAuthenticatedInternal(out GameJoltResult result))
			{
				return GameJoltResult<string>.Error(result.Exception!);
			}
			
			return await GetValueInternalAsync(key, users.myUsername, users.myToken, cancellationToken).ConfigureAwait(false);
		}
		
		public async Task<GameJoltResult<int>> GetValueAsIntAsCurrentUserAsync(string key, CancellationToken cancellationToken = default)
		{
			if (!users.IsAuthenticatedInternal(out GameJoltResult result))
			{
				return GameJoltResult<int>.Error(result.Exception!);
			}
			
			GameJoltResult<string> result2 = await GetValueInternalAsync(key, users.myUsername, users.myToken, cancellationToken).ConfigureAwait(false);

			if (result2.HasError)
			{
				return GameJoltResult<int>.Error(result2.Exception!);
			}

			if (!int.TryParse(result2.Value, NumberStyles.Integer, CultureInfo.InvariantCulture, out int intValue))
			{
				return GameJoltResult<int>.Error(new GameJoltInvalidDataStoreValueException("The value stored is not an integer."));
			}

			return GameJoltResult<int>.Success(intValue);
		}
		
		public async Task<GameJoltResult<byte[]>> GetValueAsBytesAsCurrentUserAsync(string key, CancellationToken cancellationToken = default)
		{
			if (!users.IsAuthenticatedInternal(out GameJoltResult result))
			{
				return GameJoltResult<byte[]>.Error(result.Exception!);
			}
			
			GameJoltResult<string> result2 = await GetValueInternalAsync(key, users.myUsername, users.myToken, cancellationToken).ConfigureAwait(false);

			if (result2.HasError)
			{
				return GameJoltResult<byte[]>.Error(result2.Exception!);
			}

			try
			{
				return GameJoltResult<byte[]>.Success(Convert.FromBase64String(result2.Value));
			}
			catch (FormatException e)
			{
				return GameJoltResult<byte[]>.Error(e);
			}
		}
		
		private async Task<GameJoltResult<string>> GetValueInternalAsync(string key, string? username, string? token, CancellationToken cancellationToken)
		{
			using (StringBuilderPool.Rent(out StringBuilder sb))
			{
				sb.Append(FETCH_ENDPOINT);
				sb.Append("?key=");
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

				string json = await webClient.GetStringAsync(sb.ToString(), cancellationToken).ConfigureAwait(false);
				GetDataResponse result = serializer.Deserialize<GetDataResponse>(json);

				if (result.TryGetException(out Exception? exception))
				{
					return GameJoltResult<string>.Error(exception);
				}

				Debug.Assert(result.Success, "Result was successful, but the success flag was false.");

				return GameJoltResult<string>.Success(result.data);
			}
		}
		
		public async Task<GameJoltResult<string[]>> GetKeysAsync(string? pattern = null, CancellationToken cancellationToken = default)
		{
			return await GetKeysInternalAsync(pattern, null, null, cancellationToken).ConfigureAwait(false);
		}
		
		public async Task<GameJoltResult<string[]>> GetKeysAsCurrentUserAsync(string? pattern = null, CancellationToken cancellationToken = default)
		{
			if (!users.IsAuthenticatedInternal(out GameJoltResult result))
			{
				return GameJoltResult<string[]>.Error(result.Exception!);
			}
			
			return await GetKeysInternalAsync(pattern, users.myUsername, users.myToken, cancellationToken).ConfigureAwait(false);
		}
		
		private async Task<GameJoltResult<string[]>> GetKeysInternalAsync(string? pattern, string? username, string? token, CancellationToken cancellationToken)
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

				string json = await webClient.GetStringAsync(sb.ToString(), cancellationToken).ConfigureAwait(false);
				GetKeysResponse result = serializer.Deserialize<GetKeysResponse>(json);

				if (result.TryGetException(out Exception? exception))
				{
					return GameJoltResult<string[]>.Error(exception);
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

		private static string GetStringOperation(StringOperation operation)
		{
			switch (operation)
			{
				case StringOperation.Prepend:
					return "prepend";
				case StringOperation.Append:
					return "append";
				default:
					throw new ArgumentOutOfRangeException(nameof(operation), operation, null);
			}
		}

		private static string GetNumberOperation(NumericOperation operation)
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