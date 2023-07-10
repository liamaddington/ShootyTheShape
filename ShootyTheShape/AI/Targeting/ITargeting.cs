using Microsoft.Xna.Framework;
using ShootyTheShape.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShootyTheShape.AI.Targeting;
public interface ITargeting
{
	Entity HostEntity { get; }
	Entity EntityToTarget { get; }

	Vector2 VectorDistance { get; }
	float MeasuredDistance { get; }
	float targetRange { get; }
	bool InRange { get; }

	void TargetingLogic();
}
