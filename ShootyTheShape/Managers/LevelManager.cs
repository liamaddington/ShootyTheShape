using ShootyTheShape.Levels;
using ShootyTheShape.Levels.NG.Aratif.WaveLevels;
using ShootyTheShape.Runtime;
using ShootyTheShape.Services.Content;
using ShootyTheShape.Services.Rendering;
using ShootyTheShape.Services.Spawning;

namespace ShootyTheShape.Managers;

public class LevelManager
{
	private ISpawnService _spawnService { get; }
	private IContentService _contentService { get; }
	private IRenderService _renderService { get; }
	private GameSession _gameSession { get; }

	public int CurrentLevel { get; set; } = 1;
	public ILevel CurrentLoadedLevel { get; private set; }

	public LevelManager(
		ISpawnService spawnService,
		IContentService contentService,
		IRenderService renderService,
		GameSession gameSession)
	{
		_spawnService = spawnService;
		_contentService = contentService;
		_renderService = renderService;
		_gameSession = gameSession;
	}

	public void LoadCurrentLevel()
	{
		switch (CurrentLevel)
		{
			case 1://TODO Add more levels
				CurrentLoadedLevel = new AratifWaveMission1(_spawnService, _contentService, _renderService, _gameSession);
				break;
			case 2:
				CurrentLoadedLevel = new AratifWaveMission1(_spawnService, _contentService, _renderService, _gameSession);
				break;
			case 3:
				break;
			case 4:
				break;
			case 5:
				break;
			case 6:
				break;
			case 7:
				break;
		}
	}
	public void Update()
	{
		CurrentLoadedLevel.Update();
		CurrentLoadedLevel.DrawHud();
	}
}
