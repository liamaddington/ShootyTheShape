using ShootyTheShape.AI.Behaviours;
using ShootyTheShape.AI.Movement.MovementsTypes;
using ShootyTheShape.AI.Targeting.TargetingTypes;
using ShootyTheShape.Entities.Enemies.Enums;
using ShootyTheShape.Managers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShootyTheShape.Entities.Enemies.Bosses;
class Zain : Enemy
{
	public Zain(Vector2 position) : base(position)
	{
		base.Name = EnemyName.Zain;
		texture = contentService.GetEnemyTexture(Name);
		HitPoints = 100;
		IsBoss = true;
		Radius = texture.Width / 2f;

		behaviours.Add(new FollowEntity(this, new SmoothFlying(this, .5f), new TargetEntity(this)));
		behaviours.Add(new AvoidEntities(this, new SmoothFlying(this, .5f), new TargetEntity(this)));

		EntityManager.Add(this);
	}

}