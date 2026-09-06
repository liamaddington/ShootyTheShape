using ShootyTheShape.AI.Behaviours;
using ShootyTheShape.Entities;

namespace ShootyTheShape.Extensions;

public static class EntityExtensions
{
	public static void AddBehaviour(this Entity entity, Behaviour behaviour)
	{
		behaviour.MainEntity = entity;
		entity.Behaviours.Add(behaviour);
	}
}
