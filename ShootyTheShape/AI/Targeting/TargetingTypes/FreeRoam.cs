using System;
using ShootyTheShape.Entities;

namespace ShootyTheShape.AI.Targeting.TargetingTypes;

internal class FreeRoam : ITargeting
{
	public Vector2 VectorDistance => new Vector2(0f);

	public float MeasuredDistance => 0;

	public bool InRange => true;

	public Entity HostEntity => throw new NotImplementedException();

	public Entity EntityToTarget => throw new NotImplementedException();

	public float TargetRange => throw new NotImplementedException();

	private Entity hostEntity;

	private int _initialDirection { get; }

	public FreeRoam(Entity hostEntity)
	{
		this.hostEntity = hostEntity;

		_initialDirection = new Random().Next(1, 360);
	}

	public void TargetingLogic()
	{
		hostEntity.AimDirection = _initialDirection;
	}
}
