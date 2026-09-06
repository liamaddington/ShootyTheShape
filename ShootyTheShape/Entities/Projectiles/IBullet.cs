using ShootyTheShape.AI.Behaviours;

namespace ShootyTheShape.Entities.Projectiles;

internal interface IBullet
{
	int Damage { get; set; }

	void Update();
	void AddBehaviour(Behaviour behaviour);
}
