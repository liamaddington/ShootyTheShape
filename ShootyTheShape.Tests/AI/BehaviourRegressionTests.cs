using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using NSubstitute;
using ShootyTheShape.AI.Behaviours;
using ShootyTheShape.AI.Movement;
using ShootyTheShape.AI.Movement.MovementsTypes;
using ShootyTheShape.AI.Targeting;
using ShootyTheShape.AI.Targeting.TargetingTypes;
using ShootyTheShape.Entities;
using ShootyTheShape.Entities.Enums;
using ShootyTheShape.Managers;
using Xunit;

namespace ShootyTheShape.Tests.AI;

// Entity targeting subscribes to global entity notifications.
[CollectionDefinition("Game state", DisableParallelization = true)]
public class GameStateCollection
{
}

[Collection("Game state")]
public class BehaviourRegressionTests : IDisposable
{
	private EntityManager.AddNewDistanceMatrixDelegate _originalEntityAdded { get; } = EntityManager.AddNewEntityToDistanceMatrixDelegate;
	private EntityManager.RemoveEntityDistanceMatrixDelegate _originalEntityRemoved { get; } = EntityManager.RemoveEntityFromDistanceMatrixDelegate;

	[Theory]
	[InlineData(-1, true)]
	[InlineData(6, true)]
	[InlineData(5, false)]
	[InlineData(4, false)]
	public void TargetingPreservesExclusiveRangeAndOnlyAimsAtTargetsInRange(float range, bool expectedInRange)
	{
		var host = new TestEntity { Position = new Vector2(10, 20), AimDirection = -2 };
		var target = new TestEntity { Position = new Vector2(13, 24) };
		var targeting = new TargetEntity(host, target, range);

		targeting.TargetingLogic();

		Assert.Equal(new Vector2(3, 4), targeting.VectorDistance);
		Assert.Equal(5f, targeting.MeasuredDistance);
		Assert.Equal(expectedInRange, targeting.InRange);
		Assert.Equal(expectedInRange ? (float)Math.Atan2(4, 3) : -2, host.AimDirection);
	}

	[Fact]
	public void SmoothFlyingAddsDefaultSpeedToExistingVelocity()
	{
		var entity = new TestEntity { VectorDistanceToTarget = new Vector2(3, 4), Velocity = new Vector2(1, 2) };

		new SmoothFlying(entity).MovementLogic();

		Assert.Equal(1.36f, entity.Velocity.X, 5);
		Assert.Equal(2.48f, entity.Velocity.Y, 5);
	}

	[Theory]
	[InlineData(0, 4)]
	[InlineData(3, 0)]
	[InlineData(0, 0)]
	public void SmoothFlyingPreservesCurrentAxisAlignedMovementRule(float horizontalDistance, float verticalDistance)
	{
		var entity = new TestEntity
		{
			VectorDistanceToTarget = new Vector2(horizontalDistance, verticalDistance),
			Velocity = Vector2.One
		};

		new SmoothFlying(entity).MovementLogic();

		Assert.Equal(Vector2.One, entity.Velocity);
	}

	[Fact]
	public void SmoothFlyingPreservesAccelerationCalculation()
	{
		var entity = new TestEntity
		{
			Position = new Vector2(1, 2),
			TargetPosition = new Vector2(4, 6),
			VectorDistanceToTarget = new Vector2(3, 4)
		};

		new SmoothFlying(entity, 2, 3).MovementLogic();

		Assert.Equal(3.6f, entity.Velocity.X, 5);
		Assert.Equal(4.8f, entity.Velocity.Y, 5);
	}

