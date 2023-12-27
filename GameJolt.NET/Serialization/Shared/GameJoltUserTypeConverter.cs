using System;

namespace Hertzole.GameJolt
{
	internal sealed class GameJoltUserTypeConverter : GameJoltEnumConverter<UserType>
	{
		protected override bool GetValueFromString(string value, out UserType result)
		{
			if (value.Equals("user", StringComparison.OrdinalIgnoreCase))
			{
				result = UserType.User;
				return true;
			}

			if (value.Equals("developer", StringComparison.OrdinalIgnoreCase))
			{
				result = UserType.Developer;
				return true;
			}

			if (value.Equals("moderator", StringComparison.OrdinalIgnoreCase))
			{
				result = UserType.Moderator;
				return true;
			}

			if (value.Equals("administrator", StringComparison.OrdinalIgnoreCase))
			{
				result = UserType.Administrator;
				return true;
			}

			result = default;
			return false;
		}

		protected override bool GetValueFromInt(int value, out UserType result)
		{
			switch (value)
			{
				case 0:
					result = UserType.User;
					return true;
				case 1:
					result = UserType.Developer;
					return true;
				case 2:
					result = UserType.Moderator;
					return true;
				case 3:
					result = UserType.Administrator;
					return true;
				default:
					result = default;
					return false;
			}
		}
	}
}