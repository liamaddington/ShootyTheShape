using Microsoft.Xna.Framework.Graphics;
using ShootyTheShape.Enums;
using ShootyTheShape.Managers;
using ShootyTheShape.Services.Content;
using ShootyTheShape.Services.Rendering;

namespace ShootyTheShape.GameModes.WaveMode;

internal class WaveGameModeHud : IHud
{
	private string _killCountLabel { get; } = "Kills";
	private string _waveCountLabel { get; } = "Wave";
	private string _remainingLivesLabel { get; } = "Lives Remaining";
	private string _timerLabel { get; } = "Time";

	private Vector2 _killCountPosition { get; } = new Vector2(GameRoot.Graphics.PreferredBackBufferWidth / 2, 30);
	private Vector2 _waveCountPosition { get; } = new Vector2(GameRoot.Graphics.PreferredBackBufferWidth / 2, GameRoot.Graphics.PreferredBackBufferHeight - 50);
	private Vector2 _remainingLivesPosition { get; } = new Vector2(GameRoot.Graphics.PreferredBackBufferWidth - 280, 30);
	private Vector2 _timerPosition { get; } = new Vector2(50, 30);

	/*Debug Values - These will only show if the Debug setting is set to true*/
	private string debugEntityCountLabel = "Entity count";
	private Vector2 _debugEntityCountPosition { get; } = new Vector2(GameRoot.Graphics.PreferredBackBufferWidth - 450, 100);

	private string debugEnemyCountLabel = "Enemy count";
	private Vector2 _debugEnemyCountPosition { get; } = new Vector2(GameRoot.Graphics.PreferredBackBufferWidth - 450, 130);

	private string debugBulletCountLabel = "WeaponFire count";
	private Vector2 _debugBulletCountPosition { get; } = new Vector2(GameRoot.Graphics.PreferredBackBufferWidth - 450, 160);

	private IRenderService _renderService { get; }
	public SpriteFont Font { get; set; }
	public float FontScale { get; set; }
	public bool Visible { get; set; } = true;

	private bool IsBossWave { get; }
	private int BossTotalHP { get; }

	private IContentService _contentService { get; }

	public WaveGameModeHud(bool IsBossWave, int BossTotalHP, IContentService contentService, IRenderService renderService)
	{
		this.IsBossWave = IsBossWave;
		this.BossTotalHP = BossTotalHP;

		_renderService = renderService;
		_contentService = contentService;
		Font = _contentService.GetFont(FontStyles.Default);
		this.FontScale = .7f;
	}

	public void Draw()
	{
		if (!IsBossWave)
		{
			_renderService.DrawString(Font,
				$"{_killCountLabel}: {CurrentGameStats.KillCounter} / {WaveHudStats.KillsUntilNextWave}",
				_killCountPosition, Color.White, 0, Vector2.Zero, FontScale, SpriteEffects.None, 0);
		}
		else
		{
			_renderService.DrawString(Font,
				$"{_killCountLabel}: {WaveHudStats.BossCurrentHP} / {BossTotalHP}",
				_killCountPosition, Color.White, 0, Vector2.Zero, FontScale, SpriteEffects.None, 0);
		}

		_renderService.DrawString(Font, $"{_waveCountLabel}: {WaveHudStats.CurrentWave} / {WaveHudStats.WaveCount}",
			_waveCountPosition, Color.White, 0, Vector2.Zero, FontScale, SpriteEffects.None, 0);

		_renderService.DrawString(Font, $"{_remainingLivesLabel}: {CurrentGameStats.RemainingLives}",
			_remainingLivesPosition, Color.White, 0, Vector2.Zero, FontScale, SpriteEffects.None, 0);

		_renderService.DrawString(Font,
			$"{_timerLabel}: {CurrentGameStats.GameTimer.Elapsed.Minutes}m:{CurrentGameStats.GameTimer.Elapsed.Seconds}s",
			_timerPosition, Color.White, 0, Vector2.Zero, FontScale, SpriteEffects.None, 0);

		if (GameRoot.DebugModeStats)
		{
			_renderService.DrawString(Font, $"{debugEntityCountLabel}: {EntityManager.EntityCount}",
				_debugEntityCountPosition, Color.White, 0, Vector2.Zero, FontScale, SpriteEffects.None, 0);

			_renderService.DrawString(Font, $"{debugEnemyCountLabel}: {EntityManager.EnemyCount}",
				_debugEnemyCountPosition, Color.White, 0, Vector2.Zero, FontScale, SpriteEffects.None, 0);

			_renderService.DrawString(Font, $"{debugBulletCountLabel}: {EntityManager.BulletCount}",
				_debugBulletCountPosition, Color.White, 0, Vector2.Zero, FontScale, SpriteEffects.None, 0);
		}
	}
}
