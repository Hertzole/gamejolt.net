#if !DISABLE_GAMEJOLT // Disables all GameJolt-related code

#if UNITY_EDITOR
using System.Threading.Tasks;
using GameJolt.NET.Tests.Attributes;
using Hertzole.GameJolt;
using NUnit.Framework;

namespace GameJolt.NET.Tests.Unity.Editor
{
	[SkipInitialization]
	public class GameJoltInitializationTest : BaseEditorTest
	{
#if UNITY_INCLUDE_TESTS
		protected override Task OnSetupAsync()
		{
			// Makes sure the initialization DOES run even if we're running tests.
			GameJoltInitialization.ReturnIfTestIsRunning = false;
			return base.OnSetupAsync();
		}

		protected override Task OnTearDownAsync()
		{
			GameJoltInitialization.ReturnIfTestIsRunning = true;
			return base.OnTearDownAsync();
		}
#endif // UNITY_INCLUDE_TESTS

		[Test]
		public async Task EnterPlayMode_Initializes()
		{
			GameJoltSettings.AutoInitialize = true;
			
			// Act
			await EnterPlayModeAsync();
			
			// Assert
			Assert.That(GameJoltInitialization.manager, Is.Not.Null);
			Assert.That(GameJoltInitialization.manager.hasInitialized, Is.True);
		}
		
		[Test]
		public async Task ExitPlayMode_Disposes()
		{
			GameJoltSettings.AutoInitialize = true;
			
			// Act
			await EnterPlayModeAsync();
			await ExitPlayModeAsync();
			
			// Assert
			Assert.That(GameJoltInitialization.manager, Is.Null);
			Assert.That(GameJoltInitialization.exitTokenSource.IsCancellationRequested, Is.True);
		}
	}
}
#endif // UNITY_EDITOR
#endif // DISABLE_GAMEJOLT