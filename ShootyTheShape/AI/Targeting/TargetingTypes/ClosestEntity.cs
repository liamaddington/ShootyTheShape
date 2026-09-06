using System;
using System.Collections.Generic;
using System.Linq;
using ShootyTheShape.Entities;
using ShootyTheShape.Entities.Enemies;
using ShootyTheShape.Entities.Enums;
using ShootyTheShape.Entities.Player;
using ShootyTheShape.Entities.Projectiles;
using ShootyTheShape.Managers;

namespace ShootyTheShape.AI.Targeting.TargetingTypes;

internal class ClosestEntity : ITargeting
{
	public Entity HostEntity { get; private set; }
	public Entity EntityToTarget { get; private set; }

	public Vector2 VectorDistance { get; private set; }

	public float MeasuredDistance { get; private set; }

	public float TargetRange { get; private set; }
	public bool InRange { get; private set; } = false;

	public Dictionary<Guid, DistanceMatrixObject> DistanceMatrixStore = new Dictionary<Guid, DistanceMatrixObject>();

	private List<EntityTypes> entityTypesToTarget;

	public ClosestEntity(Entity entity, List<EntityTypes> entityTypesToTarget)
		: this(entity, -1, entityTypesToTarget)
	{
	}

	public ClosestEntity(Entity entity, float targetRange, List<EntityTypes> entityTypesToTarget)
	{
		this.HostEntity = entity;
		this.TargetRange = targetRange;
		this.entityTypesToTarget = entityTypesToTarget.ToList();

		PopulateDistanceMatrixWithExistingEntities(entityTypesToTarget);

		AssignDelegatesToEntityManager();
	}

	private void AssignDelegatesToEntityManager()
	{
		//These are added as delegates so that each individual instance of this class (As assigned to an new entity)
		//Will properly update all, without having to perform this calculation for all entities globally, only the ones that need it
		EntityManager.AddNewEntityToDistanceMatrixDelegate += AddEntityToDistanceMatrix;

		EntityManager.RemoveEntityFromDistanceMatrixDelegate += RemoveEntityFromDistanceMatrix;
	}

	private void RemoveEntityFromDistanceMatrix(Guid entityId)
	{
		DistanceMatrixStore.Remove(entityId);
	}

	private void AddEntityToDistanceMatrix(Entity entity)
	{
		if (entity.Id == HostEntity.Id)
		{
			return;
		}

		bool isTargetedEntity = entity switch
		{
			Enemy => entityTypesToTarget.Contains(EntityTypes.Enemy),
			Bullet => entityTypesToTarget.Contains(EntityTypes.Bullet),
			PlayerShip => entityTypesToTarget.Contains(EntityTypes.Player),
			_ => false
		};

		if (isTargetedEntity)
		{
			DistanceMatrixStore.Add(entity.Id, new DistanceMatrixObject(HostEntity, entity));
		}
	}

	public void TargetingLogic()
	{
		if (DistanceMatrixStore.Count > 0)
		{
			foreach (var distanceObject in DistanceMatrixStore.Values)
			{
				distanceObject.Update();
			}

			DistanceMatrixObject closestDistanceObject = null;

			foreach (var matrixObject in DistanceMatrixStore.Values)
			{
				if (closestDistanceObject == null || closestDistanceObject.MeasuredDistance > matrixObject.MeasuredDistance)
				{
					closestDistanceObject = matrixObject;
				}
			}

			if (closestDistanceObject != null)
			{
				VectorDistance = closestDistanceObject.VectorDistance;
				MeasuredDistance = closestDistanceObject.MeasuredDistance;

				InRange = TargetRange == -1 || MeasuredDistance < TargetRange;
				if (InRange)
				{
					HostEntity.AimDirection = VectorDistance.ToAngle();
				}
			}
		}
		else
		{
			VectorDistance = Vector2.Zero;
		}
	}

	private void PopulateDistanceMatrixWithExistingEntities(IList<EntityTypes> entityTypesToTarget)
	{
		foreach (EntityTypes type in entityTypesToTarget)
		{
			switch (type)
			{
				case EntityTypes.Enemy:
					foreach (Enemy enemy in EntityManager.Enemies.Values)
					{
						DistanceMatrixStore.Add(enemy.Id, new DistanceMatrixObject(HostEntity, enemy));
					}
					break;
				case EntityTypes.Bullet:
					foreach (Bullet bullet in EntityManager.Bullets.Values)
					{
						DistanceMatrixStore.Add(bullet.Id, new DistanceMatrixObject(HostEntity, bullet));
					}
					break;
				case EntityTypes.Player:
					foreach (PlayerShip player in EntityManager.PlayerShips.Values)
					{
						DistanceMatrixStore.Add(player.Id, new DistanceMatrixObject(HostEntity, player));
					}
					break;
			}
		}
	}
}
