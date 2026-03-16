#if !DISABLE_GAMEJOLT
#if !NETSTANDARD2_1_OR_GREATER && !NET5_0_OR_GREATER
namespace System.Diagnostics.CodeAnalysis
{
	[AttributeUsage(AttributeTargets.Field | AttributeTargets.Parameter | AttributeTargets.Property | AttributeTargets.ReturnValue)]
	internal sealed class NotNullAttribute : Attribute { }

	[AttributeUsage(AttributeTargets.Method, Inherited = false)]
	internal sealed class DoesNotReturnAttribute : Attribute { }
}
#endif // !NETSTANDARD2_1_OR_GREATER && !NET5_0_OR_GREATER
#endif // !DISABLE_GAMEJOLT