using ShootyTheShape.AI.Behaviours;
using ShootyTheShape.AI.Movement.MovementsTypes;
using ShootyTheShape.AI.Targeting.TargetingTypes;
using ShootyTheShape.Entities.Projectiles.Enums;

namespace ShootyTheShape.Entities.Projectiles;
public class WeaponFire : Entity
{
	public WeaponFire(Vector2 position, Vector2 velocity)
	{
		texture = contentService.GetBulletTexture(BulletTypes.Bullet);
		Position = position;
		//Velocity = velocity;
		Radius = 8;

		behaviours.Add(new FollowEntity(this, new Dash(this), new TargetEntity(this, 100f)));
		behaviours.Add(new AvoidEntities(this, 100f, 1f));
	}

	public override void Update()
	{
		RunAllBehaviours();
		Position += Velocity;

		// delete bullets that go off-screen
		if (!GameRoot.Viewport.Bounds.Contains(Position.ToPoint()))
			IsExpired = true;
	}
}