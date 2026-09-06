using ShootyTheShape.AI.Behaviours;
using ShootyTheShape.AI.Movement.MovementsTypes;
using ShootyTheShape.AI.Targeting.TargetingTypes;
using ShootyTheShape.Entities.Projectiles.Enums;
using ShootyTheShape.Services.Audio;
using ShootyTheShape.Services.Content;
using ShootyTheShape.Services.Rendering;

namespace ShootyTheShape.Entities.Projectiles;

public class WeaponFire : Entity
{
	public WeaponFire(Vector2 position, Vector2 velocity, IContentService contentService, IAudioService audioService, IRenderService renderService)
		: base(contentService, audioService, renderService)
	{
		texture = contentService.GetBulletTexture(BulletTypes.Bullet);
		Position = position;
		//Velocity = velocity;
		Radius = 8;

		Behaviours.Add(new FollowEntity(this, new Dash(this), new TargetEntity(this, 100f)));
		Behaviours.Add(new AvoidEntities(this, 100f, 1f));
	}

	public override void Update()
	{
		RunAllBehaviours();
		Position += Velocity;

		if (GameRoot.IsOutsideArena(Position, Radius))
		{
			IsExpired = true;
		}
	}
}
