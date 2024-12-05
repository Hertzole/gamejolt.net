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
		protected override Task OnSetupAsync()
		{
			// Makes sure the initialization DOES run even if we're running tests.
			GameJoltInitialization.ReturnIfTestRunning = false;
			return base.OnSetupAsync();
		}

		protected override Task OnTearDownAsync()
		{
			GameJoltInitialization.ReturnIfTestRunning = true;
			return base.OnTearDownAsync();
		}

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