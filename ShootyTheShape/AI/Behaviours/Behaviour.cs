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

	public Entity MainEntity { get; set; }

	protected Vector2 entityInitialSpawnPosition { get; }
	private int behaviourDelayDuration = 0;
	private float initialOrientationAngle;


	public Vector2 VectorDistance { get; set; }
	public float MeasuredDistance { get; }
	public bool InRange { get; }

	public Behaviour(Entity entity, float? initialOrientationAngle = null, int? behaviourDelayDuration = null)
	{
		MainEntity = entity;
		targeting = new List<ITargeting>();
		entityInitialSpawnPosition = MainEntity.Position;

		this.initialOrientationAngle = initialOrientationAngle ?? 0;
		this.behaviourDelayDuration = behaviourDelayDuration ?? 0;

	}

	protected void DelayedBehaviourMovement(float speed, float aimAngle)
	{
		var movementDirection = new Vector2((float)Math.Cos(aimAngle),
			(float)Math.Sin(aimAngle));

		VectorDistance = MainEntity.Position - entityInitialSpawnPosition;

		MainEntity.Velocity = movementDirection;
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
