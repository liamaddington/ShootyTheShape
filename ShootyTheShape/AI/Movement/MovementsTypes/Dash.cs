using ShootyTheShape.Entities;
using ShootyTheShape.Entities.Player;

namespace ShootyTheShape.AI.Movement.MovementsTypes;
class Dash : IMovementType
{
	Entity entity;

	float dashSpeed = .6f;
	int dashDuration = 60;
	int dashCooldown = 60;

	private int originalDashCooldown;
	private int originalDashDuration;


	public Dash(Entity entity)
	{
		this.entity = entity;

		SetOriginalValues();
	}

	public Dash(Entity entity, float dashSpeed, int dashDuration)
	{
		this.entity = entity;

		this.dashSpeed = dashSpeed;
		this.dashDuration = dashDuration;

		SetOriginalValues();
	}

	public Dash(Entity entity, float dashSpeed, int dashDuration, int dashCooldown)
	{
		this.entity = entity;

		this.dashSpeed = dashSpeed;
		this.dashDuration = dashDuration;
		this.dashCooldown = dashCooldown;

		SetOriginalValues();
	}

	private void SetOriginalValues()
	{
		this.originalDashCooldown = dashCooldown;
		this.originalDashDuration = dashDuration;
	}

	public void MovementLogic()
	{
		if (entity.VectoreDistanceToTarget.X != 0 && entity.VectoreDistanceToTarget.Y != 0)
		{
			dashCooldown--;
			if (!PlayerShip.Instance.IsDead && dashCooldown == 0)
			{
				while (dashDuration > 0)
				{
					entity.Velocity += entity.VectoreDistanceToTarget * (dashSpeed / entity.VectoreDistanceToTarget.Length());
					dashDuration--;
				}
				dashDuration = originalDashDuration;
				dashCooldown = originalDashCooldown;
			}
		}
	}

	public void DeselerateLogic()
	{
		//throw new NotImplementedException();
	}
}
