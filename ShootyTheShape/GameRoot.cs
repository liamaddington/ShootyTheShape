using Microsoft.Xna.Framework.Graphics;
using ShootyTheShape.Runtime;

namespace ShootyTheShape;

public class GameRoot : Game
{
	// some helpful static properties
	public static Viewport Viewport { get { return Graphics.GraphicsDevice.Viewport; } }
	public static Vector2 ScreenSize { get { return new Vector2(Viewport.Width, Viewport.Height); } }
	public static GameTime GameTime { get; private set; }

	public static GraphicsDeviceManager Graphics;

	public static bool DebugModeStats = true;

	public static bool NoEnemyMode = true;

	private GameBootstrapper _bootstrapper;

	public GameRoot()
	{
		Graphics = new GraphicsDeviceManager(this);

		Graphics.IsFullScreen = false;

		//All textures should be created on the expectation of a 1920x1080 resolution
		//Scaling will be handled inside the RenderService
		Graphics.PreferredBackBufferWidth = 1920;
		Graphics.PreferredBackBufferHeight = 1080;
	}

	protected override void Initialize()
	{
		base.Initialize();

		GraphicsDevice.DepthStencilState = DepthStencilState.Default;

		this.IsMouseVisible = true;
		var gameSession = new GameSession();
		_bootstrapper = new GameBootstrapper(this, gameSession);
		_bootstrapper.Initialize();
	}

	protected override void Update(GameTime gameTime)
	{
		GameTime = gameTime;
		_bootstrapper.Input.Update();

		// Allows the game to exit
		if (_bootstrapper.Input.ExitGame())
		{
			this.Exit();
		}

		_bootstrapper.ScreenStateManager.Update();

		base.Update(gameTime);
	}

	protected override void Dispose(bool disposing)
	{
		if (disposing)
		{
			_bootstrapper?.Dispose();
		}

		base.Dispose(disposing);
	}

}