	[Fact]
	public void DelayedBehaviourWaitsForConfiguredFramesAndRetainsSpawnPosition()
	{
		var entity = new TestEntity { Position = new Vector2(10, 20) };
		var behaviour = new CountingBehaviour(entity, 0, 2);
		entity.Position += new Vector2(3, 4);

		behaviour.RunBehaviour();
		behaviour.RunBehaviour();

		Assert.Equal(0, behaviour.ExecutionCount);
		Assert.Equal(Vector2.UnitX, entity.Velocity);
		Assert.Equal(new Vector2(3, 4), behaviour.VectorDistance);

		behaviour.RunBehaviour();

		Assert.Equal(1, behaviour.ExecutionCount);
	}

	[Fact]
	public void FollowBehaviourUpdatesTargetBeforeMoving()
	{
		var entity = new TestEntity();
		var targeting = Substitute.For<ITargeting>();
		var movement = Substitute.For<IMovementType>();
		targeting.When(target => target.TargetingLogic())
			.Do(_ => targeting.VectorDistance.Returns(new Vector2(3, 4)));
		movement.When(strategy => strategy.MovementLogic())
			.Do(_ => Assert.Equal(new Vector2(3, 4), entity.VectorDistanceToTarget));

		new FollowEntity(entity, movement, targeting).BehaviourLogic();

		Received.InOrder(() =>
		{
			targeting.TargetingLogic();
			movement.MovementLogic();
		});
	}

	[Theory]
	[InlineData(true)]
	[InlineData(false)]
	public void AvoidBehaviourMovesOnlyWhenTargetIsInRange(bool inRange)
	{
		var entity = new TestEntity { VectorDistanceToTarget = Vector2.One };
		var targeting = Substitute.For<ITargeting>();
		var movement = Substitute.For<IMovementType>();
		targeting.InRange.Returns(inRange);
		targeting.VectorDistance.Returns(new Vector2(3, 4));

		new AvoidEntities(entity, movement, targeting).BehaviourLogic();

		targeting.Received(1).TargetingLogic();
		movement.Received(inRange ? 1 : 0).MovementLogic();
		movement.Received(inRange ? 0 : 1).DecelerateLogic();
		Assert.Equal(inRange ? new Vector2(-3, -4) : Vector2.One, entity.VectorDistanceToTarget);
	}

	[Fact]
	public void ClosestTargetPreservesFirstTargetOnDistanceTieAndUpdatesAfterRemoval()
	{
		var host = new TestEntity();
		var firstTarget = new TestEntity { Position = new Vector2(3, 4) };
		var secondTarget = new TestEntity { Position = new Vector2(-3, -4) };
		var targeting = new ClosestEntity(host, new List<EntityTypes>());
		targeting.DistanceMatrixStore.Add(firstTarget.Id, new DistanceMatrixObject(host, firstTarget));
		targeting.DistanceMatrixStore.Add(secondTarget.Id, new DistanceMatrixObject(host, secondTarget));

		targeting.TargetingLogic();
		Assert.Equal(firstTarget.Position, targeting.VectorDistance);

		EntityManager.RemoveEntityFromDistanceMatrixDelegate.Invoke(firstTarget.Id);
		targeting.TargetingLogic();
		Assert.Equal(secondTarget.Position, targeting.VectorDistance);

		EntityManager.RemoveEntityFromDistanceMatrixDelegate.Invoke(secondTarget.Id);
		targeting.TargetingLogic();
		Assert.Equal(Vector2.Zero, targeting.VectorDistance);
	}

	public void Dispose()
	{
		EntityManager.AddNewEntityToDistanceMatrixDelegate = _originalEntityAdded;
		EntityManager.RemoveEntityFromDistanceMatrixDelegate = _originalEntityRemoved;
	}

	private sealed class TestEntity : Entity
	{
		public TestEntity() : base(null, null, null)
		{
		}

		public override void Update()
		{
		}
	}

	private sealed class CountingBehaviour : Behaviour
	{
		public int ExecutionCount { get; private set; }

		public CountingBehaviour(Entity entity, float angle, int delay) : base(entity, angle, delay)
		{
		}

		public override void BehaviourLogic()
		{
			ExecutionCount++;
		}
	}
}
