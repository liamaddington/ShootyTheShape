using Microsoft.Xna.Framework;
using ShootyTheShape.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShootyTheShape.AI.Targeting.TargetingTypes;
class FreeRoam : ITargeting
{
	public Vector2 VectorDistance => new Vector2(0f);

	public float MeasuredDistance => 0;

	public bool InRange => true;

	public Entity HostEntity => throw new NotImplementedException();

	public Entity EntityToTarget => throw new NotImplementedException();

	public float targetRange => throw new NotImplementedException();

	private Entity hostEntity;

	private readonly int initialDirection;
	private int degreeCorrection = 90;

	public FreeRoam(Entity hostEntity)
	{
		this.hostEntity = hostEntity;

		initialDirection = new Random().Next(1, 360);
	}

	public void TargetingLogic()
	{
		hostEntity.AimDirection = initialDirection;
	}
}
