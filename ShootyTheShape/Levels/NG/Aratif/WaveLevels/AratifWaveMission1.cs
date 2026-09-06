using System.Collections.Generic;
using ShootyTheShape.Entities.Enemies.Enums;
using ShootyTheShape.GameModes.WaveGameMode;
using ShootyTheShape.Runtime;
using ShootyTheShape.Services.Content;
using ShootyTheShape.Services.Rendering;
using ShootyTheShape.Services.Spawning;

namespace ShootyTheShape.Levels.NG.Aratif.WaveLevels;

internal class AratifWaveMission1 : ILevel
{
	private WaveGameMode _level { get; }

	public AratifWaveMission1(
		ISpawnService spawnService,
		IContentService contentService,
		IRenderService renderService,
		GameSession gameSession)
	{
		//TODO: Think about boss wave implementation, how should it display in the UI, how is it implemented
		var waves = new List<EnemyWave> {
				new EnemyWave(
					new List<EnemySpawnObject>
					{
						new EnemySpawnObject(3, EnemyName.Seeker)
					},
					spawnLimit: 5,
					killsUntilNextWave: 20
				),

				new EnemyWave(
					new List<EnemySpawnObject>
					{
						new EnemySpawnObject(spawnChance: 20, EnemyName.Seeker),
						new EnemySpawnObject(spawnChance: 10, EnemyName.Dasher)
					},
					spawnLimit: 8,
					killsUntilNextWave: 24
				),

				new EnemyWave(
					new List<EnemySpawnObject>
					{
						new EnemySpawnObject(spawnChance: 20, EnemyName.Seeker),
						new EnemySpawnObject(spawnChance: 10, EnemyName.Dasher)
					},
					spawnLimit: 10,
					killsUntilNextWave: 26
				),

				new EnemyWave(
					new List<EnemySpawnObject>
					{
						new EnemySpawnObject(spawnChance: 20, EnemyName.Seeker),
						new EnemySpawnObject(spawnChance: 10, EnemyName.Dasher)
					},
					spawnLimit: 10,
					killsUntilNextWave:28
				),

				new EnemyWave(
					new List<EnemySpawnObject>
					{
						new EnemySpawnObject(spawnChance: 20, EnemyName.Seeker),
						new EnemySpawnObject(spawnChance: 10, EnemyName.Dasher)
					},
					spawnLimit: 10,
					killsUntilNextWave:30
				),

				new EnemyWave(
					new List<EnemySpawnObject>
					{
						new EnemySpawnObject(spawnChance: 10, EnemyName.Seeker),
						new EnemySpawnObject(spawnChance: 10, EnemyName.Dasher),
						new EnemySpawnObject(EnemyName.Zain)
					},
					spawnLimit: 5,
					killsUntilNextWave: 1
				)
			};

		_level = new WaveGameMode(waves, spawnService, contentService, renderService, gameSession);
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
