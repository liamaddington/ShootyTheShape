using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShootyTheShape.AI.Movement.MovementsTypes;
class Teleport : IMovementType
{
	int distance = 20;
	int cooldown = 150;

	public void MovementLogic()
	{
		throw new NotImplementedException();
	}

	public void DeselerateLogic()
	{
		throw new NotImplementedException();
	}
}
