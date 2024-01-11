<div align="center">
<img src="https://github.com/Hertzole/gamejolt.net/assets/5569364/c2574e9a-2ab8-4e35-8c00-994f6cf07fd7" alt="GameJolt.NET Logo">
<h4>A modern C# wrapper around the GameJolt Game API for .NET and Unity</h4>
<a href="https://gamejolt.com/game-api"><img src="https://img.shields.io/badge/Game_Jolt_API-v1.2-%23CCFF00?style=flat&logo=gamejolt"></a>
<br>
<a href="https://github.com/sponsors/Hertzole"><img src="https://img.shields.io/badge/Sponsor_me-GitHub-%23EA4AAA?style=flat&logo=githubsponsors" alt="Sponsor me on github badge"></a>
<a href="https://ko-fi.com/Hertzole"><img src="https://img.shields.io/badge/Support_me-Ko--fi-%23FF5E5B?style=flat&logo=ko-fi" alt="Support me on ko-fi badge"></a>
<br>
<a href="https://sonarcloud.io/project/overview?id=hertzole_gamejolt-net"><img src="https://sonarcloud.io/api/project_badges/measure?project=hertzole_gamejolt-net&metric=sqale_rating" alt="Sonarcloud maintainability"></a>
<a href="https://sonarcloud.io/project/overview?id=hertzole_gamejolt-net"><img src="https://sonarcloud.io/api/project_badges/measure?project=hertzole_gamejolt-net&metric=bugs" alt="Sonarcloud bugs"></a>
<a href="https://sonarcloud.io/project/overview?id=hertzole_gamejolt-net"><img src="https://sonarcloud.io/api/project_badges/measure?project=hertzole_gamejolt-net&metric=code_smells" alt="Sonarcloud code smells"></a>
<a href="https://sonarcloud.io/project/overview?id=hertzole_gamejolt-net"><img src="https://sonarcloud.io/api/project_badges/measure?project=hertzole_gamejolt-net&metric=coverage" alt="Sonarcloud code coverage"></a>
</div>

## 🔨 Getting Started

### Result Pattern

The API uses the result pattern for calls. This means that all calls return a `GameJoltResult` object. This object contains a `HasError` property that tells you if the call was successful or not. If it was successful, you can access the result through the `Value` property. If it was not successful, you can access the error through the `Exception` property.

**All the methods return a `GameJoltResult` object. Some have values, some don't.**

It can be used like this:

```csharp
GameJoltResult<GameJoltUser> result = await GameJoltAPI.Users.GetUserAsync(123456);

if (result.HasError)
{
    // Something went wrong.
    Console.WriteLine(result.Exception);
}
else
{
    // Do something with the user.
    GameJoltUser user = result.Value;
}
```

### Initialization and Shutdown

Before doing anything, you need to initialize the API. This is done by calling `GameJolt.Initialize` and passing in your game ID and private key. You can find these on your game's dashboard on GameJolt.

```csharp
int gameId = 123456;
string privateKey = "abcdefghijklmnopqrstuvwxyz1234567890";
GameJoltAPI.Initialize(gameId, privateKey);
```

You should also shut down the API when you're done with it. This is done by calling `GameJolt.Shutdown`.

```csharp
// Call this when your game quits.
GameJoltAPI.Shutdown();
```

