using Microsoft.Xna.Framework.Input;
using ShootyTheShape.Entities.Player;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShootyTheShape.Services.Input;
public class InputService : GameComponent, IInputService
{
	private KeyboardState keyboardState, lastKeyboardState;
	private MouseState mouseState, lastMouseState;
	private GamePadState gamepadState, lastGamepadState;

	public Vector2 MousePosition { get { return new Vector2(mouseState.X, mouseState.Y); } }

	private Keys exitGameKey;
	private Buttons exitGameButton;

	////Bindings
	//private Keys primaryFireButton; //TODO: Need to find a way to be able to rebind mouse buttons
	//private Buttons secondaryFireMouseButton;

	private KeyState abilityOneButtonState;
	private KeyState abilityTwoButtonState;
	private KeyState abilityThreeButtonState;
	private KeyState abilityFourButtonState;

	private KeyState ultimateButtonState;

	private KeyState dashButtonState;

	public InputService(Game game) : base(game)
	{
		this.exitGameKey = Keys.Escape;
		this.exitGameButton = Buttons.Start;


		this.abilityOneButtonState = keyboardState[Keys.NumPad1];
		this.abilityTwoButtonState = keyboardState[Keys.NumPad2];
		this.abilityThreeButtonState = keyboardState[Keys.NumPad3];
		this.abilityFourButtonState = keyboardState[Keys.NumPad4];

		this.ultimateButtonState = keyboardState[Keys.F];

		this.dashButtonState = keyboardState[Keys.LeftShift];

		game.Components.Add(this);
	}

	public void Update()
	{
		UpdateLastInputStates();
		GetInputStates();
	}

	private void UpdateLastInputStates()
	{
		lastKeyboardState = keyboardState;
		lastMouseState = mouseState;
		lastGamepadState = gamepadState;
	}

	public void GetInputStates()
	{
		keyboardState = Keyboard.GetState();
		mouseState = Mouse.GetState();
		gamepadState = GamePad.GetState(PlayerIndex.One);
	}

	private bool IsRightThumbstickActive()
	{
		return gamepadState.ThumbSticks.Right.X > 0 ||
			gamepadState.ThumbSticks.Right.X < 0 ||
			gamepadState.ThumbSticks.Right.Y > 0 ||
			gamepadState.ThumbSticks.Right.Y < 0;
	}

	public bool WasGamepadButtonPressed(Buttons button)
	{
		return lastGamepadState.IsButtonUp(button) && gamepadState.IsButtonDown(button);
	}

	public Vector2 GetMovementDirection()
	{
		Vector2 direction = gamepadState.ThumbSticks.Left;
		direction.Y *= -1;  // invert the y-axis

		if (keyboardState.IsKeyDown(Keys.A))
			direction.X -= 1;
		if (keyboardState.IsKeyDown(Keys.D))
			direction.X += 1;
		if (keyboardState.IsKeyDown(Keys.W))
			direction.Y -= 1;
		if (keyboardState.IsKeyDown(Keys.S))
			direction.Y += 1;

		// Clamp the length of the vector to a maximum of 1.
		if (direction.LengthSquared() > 1)
			direction.Normalize();

		return direction;
	}

	public bool ExitGame()
	{
		return keyboardState.IsKeyDown(exitGameKey) || gamepadState.IsButtonDown(exitGameButton);
	}

	public Vector2 GetAimDirection()
	{
		Vector2 direction = MousePosition.PositionalOffset() - PlayerShip.Instance.Position;

		if (direction == Vector2.Zero)
			return Vector2.Zero;
		else
			return Vector2.Normalize(direction);
	}

	public bool PrimaryFire()
	{
		if (mouseState.LeftButton == ButtonState.Pressed)
		{
			return true;
		}
		return IsRightThumbstickActive();
	}

	public bool SecondaryFire()
	{
		throw new NotImplementedException();
	}

	public bool AbilityOne()
	{
		throw new NotImplementedException();
	}

	public bool AbilityTwo()
	{
		throw new NotImplementedException();
	}

	public bool AbilityThree()
	{
		throw new NotImplementedException();
	}

	public bool AbilityFour()
	{
		throw new NotImplementedException();
	}

	public bool Ultimate()
	{
		throw new NotImplementedException();
	}

	public bool Dash()
	{
		throw new NotImplementedException();
	}
}