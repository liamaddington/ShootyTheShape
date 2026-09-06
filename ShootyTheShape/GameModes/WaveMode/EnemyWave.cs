using System.Collections.Generic;
using System.Linq;
using ShootyTheShape.Services.Spawning;

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

	public EnemyWave(IList<EnemySpawnObject> enemySpawnObjects, int spawnLimit, int killsUntilNextWave)
	{
		this.SpawnLimit = spawnLimit;
		this.KillsUntilNextWave = killsUntilNextWave;
		this.EnemySpawnObjects = enemySpawnObjects.ToList();
	}

	public EnemyWave(IList<EnemySpawnObject> enemySpawnObjects, int spawnLimit, int killsUntilNextWave, int waveDuration)
		: this(enemySpawnObjects, spawnLimit, killsUntilNextWave)
	{
		this.WaveDuration = waveDuration;
	}

	public EnemyWave(IList<EnemySpawnObject> enemySpawnObjects, IList<EnemySpawnObject> bossSpawnObjects, int spawnLimit, int killsUntilNextWave, int waveDuration)
		: this(enemySpawnObjects, spawnLimit, killsUntilNextWave)
	{
		this.BossSpawnObjects = bossSpawnObjects.ToList();
		this.BossSpawnLimit = bossSpawnObjects.Count;
		this.WaveDuration = waveDuration;
		this.IsBossWave = true;
	}

	public EnemyWave(IList<EnemySpawnObject> enemySpawnObjects, IList<EnemySpawnObject> bossSpawnObjects, int spawnLimit, int killsUntilNextWave)
		: this(enemySpawnObjects, spawnLimit, killsUntilNextWave)
	{
		this.BossSpawnObjects = bossSpawnObjects.ToList();
		this.BossSpawnLimit = bossSpawnObjects.Count;

		this.IsBossWave = true;
	}
}
