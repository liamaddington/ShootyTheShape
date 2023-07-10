using ShootyTheShape.AI.Behaviours;
using ShootyTheShape.AI.Movement.MovementsTypes;
using ShootyTheShape.AI.Targeting.TargetingTypes;
using ShootyTheShape.Entities.Enemies.Enums;
using ShootyTheShape.Entities.Player;
using ShootyTheShape.Managers;

namespace ShootyTheShape.Entities.Enemies.Generic;
class Dasher : Enemy
{
	public EnemyName EnemyName = Enums.EnemyName.Dasher;

	public Dasher(Vector2 position) : base(position)
	{
		base.texture = base.contentService.GetEnemyTexture(EnemyName);
		base.Radius = base.texture.Width / 2f;
		base.HitPoints = 2;


		behaviours.Add(
			new FollowEntity(
				entity: this,
				movementType: new Dash(this),
				new TargetEntity(
					hostEntity: this,
					targetEntity: PlayerShip.Instance,
					targetRange: 400)
			));

		behaviours.Add(
			new AvoidEntities(
				entity: this,
				avoidanceSpeed: 20f,
				targeting: new TargetEntity(
					hostEntity: this,
					targetEntity: PlayerShip.Instance,
					targetRange: 180)));


		EntityManager.Add(this);
	}
}