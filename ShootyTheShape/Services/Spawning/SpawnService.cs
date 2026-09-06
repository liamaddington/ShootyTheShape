using System;
using System.Collections.Generic;
using System.Linq;
using ShootyTheShape.Entities.Enemies.Bosses;
using ShootyTheShape.Entities.Enemies.Enums;
using ShootyTheShape.Entities.Enemies.Generic;
using ShootyTheShape.Entities.Player;
using ShootyTheShape.Managers;
using ShootyTheShape.Services.Audio;
using ShootyTheShape.Services.Content;
using ShootyTheShape.Services.Rendering;

namespace ShootyTheShape.Services.Spawning;

internal class SpawnService : ISpawnService
{
	private static Random _random { get; } = new Random();
	private IContentService _contentService { get; }
	private IAudioService _audioService { get; }
	private IRenderService _renderService { get; }

	private List<EnemySpawnObject> genericEnemySpawnList = new List<EnemySpawnObject>();

	private List<EnemySpawnObject> bossSpawnList = new List<EnemySpawnObject>();

	public SpawnService(IContentService contentService, IAudioService audioService, IRenderService renderService)
	{
		_contentService = contentService;
		_audioService = audioService;
		_renderService = renderService;
	}

	public void PopulateSpawnList(IList<EnemySpawnObject> enemySpawnList)
	{
		if (enemySpawnList != null)
		{
			this.genericEnemySpawnList = enemySpawnList.ToList();
		}
	}

	public void PopulateBossSpawnList(IList<EnemySpawnObject> bossSpawnObjects)
	{
		if (bossSpawnObjects != null)
		{
			this.bossSpawnList = bossSpawnObjects.ToList();
		}
	}

	public void SpawnEnemiesUsingSpawnLists()
	{
		foreach (EnemySpawnObject enemySpawnObject in genericEnemySpawnList)
		{
			if (enemySpawnObject.SpawnChance > 0 && _random.Next(enemySpawnObject.SpawnChance) == 0)
			{
				SpawnEnemyOfType(enemySpawnObject.EnemyType);
			}
		}
	}

	public void SpawnBossesUsingSpawnList()
	{
		foreach (var bossSpawnObject in bossSpawnList)
		{
			SpawnEnemyOfType(bossSpawnObject.EnemyType);
			++EntityManager.TotalBossCount;
		}
	}

	public Vector2 GetRandomSpawnPosition()
	{
		Vector2 spawnPosition;

		//TODO Find a better way to spawn enemies outside the vicinity of the player, perhaps by adjust the random value up to a point where it is possible to spawn instead of random gen'ing until it works
		do
		{
			spawnPosition = new Vector2(_random.Next((int)GameRoot.ScreenSize.X), _random.Next((int)GameRoot.ScreenSize.Y));
		}
		while (Vector2.DistanceSquared(spawnPosition, PlayerShip.Instance.Position) < 300 * 300); //Stop enemies spawning ON the players

		return spawnPosition;
	}

	private void SpawnEnemyOfType(EnemyName enemyTypes)
	{
		switch (enemyTypes)
		{
			case EnemyName.Dasher:
				new Dasher(GetRandomSpawnPosition(), _contentService, _audioService, _renderService);
				break;
			case EnemyName.Seeker:
				new Seeker(GetRandomSpawnPosition(), _contentService, _audioService, _renderService);
				break;
			case EnemyName.Wanderer:
				//new Wanderer(GetRandomSpawnPosition()); TODO: Create Wanderer enemy type
				break;
			case EnemyName.Zain:
				new Zain(new Vector2(100, 100), _contentService, _audioService, _renderService); //TODO: Think of better boss spawn position methods (Come in from the side of the screen)
				break;
		}
	}

	//TODO Make it possible to create advance spawn configurations, such as spawn groups that will appear in a predetermined (But still random) area of the screen
	public Vector2 GetSpawnWithinBounds(Rectangle bounds)
	{
		throw new NotImplementedException();
	}
}
