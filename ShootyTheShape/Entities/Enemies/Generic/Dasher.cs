using ShootyTheShape.AI.Behaviours;
using ShootyTheShape.AI.Movement.MovementsTypes;
using ShootyTheShape.AI.Targeting.TargetingTypes;
using ShootyTheShape.Entities.Enemies.Enums;
using ShootyTheShape.Entities.Player;
using ShootyTheShape.Managers;
using ShootyTheShape.Services.Audio;
using ShootyTheShape.Services.Content;
using ShootyTheShape.Services.Rendering;

namespace ShootyTheShape.Entities.Enemies.Generic;

internal class Dasher : Enemy
{
	public EnemyName EnemyName = Enums.EnemyName.Dasher;

	public Dasher(Vector2 position, IContentService contentService, IAudioService audioService, IRenderService renderService)
		: base(position, contentService, audioService, renderService)
	{
		base.texture = base.contentService.GetEnemyTexture(EnemyName);
		base.Radius = base.texture.Width / 2f;
		base.HitPoints = 2;

		Behaviours.Add(
			new FollowEntity(
				entity: this,
				movementType: new Dash(this),
				new TargetEntity(
					hostEntity: this,
					targetEntity: PlayerShip.Instance,
					targetRange: 400)
			));

		Behaviours.Add(
			new AvoidEntities(
				entity: this,
				avoidanceSpeed: 20f,
				targeting: new TargetEntity(
					hostEntity: this,
					targetEntity: PlayerShip.Instance,
					targetRange: 180)));

		EntityManager.Add(this);
	}
}
