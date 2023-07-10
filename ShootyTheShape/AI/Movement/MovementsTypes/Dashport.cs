using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShootyTheShape.AI.Movement.MovementsTypes;
class Dashport : IMovementType
{
	float dashStartSpeed = .5f;
	float dashEndSpeed = 60;
	int dashStartDuration = 5;
	int dashEndDuration = 5;
	int distance = 20;
	int dashCooldown = 60;

	public void MovementLogic()
	{
		throw new NotImplementedException();
	}

	public void DeselerateLogic()
	{
		throw new NotImplementedException();
	}
}
