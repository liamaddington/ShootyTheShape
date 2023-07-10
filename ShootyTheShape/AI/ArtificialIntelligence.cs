using ShootyTheShape.AI.Behaviours;
using ShootyTheShape.Entities.Player;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShootyTheShape.AI;
public class ArtificialIntelligence
{
	public List<Behaviour> behaviours;

	public ArtificialIntelligence()
	{
		this.behaviours = new List<Behaviour>();
	}

	public void RunAllBehaviours()
	{
		if (!PlayerShip.Instance.IsDead)
		{
			foreach (var behaviour in behaviours)
			{
				behaviour.RunBehaviour();
			}
		}
	}
}