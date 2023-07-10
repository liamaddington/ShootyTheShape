using ShootyTheShape.Entities.Player.Weapons;
using ShootyTheShape.Entities.Projectiles;
using ShootyTheShape.Managers;
using ShootyTheShape.Services.Input;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShootyTheShape.Entities.Player;
public class PlayerShip : Entity
{
	private static PlayerShip instance;
	public static PlayerShip Instance
	{
		get
		{
			if (instance == null)
				instance = new PlayerShip();

			return instance;
		}
	}

	const int cooldownFrames = 10;
	int cooldownRemaining = 0;
	int invulnerabilityTimer = 0;
	public bool invulnerable = false;

	int framesUntilRespawn = 0;
	public bool IsDead { get { return framesUntilRespawn > 0; } }
	public bool IsShooting { get; set; }

	static Random rand = new Random();

	private IInputService _inputService;

	public Quaternion AimQuaternion;

	private Weapon PrimaryWeapon;
	private Weapon SecondaryWeapon;

	private PlayerShip()
	{
		_inputService = (IInputService)GameRoot.ServiceProvider.GetService(typeof(IInputService));

		texture = contentService.GetPlayerShipTexture();
		Position = GameRoot.ScreenSize / 2;
		Radius = 10;

		PrimaryWeapon = new HomingPlasmaBurstCannon(this, 1, 1, 1, 1);
		SecondaryWeapon = new BasicShot(this, 1, 1, 1, 1);
	}

	public override void Update()
	{
		if (IsDead)
		{
			--framesUntilRespawn;
			invulnerable = true;
			invulnerabilityTimer = 100;
			return;
		}

		if (invulnerabilityTimer > 0)
		{
			--invulnerabilityTimer;
		}
		else
		{
			invulnerable = false;
		}

		MoveShip();
		CheckShooting();
		ApplyWeaponCooldowns();

		this.Orientation = _inputService.GetAimDirection().ToAngle();

	}

	private void MoveShip()
	{
		const float speed = 8;
		Velocity = speed * _inputService.GetMovementDirection();
		Position += Velocity;
		Position = Vector2.Clamp(Position, Size / 2, GameRoot.ScreenSize - Size / 2);
	}

	private void ApplyWeaponCooldowns()
	{
		PrimaryWeapon.ApplyCooldowns();
		SecondaryWeapon.ApplyCooldowns();
	}

	public void CheckShooting()
	{
		if (_inputService.PrimaryFire())
		{
			Vector2 aim = _inputService.GetAimDirection();
			if (aim.LengthSquared() > 0 && cooldownRemaining <= 0)
			{

				PrimaryWeapon.Fire();
			}
			return;
		}

		if (_inputService.SecondaryFire())
		{
			Vector2 aim = _inputService.GetAimDirection();
			if (aim.LengthSquared() > 0 && cooldownRemaining <= 0)
			{
				SecondaryWeapon.Fire();
			}
			return;
		}
	}

	public void Shoot(Vector2 aim)
	{
		cooldownRemaining = cooldownFrames;
		float aimAngle = aim.ToAngle();
		Quaternion aimQuat = Quaternion.CreateFromYawPitchRoll(0, 0, aimAngle);

		float randomSpread = rand.NextFloat(-0.04f, 0.04f) + rand.NextFloat(-0.04f, 0.04f);
		Vector2 velocity = 11f * new Vector2((float)Math.Cos(aimAngle + randomSpread), (float)Math.Sin(aimAngle + randomSpread));

		Vector2 offset = Vector2.Transform(new Vector2(35, -8), aimQuat);
		EntityManager.Add(new WeaponFire(Position + offset, velocity));

		offset = Vector2.Transform(new Vector2(35, 8), aimQuat);
		EntityManager.Add(new WeaponFire(Position + offset, velocity));

		audioService.PlayShotSfx();
	}

	public override void Draw()
	{
		if (!IsDead)
			base.Draw();
	}

	public void Kill()
	{
		--CurrentGameStats.RemainingLives;
		framesUntilRespawn = 60;
	}
}