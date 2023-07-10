using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;
using ShootyTheShape.Entities.Player;
using ShootyTheShape.Entities.Projectiles.Enums;
using ShootyTheShape.Enums;
using ShootyTheShape.Managers;
using ShootyTheShape.Menus.MainMenu;
using ShootyTheShape.Services.Audio;
using ShootyTheShape.Services.Input;
using ShootyTheShape.Services.Rendering;
using ShootyTheShape.Services.Spawning;
using System;
using TheSymbioticShip.Services;

namespace ShootyTheShape;
public class GameRoot : Game
{
	// some helpful static properties
	public static GameRoot Instance { get; private set; }
	public static Viewport Viewport { get { return Instance.GraphicsDevice.Viewport; } }
	public static Vector2 ScreenSize { get { return new Vector2(Viewport.Width, Viewport.Height); } }
	public static GameTime GameTime { get; private set; }

	public static GraphicsDeviceManager graphics;

	public static GameState gameState;

	private IRenderService renderService;

	private string contentRoot = @"Content";

	IContentService contentService;
	IAudioService audioService;

	public static bool DebugModeStats = true;

	public static bool NoEnemyMode = true;

	public static IServiceProvider ServiceProvider;

	private IInputService input;

	private ScreenStateManager screenStateManager;

	public GameRoot()
	{
		Instance = this;
		graphics = new GraphicsDeviceManager(this);

		graphics.IsFullScreen = false;

		//All textures should be created on the expectation of a 1920x1080 resolution
		//Scaling will be handled inside the RenderService
		graphics.PreferredBackBufferWidth = 1920;
		graphics.PreferredBackBufferHeight = 1080;
	}

	protected override void Initialize()
	{
		base.Initialize();

		//Key sub-systems that should throw compile error if ever their parameters do not exist
		this.contentService = new ContentService(this.Services, contentRoot);
		this.audioService = new AudioService(this.Services, contentRoot);
		ISpawnService spawnService = new SpawnService();

		GraphicsDevice.DepthStencilState = DepthStencilState.Default;

		PresentationParameters pp = GraphicsDevice.PresentationParameters;
		renderService = new RenderService(this,
			GraphicsDevice,
			pp.BackBufferWidth,
			pp.BackBufferHeight,
			false,
			pp.BackBufferFormat,
			DepthFormat.Depth24,
			new SpriteBatch(GraphicsDevice));

		//Components used to throughout the entire lifetime of the game
		this.input = new InputService(this);

		//Add services to the GameServiceProvider object
		Services.AddService(typeof(IContentService), contentService);
		Services.AddService(typeof(IAudioService), audioService);
		Services.AddService(typeof(IRenderService), renderService);
		Services.AddService(typeof(ISpawnService), spawnService);
		Services.AddService(typeof(IInputService), input);

		//Instantiate global IServiceProvider using the assigned services above
		ServiceProvider = this.Services;

		this.IsMouseVisible = true;

		contentService.LoadBackgrounds();
		contentService.LoadButtons();
		contentService.LoadFonts();
		contentService.LoadHudElements();
		contentService.LoadBulletType(BulletTypes.Bullet);
		audioService.LoadBgm();
		audioService.LoadSfx();

		EntityManager.Add(PlayerShip.Instance);

		SoundEffect.MasterVolume = 0.5f;

		MediaPlayer.IsRepeating = true;
		MediaPlayer.Volume = 0.5f;

		LevelManager.LoadCurrentLevel();

		this.screenStateManager = new ScreenStateManager(new MainMenu());

	}

	protected override void Update(GameTime gameTime)
	{
		GameTime = gameTime;
		input.Update();

		// Allows the game to exit
		if (input.ExitGame())
			this.Exit();

		screenStateManager.Update();

		base.Update(gameTime);
	}

	public static void ResetGameInstance()
	{
		EntityManager.RemoveAllEnemies();
		EntityManager.RemoveAllBosses();
		CurrentGameStats.KillCounter = 0;
		CurrentGameStats.RemainingLives = 3;
		CurrentGameStats.BossWave = false;
		CurrentGameStats.GameTimer.Restart();
	}
}
