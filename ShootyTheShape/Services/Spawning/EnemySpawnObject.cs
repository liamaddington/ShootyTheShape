using ShootyTheShape.Entities.Enemies.Enums;

namespace ShootyTheShape.Services.Spawning;

public class EnemySpawnObject
{
	public int SpawnChance { get; set; }
	public EnemyName EnemyType { get; set; }

	public EnemySpawnObject(int spawnChance, EnemyName enemyType)
	{
		this.SpawnChance = spawnChance;
		this.EnemyType = enemyType;
	}

	public EnemySpawnObject(EnemyName enemyType) : this(-1, enemyType)
	{
	}
}
