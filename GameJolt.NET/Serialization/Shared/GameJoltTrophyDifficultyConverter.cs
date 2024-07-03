using System;
#if NET6_0_OR_GREATER || FORCE_SYSTEM_JSON
using BaseConverter = Hertzole.GameJolt.Serialization.System.GameJoltEnumConverter<Hertzole.GameJolt.TrophyDifficulty>;
#else
using BaseConverter = Hertzole.GameJolt.Serialization.Newtonsoft.GameJoltEnumConverter<Hertzole.GameJolt.TrophyDifficulty>;
#endif

namespace Hertzole.GameJolt.Serialization.Shared
{
	internal sealed class GameJoltTrophyDifficultyConverter : BaseConverter
	{
		public static readonly GameJoltTrophyDifficultyConverter Instance = new GameJoltTrophyDifficultyConverter();

		protected override bool GetValueFromString(string value, out TrophyDifficulty result)
		{
			if (value.Equals("bronze", StringComparison.OrdinalIgnoreCase))
			{
				result = TrophyDifficulty.Bronze;
				return true;
			}

			if (value.Equals("silver", StringComparison.OrdinalIgnoreCase))
			{
				result = TrophyDifficulty.Silver;
				return true;
			}

			if (value.Equals("gold", StringComparison.OrdinalIgnoreCase))
			{
				result = TrophyDifficulty.Gold;
				return true;
			}

			if (value.Equals("platinum", StringComparison.OrdinalIgnoreCase))
			{
				result = TrophyDifficulty.Platinum;
				return true;
			}

			result = default;
			return false;
		}

		protected override bool GetValueFromInt(int value, out TrophyDifficulty result)
		{
			switch (value)
			{
				case 0:
					result = TrophyDifficulty.Bronze;
					return true;
				case 1:
					result = TrophyDifficulty.Silver;
					return true;
				case 2:
					result = TrophyDifficulty.Gold;
					return true;
				case 3:
					result = TrophyDifficulty.Platinum;
					return true;
				default:
					result = default;
					return false;
			}
		}
	}
}