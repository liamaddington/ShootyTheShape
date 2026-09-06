using ShootyTheShape.AI.Behaviours;
using ShootyTheShape.Entities.Projectiles.Enums;
using ShootyTheShape.Managers;
using ShootyTheShape.Services.Audio;
using ShootyTheShape.Services.Content;
using ShootyTheShape.Services.Rendering;

namespace ShootyTheShape.Entities.Projectiles;

public class Bullet : Entity, IBullet
{
	public int Damage { get; set; } = 1;

	public Bullet(Vector2 position, IContentService contentService, IAudioService audioService, IRenderService renderService)
		: base(contentService, audioService, renderService)
	{
		texture = contentService.GetBulletTexture(BulletTypes.Bullet);
		Position = position;
		Radius = 8;

		EntityManager.Add(this);
		EntityManager.Bullets.Add(this.Id, this);
	}

	public override void Update()
	{
		RunAllBehaviours();

		Position += Velocity;

		this.Orientation = Velocity.ToAngle();

		if (!GameRoot.Viewport.Bounds.Contains(Position.ToPoint()))
		{
			IsExpired = true;
		}
	}

	public void AddBehaviour(Behaviour behaviour)
	{
		this.Behaviours.Add(behaviour);
	}
}
