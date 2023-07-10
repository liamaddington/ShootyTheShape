using ShootyTheShape.Entities.Enemies.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShootyTheShape.Services.Spawning;
public class EnemySpawnObject
{
	public int SpawnChance { get; set; }
	public EnemyName enemyType { get; set; }

	public EnemySpawnObject(int spawnChance, EnemyName enemyType)
	{
		this.SpawnChance = spawnChance;
		this.enemyType = enemyType;
	}

	public EnemySpawnObject(EnemyName enemyType)
	{
		this.SpawnChance = -1;
		this.enemyType = enemyType;
	}
}