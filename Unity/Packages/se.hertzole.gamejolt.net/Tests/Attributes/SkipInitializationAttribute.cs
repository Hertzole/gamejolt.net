using System;
using NUnit.Framework;

namespace GameJolt.NET.Tests.Attributes
{
	[AttributeUsage(AttributeTargets.Class | AttributeTargets.Method)]
	public sealed class SkipInitializationAttribute : PropertyAttribute { }
}