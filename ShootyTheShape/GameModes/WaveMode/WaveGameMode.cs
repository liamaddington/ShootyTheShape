using ShootyTheShape.Enums;
using ShootyTheShape.GameModes.WaveMode;
using ShootyTheShape.Managers;
using ShootyTheShape.Services.Spawning;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShootyTheShape.GameModes.WaveGameMode;
public class EnemyWave
{
	public int WaveNumber { get; set; }
	public int SpawnLimit { get; set; }
	public int KillsUntilNextWave { get; set; }
	public bool IsBossWave { get; set; } = false;
	public int BossSpawnLimit { get; set; } = -1;
	public int WaveDuration { get; set; } = -1;
	public List<EnemySpawnObject> EnemySpawnObjects { get; set; }
	public List<EnemySpawnObject> BossSpawnObjects { get; set; }

	public EnemyWave(IList<EnemySpawnObject> EnemySpawnObjects, int SpawnLimit, int KillsUntilNextWave)
	{
		this.SpawnLimit = SpawnLimit;
		this.KillsUntilNextWave = KillsUntilNextWave;
		this.EnemySpawnObjects = EnemySpawnObjects.ToList();
	}

	public EnemyWave(IList<EnemySpawnObject> EnemySpawnObjects, int SpawnLimit, int KillsUntilNextWave, int WaveDuration)
		: this(EnemySpawnObjects, SpawnLimit, KillsUntilNextWave)
	{
		this.WaveDuration = WaveDuration;
	}

	public EnemyWave(IList<EnemySpawnObject> EnemySpawnObjects, IList<EnemySpawnObject> BossSpawnObjects, int SpawnLimit, int KillsUntilNextWave, int WaveDuration)
		: this(EnemySpawnObjects, SpawnLimit, KillsUntilNextWave)
	{
		this.BossSpawnObjects = BossSpawnObjects.ToList();
		this.BossSpawnLimit = BossSpawnObjects.Count;
		this.WaveDuration = WaveDuration;
		this.IsBossWave = true;
	}

	public EnemyWave(IList<EnemySpawnObject> EnemySpawnObjects, IList<EnemySpawnObject> BossSpawnObjects, int SpawnLimit, int KillsUntilNextWave)
		: this(EnemySpawnObjects, SpawnLimit, KillsUntilNextWave)
	{
		this.BossSpawnObjects = BossSpawnObjects.ToList();
		this.BossSpawnLimit = BossSpawnObjects.Count;
		this.WaveDuration = WaveDuration;
		this.IsBossWave = true;
	}

}

class WaveGameMode : GameModeBaseClass
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
		base.ContentService.LoadEnemies(currentWave.EnemySpawnObjects.Select(x => x.enemyType).ToList());

		if (currentWave.BossSpawnObjects != null)
			base.ContentService.LoadEnemies(currentWave.BossSpawnObjects.Select(x => x.enemyType).ToList());

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
				base.SpawnService.SpawnBossesUsingSpawnList();
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
			ScreenStateManager.currentGameState = GameState.mainMenu;
			return;
		}

		currentWave = waves.First();

		UpdateHudData();

		base.SpawnService.PopulateSpawnList(currentWave.EnemySpawnObjects);
		base.SpawnService.PopulateBossSpawnList(currentWave.BossSpawnObjects);
		base.ContentService.LoadEnemies(currentWave.EnemySpawnObjects.Select(x => x.enemyType).ToList());
	}

	private void UpdateHudData()
	{
		WaveHudStats.CurrentWave++;
		WaveHudStats.KillsUntilNextWave = currentWave.KillsUntilNextWave;
		WaveHudStats.BossCurrentHP = 0;
	}
	public override void EnableHud()
	{
		Hud.Visable = true;
	}

	public override void DisableHud()
	{
		Hud.Visable = false;
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