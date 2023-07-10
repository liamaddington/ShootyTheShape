using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShootyTheShape.Services.Rendering;
public class RenderService : IRenderService
{
	private RenderTarget2D mainRenderTarget;
	private SpriteBatch spriteBatch;

	private GraphicsDevice graphicsDevice;

	private Game game;

	public RenderService(Game game,
		GraphicsDevice graphicsDevice,
		int width,
		int height,
		bool mipMap,
		SurfaceFormat preferredFormat,
		DepthFormat preferredDepthFormat,
		SpriteBatch spriteBatch)
	{
		this.game = game;

		this.mainRenderTarget = new RenderTarget2D(graphicsDevice,
			width,
			height,
			mipMap,
			preferredFormat,
			preferredDepthFormat);

		this.graphicsDevice = graphicsDevice;

		this.spriteBatch = spriteBatch;
	}

	public void StartRenderer()
	{
		graphicsDevice.Clear(Color.Blue);
		graphicsDevice.SetRenderTarget(mainRenderTarget);
		spriteBatch.Begin(SpriteSortMode.Texture, BlendState.AlphaBlend,
			SamplerState.LinearClamp, DepthStencilState.Default,
			RasterizerState.CullNone);
	}

	public void StopRenderer()
	{
		spriteBatch.End();
		graphicsDevice.SetRenderTarget(null);

		DrawAllTexturesToDisplay();
	}

	private void DrawAllTexturesToDisplay()
	{
		spriteBatch.Begin(SpriteSortMode.Texture, BlendState.AlphaBlend,
			SamplerState.PointClamp, DepthStencilState.Default,
			RasterizerState.CullNone);
		Draw(mainRenderTarget,
			Vector2.Zero,
			null,
			Color.White,
			0f,
			Vector2.Zero,
			1f, //TODO: Create calculation that will scale the game to any resolution
			SpriteEffects.None,
			0);
		spriteBatch.End();
	}

	public void Draw(Texture2D texture, Vector2 position, Color color)
	{
		spriteBatch.Draw(texture, position, color);
	}

	public void Draw(Texture2D texture, Vector2 position, Rectangle? sourceRectangle, Color color, float rotation, Vector2 origin,
		float scale, SpriteEffects effects, float layerDepth)
	{
		spriteBatch.Draw(texture, position, sourceRectangle, color, rotation, origin, scale, effects, layerDepth);
	}

	public void DrawString(SpriteFont spriteFont, string text, Vector2 position, Color color, float rotation, Vector2 origin,
		float scale, SpriteEffects effects, float layerDepth)
	{
		spriteBatch.DrawString(spriteFont, text, position, color, rotation, origin, scale, effects, layerDepth);
	}

	//TODO Find way to use this rather than calculating inside the InputService method
	public Vector2 PositionalOffset()
	{
		//Adjust measurments based on preffered height/width vs actual height/width of the users screen
		return new Vector2(
			GameRoot.graphics.PreferredBackBufferWidth - graphicsDevice.Viewport.Width,
			GameRoot.graphics.PreferredBackBufferHeight - graphicsDevice.Viewport.Width);
	}

	private int WidthScaleOffset(int width)
	{
		return width * (GameRoot.graphics.PreferredBackBufferWidth / graphicsDevice.Viewport.Width);
	}

	private int HeightScaleOffset(int height)
	{
		return height * (GameRoot.graphics.PreferredBackBufferWidth / graphicsDevice.Viewport.Width);
	}

	public Rectangle ScaleOffset(Rectangle rectangle)
	{

		rectangle.Height *= (graphicsDevice.Viewport.Height / GameRoot.graphics.PreferredBackBufferHeight);
		rectangle.Width *= (graphicsDevice.Viewport.Width / GameRoot.graphics.PreferredBackBufferWidth);

		return rectangle;
	}

}