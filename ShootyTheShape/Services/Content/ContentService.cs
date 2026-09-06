using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using ShootyTheShape.Entities.Enemies.Enums;
using ShootyTheShape.Entities.Projectiles.Enums;
using ShootyTheShape.Enums;
using ShootyTheShape.Menus;
using ShootyTheShape.Menus.MainMenu.Enums;

namespace ShootyTheShape.Services.Content;

public class ContentService : ContentManager, IContentService
{
	//Textures used while playing in the levels
	//TODO: Have all content implementation access textures by IDictionaries to allow content loading/unloading and
	//fast hashtable value accessors
	public Dictionary<EnemyName, Texture2D> EnemyTextures = new();

	public Dictionary<BulletTypes, Texture2D> BulletTextures = new();
	public Dictionary<UiButtons, Texture2D> ButtonTextures = new();

	public Dictionary<Backgrounds, Texture2D> BackgroundTextures = new();

	public Dictionary<FontStyles, SpriteFont> Fonts = new();

	//Enemies
	public Texture2D Player { get; set; }
	public Texture2D Seeker { get; set; }
	public Texture2D Dasher { get; set; }
	public Texture2D Wanderer { get; set; }
	public Texture2D Boss { get; set; }
	public Texture2D Bullet { get; set; }
	public Texture2D LevelOneBackground { get; set; }

	//HUD
	public Texture2D Score { get; set; }
	public Texture2D WaveNumber { get; set; }
	public Texture2D LivesRemaining { get; set; }
	public Texture2D BossHp { get; set; }

	//Menu textures
	public Texture2D MainMenuBackground { get; set; }
	public Texture2D MainMenuStartGameButtonNotHovered { get; set; }
	public Texture2D MainMenuStartGameButtonHovered { get; set; }
	public Texture2D MainMenuExitGameButtonNotHovered { get; set; }
	public Texture2D MainMenuExitGameButtonHovered { get; set; }

	//Misceleneous Textures
	public Texture2D Pointer { get; set; }

	public SpriteFont Font { get; set; }

	private const string contentPath = "Art/Final Art/";

	public ContentService(IServiceProvider serviceProvider, string contentRoot)
		: base(serviceProvider, contentRoot)
	{
	}

	public void LoadButtons()
	{
		ButtonTextures.Add(UiButtons.ExitGame, base.Load<Texture2D>("Art/Final Art/ExitButton"));
		ButtonTextures.Add(UiButtons.ExitGameHovered, base.Load<Texture2D>("Art/Final Art/ExitButtonHovered"));
		ButtonTextures.Add(UiButtons.StartGame, base.Load<Texture2D>("Art/Final Art/StartGameButton"));
		ButtonTextures.Add(UiButtons.StartGameHovered, base.Load<Texture2D>("Art/Final Art/StartGameButtonHovered"));
	}

	public void LoadBackgrounds()
	{
		BackgroundTextures.Add(Backgrounds.MainMenu, base.Load<Texture2D>("Art/Final Art/MainMenuBackground"));
	}

	public void LoadHudElements()
	{
	}

	public void LoadFonts()
	{
		Fonts.Add(FontStyles.Default, base.Load<SpriteFont>("MainFont"));
	}

	public void LoadEnemies(IList<EnemyName> enemiesToLoad)
	{
		foreach (var enemy in enemiesToLoad)
		{
			LoadEnemy(enemy);
		}
	}

	private void LoadEnemy(EnemyName enemyName)
	{
		try
		{
			EnemyTextures.Add(enemyName, base.Load<Texture2D>(contentPath + enemyName.ToString()));
		}
		catch (Exception e)
		{
			Console.WriteLine("Failed to load enemy texture as it does not exist:" + e);
		}
	}

	public Texture2D GetEnemyTexture(EnemyName enemy)
	{
		if (EnemyTextures.TryGetValue(enemy, out Texture2D texture))
		{
			return texture;
		}

		return LoadPlaceholderTexture();
	}

	private Texture2D LoadPlaceholderTexture()
	{
		return base.Load<Texture2D>(contentPath + "brokenTexture");
	}

	public void UnloadAssets() => base.Unload(); //TODO: Look into how Unload works, and if it's possible to unload specific assets

	public void LoadBulletType(BulletTypes bullet)
	{
		BulletTextures.Add(bullet, base.Load<Texture2D>(contentPath + bullet.ToString()));
	}

	public Texture2D GetBulletTexture(BulletTypes bullet)
	{
		BulletTextures.TryGetValue(bullet, out Texture2D texture);
		return texture;
	}

	public Texture2D GetPlayerShipTexture()
	{
		return base.Load<Texture2D>(contentPath + "playership");
	}

	public Texture2D GetBackground(Backgrounds background)
	{
		BackgroundTextures.TryGetValue(background, out Texture2D backgroundTexture);
		return backgroundTexture;
	}

	public Texture2D GetButtonTexture(UiButtons button)
	{
		ButtonTextures.TryGetValue(button, out Texture2D texture);
		return texture;
	}

	public SpriteFont GetFont(FontStyles font)
	{
		Fonts.TryGetValue(font, out SpriteFont fontStyle);
		return fontStyle;
	}
}
