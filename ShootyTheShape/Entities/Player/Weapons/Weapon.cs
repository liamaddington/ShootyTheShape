using ShootyTheShape.Entities.Projectiles;
using ShootyTheShape.Managers;
using ShootyTheShape.Services.Audio;
using ShootyTheShape.Services.Input;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShootyTheShape.Entities.Player.Weapons;
interface IWeapon
{

}

public abstract class Weapon : IWeapon
{
	protected IInputService inputService { get; set; }
	protected IAudioService audioService { get; set; }

	protected Dictionary<Guid, Bullet> ActiveBullets = new();

	protected Entity entityWithWeaponEquipped;

	internal virtual float baseDamage { get; set; }
	internal float damageMultiplier { get; set; }

	internal abstract float baseFireRate { get; set; }
	internal float fireRateMultiplier { get; set; }

	internal abstract float baseShotVelocity { get; set; }
	internal float shotSpeedMultiplier { get; set; }

	internal abstract float baseAccuracy { get; set; }
	internal float accuracyMultiplier { get; set; }

	internal abstract float bulletRadius { get; set; }

	internal float shotCooldown { get; set; } = 0;

	internal Weapon(Entity entityWithWeaponEquipped,
		float damageMultiplier,
		float fireRateMultiplier,
		float shotSpeedMultiplier,
		float accuracyMultiplier)
	{
		this.entityWithWeaponEquipped = entityWithWeaponEquipped;

		this.damageMultiplier = damageMultiplier;
		this.fireRateMultiplier = fireRateMultiplier;
		this.shotSpeedMultiplier = shotSpeedMultiplier;
		this.accuracyMultiplier = accuracyMultiplier;

		inputService = (IInputService)GameRoot.ServiceProvider.GetService(typeof(IInputService));
		audioService = (IAudioService)GameRoot.ServiceProvider.GetService(typeof(IAudioService));

		EntityManager.RemoveBulletFromWeaponActiveBulletsDelegate += RemoveFromActiveBullets;
	}

	public abstract void Fire();

	public void ApplyCooldowns()
	{
		if (shotCooldown <= baseFireRate)
			shotCooldown++;
	}

	public void RemoveFromActiveBullets(Guid bulletId)
	{
		ActiveBullets.Remove(bulletId);
	}
}