using System;
using Microsoft.Extensions.DependencyInjection;
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
	private IServiceProvider _serviceProvider { get; }

	public IInputService Input { get; private set; }
	public ScreenStateManager ScreenStateManager { get; private set; }

	public GameBootstrapper(GameRoot game, GameSession gameSession)
	{
		_game = game;
		_serviceProvider = BuildServiceProvider(gameSession);
	}

	public void Initialize()
	{
		IContentService contentService = _serviceProvider.GetRequiredService<IContentService>();
		IAudioService audioService = _serviceProvider.GetRequiredService<IAudioService>();
		IRenderService renderService = _serviceProvider.GetRequiredService<IRenderService>();
		IInputService inputService = _serviceProvider.GetRequiredService<IInputService>();

		LoadAssets(contentService, audioService);
		ConfigureAudio();

		var playerShip = PlayerShip.Create(
			contentService,
			audioService,
			renderService,
			inputService);
		EntityManager.Add(playerShip);
		renderService.SetCameraPosition(playerShip.Position);

		var levelManager = _serviceProvider.GetRequiredService<LevelManager>();
		levelManager.LoadCurrentLevel();

		Input = _serviceProvider.GetRequiredService<IInputService>();
		ScreenStateManager = _serviceProvider.GetRequiredService<ScreenStateManager>();
	}

	public void Dispose()
	{
		if (_serviceProvider is IDisposable disposableServiceProvider)
		{
			disposableServiceProvider.Dispose();
		}
	}

	private IServiceProvider BuildServiceProvider(GameSession gameSession)
	{
		var services = new ServiceCollection();
		services.AddSingleton(gameSession);
		services.AddSingleton<IContentService>(_ => new ContentService(_game.Services, ContentRoot));
		services.AddSingleton<IAudioService>(_ => new AudioService(_game.Services, ContentRoot));
		services.AddSingleton<IRenderService>(_ => CreateRenderService());
		services.AddSingleton<IInputService>(serviceProvider => new InputService(
			_game,
			serviceProvider.GetRequiredService<IRenderService>()));
		services.AddSingleton<ISpawnService, SpawnService>();
		services.AddSingleton<LevelManager>();
		services.AddSingleton<MainMenu>(serviceProvider => new MainMenu(
			serviceProvider.GetRequiredService<IInputService>(),
			serviceProvider.GetRequiredService<IContentService>(),
			serviceProvider.GetRequiredService<IRenderService>(),
			serviceProvider.GetRequiredService<GameSession>(),
			_game.Exit));
		services.AddSingleton<ScreenStateManager>();

		return services.BuildServiceProvider();
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
