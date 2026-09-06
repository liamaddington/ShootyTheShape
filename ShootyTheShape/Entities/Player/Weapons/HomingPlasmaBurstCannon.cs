using ShootyTheShape.AI.Behaviours;
using ShootyTheShape.AI.Movement.MovementsTypes;
using ShootyTheShape.AI.Targeting.TargetingTypes;
using ShootyTheShape.Entities.Enums;
using ShootyTheShape.Entities.Projectiles;

namespace ShootyTheShape.Entities.Player.Weapons;

internal class HomingPlasmaBurstCannon : Weapon
{
	internal override float BaseDamage { get; set; } = 2f;
	internal override float BaseFireRate { get; set; } = 10;
	internal override float BaseShotVelocity { get; set; } = 1f;
	internal override float BaseAccuracy { get; set; } = .2F;
	internal override float BulletRadius { get; set; } = 8f;

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
		BaseFireRate *= fireRateMultiplier;
	}

	public override void Fire()
	{
		var aimAngle = inputService.GetAimDirection().ToAngle();

		if (ShotCooldown >= BaseFireRate)
		{
			var positionOffset = new Vector2(1, 1);

			var bullet = new Bullet(entityWithWeaponEquipped.Position + positionOffset);

			bullet.AddBehaviour(
				new FollowEntity(
					entity: bullet,
					movementType: new SmoothFlying(bullet, BaseShotVelocity * ShotSpeedMultiplier),
					targeting: new ClosestEntity(bullet, new() { EntityTypes.Enemy })
				));

			base.ActiveBullets.Add(bullet.Id, bullet);

			audioService.PlayShotSfx();

			ShotCooldown = 0;
		}
	}
}
