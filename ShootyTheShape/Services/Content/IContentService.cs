using System.Collections.Generic;
using Microsoft.Xna.Framework.Graphics;
using ShootyTheShape.Entities.Enemies.Enums;
using ShootyTheShape.Entities.Projectiles.Enums;
using ShootyTheShape.Enums;
using ShootyTheShape.Menus;
using ShootyTheShape.Menus.MainMenu.Enums;

namespace ShootyTheShape.Services.Content;

public interface IContentService
{
	Texture2D GetPlayerShipTexture();
	void LoadEnemies(IList<EnemyName> enemy);
	void LoadBulletType(BulletTypes bullet);
	void LoadButtons();

	void LoadBackgrounds();
	Texture2D GetBackground(Backgrounds background);
	void LoadHudElements();
	void LoadFonts();
	SpriteFont GetFont(FontStyles font);
	Texture2D GetEnemyTexture(EnemyName enemy);
	Texture2D GetBulletTexture(BulletTypes bullet);
	Texture2D GetButtonTexture(UiButtons button);

	void UnloadAssets();
}
