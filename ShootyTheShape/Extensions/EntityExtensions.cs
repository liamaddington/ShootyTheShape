using ShootyTheShape.AI.Behaviours;
using ShootyTheShape.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShootyTheShape.Extensions;
public static class EntityExtensions
{
	public static void AddBehaviour(this Entity entity, Behaviour behaviour)
	{
		behaviour.MainEntity = entity;
		entity.behaviours.Add(behaviour);
	}
}
