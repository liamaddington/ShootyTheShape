using System;
using ShootyTheShape.AI.Targeting.TargetingTypes;
using ShootyTheShape.Entities;

namespace ShootyTheShape.AI.Behaviours;

internal class SinglePathWonder : Behaviour
{
	public SinglePathWonder(Entity entity) : base(entity)
	{
		Targets.Add(new FreeRoam(entity));
	}

	public override void BehaviourLogic()
	{
		if (GameRoot.IsOutsideArena(MainEntity.Position, MainEntity.Radius))
		{
			//TODO: Correct pathing to keep on-screen
		}

		throw new NotImplementedException();
	}
}
