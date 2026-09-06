using ShootyTheShape.AI.Movement;
using ShootyTheShape.AI.Targeting;

namespace ShootyTheShape.AI.Behaviours;

public class BehaviourConfig
{
	public IMovementType MovementType { get; set; }
	public ITargeting TargetingType { get; set; }
	public int InitialAngle { get; set; } = 0;
	public int InitialDelay { get; set; } = 0;
}
