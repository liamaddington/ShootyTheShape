using Microsoft.Xna.Framework.Graphics;

namespace ShootyTheShape.Services.Rendering;

public interface IRenderService
{
	void StartRenderer();

	void StopRenderer();

	void Draw(Texture2D texture, Vector2 position, Color color);

	void DrawWorld(Texture2D texture, Vector2 position, Rectangle? sourceRectangle, Color color, float rotation, Vector2 origin, float scale, SpriteEffects effects, float layerDepth);

	void Draw(Texture2D texture, Vector2 position, Rectangle? sourceRectangle, Color color, float rotation, Vector2 origin, float scale, SpriteEffects effects, float layerDepth);

	void DrawString(SpriteFont spriteFont, string text, Vector2 position, Color color, float rotation, Vector2 origin, float scale, SpriteEffects effects, float layerDepth);

	void SetCameraPosition(Vector2 worldPosition);

	Vector2 ScreenToWorld(Vector2 screenPosition);

	Vector2 PositionalOffset();

	Rectangle ScaleOffset(Rectangle rectangle);
}
