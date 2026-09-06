using Microsoft.Xna.Framework.Graphics;

namespace ShootyTheShape.GameModes;

public interface IHud
{
	SpriteFont Font { get; set; }
	float FontScale { get; set; }
	bool Visible { get; set; }

	void Draw();
}
