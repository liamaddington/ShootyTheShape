using ShootyTheShape.Entities.Enemies.Bosses;
using ShootyTheShape.Entities.Enemies.Enums;
using ShootyTheShape.Entities.Enemies.Generic;
using ShootyTheShape.Entities.Player;
using ShootyTheShape.Managers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShootyTheShape.Services.Spawning;
class SpawnService : ISpawnService
{
	static Random rand = new Random();

	private List<EnemySpawnObject> genericEnemySpawnList = new List<EnemySpawnObject>();

	private List<EnemySpawnObject> bossSpawnList = new List<EnemySpawnObject>();

	public void PopulateSpawnList(IList<EnemySpawnObject> enemySpawnList)
	{
		if (enemySpawnList != null)
			this.genericEnemySpawnList = enemySpawnList.ToList();
	}

	public void PopulateBossSpawnList(IList<EnemySpawnObject> bossSpawnObjects)
	{
		if (bossSpawnObjects != null)
			this.bossSpawnList = bossSpawnObjects.ToList();
	}

	public void SpawnEnemiesUsingSpawnLists()
	{
		foreach (EnemySpawnObject enemySpawnObject in genericEnemySpawnList)
		{
			if (rand.Next(enemySpawnObject.SpawnChance) == 0)
			{
				SpawnEnemyOfType(enemySpawnObject.enemyType);
			}
		}
	}

	public void SpawnBossesUsingSpawnList()
	{
		foreach (var bossSpawnObject in bossSpawnList)
		{
			SpawnEnemyOfType(bossSpawnObject.enemyType);
			++EntityManager.TotalBossCount;
		}
	}

	public Vector2 GetRandomSpawnPosition()
	{
		Vector2 pos;

		//TODO Find a better way to spawn enemies outside the vicinity of the player, perhaps by adjust the random value up to a point where it is possible to spawn instead of random gen'ing until it works
		do
		{
			pos = new Vector2(rand.Next((int)GameRoot.ScreenSize.X), rand.Next((int)GameRoot.ScreenSize.Y));
		}
		while (Vector2.DistanceSquared(pos, PlayerShip.Instance.Position) < 300 * 300); //Stop enemies spawning ON the players

		return pos;
	}

	private void SpawnEnemyOfType(EnemyName enemyTypes)
	{
		switch (enemyTypes)
		{
			case EnemyName.Dasher:
				new Dasher(GetRandomSpawnPosition());
				break;
			case EnemyName.Seeker:
				new Seeker(GetRandomSpawnPosition());
				break;
			case EnemyName.Wanderer:
				//new Wanderer(GetRandomSpawnPosition()); TODO: Create Wanderer enemy type
				break;
			case EnemyName.Zain:
				new Zain(new Vector2(100, 100)); //TODO: Think of better boss spawn position methods (Come in from the side of the screen)
				break;
		}
	}


	//TODO Make it possible to create advance spawn configurations, such as spawn groups that will appear in a predetermined (But still random) area of the screen
	public Vector2 GetSpawnWithinBounds(Rectangle bounds)
	{
		throw new NotImplementedException();
	}
}