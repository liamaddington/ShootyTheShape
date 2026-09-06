using System.Collections.Generic;
using System.Linq;
using ShootyTheShape.Enums;
using ShootyTheShape.GameModes.WaveMode;
using ShootyTheShape.Managers;
using ShootyTheShape.Runtime;
using ShootyTheShape.Services.Content;
using ShootyTheShape.Services.Rendering;
using ShootyTheShape.Services.Spawning;

namespace ShootyTheShape.GameModes.WaveGameMode;

internal class WaveGameMode : GameModeBaseClass
{
	private List<EnemyWave> waves { get; }
	private List<EnemyWave> completedWaves { get; }
	private EnemyWave currentWave { get; set; }

	public override IHud Hud { get; set; }

	public WaveGameMode(
		IList<EnemyWave> waves,
		ISpawnService spawnService,
		IContentService contentService,
		IRenderService renderService,
		GameSession gameSession)
		: base(spawnService, contentService, renderService, gameSession)
	{
		this.waves = waves.ToList();
		this.currentWave = this.waves.FirstOrDefault();
		this.completedWaves = new List<EnemyWave>();

		this.Hud = new WaveGameModeHud(currentWave.IsBossWave, 500, contentService, renderService);
		EnableHud();

		WaveHudStats.WaveCount = waves.Count;
		UpdateHudData();

		SpawnService.PopulateSpawnList(currentWave.EnemySpawnObjects);
		SpawnService.PopulateBossSpawnList(currentWave.BossSpawnObjects);
		ContentService.LoadEnemies(currentWave.EnemySpawnObjects.Select(x => x.EnemyType).ToList());

		if (currentWave.BossSpawnObjects != null)
		{
			ContentService.LoadEnemies(currentWave.BossSpawnObjects.Select(x => x.EnemyType).ToList());
		}

		SpawnService.SpawnBossesUsingSpawnList();

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
			SpawnService.SpawnEnemiesUsingSpawnLists();
		}

		if (currentWave.BossSpawnLimit > EntityManager.TotalBossCount)
		{
			SpawnService.SpawnBossesUsingSpawnList();
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
			GameSession.Reset();
			if (currentWave.IsBossWave)
			{
				SpawnService.SpawnBossesUsingSpawnList();
			}
		}
	}

	public void ProgressToNextWaveOrScreen()
	{
		completedWaves.Add(waves[0]);
		waves.RemoveAt(0);
		GameSession.Reset();

		if (NoMoreWavesLeft())
		{
			DisableHud(); //TODO Also unload the component from the Game components list
			CurrentGameStats.GameTimer.Stop();
			//TODO: Create wave completed screen showing stats and scoresHandle wave completion screen
			GameSession.CurrentGameState = GameState.MainMenu;
			return;
		}

		currentWave = waves.First();

		UpdateHudData();

		SpawnService.PopulateSpawnList(currentWave.EnemySpawnObjects);
		SpawnService.PopulateBossSpawnList(currentWave.BossSpawnObjects);
		ContentService.LoadEnemies(currentWave.EnemySpawnObjects.Select(x => x.EnemyType).ToList());

		if (currentWave.BossSpawnObjects != null)
		{
			ContentService.LoadEnemies(currentWave.BossSpawnObjects.Select(x => x.EnemyType).ToList());
		}
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
