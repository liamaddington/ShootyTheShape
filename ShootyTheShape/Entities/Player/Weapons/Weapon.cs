using System;
using System.Collections.Generic;
using ShootyTheShape.Entities.Projectiles;
using ShootyTheShape.Managers;
using ShootyTheShape.Services.Audio;
using ShootyTheShape.Services.Content;
using ShootyTheShape.Services.Input;
using ShootyTheShape.Services.Rendering;

namespace ShootyTheShape.Entities.Player.Weapons;

public abstract class Weapon : IWeapon
{
	protected IInputService inputService { get; set; }
	protected IAudioService audioService { get; set; }
	protected IContentService contentService { get; }
	protected IRenderService renderService { get; }

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
		IContentService contentService,
		IAudioService audioService,
		IRenderService renderService,
		IInputService inputService,
		float damageMultiplier,
		float fireRateMultiplier,
		float shotSpeedMultiplier,
		float accuracyMultiplier)
	{
		this.entityWithWeaponEquipped = entityWithWeaponEquipped;
		this.contentService = contentService;
		this.audioService = audioService;
		this.renderService = renderService;
		this.inputService = inputService;

		this.DamageMultiplier = damageMultiplier;
		this.FireRateMultiplier = fireRateMultiplier;
		this.ShotSpeedMultiplier = shotSpeedMultiplier;
		this.AccuracyMultiplier = accuracyMultiplier;

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
