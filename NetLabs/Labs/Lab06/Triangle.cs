using System;

namespace NetLabs.Labs.Lab06;

public static class Triangle
{
	public static double GetHypotenuse(double a, double b)
	{
		return Math.Sqrt(a * a + b * b);
	}

	public static double GetArea(double a, double b)
	{
		return a * b / 2;
	}
}
