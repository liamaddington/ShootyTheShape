using ShootyTheShape.Entities.Player;
using ShootyTheShape.Enums;
using ShootyTheShape.Menus.MainMenu;
using ShootyTheShape.Runtime;
using ShootyTheShape.Services.Rendering;

namespace ShootyTheShape.Managers;

public class ScreenStateManager
{
	private GameSession _gameSession { get; }
	private MainMenu _mainMenu { get; }
	private IRenderService _renderService { get; }
	private LevelManager _levelManager { get; }

	public ScreenStateManager(GameSession gameSession, MainMenu mainMenu, IRenderService renderService, LevelManager levelManager)
	{
		_gameSession = gameSession;
		_mainMenu = mainMenu;
		_renderService = renderService;
		_levelManager = levelManager;
	}

	public void Update()
	{
		_renderService.StartRenderer();
		if (_gameSession.CurrentGameState == GameState.Playing)
		{
			EntityManager.Update();
			_levelManager.Update();
			_renderService.SetCameraPosition(PlayerShip.Instance.Position);
			EntityManager.Draw();
		}
		else if (_gameSession.CurrentGameState == GameState.Paused)
		{
			//Create the pause Menu
		}
		else if (_gameSession.CurrentGameState == GameState.MainMenu)
		{
			_mainMenu.Update();
			_mainMenu.Draw();
		}
		_renderService.StopRenderer();
	}
}
