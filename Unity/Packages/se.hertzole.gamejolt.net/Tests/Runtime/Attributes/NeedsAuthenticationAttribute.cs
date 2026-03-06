#if !DISABLE_GAMEJOLT
using System;
using NUnit.Framework;

namespace GameJolt.NET.Tests.Attributes
{
	[AttributeUsage(AttributeTargets.Class | AttributeTargets.Method)]
	public sealed class NeedsAuthenticationAttribute : PropertyAttribute { }
}
#endif // DISABLE_GAMEJOLT