using ShootyTheShape.AI.Behaviours;
using ShootyTheShape.AI.Movement.MovementsTypes;
using ShootyTheShape.AI.Targeting.TargetingTypes;
using ShootyTheShape.Entities.Enemies.Enums;
using ShootyTheShape.Managers;
using ShootyTheShape.Services.Audio;
using ShootyTheShape.Services.Content;
using ShootyTheShape.Services.Rendering;

namespace ShootyTheShape.Entities.Enemies.Bosses;

internal class Zain : Enemy
{
	public Zain(Vector2 position, IContentService contentService, IAudioService audioService, IRenderService renderService)
		: base(position, contentService, audioService, renderService)
	{
		base.Name = EnemyName.Zain;
		texture = contentService.GetEnemyTexture(Name);
		HitPoints = 100;
		IsBoss = true;
		Radius = texture.Width / 2f;

		Behaviours.Add(new FollowEntity(this, new SmoothFlying(this, .5f), new TargetEntity(this)));
		Behaviours.Add(new AvoidEntities(this, new SmoothFlying(this, .5f), new TargetEntity(this)));

		EntityManager.Add(this);
	}
}
