using ShootyTheShape.Entities;

namespace ShootyTheShape.AI.Targeting;

public interface ITargeting
{
	Entity HostEntity { get; }
	Entity EntityToTarget { get; }

	Vector2 VectorDistance { get; }
	float MeasuredDistance { get; }
	float TargetRange { get; }
	bool InRange { get; }

	void TargetingLogic();
}
