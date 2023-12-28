using System;

namespace Hertzole.GameJolt
{
	internal sealed class GameJoltStatusConverter : GameJoltEnumConverter<UserStatus>
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