using System.Linq;
using ShootyTheShape.AI.Movement;
using ShootyTheShape.AI.Movement.MovementsTypes;
using ShootyTheShape.AI.Targeting;
using ShootyTheShape.AI.Targeting.TargetingTypes;
using ShootyTheShape.Entities;

namespace ShootyTheShape.AI.Behaviours;

internal class OrbitPlayer : Behaviour
{
	AvoidEntities avoidEntity;
	FollowEntity followEntity;

	public OrbitPlayer(Entity entity) : base(entity)
	{
		Targets.Add(new TargetEntity(entity, 300f));

		avoidEntity = new AvoidEntities(entity, new SmoothFlying(entity), Targets.FirstOrDefault());
		followEntity = new FollowEntity(entity, new SmoothFlying(entity), Targets.FirstOrDefault());
	}

	public OrbitPlayer(Entity entity, IMovementType movementType, ITargeting target) : base(entity)
	{
		Targets.Add(target);

		avoidEntity = new AvoidEntities(entity, movementType, Targets.FirstOrDefault());
		followEntity = new FollowEntity(entity, movementType, Targets.FirstOrDefault());
	}

	public override void BehaviourLogic()
	{
		if (EntityOutsideOfOrbitRadius())
		{
			followEntity.BehaviourLogic();
		}
		else
		{
			avoidEntity.StaticBehaviourLogic();
		}
	}

	private bool EntityOutsideOfOrbitRadius()
	{
		return !Targets.FirstOrDefault().InRange;
	}
}
