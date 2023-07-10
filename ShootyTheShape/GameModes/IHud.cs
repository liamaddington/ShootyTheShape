using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShootyTheShape.GameModes;
public interface IHud
{
	SpriteFont Font { get; set; }
	float FontScale { get; set; }
	bool Visable { get; set; }

	void Draw();
}