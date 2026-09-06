using Microsoft.Xna.Framework.Graphics;
using ShootyTheShape.Runtime;

namespace ShootyTheShape;

public class GameRoot : Game
{
	private const int ArenaScreenCount = 3;

	// some helpful static properties
	public static Viewport Viewport { get { return Graphics.GraphicsDevice.Viewport; } }
	public static Vector2 ScreenSize { get { return new Vector2(Viewport.Width, Viewport.Height); } }
	public static Vector2 ArenaSize { get; private set; }
	public static Rectangle ArenaBounds => new Rectangle(0, 0, (int)ArenaSize.X, (int)ArenaSize.Y);
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
		ArenaSize = new Vector2(
			Graphics.PreferredBackBufferWidth * ArenaScreenCount,
			Graphics.PreferredBackBufferHeight * ArenaScreenCount);
	}

	public static Vector2 ClampToArena(Vector2 position, Vector2 halfObjectSize)
	{
		return Vector2.Clamp(position, halfObjectSize, ArenaSize - halfObjectSize);
	}

	public static bool IsOutsideArena(Vector2 position, float radius)
	{
		return position.X - radius < ArenaBounds.Left ||
			position.X + radius > ArenaBounds.Right ||
			position.Y - radius < ArenaBounds.Top ||
			position.Y + radius > ArenaBounds.Bottom;
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
