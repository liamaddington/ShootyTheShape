using ShootyTheShape.AI.Behaviours;
using ShootyTheShape.Entities;
using System;

namespace ShootyTheShape.AI.Behaviours;

class Wander : Behaviour
{
	float speed = 1f;
	int directionChangeCooldown = 1000;
	int angleVariance = 360;

	public override void BehaviourLogic()
	{
		throw new NotImplementedException();
	}

	public Wander(Entity entity) : base(entity)
	{
	}
}
