using System;
using Microsoft.Xna.Framework.Graphics;
using ShootyTheShape.Enums;
using ShootyTheShape.Menus.MainMenu.Enums;
using ShootyTheShape.Services.Content;
using ShootyTheShape.Services.Input;
using ShootyTheShape.Services.Rendering;

namespace ShootyTheShape.Menus.MainMenu;

public class MainMenu
{
	private static Vector2 StartButtonLocation = new Vector2(50, 850);
	private static Vector2 ExitButtonLocation = new Vector2(550, 850);

	//TODO: Remove direct reference to contentService depending on if there is an alternative to static managers
	private static IContentService contentService = (IContentService)GameRoot.ServiceProvider.GetService(typeof(IContentService));
	private static IRenderService renderService = (IRenderService)GameRoot.ServiceProvider.GetService(typeof(IRenderService));

	private static Rectangle StartGameRectangle = new Rectangle(
			(int)StartButtonLocation.X, (int)StartButtonLocation.Y,
			contentService.GetButtonTexture(UiButtons.StartGame).Width,
			contentService.GetButtonTexture(UiButtons.StartGame).Height);

	private static Rectangle ExitGameRectangle = new Rectangle(
			(int)ExitButtonLocation.X, (int)ExitButtonLocation.Y,
			contentService.GetButtonTexture(UiButtons.ExitGame).Width,
			contentService.GetButtonTexture(UiButtons.ExitGame).Height);

	private IInputService inputService;

	public MainMenu()
	{
		inputService = (IInputService)GameRoot.ServiceProvider.GetService(typeof(IInputService));
	}

	public void Update()
	{
		if (IsStartGameHovered() && inputService.PrimaryFire())
		{
			GameRoot.CurrentGameState = GameState.Playing;
			CurrentGameStats.GameTimer.Restart();
		}

		if (IsExitGameHovered() && inputService.PrimaryFire())
		{
			GameRoot.Instance.Exit();
		}
	}

	private bool IsStartGameHovered()
	{
		return StartGameRectangle.Contains(inputService.MousePosition);
	}

	private bool IsExitGameHovered()
	{
		return ExitGameRectangle.Contains(inputService.MousePosition);
	}

	static Vector2 BestTimePosition = new Vector2(GameRoot.Graphics.PreferredBackBufferWidth - 500, GameRoot.Graphics.PreferredBackBufferHeight - 80);

	internal void Draw()
	{
		//Draw elements for the main menu
		renderService.Draw(contentService.GetBackground(Backgrounds.MainMenu), Vector2.Zero, Color.White);

		if (IsStartGameHovered())
		{
			renderService.Draw(contentService.GetButtonTexture(UiButtons.StartGameHovered),
				StartButtonLocation, Color.White);

			renderService.Draw(contentService.GetButtonTexture(UiButtons.ExitGame),
			ExitButtonLocation, Color.White);
		}
		else if (IsExitGameHovered())
		{
			renderService.Draw(contentService.GetButtonTexture(UiButtons.ExitGameHovered),
			ExitButtonLocation, Color.White);

			renderService.Draw(contentService.GetButtonTexture(UiButtons.StartGame),
			StartButtonLocation, Color.White);
		}
		else
		{
			renderService.Draw(contentService.GetButtonTexture(UiButtons.ExitGame),
			ExitButtonLocation, Color.White);

			renderService.Draw(contentService.GetButtonTexture(UiButtons.StartGame),
				StartButtonLocation, Color.White);
		}

		if (CurrentGameStats.BestTime != TimeSpan.Zero)
		{
			renderService.DrawString(contentService.GetFont(FontStyles.Default), "Best Time: " + CurrentGameStats.BestTime.Minutes + "m:" + CurrentGameStats.BestTime.Seconds + "s",
			BestTimePosition, Color.White, 0, Vector2.Zero, 1.2f, SpriteEffects.None, 0);
		}
	}
}
