using System.Collections.Generic;
using System.Linq;
using ShootyTheShape.Enums;
using ShootyTheShape.GameModes.WaveMode;
using ShootyTheShape.Managers;

namespace ShootyTheShape.GameModes.WaveGameMode;

internal class WaveGameMode : GameModeBaseClass
{
	private List<EnemyWave> waves { get; }
	private List<EnemyWave> completedWaves { get; }
	private EnemyWave currentWave { get; set; }

	public override IHud Hud { get; set; }

	public WaveGameMode(IList<EnemyWave> waves)
	{
		this.waves = waves.ToList();
		this.currentWave = this.waves.FirstOrDefault();
		this.completedWaves = new List<EnemyWave>();

		this.Hud = new WaveGameModeHud(currentWave.IsBossWave, 500);
		EnableHud();

		WaveHudStats.WaveCount = waves.Count;
		UpdateHudData();

		base.LoadSpawnService();
		base.LoadContentService();

		base.SpawnService.PopulateSpawnList(currentWave.EnemySpawnObjects);
		base.SpawnService.PopulateBossSpawnList(currentWave.BossSpawnObjects);
		base.ContentService.LoadEnemies(currentWave.EnemySpawnObjects.Select(x => x.EnemyType).ToList());

		if (currentWave.BossSpawnObjects != null)
		{
			base.ContentService.LoadEnemies(currentWave.BossSpawnObjects.Select(x => x.EnemyType).ToList());
		}

		base.SpawnService.SpawnBossesUsingSpawnList();

		CurrentGameStats.GameTimer.Start();
	}

	public override void Update()
	{
		CheckAndHandleWinCondition();
		CheckAndHandleLoseCondition();

		SpawnEnemies();
	}

	public override void DrawHud()
	{
		Hud.Draw();
	}

	private void SpawnEnemies()
	{
		if (currentWave.SpawnLimit > EntityManager.EnemyCount)
		{
			base.SpawnService.SpawnEnemiesUsingSpawnLists();
		}

		if (currentWave.BossSpawnLimit > EntityManager.TotalBossCount)
		{
			base.SpawnService.SpawnBossesUsingSpawnList();
		}
	}

	public override void CheckAndHandleWinCondition()
	{
		if (!currentWave.IsBossWave)
		{
			if (currentWave.KillsUntilNextWave <= CurrentGameStats.KillCounter)
			{
				ProgressToNextWaveOrScreen();
			}

			return;
		}

		if (currentWave.KillsUntilNextWave <= CurrentGameStats.BossKillCounter)
		{
			ProgressToNextWaveOrScreen();
		}
	}

	public override void CheckAndHandleLoseCondition()
	{
		if (CurrentGameStats.RemainingLives < 0)
		{
			ResetGameMode();
			GameRoot.ResetGameInstance();
			if (currentWave.IsBossWave)
			{
				base.SpawnService.SpawnBossesUsingSpawnList();
			}
		}
	}

	public void ProgressToNextWaveOrScreen()
	{
		completedWaves.Add(waves[0]);
		waves.RemoveAt(0);
		GameRoot.ResetGameInstance();

		if (NoMoreWavesLeft())
		{
			DisableHud(); //TODO Also unload the component from the Game components list
			CurrentGameStats.GameTimer.Stop();
			//TODO: Create wave completed screen showing stats and scoresHandle wave completion screen
			ScreenStateManager.CurrentGameState = GameState.MainMenu;
			return;
		}

		currentWave = waves.First();

		UpdateHudData();

		base.SpawnService.PopulateSpawnList(currentWave.EnemySpawnObjects);
		base.SpawnService.PopulateBossSpawnList(currentWave.BossSpawnObjects);
		base.ContentService.LoadEnemies(currentWave.EnemySpawnObjects.Select(x => x.EnemyType).ToList());
	}

	private void UpdateHudData()
	{
		WaveHudStats.CurrentWave++;
		WaveHudStats.KillsUntilNextWave = currentWave.KillsUntilNextWave;
		WaveHudStats.BossCurrentHP = 0;
	}
	public override void EnableHud()
	{
		Hud.Visible = true;
	}

	public override void DisableHud()
	{
		Hud.Visible = false;
	}

	private bool NoMoreWavesLeft()
	{
		return waves.Count == 0;
	}

	public override void ResetGameMode()
	{
		foreach (var completedWave in completedWaves)
		{
			waves.Add(completedWave);
		}
		waves.OrderBy(w => w.WaveNumber);
		completedWaves.Clear();
		CurrentGameStats.GameTimer.Reset();
		CurrentGameStats.KillCounter = 0;
		CurrentGameStats.RemainingLives = 3;
	}
}
