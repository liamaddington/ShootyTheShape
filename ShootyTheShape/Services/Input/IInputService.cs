namespace ShootyTheShape.Services.Input;

public interface IInputService
{
	void Update();

	Vector2 MousePosition { get; }

	float GetThrottleInput();

	float GetSteeringInput();

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
