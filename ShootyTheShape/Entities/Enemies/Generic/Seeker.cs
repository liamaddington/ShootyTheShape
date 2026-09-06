using ShootyTheShape.AI.Behaviours;
using ShootyTheShape.Entities.Enemies.Enums;
using ShootyTheShape.Managers;

namespace ShootyTheShape.Entities.Enemies.Generic;

internal class Seeker : Enemy
{
	public EnemyName EnemyName = EnemyName.Seeker;

	public Seeker(Vector2 position) : base(position)
	{
		base.texture = contentService.GetEnemyTexture(EnemyName);
		base.HitPoints = 1;
		base.Radius = base.texture.Width / 2f;

		var followPlayerBehaviour = new FollowEntity(entity: this, acceleration: .7f);

		this.AddBehaviour(followPlayerBehaviour);

		EntityManager.Add(this);
	}
}
