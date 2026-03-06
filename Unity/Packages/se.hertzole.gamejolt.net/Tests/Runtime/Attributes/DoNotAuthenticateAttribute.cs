#if !DISABLE_GAMEJOLT
using System;
using NUnit.Framework;

namespace GameJolt.NET.Tests.Attributes
{
	[AttributeUsage(AttributeTargets.Method)]
	public sealed class DoNotAuthenticateAttribute : PropertyAttribute { }
}
#endif