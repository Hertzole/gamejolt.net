#if !DISABLE_GAMEJOLT // Disables all GameJolt-related code

using System.Runtime.CompilerServices;

[assembly: InternalsVisibleTo("GameJolt.NET.Tests")]
[assembly: InternalsVisibleTo("GameJolt.NET.Tests.Editor")]
[assembly: InternalsVisibleTo("GameJolt.NET.Editor")]
[assembly: InternalsVisibleTo("DynamicProxyGenAssembly2")]
#endif // DISABLE_GAMEJOLT