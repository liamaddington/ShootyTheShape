using ShootyTheShape.Entities;
using ShootyTheShape.Entities.Player;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShootyTheShape.AI.Targeting.TargetingTypes;
public class TargetEntity : ITargeting
{
	public Entity HostEntity { get; private set; }

	public Entity EntityToTarget { get; private set; }

	public Vector2 VectorDistance { get; private set; }
	public float MeasuredDistance { get; private set; }
	public float targetRange { get; private set; }
	public bool InRange { get; private set; }

	public TargetEntity(Entity hostEntity, Entity targetEntity, float targetRange)
	{
		this.targetRange = targetRange;
		this.HostEntity = hostEntity;
		this.EntityToTarget = targetEntity;

		VectorDistance = hostEntity.Position - targetEntity.Position;
		MeasuredDistance = VectorDistance.Length();
	}

	public TargetEntity(Entity hostEntity, float targetRange)
	{
		this.targetRange = targetRange;
		this.HostEntity = hostEntity;
		this.EntityToTarget = PlayerShip.Instance;
	}

	public TargetEntity(Entity hostEntity)
	{
		this.HostEntity = hostEntity;
		this.targetRange = -1;
		this.EntityToTarget = PlayerShip.Instance;
	}

	public void TargetingLogic()
	{
		VectorDistance = EntityToTarget.Position - HostEntity.Position;
		MeasuredDistance = VectorDistance.Length();
		if (targetRange != -1)
		{
			if (MeasuredDistance < targetRange)
			{
				HostEntity.AimDirection = VectorDistance.ToAngle();
				InRange = true;
			}
			else
			{
				InRange = false;
			}
		}
		else
		{
			HostEntity.AimDirection = VectorDistance.ToAngle();
			InRange = true;
		}
	}
}