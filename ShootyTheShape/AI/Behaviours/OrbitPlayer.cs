using ShootyTheShape.AI.Movement;
using ShootyTheShape.AI.Movement.MovementsTypes;
using ShootyTheShape.AI.Targeting;
using ShootyTheShape.AI.Targeting.TargetingTypes;
using ShootyTheShape.Entities;
using System.Linq;

namespace ShootyTheShape.AI.Behaviours;

class OrbitPlayer : Behaviour
{
	AvoidEntities avoidEntity;
	FollowEntity followEntity;

	public OrbitPlayer(Entity entity) : base(entity)
	{
		this.targeting.Add(new TargetEntity(entity, 300f));

		avoidEntity = new AvoidEntities(entity, new SmoothFlying(entity), this.targeting.FirstOrDefault());
		followEntity = new FollowEntity(entity, new SmoothFlying(entity), this.targeting.FirstOrDefault());
	}

	public OrbitPlayer(Entity entity, IMovementType movementType, ITargeting target) : base(entity)
	{
		this.targeting.Add(target);

		avoidEntity = new AvoidEntities(entity, movementType, this.targeting.FirstOrDefault());
		followEntity = new FollowEntity(entity, movementType, this.targeting.FirstOrDefault());
	}

	public override void BehaviourLogic()
	{
		if (EntityOutsideOfOrbitRadius())
		{
			followEntity.BehaviourLogic();
		}
		else if (!EntityOutsideOfOrbitRadius())
		{
			avoidEntity.StaticBehaviourLogic();
		}
	}

	private bool EntityOutsideOfOrbitRadius()
	{
		return !targeting.FirstOrDefault().InRange;
	}

}
