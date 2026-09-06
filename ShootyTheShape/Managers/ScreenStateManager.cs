using ShootyTheShape.Enums;
using ShootyTheShape.Menus.MainMenu;
using ShootyTheShape.Services.Rendering;

namespace ShootyTheShape.Managers;

public class ScreenStateManager
{
	public static GameState CurrentGameState = GameState.Playing;

	private MainMenu mainMenu;

	private IRenderService renderService;

	public ScreenStateManager(MainMenu mainMenu)
	{
		this.mainMenu = mainMenu;
		this.renderService = (IRenderService)GameRoot.ServiceProvider.GetService(typeof(IRenderService));
	}

	public void Update()
	{
		renderService.StartRenderer();
		if (CurrentGameState == GameState.Playing)
		{
			EntityManager.Update();
			LevelManager.Update();
			EntityManager.Draw();
		}
		else if (CurrentGameState == GameState.Paused)
		{
			//Create the pause Menu
		}
		else if (CurrentGameState == GameState.MainMenu)
		{
			mainMenu.Update();
			mainMenu.Draw();
		}
		renderService.StopRenderer();
	}
}
