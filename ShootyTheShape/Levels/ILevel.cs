using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShootyTheShape.Levels;
public interface ILevel
{
	void Update();
	void DrawHud();
}