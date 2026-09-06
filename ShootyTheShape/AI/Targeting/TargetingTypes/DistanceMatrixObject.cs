using ShootyTheShape.Entities;

namespace ShootyTheShape.AI.Targeting.TargetingTypes;

public class DistanceMatrixObject
{
	public DistanceMatrixObject(Entity hostEntity, Entity entityToTarget)
	{
		this.HostEntity = hostEntity;
		this.EntityToTarget = entityToTarget;
	}

	public void Update()
	{
		VectorDistance = EntityToTarget.Position - HostEntity.Position;
		MeasuredDistance = VectorDistance.Length();
	}

	public Vector2 VectorDistance { get; private set; }
	public float MeasuredDistance { get; private set; }
	public Entity HostEntity { get; set; }
	public Entity EntityToTarget { get; set; }
}
