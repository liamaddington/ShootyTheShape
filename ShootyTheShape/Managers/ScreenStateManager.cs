using ShootyTheShape.Enums;
using ShootyTheShape.Menus.MainMenu;
using ShootyTheShape.Services.Rendering;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShootyTheShape.Managers;
public class ScreenStateManager
{
	public static GameState currentGameState = GameState.playing;

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
		if (currentGameState == GameState.playing)
		{
			EntityManager.Update();
			LevelManager.Update();
			EntityManager.Draw();
		}
		else if (currentGameState == GameState.paused)
		{
			//Create the pause Menu
		}
		else if (currentGameState == GameState.mainMenu)
		{
			mainMenu.Update();
			mainMenu.Draw();
		}
		renderService.StopRenderer();
	}
}
