using System;

namespace NetLabs.Labs.Lab04;

public static class Delegates
{
	public delegate double Function(double x);

	// f(x)
	public static readonly Function FirstFunction = (x) => 1 / Math.Pow(x, 1.0 / 3.0);
	public static readonly Function SecondFunction = (x) => Math.Sin(x) / Math.Sqrt(x * x);
	public static readonly Function ThirdFunction = (x) => x * Math.Cos(x);
	public static readonly Function FourthFunction = (x) => x * x;

	// f(x)d/x
	public static readonly Function FirstIntegral = (x) => 3 * Math.Pow(x * x, 1.0 / 3.0) / 2;
	public static readonly Function SecondIntegral = (x) => Si(Math.Abs(x));
	public static readonly Function ThirdIntegral = (x) => x * Math.Sin(x) + Math.Cos(x);
	public static readonly Function FourthIntegral = (x) => Math.Pow(x, 3) / 3;

	public static double Si(double x)
	{
		double sum = 0;
		for (int n = 0; n < 100; n++)
		{
			sum += Math.Pow(-1, n) * Math.Pow(x, 2 * n + 1) / (Factorial(2 * n + 1) * (2 * n + 1));
		}
		return sum;
	}

	public static double DefinedIntegral(Function function, double a, double b) => function(b) - function(a);
	public static double RiemannSum(Function function, double a, double b, int n)
	{
		double dx = (b - a) / n;
		double sum = 0;
		for (int i = 0; i < n; i++)
		{
			double x = a + i * dx;
			sum += function(x);
		}
		return sum * dx;
	}

	private static double Factorial(int n)
	{
		if (n <= 1) return 1;
		else return n * Factorial(n - 1);
	}
}