> [!IMPORTANT]
> Sessions are not handled automatically for you in the base API. You need to open and close them manually. See [sessions](#sessions)

### Authenticate

For most calls, you need to be authenticated. If you're not authenticated and try to call an authenticated method, you will get an `GameJoltAuthorizationException` in the result.

#### Authenticate with username and token

```csharp
GameJoltResult result = await GameJoltAPI.Users.AuthenticateAsync("username", "token");

if (result.HasError)
{
    // Something went wrong.
    Console.WriteLine(result.Exception);
}
else
{
    // We're authenticated!
}
```

#### Authenticate with URL

You can authenticate using a URL. This is useful for when you have a web build and want to authenticate the user using the browser. Game Jolt will provide a URL that has gjapi_username and gjapi_token as query parameters. You can pass that URL to the API and it will authenticate the user.

```csharp
GameJoltResult result = await GameJoltAPI.Users.AuthenticateFromUrlAsync("url");

if (result.HasError)
{
    // Something went wrong.
    Console.WriteLine(result.Exception);
}
else
{
    // We're authenticated!
}
```

#### Authenticate with credentials file

You can authenticate using a credentials file. When a player starts your application using the Game Jolt client, a credentials file will be created. You can pass that file to the API and it will authenticate the user.

```csharp
string credentialsContent = File.ReadAllText(".gj-credentials");
GameJoltResult result = await GameJoltAPI.Users.AuthenticateFromCredentialsFileAsync(credentialsContent);

if (result.HasError)
{
    // Something went wrong.
    Console.WriteLine(result.Exception);
}
else
{
    // We're authenticated!
}
```

### Data Store

The data store is a simple key-value store. You can store strings, integers, floats and booleans. This API also helps you store byte arrays as base64 strings.

#### Set data

```csharp
// Set string
await GameJoltAPI.DataStore.SetAsync("key", "value");
// Set int
await GameJoltAPI.DataStore.SetAsync("key", 123);
// Set bool
await GameJoltAPI.DataStore.SetAsync("key", true);
// Set byte array
await GameJoltAPI.DataStore.SetAsync("key", new byte[] { 1, 2, 3, 4, 5 });
```

All these methods also have a variant with the current user that will automatically use the authenticated user as the owner of the data.

#### Get Data

```csharp
// Get string
GameJoltResult<string> stringResult = await GameJoltAPI.DataStore.GetValueAsStringAsync("key");
// Get int
GameJoltResult<int> intResult = await GameJoltAPI.DataStore.GetValueAsIntAsync("key");
// Get bool
GameJoltResult<bool> boolResult = await GameJoltAPI.DataStore.GetValueAsBoolAsync("key");
// Get byte array
GameJoltResult<byte[]> byteArrayResult = await GameJoltAPI.DataStore.GetValueAsByteArrayAsync("key");
```

All these methods also have a variant with the current user that will automatically use the authenticated user as the owner of the data.

#### Update Data

```csharp
// Update string
GameJoltResult<string> stringResult = await GameJoltAPI.DataStore.UpdateAsync("key", "value", StringOperation.Append);
// Update int
GameJoltResult<int> intResult = await GameJoltAPI.DataStore.UpdateAsync("key", 123, NumericOperation.Add);
```

All these methods also have a variant with the current user that will automatically use the authenticated user as the owner of the data.

#### Remove Data

```csharp
// Remove data
await GameJoltAPI.DataStore.RemoveAsync("key");
```

The method also has a variant with the current user that will automatically use the authenticated user as the owner of the data.

#### Get Keys

```csharp
// Get all keys
GameJoltResult<string[]> result = await GameJoltAPI.DataStore.GetKeysAsync();
// With a pattern
GameJoltResult<string[]> result = await GameJoltAPI.DataStore.GetKeysAsync("custom_level_*");
```

The method also has a variant with the current user that will automatically use the authenticated user as the owner of the data.

### Friends

You can get a list of friends for the current user.

#### Get Friends

```csharp
// Get all friends as IDs
GameJoltResult<int[]> result = await GameJoltAPI.Friends.FetchAsync();
```

### Scores

Use scores to create leaderboards for your game.

#### Submit Score

```csharp
int sort = 123;
string score = sort + " coins";
string extraData = "Level 1"; // Optional

// As the current user
await GameJoltAPI.Scores.SubmitScoreAsync(sort, score, extraData);
// As a guest
await GameJoltAPI.Scores.SubmitScoreAsync("Guest Name", sort, score, extraData);
```

#### Get Scores

You can get scores using a query struct. Inside this struct, you can set options for filtering the scores.

```csharp
// Get all scores
GameJoltResult<GameJoltScore[]> result = await GameJoltAPI.Scores.QueryScores().Limit(100).GetAsync();
// Get all scores for a table
GameJoltResult<GameJoltScore[]> result = await GameJoltAPI.Scores.QueryScores().ForTable(123).GetAsync();
// Get all scores for current user
GameJoltResult<GameJoltScore[]> result = await GameJoltAPI.Scores.QueryScores().ForCurrentUser().GetAsync();
```

There are more options to set, but to keep this short, I won't list them all here.

#### Get Score Tables

```csharp
// Get all score tables
GameJoltResult<GameJoltScoreTable[]> result = await GameJoltAPI.Scores.GetTablesAsync();
```

#### Get Score Rank
    
```csharp
// Get rank for score
int tableId = 123;
int score = 123;
GameJoltResult<int> result = await GameJoltAPI.Scores.GetRankAsync(tableId, score);
```

### Sessions

Sessions tell Game Jolt that a user is playing your game right now. They are not handled automatically for you in the base API. You need to open, ping, and close them manually.

#### Open Session

```csharp
// Open a session for the current user
await GameJoltAPI.Sessions.OpenAsync();
```

#### Ping Session

```csharp
// Ping a session for the current user
SessionStatus status = SessionStatus.Active;
await GameJoltAPI.Sessions.PingAsync(status);
```

#### Close Session

```csharp
// Close a session for the current user
await GameJoltAPI.Sessions.CloseAsync();
```

#### Check Session

```csharp
// Check if a session is open for the current user according to the server
GameJoltResult<bool> result = await GameJoltAPI.Sessions.CheckAsync();
// Check if a session is open for the current user according to the client
bool isOpen = GameJoltAPI.Sessions.IsSessionOpen;
```

### Time

You can get the server time from Game Jolt.

#### Get Time

```csharp
// Get the server time
GameJoltResult<DateTime> result = await GameJoltAPI.Time.GetAsync();
// Get the time zone
TimeZoneInfo timeZone = GameJoltAPI.Time.TimeZone;
```

### Trophies

Use trophies to create achievements for your game.

#### Get Trophies

```csharp
// Get all trophies
GameJoltResult<GameJoltTrophy[]> result = await GameJoltAPI.Trophies.GetTrophiesAsync();
// Get all achieved trophies for current user
bool achieved = true;
GameJoltResult<GameJoltTrophy[]> result = await GameJoltAPI.Trophies.GetTrophisAsync(achieved);
// Get trophies by ID
int[] ids = new int[] { 123, 456, 789 };
GameJoltResult<GameJoltTrophy[]> result = await GameJoltAPI.Trophies.GetTrophiesAsync(ids);
// Get single trophy by ID
GameJoltResult<GameJoltTrophy> result = await GameJoltAPI.Trophies.GetTrophyAsync(123);
```

#### Unlock Trophy

```csharp
// Unlock a trophy for the current user
await GameJoltAPI.Trophies.UnlockTrophyAsync(123);
```

#### Remove Trophy

```csharp
// Remove a trophy for the current user
await GameJoltAPI.Trophies.RemoveUnlockedTrophyAsync(123);
```

### Users

You can get information about users and authenticate the current user.

#### Authenticate

See [authenticate](#authenticate) higher up.

#### Get User Information

```csharp
// Get a user by username
GameJoltResult<GameJoltUser> result = await GameJoltAPI.Users.GetUserAsync("username");
// Get a user by ID
GameJoltResult<GameJoltUser> result = await GameJoltAPI.Users.GetUserAsync(123456);
// Get multiple users by usernames
string[] usernames = new string[] { "username1", "username2", "username3" };
GameJoltResult<GameJoltUser[]> result = await GameJoltAPI.Users.GetUsersAsync(usernames);
// Get multiple users by IDs
int[] ids = new int[] { 123456, 789012, 345678 };
GameJoltResult<GameJoltUser[]> result = await GameJoltAPI.Users.GetUsersAsync(ids);
```

## 📦 Installation

### Unity

#### OpenUPM (Recommended)

The minimum Unity version for GameJolt.NET is 2021.1.

You can install the package through [OpenUPM](https://openupm.com/) by using the [OpenUPM CLI](https://github.com/openupm/openupm-cli#openupm-cli).

```bash
openupm add se.hertzole.gamejolt.net
```

If you don't have the CLI installed, you can follow these steps:

1. Open Edit/Project Settings/Package Manager
2. Add a new Scoped Registry (or edit the existing OpenUPM entry)   
     Name: `package.openupm.com`  
     URL: `https://package.openupm.com`  
     Scope: `se.hertzole.gamejolt.net`
3. Click `Save` (or `Apply`)
4. Open Window/Package Manager
5. Click `+`
6. Select `Add package by name...` or `Add package from git URL...`
7. Paste `se.hertzole.gamejolt.net` into name 
8. Click `Add`

### .NET Project

You can install the package through NuGet. GameJolt.NET supports .NET Standard 2.0/2.1 and .NET 5.0+.

```bash
dotnet add package GameJolt.Net
```

## 💻 Development

### Requirements

For standard .NET development, you need the following:
- [.NET 8.0 SDK](https://dotnet.microsoft.com/en-us/download/dotnet/8.0)

For Unity development, you need the following:
- [Unity 2022.3.15f1](https://unity.com/releases/editor/whats-new/2022.3.15)

### Building

To build the project, you can use the `dotnet` CLI.

```bash
dotnet build
```

### Testing

To run the tests, you can use the `dotnet` CLI.

```bash
dotnet test
```

### Unity

The main SDK should be the "single source of truth". This means that all code should be written in the main project and then copied over to the Unity project. The only part that is exclusive to the Unity project is the `Editor` folder which contains the editor scripts.

To open the project in Unity, you need to open the `Unity` folder as a project.

## 🤝 Contributing

Contributions, issues and feature requests are welcome!

Please make sure your pull requests are made to the `develop` branch and that you have tested your changes. If you're adding a new feature, please also add tests for it.

Your code should follow the [C# Coding Conventions](https://docs.microsoft.com/en-us/dotnet/csharp/programming-guide/inside-a-program/coding-conventions). Your commits should follow the [Conventional Commits](https://www.conventionalcommits.org/en/v1.0.0/) standard.

## 📃 License

[MIT](https://github.com/Hertzole/gamejolt.net/blob/master/LICENSE)
