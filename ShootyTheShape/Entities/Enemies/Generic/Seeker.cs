using ShootyTheShape.AI.Behaviours;
using ShootyTheShape.AI.Movement.MovementsTypes;
using ShootyTheShape.AI.Targeting.TargetingTypes;
using ShootyTheShape.Entities.Enemies.Enums;
using ShootyTheShape.Entities.Enums;
using ShootyTheShape.Managers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShootyTheShape.Entities.Enemies.Generic;
class Seeker : Enemy
{
	public EnemyName EnemyName = EnemyName.Seeker;

	public Seeker(Vector2 position) : base(position)
	{
		base.texture = contentService.GetEnemyTexture(EnemyName);
		base.HitPoints = 1;
		base.Radius = base.texture.Width / 2f;

		var followPlayerBehaviour = new FollowEntity(entity: this, acceleration: .7f);

		this.AddBehaviour(followPlayerBehaviour);

		//behaviours.Add(
		//	new AvoidEntities(
		//		entity: this,
		//		avoidanceRadius: 100f,
		//		avoidanceSpeed: 6f));

		//behaviours.Add(
		//	new AvoidEntities(
		//		entity: this,
		//		movementType: new Dash(entity: this, dashSpeed: .1f, dashDuration: 250),
		//		targeting: new ClosestEntity(
		//						entity: this,
		//						targetRange: 130f,
		//						entityTypesToTarget: new List<EntityTypes>() { EntityTypes.enemy }
		//					)
		//		));

		//behaviours.Add(
		//	new AvoidEntities(
		//		entity: this,
		//		movementType: new SmoothFlying(this, .1f, 1000),
		//		targeting: new ClosestEntity(
		//						entity: this,
		//						targetRange: 500f,
		//						entityTypesToTarget: new List<EntityTypes>() { EntityTypes.bullet }
		//					)
		//		));

		EntityManager.Add(this);
	}
}