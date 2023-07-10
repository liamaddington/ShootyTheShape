using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using ShootyTheShape.AI.Movement;
using ShootyTheShape.AI.Targeting;
using ShootyTheShape.Entities;

namespace ShootyTheShape.AI.Behaviours;

public abstract class Behaviour
{
	protected List<ITargeting> targeting { get; set; }
	protected IMovementType movementType;

	protected Entity entity;
	protected Vector2 entityInitialSpawnPosition;
	private int behaviourDelayDuration = 0;
	private float initialOrientationAngle;


	public Vector2 VectorDistance { get; set; }
	public float MeasuredDistance { get; }
	public bool InRange { get; }

	public Behaviour(Entity entity, float initialOrientationAngle, int behaviourDelayDuration)
	{
		this.targeting = new List<ITargeting>();
		this.entityInitialSpawnPosition = entity.Position;

		this.initialOrientationAngle = initialOrientationAngle;
		this.behaviourDelayDuration = behaviourDelayDuration;

		this.entity = entity;
	}

	public Behaviour(Entity entity)
	{
		this.targeting = new List<ITargeting>();
		this.entityInitialSpawnPosition = entity.Position;

		this.entity = entity;
	}

	protected void DelayedBehaviourMovement(float speed, float aimAngle)
	{
		var movementDirection = new Vector2((float)Math.Cos(aimAngle),
			(float)Math.Sin(aimAngle));

		VectorDistance = entity.Position - entityInitialSpawnPosition;

		entity.Velocity = movementDirection;
	}

	public void RunBehaviour()
	{
		if (behaviourDelayDuration <= 0)
		{
			BehaviourLogic();
		}
		else
		{
			DelayedBehaviourMovement(5f, initialOrientationAngle);
			--behaviourDelayDuration;
		}
	}

	public abstract void BehaviourLogic();
}
