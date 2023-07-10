using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShootyTheShape.Services.Rendering;
public interface IRenderService
{
	void StartRenderer();

	void StopRenderer();

	void Draw(Texture2D texture, Vector2 position, Color color);

	void Draw(Texture2D texture, Vector2 position, Rectangle? sourceRectangle, Color color, float rotation, Vector2 origin, float scale, SpriteEffects effects, float layerDepth);

	void DrawString(SpriteFont spriteFont, string text, Vector2 position, Color color, float rotation, Vector2 origin, float scale, SpriteEffects effects, float layerDepth);

	Vector2 PositionalOffset();

	Rectangle ScaleOffset(Rectangle rectangle);
}