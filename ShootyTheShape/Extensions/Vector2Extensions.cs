using Microsoft.Xna.Framework.Graphics;
using System;

namespace ShootyTheShape.Extensions;
public static class Vector2Extensions
{

	public static Vector2 PositionalOffset(this Vector2 vector2)
	{
		DisplayMode actualDisplay = GameRoot.graphics.GraphicsDevice.DisplayMode;

		vector2.X *= actualDisplay.Width / GameRoot.graphics.PreferredBackBufferWidth;
		vector2.Y *= actualDisplay.Height / GameRoot.graphics.PreferredBackBufferHeight;

		return vector2;
	}

	// Convert 2D Vector to direction angle
	public static float ToAngle(this Vector2 vector)
	{
		return (float)Math.Atan2(vector.Y, vector.X);
	}

	public static float ToOppositeAngle(this Vector2 vector)
	{
		return (float)Math.Atan2(vector.Y * -1, vector.X * -1); //Change both of these values to their inverted position
	}

	// Convert 2D Vector to Point
	public static Point ToPoint(this Vector2 vector)
	{
		return new Point((int)vector.X, (int)vector.Y);
	}

	// Generate random vector of length between minLengh
	public static Vector2 NextVector2(this Random rand, float minLength, float maxLength)
	{
		double theta = rand.NextDouble() * 2 * Math.PI;
		float length = rand.NextFloat(minLength, maxLength);
		return new Vector2(length * (float)Math.Cos(theta), length * (float)Math.Sin(theta));
	}

}
