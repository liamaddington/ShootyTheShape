using ShootyTheShape.AI.Behaviours;
using ShootyTheShape.Entities;
using ShootyTheShape;
using System;
using ShootyTheShape.AI.Targeting.TargetingTypes;

namespace ShootyTheShape.AI.Behaviours;

class SinglePathWonder : Behaviour
{
	float speed = 1f;

	public SinglePathWonder(Entity entity) : base(entity)
	{
		targeting.Add(new FreeRoam(entity));
	}

	public override void BehaviourLogic()
	{
		if (!GameRoot.Viewport.Bounds.Contains(MainEntity.Position.ToPoint()))
		{
			//TODO: Correct pathing to keep on-screen
		}

		throw new NotImplementedException();
	}
}
