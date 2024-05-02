using System;
#if NET6_0_OR_GREATER
using BaseConverter = Hertzole.GameJolt.Serialization.System.GameJoltEnumConverter<Hertzole.GameJolt.UserStatus>;
#else
using BaseConverter = Hertzole.GameJolt.Serialization.Newtonsoft.GameJoltEnumConverter<Hertzole.GameJolt.UserStatus>;
#endif

namespace Hertzole.GameJolt.Serialization.Shared
{
	internal sealed class GameJoltStatusConverter : BaseConverter
	{
		public static readonly GameJoltStatusConverter Instance = new GameJoltStatusConverter();

		protected override bool GetValueFromString(string value, out UserStatus result)
		{
			if (value.Equals("active", StringComparison.OrdinalIgnoreCase))
			{
				result = UserStatus.Active;
				return true;
			}

			if (value.Equals("banned", StringComparison.OrdinalIgnoreCase))
			{
				result = UserStatus.Banned;
				return true;
			}

			result = default;
			return false;
		}

		protected override bool GetValueFromInt(int value, out UserStatus result)
		{
			switch (value)
			{
				case 0:
					result = UserStatus.Active;
					return true;
				case 10:
					result = UserStatus.Banned;
					return true;
				default:
					result = default;
					return false;
			}
		}
	}
}