using System.Collections.Generic;
using ShootyTheShape.AI.Behaviours;
using ShootyTheShape.Entities.Player;

namespace ShootyTheShape.AI;

public class ArtificialIntelligence
{
	public List<Behaviour> Behaviours;

	public ArtificialIntelligence()
	{
		this.Behaviours = new List<Behaviour>();
	}

	public void RunAllBehaviours()
	{
		if (PlayerShip.Instance.IsDead)
		{
			return;
		}

		foreach (var behaviour in Behaviours)
		{
			behaviour.RunBehaviour();
		}
	}
}
