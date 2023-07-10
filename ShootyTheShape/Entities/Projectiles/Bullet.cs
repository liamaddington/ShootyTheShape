using ShootyTheShape.AI.Behaviours;
using ShootyTheShape.Entities.Projectiles.Enums;
using ShootyTheShape.Managers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShootyTheShape.Entities.Projectiles;

public class Bullet : Entity, IBullet
{
	public int damage { get; set; } = 1;

	public Bullet(Vector2 position)
	{
		texture = contentService.GetBulletTexture(BulletTypes.Bullet);
		Position = position;
		Radius = 8;

		EntityManager.Add(this);
		EntityManager.Bullets.Add(this.Id, this);
	}

	public override void Update()
	{
		RunAllBehaviours();

		Position += Velocity;

		this.Orientation = Velocity.ToAngle();

		if (!GameRoot.Viewport.Bounds.Contains(Position.ToPoint()))
			IsExpired = true;
	}

	public void AddBehaviour(Behaviour behaviour)
	{
		this.behaviours.Add(behaviour);
	}
}
