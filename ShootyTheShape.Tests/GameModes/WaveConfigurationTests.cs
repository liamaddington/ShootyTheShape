using System.Collections.Generic;
using ShootyTheShape.Entities.Enemies.Enums;
using ShootyTheShape.GameModes.WaveGameMode;
using ShootyTheShape.Services.Spawning;
using Xunit;

namespace ShootyTheShape.Tests.GameModes;

public class WaveConfigurationTests
{
	[Fact]
	public void RegularWaveCopiesSpawnListAndKeepsDefaultLimits()
	{
		var enemies = new List<EnemySpawnObject> { new EnemySpawnObject(10, EnemyName.Seeker) };
		var wave = new EnemyWave(enemies, 20, 30);

		enemies.Clear();

		Assert.Single(wave.EnemySpawnObjects);
		Assert.Equal(20, wave.SpawnLimit);
		Assert.Equal(30, wave.KillsUntilNextWave);
		Assert.False(wave.IsBossWave);
		Assert.Equal(-1, wave.WaveDuration);
		Assert.Equal(-1, wave.BossSpawnLimit);
	}

	[Theory]
	[InlineData(false, -1)]
	[InlineData(true, 120)]
	public void BossWaveCopiesBossListAndPreservesDuration(bool hasDuration, int expectedDuration)
	{
		var enemies = new List<EnemySpawnObject>();
		var bosses = new List<EnemySpawnObject> { new EnemySpawnObject(EnemyName.Zain) };
		var wave = hasDuration
			? new EnemyWave(enemies, bosses, 20, 1, expectedDuration)
			: new EnemyWave(enemies, bosses, 20, 1);

		bosses.Clear();

		Assert.True(wave.IsBossWave);
		Assert.Single(wave.BossSpawnObjects);
		Assert.Equal(1, wave.BossSpawnLimit);
		Assert.Equal(expectedDuration, wave.WaveDuration);
		Assert.Equal(-1, wave.BossSpawnObjects[0].SpawnChance);
		Assert.Equal(EnemyName.Zain, wave.BossSpawnObjects[0].EnemyType);
	}
}
