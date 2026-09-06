using System;
using System.Collections.Generic;
using System.Linq;
using ShootyTheShape.Entities;
using ShootyTheShape.Entities.Enemies;
using ShootyTheShape.Entities.Player;
using ShootyTheShape.Entities.Projectiles;

namespace ShootyTheShape.Managers;

public class EntityManager
{
	public delegate void AddNewDistanceMatrixDelegate(Entity entity);
	public static AddNewDistanceMatrixDelegate AddNewEntityToDistanceMatrixDelegate;

	public delegate void RemoveEntityDistanceMatrixDelegate(Guid entityId);
	public static RemoveEntityDistanceMatrixDelegate RemoveEntityFromDistanceMatrixDelegate;

	public delegate void RemoveActiveBulletDelegate(Guid bulletId);
	public static RemoveActiveBulletDelegate RemoveBulletFromWeaponActiveBulletsDelegate;

	public static Dictionary<Guid, Entity> Entities { get; private set; } = new();
	public static Dictionary<Guid, Enemy> Enemies { get; private set; } = new();
	public static Dictionary<Guid, Enemy> Bosses { get; private set; } = new();
	public static Dictionary<Guid, Bullet> Bullets { get; private set; } = new();
	public static Dictionary<Guid, PlayerShip> PlayerShips { get; private set; } = new();

	static bool isUpdating;
	static List<Entity> addedEntities = new List<Entity>();

	public static int EntityCount { get { return Entities.Count; } }
	public static int EnemyCount { get { return Enemies.Count; } }
	public static int BossCount { get { return Bosses.Count; } }
	public static int TotalBossCount { get; set; }
	public static int BulletCount { get { return Bullets.Count; } }

	public static void Add(Entity entity)
	{
		if (!isUpdating)
		{
			AddEntity(entity);
		}
		else
		{
			addedEntities.Add(entity);
		}
	}

	private static void AddEntity(Entity entity)
	{
		Entities.Add(entity.Id, entity);

		if (entity is WeaponFire)
		{
			Bullets.Add(entity.Id, entity as Bullet);
		}
		else if (entity is Enemy)
		{
			var enemy = (Enemy)entity;
			if (enemy.IsBoss)
			{
				Bosses.Add(enemy.Id, enemy);
			}
			else
			{
				Enemies.Add(entity.Id, enemy);
			}
		}
		else if (entity is PlayerShip)
		{
			PlayerShips.Add(entity.Id, entity as PlayerShip);
		}

		AddNewEntityToDistanceMatrixDelegate?.Invoke(entity);
	}

	public static void Update()
	{
		isUpdating = true;
		HandleCollisions();

		foreach (var entity in Entities.Values)
		{
			entity.Update();
		}

		isUpdating = false;

		foreach (var entity in addedEntities)
		{
			AddEntity(entity);
		}

		addedEntities.Clear();

		RemoveExpiredEntities();
	}

	private static void RemoveExpiredEntities()
	{
		//Use sepearte list to avoid exception being thrown by changing the list being iterated over in the foreach
		var allEntities = new List<Entity>(Entities.Values);

		foreach (var entity in allEntities)
		{
			if (entity.IsExpired)
			{
				RemoveEntity(entity);
			}
		}
	}

	private static void RemoveEntity(Entity entity)
	{
		Entities.Remove(entity.Id);
		if (entity is WeaponFire)
		{
			Bullets.Remove(entity.Id);
			RemoveEntityFromDistanceMatrixDelegate?.Invoke(entity.Id);
			RemoveBulletFromWeaponActiveBulletsDelegate?.Invoke(entity.Id);
		}
		else if (entity is Enemy)
		{
			Enemies.Remove(entity.Id);
			RemoveEntityFromDistanceMatrixDelegate?.Invoke(entity.Id);
		}
	}

	static void HandleCollisions()
	{
		var enemyArray = Enemies.Values.ToArray();
		var bossArray = Bosses.Values.ToArray();
		var bulletArray = Bullets.Values.ToArray();

		//Enemy collision handling
		HandleCollisionBetweenEnemies(enemyArray);
		HandleCollisionBetweenEnemiesAndUnfriendlyBullets(enemyArray, bulletArray);
		HandleCollisionBetweenBossesAndUnfriendlyBullets(bossArray, bulletArray);

		//Player collision handling
		HandleCollisionBetweenPlayerAndEnemies(enemyArray);
		HandleCollisionBetweenPlayerAndBosses(bossArray);
	}

	private static void HandleCollisionBetweenPlayerAndEnemies(Enemy[] allEnemies)
	{
		if (!PlayerShip.Instance.IsInvulnerable)
		{
			for (int i = 0; i < EnemyCount; i++)
			{
				if (allEnemies[i].IsActive && IsColliding(PlayerShip.Instance, allEnemies[i]))
				{
					PlayerShip.Instance.Kill();
					RemoveAllEnemies();
					break;
				}
			}
		}
	}

	private static void HandleCollisionBetweenPlayerAndBosses(Enemy[] allBosses)
	{
		if (!PlayerShip.Instance.IsInvulnerable)
		{
			for (int i = 0; i < BossCount; i++)
			{
				if (allBosses[i].IsActive && IsColliding(PlayerShip.Instance, allBosses[i]))
				{
					PlayerShip.Instance.Kill();
					RemoveAllEnemies();
					break;
				}
			}
		}
	}

	private static void HandleCollisionBetweenEnemiesAndUnfriendlyBullets(Enemy[] allEnemies, Bullet[] allUnfriendlyBullets)
	{
		for (var i = 0; i < EnemyCount; i++)
		{
			for (var j = 0; j < BulletCount; j++)
			{
				if (IsColliding(allEnemies[i], allUnfriendlyBullets[j]))
				{
					allEnemies[i].WasShot(allUnfriendlyBullets[j]);
					allUnfriendlyBullets[j].IsExpired = true;
				}
			}
		}
	}

	private static void HandleCollisionBetweenBossesAndUnfriendlyBullets(Enemy[] bosses, Bullet[] allUnfriendlyBullets)
	{
		for (var i = 0; i < TotalBossCount; i++)
		{
			for (var j = 0; j < BulletCount; j++)
			{
				if (IsColliding(bosses[i], allUnfriendlyBullets[j]))
				{
					bosses[i].WasShot(allUnfriendlyBullets[j]);
					allUnfriendlyBullets[j].IsExpired = true;
				}
			}
		}
	}

	private static void HandleCollisionBetweenEnemies(Enemy[] allEnemies)
	{
		for (var i = 0; i < EnemyCount; i++)
		{
			for (var j = i + 1; j < EnemyCount; j++)
			{
				if (IsColliding(allEnemies[i], allEnemies[j]))
				{
					allEnemies[i].MoveAwayFromCollidingEnemy(allEnemies[j]);
					allEnemies[j].MoveAwayFromCollidingEnemy(allEnemies[i]);
				}
			}
		}
	}

	public static void RemoveAllEnemies()
	{
		foreach (var entity in Enemies.Values)
		{
			entity.WasTouched();
		}
	}

	public static void RemoveAllBosses()
	{
		foreach (var boss in Bosses.Values)
		{
			boss.WasTouched();
		}
	}

	private static bool IsColliding(Entity a, Entity b)
	{
		float radius = a.Radius + b.Radius;
		return !a.IsExpired && !b.IsExpired && Vector2.DistanceSquared(a.Position, b.Position) < radius * radius;
	}

	public static void Draw()
	{
		foreach (var entity in Entities.Values)
		{
			entity.Draw();
		}
	}
}
