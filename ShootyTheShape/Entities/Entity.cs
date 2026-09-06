using System;
using Microsoft.Xna.Framework.Graphics;
using ShootyTheShape.AI;
using ShootyTheShape.Services.Audio;
using ShootyTheShape.Services.Content;
using ShootyTheShape.Services.Rendering;

namespace ShootyTheShape.Entities;

public abstract class Entity : ArtificialIntelligence
{
	protected IContentService contentService;
	protected Texture2D texture; //Make this initialise with a default texture
								 // The tint of the image. This will also allow us to change the transparency.
	protected Color color = Color.White;

	protected IAudioService audioService;
	protected IRenderService renderService;

	protected float scale = 1f;

	public Vector2 Position;
	public Vector2 TargetPosition = new Vector2(3f, 3f);
	public Vector2 Velocity;

	public float Orientation = 0.50f;
	public Vector2 VectorDistanceToTarget;
	public float AimDirection = 0.5f;

	public float Radius = 20;   // used for circular collision detection
	public bool IsExpired = false;      // true if the entity was destroyed and should be deleted.

	public Guid Id { get; set; }

	protected Entity(IContentService contentService, IAudioService audioService, IRenderService renderService)
	{
		this.contentService = contentService;
		this.audioService = audioService;
		this.renderService = renderService;
		Id = Guid.NewGuid();
	}

	public Vector2 Size => texture == null ? Vector2.Zero : new Vector2(texture.Width, texture.Height);

	public abstract void Update();

	public virtual void Draw()
	{
		renderService.DrawWorld(texture, Position, null, color, Orientation, Size / 2f, scale, 0, 0);
	}
}
