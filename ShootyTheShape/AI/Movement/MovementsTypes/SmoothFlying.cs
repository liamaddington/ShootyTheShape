using ShootyTheShape.Entities;

namespace ShootyTheShape.AI.Movement.MovementsTypes;

public class SmoothFlying : IMovementType
{
	private float _speed { get; }
	private float _acceleration { get; }
	private Entity _entity { get; }

	public SmoothFlying(Entity entity) : this(entity, 0.6f)
	{
	}

	//TODO: Think about how acceleration could be used without resorting to -1 checking values
	public SmoothFlying(Entity entity, float speed, float acceleration = -1)
	{
		_entity = entity;
		_speed = speed;
		_acceleration = acceleration;
	}

	public void MovementLogic()
	{
		if (_entity.VectorDistanceToTarget.X != 0 && _entity.VectorDistanceToTarget.Y != 0) //Don't want to divide by 0
		{
			if (_acceleration == -1)
			{
				_entity.Velocity += _entity.VectorDistanceToTarget * (_speed / _entity.VectorDistanceToTarget.Length());
			}
			else
			{
				MoveWithAcceleration();
			}
		}
	}

	public void DecelerateLogic()
	{
		// Smooth flying currently relies on the entity to apply drag.
	}

	public void MoveWithAcceleration()
	{
		_entity.Velocity += _entity.VectorDistanceToTarget * ((_speed * _acceleration) / (_entity.TargetPosition - _entity.Position).Length());
	}
}
