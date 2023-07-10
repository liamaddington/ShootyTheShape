using ShootyTheShape.AI.Behaviours;
using ShootyTheShape.Entities.Projectiles;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShootyTheShape.Entities.Player.Weapons;
class HomingPlasmaBurstCannon : Weapon
{
	private float damageMultiplier;
	internal override float baseDamage { get; set; } = 2f;
	internal override float baseFireRate { get; set; } = 10;
	internal override float baseShotVelocity { get; set; } = 1f;
	internal override float baseAccuracy { get; set; } = .2F;
	internal override float bulletRadius { get; set; } = 8f;

	public HomingPlasmaBurstCannon(
		Entity entityWithWeaponEquipped,
		float damageMultiplier,
		float fireRateMultiplier,
		float shotSpeedMultiplier,
		float accuracyMultiplier)
		: base(
			  entityWithWeaponEquipped,
			  damageMultiplier,
			  fireRateMultiplier,
			  shotSpeedMultiplier,
			  accuracyMultiplier)
	{
		baseFireRate *= fireRateMultiplier;
	}

	public override void Fire()
	{
		var aimAngle = inputService.GetAimDirection().ToAngle();

		if (shotCooldown >= baseFireRate)
		{
			var positionOffset = new Vector2(1, 1);

			var bullet = new Bullet(entityWithWeaponEquipped.Position + positionOffset);

			bullet.AddBehaviour(
				new FollowEntity(
					entity: bullet,
					acceleration: baseShotVelocity * shotSpeedMultiplier,
					initialShotAimAngle: aimAngle,
					delayedBehaviourDuration: 30
				));


			//bullet.AddBehaviour(
			//	new AvoidEntities(
			//		entity: bullet, 
			//		avoidanceSpeed: 5f, 
			//		targeting: new ClosestEntity(
			//			entity: bullet, 
			//			targetRange: 100f, 
			//			entityTypesToTarget: new List<EntityTypes>() { EntityTypes.bullet }
			//		)
			//	));

			base.ActiveBullets.Add(bullet.Id, bullet);

			audioService.PlayShotSfx();

			shotCooldown = 0;
		}

	}

}