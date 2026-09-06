using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Media;
using ShootyTheShape.Entities.Player;
using ShootyTheShape.Entities.Projectiles.Enums;
using ShootyTheShape.Managers;
using ShootyTheShape.Menus.MainMenu;
using ShootyTheShape.Services.Audio;
using ShootyTheShape.Services.Content;
using ShootyTheShape.Services.Input;
using ShootyTheShape.Services.Rendering;
using ShootyTheShape.Services.Spawning;

namespace ShootyTheShape.Runtime;

public sealed class GameBootstrapper
{
	private const string ContentRoot = "Content";

	private GameRoot _game { get; }

	public IInputService Input { get; private set; }
	public ScreenStateManager ScreenStateManager { get; private set; }

	public GameBootstrapper(GameRoot game)
	{
		_game = game;
	}

	public void Initialize(GameSession gameSession)
	{
		IContentService contentService = new ContentService(_game.Services, ContentRoot);
		IAudioService audioService = new AudioService(_game.Services, ContentRoot);
		IRenderService renderService = CreateRenderService();
		Input = new InputService(_game);
		ISpawnService spawnService = new SpawnService(contentService, audioService, renderService);

		LoadAssets(contentService, audioService);
		ConfigureAudio();

		var playerShip = PlayerShip.Create(contentService, audioService, renderService, Input);
		EntityManager.Add(playerShip);

		var levelManager = new LevelManager(spawnService, contentService, renderService, gameSession);
		levelManager.LoadCurrentLevel();

		var mainMenu = new MainMenu(Input, contentService, renderService, gameSession, _game.Exit);
		ScreenStateManager = new ScreenStateManager(gameSession, mainMenu, renderService, levelManager);
	}

	private IRenderService CreateRenderService()
	{
		PresentationParameters presentationParameters = _game.GraphicsDevice.PresentationParameters;

		return new RenderService(
			_game,
			_game.GraphicsDevice,
			presentationParameters.BackBufferWidth,
			presentationParameters.BackBufferHeight,
			false,
			presentationParameters.BackBufferFormat,
			DepthFormat.Depth24,
			new SpriteBatch(_game.GraphicsDevice));
	}

	private static void LoadAssets(IContentService contentService, IAudioService audioService)
	{
		contentService.LoadBackgrounds();
		contentService.LoadButtons();
		contentService.LoadFonts();
		contentService.LoadHudElements();
		contentService.LoadBulletType(BulletTypes.Bullet);
		audioService.LoadBgm();
		audioService.LoadSfx();
	}

	private static void ConfigureAudio()
	{
		SoundEffect.MasterVolume = 0.5f;
		MediaPlayer.IsRepeating = true;
		MediaPlayer.Volume = 0.5f;
	}
}
