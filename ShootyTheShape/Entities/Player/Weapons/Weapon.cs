using System;
using System.Collections.Generic;
using ShootyTheShape.Entities.Projectiles;
using ShootyTheShape.Managers;
using ShootyTheShape.Services.Audio;
using ShootyTheShape.Services.Input;

namespace ShootyTheShape.Entities.Player.Weapons;

public abstract class Weapon : IWeapon
{
	protected IInputService inputService { get; set; }
	protected IAudioService audioService { get; set; }

	protected Dictionary<Guid, Bullet> ActiveBullets = new();

	protected Entity entityWithWeaponEquipped;

	internal virtual float BaseDamage { get; set; }
	internal float DamageMultiplier { get; set; }

	internal abstract float BaseFireRate { get; set; }
	internal float FireRateMultiplier { get; set; }

	internal abstract float BaseShotVelocity { get; set; }
	internal float ShotSpeedMultiplier { get; set; }

	internal abstract float BaseAccuracy { get; set; }
	internal float AccuracyMultiplier { get; set; }

	internal abstract float BulletRadius { get; set; }

	internal float ShotCooldown { get; set; } = 0;

	internal Weapon(Entity entityWithWeaponEquipped,
		float damageMultiplier,
		float fireRateMultiplier,
		float shotSpeedMultiplier,
		float accuracyMultiplier)
	{
		this.entityWithWeaponEquipped = entityWithWeaponEquipped;

		this.DamageMultiplier = damageMultiplier;
		this.FireRateMultiplier = fireRateMultiplier;
		this.ShotSpeedMultiplier = shotSpeedMultiplier;
		this.AccuracyMultiplier = accuracyMultiplier;

		inputService = (IInputService)GameRoot.ServiceProvider.GetService(typeof(IInputService));
		audioService = (IAudioService)GameRoot.ServiceProvider.GetService(typeof(IAudioService));

		EntityManager.RemoveBulletFromWeaponActiveBulletsDelegate += RemoveFromActiveBullets;
	}

	public abstract void Fire();

	public void ApplyCooldowns()
	{
		if (ShotCooldown <= BaseFireRate)
		{
			ShotCooldown++;
		}
	}

	public void RemoveFromActiveBullets(Guid bulletId)
	{
		ActiveBullets.Remove(bulletId);
	}
}
