using Microsoft.Xna.Framework;
using ShootyTheShape.Entities;
using ShootyTheShape.Entities.Enemies;
using ShootyTheShape.Entities.Enums;
using ShootyTheShape.Entities.Player;
using ShootyTheShape.Entities.Projectiles;
using ShootyTheShape.Managers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShootyTheShape.AI.Targeting.TargetingTypes;
public class DistanceMatrixObject
{
	public DistanceMatrixObject(Entity hostEntity, Entity entityToTarget)
	{
		this.hostEntity = hostEntity;
		this.EntityToTarget = entityToTarget;
	}

	public void Update()
	{
		VectorDistance = EntityToTarget.Position - hostEntity.Position;
		MeasuredDistance = VectorDistance.Length();
	}

	public Vector2 VectorDistance { get; private set; }
	public float MeasuredDistance { get; private set; }
	public Entity hostEntity { get; set; }
	public Entity EntityToTarget { get; set; }
}

class ClosestEntity : ITargeting
{

	public Entity HostEntity { get; private set; }
	public Entity EntityToTarget { get; private set; }

	public Vector2 VectorDistance { get; private set; }

	public float MeasuredDistance { get; private set; }

	public float targetRange { get; private set; }
	public bool InRange { get; private set; } = false;

	public Dictionary<Guid, DistanceMatrixObject> DistanceMatrixStore = new Dictionary<Guid, DistanceMatrixObject>();

	//TODO: Look into ordered Distance Matrix for performance purposes
	private List<DistanceMatrixObject> orderableDistanceMatrixObjects = new List<DistanceMatrixObject>();

	private List<EntityTypes> entityTypesToTarget;

	public ClosestEntity(Entity entity, IList<EntityTypes> entityTypesToTarget)
	{
		this.HostEntity = entity;
		this.targetRange = -1;
		this.entityTypesToTarget = entityTypesToTarget.ToList();

		PopulateDistanceMatrixWithExistingEntities(entityTypesToTarget);

		AssignDelegatesToEntityManager();
	}

	public ClosestEntity(Entity entity, float targetRange, IList<EntityTypes> entityTypesToTarget)
	{
		this.HostEntity = entity;
		this.targetRange = targetRange;
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
		if (entity.Id != HostEntity.Id)
		{
			//Add only targeted entities types
			//TODO: Use switch
			if (entity is Enemy)
			{
				if (entityTypesToTarget.Contains(EntityTypes.enemy))
				{
					DistanceMatrixStore.Add(entity.Id, new DistanceMatrixObject(HostEntity, entity));
					return;
				}
			}

			if (entity is Bullet)
			{
				if (entityTypesToTarget.Contains(EntityTypes.bullet))
				{
					DistanceMatrixStore.Add(entity.Id, new DistanceMatrixObject(HostEntity, entity));
					return;
				}
			}

			if (entity is PlayerShip)
			{
				if (entityTypesToTarget.Contains(EntityTypes.player))
				{
					DistanceMatrixStore.Add(entity.Id, new DistanceMatrixObject(HostEntity, entity));
					return;
				}
			}
		}
	}

	DistanceMatrixObject closestDistanceObject = null;


	public void TargetingLogic()
	{
		if (DistanceMatrixStore.Count > 0)
		{
			foreach (var distanceObject in DistanceMatrixStore.Values)
				distanceObject.Update();

			closestDistanceObject = null;

			foreach (var matrixObject in DistanceMatrixStore.Values)
			{
				if (closestDistanceObject == null)
					closestDistanceObject = matrixObject;

				if (closestDistanceObject.MeasuredDistance > matrixObject.MeasuredDistance)
					closestDistanceObject = matrixObject;
			}

			if (closestDistanceObject != null)
			{
				VectorDistance = closestDistanceObject.VectorDistance;
				MeasuredDistance = closestDistanceObject.MeasuredDistance;

				if (targetRange != -1)
				{
					if (MeasuredDistance < targetRange)
					{
						HostEntity.AimDirection = VectorDistance.ToAngle();
						InRange = true;
					}
					else
					{
						InRange = false;
					}
				}
				else
				{
					HostEntity.AimDirection = VectorDistance.ToAngle();
					InRange = true;
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
				case EntityTypes.enemy:
					foreach (Enemy enemy in EntityManager.Enemies.Values)
					{
						DistanceMatrixStore.Add(enemy.Id, new DistanceMatrixObject(HostEntity, enemy));
					}
					break;
				case EntityTypes.bullet:
					foreach (Bullet bullet in EntityManager.Bullets.Values)
					{
						DistanceMatrixStore.Add(bullet.Id, new DistanceMatrixObject(HostEntity, bullet));
					}
					break;
				case EntityTypes.player:
					foreach (PlayerShip player in EntityManager.PlayerShips.Values)
					{
						DistanceMatrixStore.Add(player.Id, new DistanceMatrixObject(HostEntity, player));
					}
					break;
			}
		}
	}
}
