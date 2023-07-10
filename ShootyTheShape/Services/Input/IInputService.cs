using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShootyTheShape.Services.Input;
public interface IInputService
{
	void Update();

	Vector2 MousePosition { get; }

	Vector2 GetMovementDirection();

	Vector2 GetAimDirection();

	bool ExitGame();

	bool PrimaryFire();

	bool SecondaryFire();

	bool AbilityOne();
	bool AbilityTwo();
	bool AbilityThree();
	bool AbilityFour();

	bool Ultimate();

	bool Dash();
}