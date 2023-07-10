using ShootyTheShape.AI.Movement;
using ShootyTheShape.AI.Movement.MovementsTypes;
using ShootyTheShape.AI.Targeting;
using ShootyTheShape.AI.Targeting.TargetingTypes;
using ShootyTheShape.Entities;
using System.Linq;

namespace ShootyTheShape.AI.Behaviours;

class FollowEntity : Behaviour
{

	private float initiaDelayFlightSpeed = 0;
	private float initialDelayFlightAngle = 0;


	public FollowEntity(Entity entity, float acceleration) : base(entity)
	{
		movementType = new SmoothFlying(base.entity, acceleration);
		targeting.Add(new TargetEntity(this.entity));
	}

	public FollowEntity(Entity entity, float acceleration, float initialShotAimAngle, int delayedBehaviourDuration) : base(entity, initialShotAimAngle, delayedBehaviourDuration)
	{
		movementType = new SmoothFlying(this.entity, acceleration);
		targeting.Add(new TargetEntity(this.entity));
	}

	public FollowEntity(Entity entity, IMovementType movementType, ITargeting targeting) : base(entity)
	{
		this.targeting.Add(targeting);
		this.movementType = movementType;
	}

	public FollowEntity(Entity entity, IMovementType movementType, ITargeting targeting, float initialShotAimAngle, int delayedBehaviourDuration) : base(entity, initialShotAimAngle, delayedBehaviourDuration)
	{
		this.targeting.Add(targeting);
		this.movementType = movementType;
	}


	public override void BehaviourLogic()
	{
		targeting.FirstOrDefault().TargetingLogic();
		entity.VectoreDistanceToTarget = targeting.FirstOrDefault().VectorDistance;

		movementType.MovementLogic();

	}

}
