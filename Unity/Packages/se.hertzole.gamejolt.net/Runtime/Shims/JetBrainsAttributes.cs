#if !GAMEJOLT_UNITY && !DISABLE_GAMEJOLT // These attributes are available in Unity, so we compile this out in Unity.
using System;
using System.Diagnostics;

namespace JetBrains.Annotations
{
	[AttributeUsage(AttributeTargets.Parameter)]
	[Conditional("JETBRAINS_ANNOTATIONS")]
	internal sealed class NoEnumerationAttribute : Attribute { }
}
#endif