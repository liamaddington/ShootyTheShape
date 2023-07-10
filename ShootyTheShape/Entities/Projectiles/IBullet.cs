using ShootyTheShape.AI.Behaviours;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShootyTheShape.Entities.Projectiles;
interface IBullet
{
	int damage { get; set; }

	void Update();
	void AddBehaviour(Behaviour behaviour);
}