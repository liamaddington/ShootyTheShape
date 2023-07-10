using ShootyTheShape.Entities.Enemies.Enums;
using ShootyTheShape.GameModes.WaveGameMode;
using ShootyTheShape.Services.Spawning;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShootyTheShape.Levels.NG.Aratif.WaveLevels;
class AratifWaveMission1 : ILevel
{
	private WaveGameMode _level;

	public AratifWaveMission1()
	{
		//TODO: Think about boss wave implementation, how should it display in the UI, how is it implemented
		_level = new WaveGameMode(new List<EnemyWave> {
				new EnemyWave(
					new List<EnemySpawnObject>
					{
						new EnemySpawnObject(3, EnemyName.Seeker)
					},
					SpawnLimit: 5,
					KillsUntilNextWave: 20
				),

				new EnemyWave(
					new List<EnemySpawnObject>
					{
						new EnemySpawnObject(spawnChance: 20, EnemyName.Seeker),
						new EnemySpawnObject(spawnChance: 10, EnemyName.Dasher)
					},
					SpawnLimit: 8,
					KillsUntilNextWave: 24
				),

				new EnemyWave(
					new List<EnemySpawnObject>
					{
						new EnemySpawnObject(spawnChance: 20, EnemyName.Seeker),
						new EnemySpawnObject(spawnChance: 10, EnemyName.Dasher)
					},
					SpawnLimit: 10,
					KillsUntilNextWave: 26
				),

				new EnemyWave(
					new List<EnemySpawnObject>
					{
						new EnemySpawnObject(spawnChance: 20, EnemyName.Seeker),
						new EnemySpawnObject(spawnChance: 10, EnemyName.Dasher)
					},
					SpawnLimit: 10,
					KillsUntilNextWave:28
				),

				new EnemyWave(
					new List<EnemySpawnObject>
					{
						new EnemySpawnObject(spawnChance: 20, EnemyName.Seeker),
						new EnemySpawnObject(spawnChance: 10, EnemyName.Dasher)
					},
					SpawnLimit: 10,
					KillsUntilNextWave:30
				),

				new EnemyWave(
					new List<EnemySpawnObject>
					{
						new EnemySpawnObject(spawnChance: 10, EnemyName.Seeker),
						new EnemySpawnObject(spawnChance: 10, EnemyName.Dasher),
						new EnemySpawnObject(EnemyName.Zain)
					},
					SpawnLimit: 5,
					KillsUntilNextWave: 1
				)
			});
	}

	public void Update()
	{
		_level.Update();
	}

	public void DrawHud()
	{
		_level.DrawHud();
	}
}