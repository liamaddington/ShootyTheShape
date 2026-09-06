using ShootyTheShape.Entities;
using ShootyTheShape.Entities.Player;

namespace ShootyTheShape.AI.Movement.MovementsTypes;

internal class Dash : IMovementType
{
	private Entity _entity { get; }
	private float _dashSpeed { get; }
	private int _originalDashCooldown { get; }
	private int _originalDashDuration { get; }
	private int dashDuration;
	private int dashCooldown;

	public Dash(Entity entity) : this(entity, .6f, 60, 60)
	{
	}

	public Dash(Entity entity, float dashSpeed, int dashDuration)
		: this(entity, dashSpeed, dashDuration, 60)
	{
	}

	public Dash(Entity entity, float dashSpeed, int dashDuration, int dashCooldown)
	{
		_entity = entity;
		_dashSpeed = dashSpeed;
		this.dashDuration = dashDuration;
		this.dashCooldown = dashCooldown;
		_originalDashDuration = dashDuration;
		_originalDashCooldown = dashCooldown;
	}

	public void MovementLogic()
	{
		if (_entity.VectorDistanceToTarget.X != 0 && _entity.VectorDistanceToTarget.Y != 0)
		{
			dashCooldown--;
			if (!PlayerShip.Instance.IsDead && dashCooldown == 0)
			{
				while (dashDuration > 0)
				{
					_entity.Velocity += _entity.VectorDistanceToTarget * (_dashSpeed / _entity.VectorDistanceToTarget.Length());
					dashDuration--;
				}
				dashDuration = _originalDashDuration;
				dashCooldown = _originalDashCooldown;
			}
		}
	}

	public void DecelerateLogic()
	{
		// Dashing does not apply deceleration.
	}
}
