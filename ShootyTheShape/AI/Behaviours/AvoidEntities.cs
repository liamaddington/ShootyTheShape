using ShootyTheShape.AI.Movement;
using ShootyTheShape.AI.Movement.MovementsTypes;
using ShootyTheShape.AI.Targeting;
using ShootyTheShape.AI.Targeting.TargetingTypes;
using ShootyTheShape.Entities;

namespace ShootyTheShape.AI.Behaviours;

internal class AvoidEntities : Behaviour
{
	public AvoidEntities(Entity entity, AvoidEntitiesConfig config) : base(entity, config.InitialAngle, config.InitialDelay)
	{
	}

	//Using default targeting/movementType for behaviour
	public AvoidEntities(Entity entity, float avoidanceRadius, float avoidanceSpeed) : base(entity)
	{
		base.movementType = new SmoothFlying(this.MainEntity, avoidanceSpeed);
		Targets.Add(new TargetEntity(this.MainEntity, avoidanceRadius));
	}

	public AvoidEntities(Entity entity, float avoidanceSpeed, ITargeting targeting) : base(entity)
	{
		base.movementType = new SmoothFlying(this.MainEntity, avoidanceSpeed);
		Targets.Add(targeting);
	}

	//Passed in targeting/movementType for behaviour
	public AvoidEntities(Entity entity, IMovementType movementType, ITargeting targeting) : base(entity)
	{
		this.movementType = movementType;
		Targets.Add(targeting);
	}

	public AvoidEntities(Entity entity, float avoidanceRadius, float avoidanceSpeed, float initialAngle, int initialDelay)
		: base(entity, initialAngle, initialDelay)
	{
		base.movementType = new SmoothFlying(this.MainEntity, avoidanceSpeed);
		Targets.Add(new TargetEntity(this.MainEntity, avoidanceRadius));
	}

	public AvoidEntities(Entity entity, float avoidanceSpeed, ITargeting targeting, float initialShotAimAngle, int delayedBehaviourDuration)
		: base(entity, initialShotAimAngle, delayedBehaviourDuration)
	{
		base.movementType = new SmoothFlying(this.MainEntity, avoidanceSpeed);
		Targets.Add(targeting);
	}

	//Passed in targeting/movementType for behaviour
	public AvoidEntities(Entity entity, IMovementType movementType, ITargeting targeting, float initialShotAimAngle, int delayedBehaviourDuration)
		: base(entity, initialShotAimAngle, delayedBehaviourDuration)
	{
		this.movementType = movementType;
		Targets.Add(targeting);
	}

	public override void BehaviourLogic()
	{
		MoveAwayFromTarget();
	}

	public void StaticBehaviourLogic()
	{
		MoveAwayFromTarget();
	}

	private void MoveAwayFromTarget()
	{
		foreach (ITargeting target in Targets)
		{
			target.TargetingLogic();
			if (target.InRange)
			{
				MainEntity.VectorDistanceToTarget = target.VectorDistance * -1;
				movementType.MovementLogic();
			}
			else
			{
				movementType.DecelerateLogic();
			}
		}
	}
}
