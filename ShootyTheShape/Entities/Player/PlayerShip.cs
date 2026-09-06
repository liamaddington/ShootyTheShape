using System;
using ShootyTheShape.Entities.Player.Weapons;
using ShootyTheShape.Entities.Projectiles;
using ShootyTheShape.Managers;
using ShootyTheShape.Services.Audio;
using ShootyTheShape.Services.Content;
using ShootyTheShape.Services.Input;
using ShootyTheShape.Services.Rendering;

namespace ShootyTheShape.Entities.Player;

public class PlayerShip : Entity
{
	private const float MaximumForwardSpeed = 8f;
	private const float MaximumReverseSpeed = 4f;
	private const float ForwardAcceleration = 0.25f;
	private const float ReverseAcceleration = 0.35f;
	private const float SteeringSpeed = 0.045f;
	private const float RollingResistance = 0.04f;

	private static PlayerShip instance;
	public static PlayerShip Instance
	{
		get
		{
			return instance ?? throw new InvalidOperationException("The player ship must be created during game startup.");
		}
	}

	public static PlayerShip Create(
		IContentService contentService,
		IAudioService audioService,
		IRenderService renderService,
		IInputService inputService)
	{
		if (instance == null)
		{
			instance = new PlayerShip(contentService, audioService, renderService, inputService);
		}

		return instance;
	}

	const int cooldownFrames = 10;
	int cooldownRemaining = 0;
	int invulnerabilityTimer = 0;
	public bool IsInvulnerable = false;

	int framesUntilRespawn = 0;
	public bool IsDead { get { return framesUntilRespawn > 0; } }
	public bool IsShooting { get; set; }

	static Random rand = new Random();

	private IInputService _inputService { get; }
	private float _currentMovementSpeed { get; set; }

	public Quaternion AimQuaternion;

	private Weapon _primaryWeapon { get; }
	private Weapon _secondaryWeapon { get; }

	private PlayerShip(
		IContentService contentService,
		IAudioService audioService,
		IRenderService renderService,
		IInputService inputService)
		: base(contentService, audioService, renderService)
	{
		_inputService = inputService;

		texture = contentService.GetPlayerShipTexture();
		Position = GameRoot.ArenaSize / 2;
		Radius = 10;
		Orientation = 0;

		_primaryWeapon = new HomingPlasmaBurstCannon(this, contentService, audioService, renderService, inputService, 1, 1, 1, 1);
		_secondaryWeapon = new BasicShot(this, contentService, audioService, renderService, inputService, 1, 1, 1, 1);
	}

	public override void Update()
	{
		if (IsDead)
		{
			--framesUntilRespawn;
			IsInvulnerable = true;
			invulnerabilityTimer = 100;
			return;
		}

		if (invulnerabilityTimer > 0)
		{
			--invulnerabilityTimer;
		}
		else
		{
			IsInvulnerable = false;
		}

		MoveShip();
		CheckShooting();
		ApplyWeaponCooldowns();
	}

	private void MoveShip()
	{
		float steeringInput = _inputService.GetSteeringInput();
		float throttleInput = _inputService.GetThrottleInput();

		Orientation = MathHelper.WrapAngle(Orientation + steeringInput * SteeringSpeed);
		ApplyThrottle(throttleInput);

		var forwardDirection = new Vector2((float)Math.Cos(Orientation), (float)Math.Sin(Orientation));
		Velocity = forwardDirection * _currentMovementSpeed;
		Position += Velocity;
		Position = GameRoot.ClampToArena(Position, Size / 2);
	}

	private void ApplyThrottle(float throttleInput)
	{
		if (throttleInput > 0)
		{
			_currentMovementSpeed += ForwardAcceleration * throttleInput;
		}
		else if (throttleInput < 0)
		{
			_currentMovementSpeed += ReverseAcceleration * throttleInput;
		}
		else
		{
			_currentMovementSpeed = MathHelper.Lerp(_currentMovementSpeed, 0, RollingResistance);
		}

		_currentMovementSpeed = MathHelper.Clamp(
			_currentMovementSpeed,
			-MaximumReverseSpeed,
			MaximumForwardSpeed);

		if (Math.Abs(_currentMovementSpeed) < 0.01f)
		{
			_currentMovementSpeed = 0;
		}
	}

	private void ApplyWeaponCooldowns()
	{
		_primaryWeapon.ApplyCooldowns();
		_secondaryWeapon.ApplyCooldowns();
	}

	public void CheckShooting()
	{
		if (_inputService.PrimaryFire())
		{
			Vector2 aim = _inputService.GetAimDirection();
			if (aim.LengthSquared() > 0 && cooldownRemaining <= 0)
			{
				_primaryWeapon.Fire();
			}
			return;
		}

		if (_inputService.SecondaryFire())
		{
			Vector2 aim = _inputService.GetAimDirection();
			if (aim.LengthSquared() > 0 && cooldownRemaining <= 0)
			{
				_secondaryWeapon.Fire();
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
		EntityManager.Add(new WeaponFire(Position + offset, velocity, contentService, audioService, renderService));

		offset = Vector2.Transform(new Vector2(35, 8), aimQuat);
		EntityManager.Add(new WeaponFire(Position + offset, velocity, contentService, audioService, renderService));

		audioService.PlayShotSfx();
	}

	public override void Draw()
	{
		if (!IsDead)
		{
			base.Draw();
		}
	}

	public void Kill()
	{
		--CurrentGameStats.RemainingLives;
		framesUntilRespawn = 60;
		_currentMovementSpeed = 0;
		Velocity = Vector2.Zero;
	}
}
