using System;

namespace ShootyTheShape.Extensions;

public static class MathRandomExtensions
{
	// Generate random number between minValue and maxValue using rand
	public static float NextFloat(this Random rand, float minValue, float maxValue)
	{
		return (float)rand.NextDouble() * (maxValue - minValue) + minValue;
	}
}
