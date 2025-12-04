#if !DISABLE_GAMEJOLT // Disables all GameJolt-related code

#nullable enable

#if NETSTANDARD2_1_OR_GREATER || NET5_0_OR_GREATER || UNITY_2021_3_OR_NEWER
using StringTask = System.Threading.Tasks.ValueTask<string>;
#else
using StringTask = System.Threading.Tasks.Task<string>;
#endif
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using Hertzole.GameJolt;
using NUnit.Framework;

namespace GameJolt.NET.Tests
{
	public sealed class WebTest
	{
		private readonly IGameJoltWebClient webClient = GameJoltAPI.GetWebClient();
		
		private const string API_URL = "https://httpbin.io";
		private const string GET_URL = API_URL + "/get";
		private const string ERROR_URL = API_URL + "/status/";

		[Test]
		[Retry(10)] // Retry up to 10 times in case of network issues.
		public async Task SendRequest_Success()
		{
			string? str = await webClient.GetStringAsync(API_URL, CancellationToken.None);

			Assert.That(str, Is.Not.Null);
		}

		[Test]
		[TestCase(404)]
		[TestCase(500)]
		[Retry(10)] // Retry up to 10 times in case of network issues.
		public async Task SendRequest_Fail(int errorCode)
		{
			bool caught = false;

			try
			{
				await webClient.GetStringAsync($"{ERROR_URL}{errorCode}", CancellationToken.None);
			}
			catch (HttpRequestException)
			{
				caught = true;
			}

			// Can't really use Assert.ThrowsAsync here since it freezes Unity. So we have to do it manually. ¯\_(ツ)_/¯
			Assert.That(caught, Is.True);
		}

		[Test]
		[Retry(10)] // Retry up to 10 times in case of network issues.
		public async Task SendRequest_Cancel([Values] bool beforeRequest)
		{
			CancellationTokenSource cancelSource = new CancellationTokenSource();

			if (beforeRequest)
			{
				cancelSource.Cancel();
			}

			bool caught = false;

			StringTask task = webClient.GetStringAsync(GET_URL, cancelSource.Token);

			if (!beforeRequest)
			{
				cancelSource.Cancel();
			}

			try
			{
				await task;
			}
			catch (TaskCanceledException)
			{
				caught = true;
			}

			// Can't really use Assert.ThrowsAsync here since it freezes Unity. So we have to do it manually. ¯\_(ツ)_/¯
			Assert.That(caught, Is.True);
		}
	}
}
#endif // DISABLE_GAMEJOLT