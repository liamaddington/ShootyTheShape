using System;
using ShootyTheShape.Entities;

namespace ShootyTheShape.AI.Targeting.TargetingTypes;

internal class InitialClosestEntity : ITargeting
{
	public Entity HostEntity { get; }
	public Entity EntityToTarget { get; }
	public Vector2 VectorDistance { get; }
	public float MeasuredDistance { get; }
	public float TargetRange { get; }
	public bool InRange { get; }

	public void TargetingLogic()
	{
		throw new NotImplementedException();
	}
}
