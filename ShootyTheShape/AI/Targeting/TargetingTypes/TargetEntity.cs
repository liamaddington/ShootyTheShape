using ShootyTheShape.Entities;
using ShootyTheShape.Entities.Player;

namespace ShootyTheShape.AI.Targeting.TargetingTypes;

public class TargetEntity : ITargeting
{
	public Entity HostEntity { get; private set; }

	public Entity EntityToTarget { get; private set; }

	public Vector2 VectorDistance { get; private set; }
	public float MeasuredDistance { get; private set; }
	public float TargetRange { get; private set; }
	public bool InRange { get; private set; }

	public TargetEntity(Entity hostEntity, Entity targetEntity, float targetRange)
	{
		this.TargetRange = targetRange;
		this.HostEntity = hostEntity;
		this.EntityToTarget = targetEntity;

		VectorDistance = hostEntity.Position - targetEntity.Position;
		MeasuredDistance = VectorDistance.Length();
	}

	public TargetEntity(Entity hostEntity, float targetRange)
	{
		this.TargetRange = targetRange;
		this.HostEntity = hostEntity;
		this.EntityToTarget = PlayerShip.Instance;
	}

	public TargetEntity(Entity hostEntity) : this(hostEntity, -1)
	{
	}

	public void TargetingLogic()
	{
		VectorDistance = EntityToTarget.Position - HostEntity.Position;
		MeasuredDistance = VectorDistance.Length();
		InRange = TargetRange == -1 || MeasuredDistance < TargetRange;
		if (InRange)
		{
			HostEntity.AimDirection = VectorDistance.ToAngle();
		}
	}
}
