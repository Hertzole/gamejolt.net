#if !DISABLE_GAMEJOLT
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Hertzole.GameJolt;
using NUnit.Framework;

namespace GameJolt.NET.Tests
{
	partial class UsersTest
	{
		[Test]
		public async Task Authenticate_Guards()
		{
			// Null username
			await AssertThrowsAsync<ArgumentNullException>(() => GameJoltAPI.Users.AuthenticateAsync(null!, "token"),
				e => MustNotBeNullPredicate<string>(e, "username"));

			// Empty username
			await AssertThrowsAsync<ArgumentException>(() => GameJoltAPI.Users.AuthenticateAsync(string.Empty, "token"),
				e => MustNotBeEmptyOrWhitespacePredicate(e, "username"));

			// Null token
			await AssertThrowsAsync<ArgumentNullException>(() => GameJoltAPI.Users.AuthenticateAsync("username", null!),
				e => MustNotBeNullPredicate<string>(e, "token"));

			// Empty token
			await AssertThrowsAsync<ArgumentException>(() => GameJoltAPI.Users.AuthenticateAsync("username", string.Empty),
				e => MustNotBeEmptyOrWhitespacePredicate(e, "token"));
		}

		[Test]
		public async Task AuthenticateFromUrl_String_Guards()
		{
			// Null URL
			await AssertThrowsAsync<ArgumentNullException>(() => GameJoltAPI.Users.AuthenticateFromUrlAsync(((string) null)!),
				e => MustNotBeNullPredicate<string>(e, "url"));

			// Empty URL
			await AssertThrowsAsync<ArgumentException>(() => GameJoltAPI.Users.AuthenticateFromUrlAsync(string.Empty),
				e => MustNotBeEmptyOrWhitespacePredicate(e, "url"));
		}

		[Test]
		public async Task AuthenticateFromUrl_Uri_Guards()
		{
			// Null URL
			await AssertThrowsAsync<ArgumentNullException>(() => GameJoltAPI.Users.AuthenticateFromUrlAsync(((Uri) null)!),
				e => MustNotBeNullPredicate<Uri>(e, "url"));
		}

		[Test]
		public async Task AuthenticateFromCredentialsFile_String_Guards()
		{
			// Null content
			await AssertThrowsAsync<ArgumentNullException>(() => GameJoltAPI.Users.AuthenticateFromCredentialsFileAsync(((string) null)!),
				e => MustNotBeNullPredicate<string>(e, "gjCredentialsContent"));

			// Empty content
			await AssertThrowsAsync<ArgumentException>(() => GameJoltAPI.Users.AuthenticateFromCredentialsFileAsync(string.Empty),
				e => MustNotBeEmptyOrWhitespacePredicate(e, "gjCredentialsContent"));
		}

		[Test]
		public async Task AuthenticateFromCredentialsFile_Array_Guards()
		{
			// Null array
			await AssertThrowsAsync<ArgumentNullException>(() => GameJoltAPI.Users.AuthenticateFromCredentialsFileAsync(((string[]) null)!),
				e => MustNotBeNullPredicate<IReadOnlyCollection<string>>(e, "lines"));

			// Empty array
			await AssertThrowsAsync<ArgumentException>(() => GameJoltAPI.Users.AuthenticateFromCredentialsFileAsync(Array.Empty<string>()),
				e => MustNotBeEmptyPredicate<IReadOnlyCollection<string>>(e, "lines"));

			// Must be >= 3 lines
			string[] lines = { "line1", "line" };
			await AssertThrowsAsync<ArgumentException>(() => GameJoltAPI.Users.AuthenticateFromCredentialsFileAsync(lines),
				e => GreaterThanOrEqualToPredicate<IReadOnlyCollection<string>>(e, "lines", 3, lines.Length));
		}

		[Test]
		public async Task GetUser_Guards()
		{
			// Null name
			await AssertThrowsAsync<ArgumentNullException>(() => GameJoltAPI.Users.GetUserAsync(null!),
				e => MustNotBeNullPredicate<string>(e, "username"));

			// Empty name
			await AssertThrowsAsync<ArgumentException>(() => GameJoltAPI.Users.GetUserAsync(string.Empty),
				e => MustNotBeEmptyOrWhitespacePredicate(e, "username"));
		}

		[Test]
		public async Task GetUsers_Usernames_Guards()
		{
			// Null usernames
			await AssertThrowsAsync<ArgumentNullException>(() => GameJoltAPI.Users.GetUsersAsync(((string[]) null)!),
				e => MustNotBeNullPredicate<IEnumerable<string>>(e, "usernames"));
		}

		[Test]
		public async Task GetUsers_Usernames_WithList_Guards()
		{
			// Null usernames
			await AssertThrowsAsync<ArgumentNullException>(() => GameJoltAPI.Users.GetUsersAsync(((string[]) null)!, new List<GameJoltUser>()),
				e => MustNotBeNullPredicate<IEnumerable<string>>(e, "usernames"));

			// Null results
			await AssertThrowsAsync<ArgumentNullException>(() => GameJoltAPI.Users.GetUsersAsync(new[] { "username" }, null!),
				e => MustNotBeNullPredicate<IList<GameJoltUser>>(e, "results"));
		}

		[Test]
		public async Task GetUsers_Ids_Guards()
		{
			// Null IDs
			await AssertThrowsAsync<ArgumentNullException>(() => GameJoltAPI.Users.GetUsersAsync(((int[]) null)!),
				e => MustNotBeNullPredicate<IEnumerable<int>>(e, "userIds"));
		}

		[Test]
		public async Task GetUsers_Ids_WithList_Guards()
		{
			// Null IDs
			await AssertThrowsAsync<ArgumentNullException>(() => GameJoltAPI.Users.GetUsersAsync(((int[]) null)!, new List<GameJoltUser>()),
				e => MustNotBeNullPredicate<IEnumerable<int>>(e, "userIds"));

			// Null results
			await AssertThrowsAsync<ArgumentNullException>(() => GameJoltAPI.Users.GetUsersAsync(new[] { 1 }, null!),
				e => MustNotBeNullPredicate<IList<GameJoltUser>>(e, "results"));
		}
	}
}
#endif