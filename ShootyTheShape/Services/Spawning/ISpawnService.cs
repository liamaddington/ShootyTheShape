using System.Collections.Generic;

namespace ShootyTheShape.Services.Spawning;

public interface ISpawnService
{
	void PopulateSpawnList(IList<EnemySpawnObject> enemySpawnList);
	void PopulateBossSpawnList(IList<EnemySpawnObject> bossSpawnObjects);
	void SpawnEnemiesUsingSpawnLists();
	void SpawnBossesUsingSpawnList();
	Vector2 GetRandomSpawnPosition();
	Vector2 GetSpawnWithinBounds(Rectangle bounds);
}
