using System;
using System.Collections.Generic;
using ShootyTheShape.AI.Movement;
using ShootyTheShape.AI.Targeting;
using ShootyTheShape.Entities;

namespace ShootyTheShape.AI.Behaviours;

public abstract class Behaviour
{
	protected List<ITargeting> Targets { get; }
	protected IMovementType movementType;

	public Entity MainEntity { get; set; }

	protected Vector2 _entityInitialSpawnPosition { get; }
	private int behaviourDelayDuration = 0;
	private float _initialOrientationAngle { get; }

	public Vector2 VectorDistance { get; set; }
	public float MeasuredDistance { get; }
	public bool InRange { get; }

	public Behaviour(Entity entity, float? initialOrientationAngle = null, int? behaviourDelayDuration = null)
	{
		MainEntity = entity;
		Targets = new List<ITargeting>();
		_entityInitialSpawnPosition = MainEntity.Position;

		_initialOrientationAngle = initialOrientationAngle ?? 0;
		this.behaviourDelayDuration = behaviourDelayDuration ?? 0;
	}

	protected void DelayedBehaviourMovement(float aimAngle)
	{
		var movementDirection = new Vector2((float)Math.Cos(aimAngle),
			(float)Math.Sin(aimAngle));

		VectorDistance = MainEntity.Position - _entityInitialSpawnPosition;

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
			DelayedBehaviourMovement(_initialOrientationAngle);
			--behaviourDelayDuration;
		}
	}

	public abstract void BehaviourLogic();
}
