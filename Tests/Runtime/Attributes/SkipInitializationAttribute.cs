#if !DISABLE_GAMEJOLT // Disables all GameJolt-related code

using System;
using NUnit.Framework;

namespace GameJolt.NET.Tests.Attributes
{
	[AttributeUsage(AttributeTargets.Class | AttributeTargets.Method)]
	public sealed class SkipInitializationAttribute : PropertyAttribute { }
}
#endif // DISABLE_GAMEJOLT