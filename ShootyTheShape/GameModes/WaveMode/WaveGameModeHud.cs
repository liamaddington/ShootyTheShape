using Microsoft.Xna.Framework.Graphics;
using ShootyTheShape.Enums;
using ShootyTheShape.Managers;
using ShootyTheShape.Services.Rendering;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TheSymbioticShip.Services;

namespace ShootyTheShape.GameModes.WaveMode;
public static class WaveHudStats
{
	public static int KillsUntilNextWave = 0;
	public static int WaveCount = 0;
	public static int CurrentWave = 0;

	public static int BossCurrentHP = 0;

}

class WaveGameModeHud : IHud
{
	private readonly string killCountLabel = "Kills";
	private readonly string waveCountLabel = "Wave";
	private readonly string remainingLivesLabel = "Lives Remaining";
	private readonly string timerLabel = "Time";

	private readonly Vector2 killCountPosition = new Vector2(GameRoot.graphics.PreferredBackBufferWidth / 2, 30);
	private readonly Vector2 waveCountPosition = new Vector2(GameRoot.graphics.PreferredBackBufferWidth / 2, GameRoot.graphics.PreferredBackBufferHeight - 50);
	private readonly Vector2 remainingLivesPosition = new Vector2(GameRoot.graphics.PreferredBackBufferWidth - 280, 30);
	private readonly Vector2 timerPosition = new Vector2(50, 30);


	/*Debug Values - These will only show if the Debug setting is set to true*/
	private string debugEntityCountLabel = "Entity count";
	private readonly Vector2 debugEntityCountPosition = new Vector2(GameRoot.graphics.PreferredBackBufferWidth - 450, 100);

	private string debugEnemyCountLabel = "Enemy count";
	private readonly Vector2 debugEnemyCountPosition = new Vector2(GameRoot.graphics.PreferredBackBufferWidth - 450, 130);

	private string debugBulletCountLabel = "WeaponFire count";
	private readonly Vector2 debugBulletCountPosition = new Vector2(GameRoot.graphics.PreferredBackBufferWidth - 450, 160);


	private IRenderService renderService { get; }
	public SpriteFont Font { get; set; }
	public float FontScale { get; set; }
	public bool Visable { get; set; } = true;

	private bool IsBossWave { get; }
	private int BossTotalHP { get; }

	private IContentService contentService;

	public WaveGameModeHud(bool IsBossWave, int BossTotalHP)
	{
		this.IsBossWave = IsBossWave;
		this.BossTotalHP = BossTotalHP;

		this.renderService = (IRenderService)GameRoot.ServiceProvider.GetService(typeof(IRenderService));

		this.contentService = (IContentService)GameRoot.ServiceProvider.GetService(typeof(IContentService));

		this.Font = contentService.GetFont(FontStyles.Default);
		this.FontScale = .7f;

	}


	public void Draw()
	{
		if (!IsBossWave)
		{
			renderService.DrawString(Font,
				$"{killCountLabel}: {CurrentGameStats.KillCounter} / {WaveHudStats.KillsUntilNextWave}",
				killCountPosition, Color.White, 0, Vector2.Zero, FontScale, SpriteEffects.None, 0);
		}
		else
		{
			renderService.DrawString(Font,
				$"{killCountLabel}: {WaveHudStats.BossCurrentHP} / {BossTotalHP}",
				killCountPosition, Color.White, 0, Vector2.Zero, FontScale, SpriteEffects.None, 0);
		}

		renderService.DrawString(Font, $"{waveCountLabel}: {WaveHudStats.CurrentWave} / {WaveHudStats.WaveCount}",
			waveCountPosition, Color.White, 0, Vector2.Zero, FontScale, SpriteEffects.None, 0);

		renderService.DrawString(Font, $"{remainingLivesLabel}: {CurrentGameStats.RemainingLives}",
			remainingLivesPosition, Color.White, 0, Vector2.Zero, FontScale, SpriteEffects.None, 0);

		renderService.DrawString(Font,
			$"{timerLabel}: {CurrentGameStats.GameTimer.Elapsed.Minutes}m:{CurrentGameStats.GameTimer.Elapsed.Seconds}s",
			timerPosition, Color.White, 0, Vector2.Zero, FontScale, SpriteEffects.None, 0);

		if (GameRoot.DebugModeStats)
		{
			renderService.DrawString(Font, $"{debugEntityCountLabel}: {EntityManager.EntityCount}",
				debugEntityCountPosition, Color.White, 0, Vector2.Zero, FontScale, SpriteEffects.None, 0);

			renderService.DrawString(Font, $"{debugEnemyCountLabel}: {EntityManager.EnemyCount}",
				debugEnemyCountPosition, Color.White, 0, Vector2.Zero, FontScale, SpriteEffects.None, 0);

			renderService.DrawString(Font, $"{debugBulletCountLabel}: {EntityManager.BulletCount}",
				debugBulletCountPosition, Color.White, 0, Vector2.Zero, FontScale, SpriteEffects.None, 0);
		}

	}
}