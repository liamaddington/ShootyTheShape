using ShootyTheShape.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShootyTheShape.AI.Movement.MovementsTypes;
public class SmoothFlying : IMovementType
{
	float speed = 0.6f;
	float acceleration = -1f;
	Entity entity;

	public SmoothFlying(Entity entity)
	{
		this.entity = entity;
	}

	//TODO: Think about how acceleration could be used without resorting to -1 checking values
	public SmoothFlying(Entity entity, float speed, float acceleration = -1)
	{
		this.entity = entity;
		this.speed = speed;
		this.acceleration = acceleration;
	}

	public void MovementLogic()
	{
		if (entity.VectoreDistanceToTarget.X != 0 && entity.VectoreDistanceToTarget.Y != 0) //Don't want to divide by 0
		{
			if (acceleration == -1)
			{
				entity.Velocity += entity.VectoreDistanceToTarget * (speed / entity.VectoreDistanceToTarget.Length());
			}
			else
			{
				MovementLogicWithAccel();
			}
		}
	}

	public void DeselerateLogic()
	{
		//entity.Velocity -= entity.Velocity * (speed / entity.Velocity.Length());
	}

	public void MovementLogicWithAccel()
	{
		entity.Velocity += entity.VectoreDistanceToTarget * ((speed * acceleration) / (entity.TargetPosition - entity.Position).Length());
	}
}