#if !DISABLE_GAMEJOLT
namespace System.Diagnostics.CodeAnalysis
{
#if !NETSTANDARD2_1_OR_GREATER && !NET5_0_OR_GREATER && !UNITY_2021_3_OR_NEWER
	[AttributeUsage(AttributeTargets.Field | AttributeTargets.Parameter | AttributeTargets.Property | AttributeTargets.ReturnValue)]
	internal sealed class NotNullAttribute : Attribute { }

	[AttributeUsage(AttributeTargets.Method, Inherited = false)]
	internal sealed class DoesNotReturnAttribute : Attribute { }

	[AttributeUsage(AttributeTargets.Parameter)]
	internal sealed class NotNullWhenAttribute : Attribute
	{
		public bool ReturnValue { get; }

		public NotNullWhenAttribute(bool returnValue)
		{
			ReturnValue = returnValue;
		}
	}
#endif // !NETSTANDARD2_1_OR_GREATER && !NET5_0_OR_GREATER && !UNITY_2021_3_OR_NEWER

#if !NET5_0_OR_GREATER
	[AttributeUsage(AttributeTargets.Method | AttributeTargets.Property, Inherited = false, AllowMultiple = true)]
	internal sealed class MemberNotNullWhenAttribute : Attribute
	{
		public bool ReturnValue { get; }

		public string[] Members { get; }

		public MemberNotNullWhenAttribute(bool returnValue, string member)
		{
			ReturnValue = returnValue;
			Members = new[] { member };
		}

		public MemberNotNullWhenAttribute(bool returnValue, params string[] members)
		{
			ReturnValue = returnValue;
			Members = members;
		}
	}

	[AttributeUsage(AttributeTargets.Method | AttributeTargets.Property, Inherited = false, AllowMultiple = true)]
	internal sealed class MemberNotNullAttribute : Attribute
	{
		public string[] Members { get; }

		public MemberNotNullAttribute(string member)
		{
			Members = new[] { member };
		}

		public MemberNotNullAttribute(params string[] members)
		{
			Members = members;
		}
	}
#endif // !NET5_0_OR_GREATER
}
#endif // !DISABLE_GAMEJOLT