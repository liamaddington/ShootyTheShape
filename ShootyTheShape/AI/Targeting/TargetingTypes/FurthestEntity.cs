using Microsoft.Xna.Framework;
using ShootyTheShape.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShootyTheShape.AI.Targeting.TargetingTypes;
class FurthestTarget : ITargeting
{
	public Entity HostEntity { get; }
	public Entity EntityToTarget { get; }
	public Vector2 VectorDistance { get; }
	public float MeasuredDistance { get; }
	public float targetRange { get; }
	public bool InRange { get; }

	public void TargetingLogic()
	{
		throw new NotImplementedException();
	}
}