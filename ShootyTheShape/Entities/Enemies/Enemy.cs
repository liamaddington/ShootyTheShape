using System;
using ShootyTheShape.Entities.Enemies.Enums;
using ShootyTheShape.Entities.Projectiles;
using ShootyTheShape.Services.Audio;
using ShootyTheShape.Services.Content;
using ShootyTheShape.Services.Rendering;

namespace ShootyTheShape.Entities.Enemies;

public class Enemy : Entity
{
	public static Random rand = new Random();

	public EnemyName Name { get; set; }
	private int timeUntilStart = 60;
	public bool IsActive { get { return timeUntilStart <= 0; } }
	public int PointValue { get; }
	public int HitPoints = 1;
	public bool IsBoss { get; set; }

	public Enemy(Vector2 position, IContentService contentService, IAudioService audioService, IRenderService renderService)
		: base(contentService, audioService, renderService)
	{
		Position = position;
		color = Color.Transparent;
		PointValue = 1;
		IsBoss = false;
	}

	public override void Update()
	{
		if (timeUntilStart <= 0)
		{
			RunAllBehaviours();
		}
		else
		{
			timeUntilStart--;
			color = Color.White * (1 - timeUntilStart / 60f);
		}

		Position += Velocity;
		this.Orientation = Velocity.ToAngle();
		Position = Vector2.Clamp(Position, Size / 2, GameRoot.ScreenSize - Size / 2);

		Velocity *= 0.8f;
	}

	public void MoveAwayFromCollidingEnemy(Enemy other)
	{
		var distanceFromOtherEnemy = Position - other.Position;
		Velocity += 10 * distanceFromOtherEnemy / (distanceFromOtherEnemy.LengthSquared() + 1);
	}

	public void WasShot(Bullet bullet)
	{
		HitPoints -= bullet.Damage;

		if (HitPoints <= 0)
		{
			IsExpired = true;
			//deathSound.Play(0.5f, rand.NextFloat(-0.2f, 0.2f), 0); TODO: Implement enemy specific sounds
			if (!IsBoss)
			{
				++CurrentGameStats.KillCounter;
			}
			else
			{
				++CurrentGameStats.BossKillCounter;
			}
		}
	}

	public void WasTouched()
	{
		IsExpired = true;
		audioService.PlayExplosionSfx();
	}
}
