using System;
using System.IO;
using System.Reflection;

namespace NetLabs.Labs.Lab02;

public static class Wrapper
{
	private const string DllName = "NetLab2.dll";
	private static readonly MethodInfo CountSpacesMI;
	private static readonly MethodInfo LatinToUpperMI;

	static Wrapper()
	{
		string path = Path.Combine(Environment.CurrentDirectory, DllName);
		Assembly assembly = Assembly.LoadFile(path);
		Type type = assembly.GetType("NetLab2.Lab2Lib")
			?? throw new Exception("Failed to load type");

		CountSpacesMI = type.GetMethod("CountSpaces")
			?? throw new Exception("CountSpaces does not exist");

		LatinToUpperMI = type.GetMethod("LatinToUpper")
			?? throw new Exception("LatinToUpper does not exist");
	}

	public static int CountSpaces(string str)
	{
		object[] args = { str };
		object res = CountSpacesMI.Invoke(null, args)
			?? throw new Exception("CountSpaces failed");
		return (int)res;
	}

	public static string LatinToUpper(string str)
	{
		object[] args = { str };
		object res = LatinToUpperMI.Invoke(null, args)
			?? throw new Exception("LatinToUpper failed");
		return (string)res;
	}
}