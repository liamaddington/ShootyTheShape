using Microsoft.Xna.Framework.Graphics;

namespace ShootyTheShape.Services.Rendering;

public class RenderService : IRenderService
{
	private const float CameraFollowSmoothing = 0.1f;

	private RenderTarget2D _mainRenderTarget { get; }
	private SpriteBatch _spriteBatch { get; }

	private GraphicsDevice _graphicsDevice { get; }
	private Vector2 _renderTargetSize { get; }
	private Vector2 _cameraPosition { get; set; }
	private bool _cameraHasBeenInitialized { get; set; }

	public RenderService(Game game,
		GraphicsDevice graphicsDevice,
		int width,
		int height,
		bool mipMap,
		SurfaceFormat preferredFormat,
		DepthFormat preferredDepthFormat,
		SpriteBatch spriteBatch)
	{
		_mainRenderTarget = new RenderTarget2D(graphicsDevice,
			width,
			height,
			mipMap,
			preferredFormat,
			preferredDepthFormat);

		_graphicsDevice = graphicsDevice;
		_renderTargetSize = new Vector2(width, height);

		_spriteBatch = spriteBatch;
	}

	public void StartRenderer()
	{
		_graphicsDevice.Clear(Color.Blue);
		_graphicsDevice.SetRenderTarget(_mainRenderTarget);
		_spriteBatch.Begin(SpriteSortMode.Texture, BlendState.AlphaBlend,
			SamplerState.LinearClamp, DepthStencilState.Default,
			RasterizerState.CullNone);
	}

	public void StopRenderer()
	{
		_spriteBatch.End();
		_graphicsDevice.SetRenderTarget(null);

		DrawAllTexturesToDisplay();
	}

	private void DrawAllTexturesToDisplay()
	{
		_spriteBatch.Begin(SpriteSortMode.Texture, BlendState.AlphaBlend,
			SamplerState.PointClamp, DepthStencilState.Default,
			RasterizerState.CullNone);
		Draw(_mainRenderTarget,
			Vector2.Zero,
			null,
			Color.White,
			0f,
			Vector2.Zero,
			1f, //TODO: Create calculation that will scale the game to any resolution
			SpriteEffects.None,
			0);
		_spriteBatch.End();
	}

	public void Draw(Texture2D texture, Vector2 position, Color color)
	{
		_spriteBatch.Draw(texture, position, color);
	}

	public void DrawWorld(Texture2D texture, Vector2 position, Rectangle? sourceRectangle, Color color, float rotation, Vector2 origin,
		float scale, SpriteEffects effects, float layerDepth)
	{
		Draw(texture, WorldToScreen(position), sourceRectangle, color, rotation, origin, scale, effects, layerDepth);
	}

	public void Draw(Texture2D texture, Vector2 position, Rectangle? sourceRectangle, Color color, float rotation, Vector2 origin,
		float scale, SpriteEffects effects, float layerDepth)
	{
		_spriteBatch.Draw(texture, position, sourceRectangle, color, rotation, origin, scale, effects, layerDepth);
	}

	public void DrawString(SpriteFont spriteFont, string text, Vector2 position, Color color, float rotation, Vector2 origin,
		float scale, SpriteEffects effects, float layerDepth)
	{
		_spriteBatch.DrawString(spriteFont, text, position, color, rotation, origin, scale, effects, layerDepth);
	}

	public void SetCameraPosition(Vector2 worldPosition)
	{
		Vector2 halfScreenSize = _renderTargetSize / 2f;
		Vector2 minimumCameraPosition = halfScreenSize;
		Vector2 maximumCameraPosition = GameRoot.ArenaSize - halfScreenSize;
		Vector2 targetCameraPosition = Vector2.Clamp(worldPosition, minimumCameraPosition, maximumCameraPosition);

		if (!_cameraHasBeenInitialized)
		{
			_cameraPosition = targetCameraPosition;
			_cameraHasBeenInitialized = true;
			return;
		}

		_cameraPosition = Vector2.Lerp(_cameraPosition, targetCameraPosition, CameraFollowSmoothing);
		_cameraPosition = Vector2.Clamp(_cameraPosition, minimumCameraPosition, maximumCameraPosition);
	}

	public Vector2 ScreenToWorld(Vector2 screenPosition)
	{
		return screenPosition + _cameraPosition - (_renderTargetSize / 2f);
	}

	//TODO Find way to use this rather than calculating inside the InputService method
	public Vector2 PositionalOffset()
	{
		//Adjust measurments based on preffered height/width vs actual height/width of the users screen
		return new Vector2(
			GameRoot.Graphics.PreferredBackBufferWidth - _graphicsDevice.Viewport.Width,
			GameRoot.Graphics.PreferredBackBufferHeight - _graphicsDevice.Viewport.Width);
	}

	public Rectangle ScaleOffset(Rectangle rectangle)
	{
		rectangle.Height *= (_graphicsDevice.Viewport.Height / GameRoot.Graphics.PreferredBackBufferHeight);
		rectangle.Width *= (_graphicsDevice.Viewport.Width / GameRoot.Graphics.PreferredBackBufferWidth);

		return rectangle;
	}

	private Vector2 WorldToScreen(Vector2 worldPosition)
	{
		return worldPosition - _cameraPosition + (_renderTargetSize / 2f);
	}
}
