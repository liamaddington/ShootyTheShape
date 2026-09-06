using System;
using Microsoft.Xna.Framework.Graphics;
using ShootyTheShape.Enums;
using ShootyTheShape.Menus.MainMenu.Enums;
using ShootyTheShape.Runtime;
using ShootyTheShape.Services.Content;
using ShootyTheShape.Services.Input;
using ShootyTheShape.Services.Rendering;

namespace ShootyTheShape.Menus.MainMenu;

public class MainMenu
{
	private Vector2 _startButtonLocation { get; } = new Vector2(50, 850);
	private Vector2 _exitButtonLocation { get; } = new Vector2(550, 850);
	private Vector2 _bestTimePosition { get; } = new Vector2(GameRoot.Graphics.PreferredBackBufferWidth - 500, GameRoot.Graphics.PreferredBackBufferHeight - 80);
	private IContentService _contentService { get; }
	private IRenderService _renderService { get; }
	private IInputService _inputService { get; }
	private GameSession _gameSession { get; }
	private Action _exitGame { get; }

	private Rectangle StartGameRectangle => CreateButtonRectangle(_startButtonLocation, UiButtons.StartGame);
	private Rectangle ExitGameRectangle => CreateButtonRectangle(_exitButtonLocation, UiButtons.ExitGame);

	public MainMenu(IInputService inputService, IContentService contentService, IRenderService renderService, GameSession gameSession, Action exitGame)
	{
		_inputService = inputService;
		_contentService = contentService;
		_renderService = renderService;
		_gameSession = gameSession;
		_exitGame = exitGame;
	}

	public void Update()
	{
		if (IsStartGameHovered() && _inputService.PrimaryFire())
		{
			_gameSession.CurrentGameState = GameState.Playing;
			CurrentGameStats.GameTimer.Restart();
		}

		if (IsExitGameHovered() && _inputService.PrimaryFire())
		{
			_exitGame();
		}
	}

	internal void Draw()
	{
		_renderService.Draw(_contentService.GetBackground(Backgrounds.MainMenu), Vector2.Zero, Color.White);
		DrawButtons();
		DrawBestTime();
	}

	private Rectangle CreateButtonRectangle(Vector2 location, UiButtons button)
	{
		Texture2D texture = _contentService.GetButtonTexture(button);
		return new Rectangle((int)location.X, (int)location.Y, texture.Width, texture.Height);
	}

	private bool IsStartGameHovered()
	{
		return StartGameRectangle.Contains(_inputService.MousePosition);
	}

	private bool IsExitGameHovered()
	{
		return ExitGameRectangle.Contains(_inputService.MousePosition);
	}

	private void DrawButtons()
	{
		UiButtons startButton = IsStartGameHovered() ? UiButtons.StartGameHovered : UiButtons.StartGame;
		UiButtons exitButton = IsExitGameHovered() ? UiButtons.ExitGameHovered : UiButtons.ExitGame;

		_renderService.Draw(_contentService.GetButtonTexture(startButton), _startButtonLocation, Color.White);
		_renderService.Draw(_contentService.GetButtonTexture(exitButton), _exitButtonLocation, Color.White);
	}

	private void DrawBestTime()
	{
		if (CurrentGameStats.BestTime == TimeSpan.Zero)
		{
			return;
		}

		_renderService.DrawString(
			_contentService.GetFont(FontStyles.Default),
			$"Best Time: {CurrentGameStats.BestTime.Minutes}m:{CurrentGameStats.BestTime.Seconds}s",
			_bestTimePosition,
			Color.White,
			0,
			Vector2.Zero,
			1.2f,
			SpriteEffects.None,
			0);
	}
}
