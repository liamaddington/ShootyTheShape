using System.Linq;
using ShootyTheShape.AI.Movement;
using ShootyTheShape.AI.Movement.MovementsTypes;
using ShootyTheShape.AI.Targeting;
using ShootyTheShape.AI.Targeting.TargetingTypes;
using ShootyTheShape.Entities;

namespace ShootyTheShape.AI.Behaviours;

internal class FollowEntity : Behaviour
{
	public FollowEntity(Entity entity, float acceleration) : base(entity)
	{
		movementType = new SmoothFlying(base.MainEntity, acceleration);
		Targets.Add(new TargetEntity(this.MainEntity));
	}

	public FollowEntity(Entity entity, float acceleration, float initialShotAimAngle, int delayedBehaviourDuration) : base(entity, initialShotAimAngle, delayedBehaviourDuration)
	{
		movementType = new SmoothFlying(this.MainEntity, acceleration);
		Targets.Add(new TargetEntity(this.MainEntity));
	}

	public FollowEntity(Entity entity, IMovementType movementType, ITargeting targeting) : base(entity)
	{
		Targets.Add(targeting);
		this.movementType = movementType;
	}

	public FollowEntity(Entity entity, IMovementType movementType, ITargeting targeting, float initialShotAimAngle, int delayedBehaviourDuration) : base(entity, initialShotAimAngle, delayedBehaviourDuration)
	{
		Targets.Add(targeting);
		this.movementType = movementType;
	}

	public override void BehaviourLogic()
	{
		var target = Targets.FirstOrDefault();
		target.TargetingLogic();
		MainEntity.VectorDistanceToTarget = target.VectorDistance;

		movementType.MovementLogic();
	}
}
