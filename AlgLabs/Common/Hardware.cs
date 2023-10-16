using Microsoft.Win32;
using System;
using System.Linq;
using System.Management;
using System.Text.RegularExpressions;

namespace AlgLabs.Common;

public static partial class Hardware
{
	public static string GetRuntimeVersion() => Environment.Version.ToString();

	public static string GetCpuName()
	{
		string path = @"HKEY_LOCAL_MACHINE\HARDWARE\DESCRIPTION\System\CentralProcessor\0";
		string cpu = (string?)Registry.GetValue(path, "ProcessorNameString", "Unknown") ?? "Unknown";
		double speed = (int?)Registry.GetValue(path, "~MHz", 0) ?? 0;

		Regex regex = CpuNameRegex();
		Match match = regex.Match(cpu);
		string vendor = match.Groups["vendor"].Value;
		string family = match.Groups["family"].Value;
		string model = match.Groups["model"].Value;

		return $"{vendor} {family}{model} ({speed / 1000:0.00}GHz)";
	}

	public static string GetOsName()
	{
		string result = string.Empty;
		ManagementObjectSearcher searcher = new("SELECT Caption FROM Win32_OperatingSystem");
		foreach (ManagementObject os in searcher.Get().Cast<ManagementObject>())
		{
			result = os["Caption"].ToString() ?? "Unknown";
			break;
		}

		// Remove everything before Windows
		int start = result.IndexOf("Windows", StringComparison.OrdinalIgnoreCase);
		if (start >= 0) result = result[start..];

		result = result.Trim();
		result += Environment.Is64BitOperatingSystem ? " 64-bit" : " 32-bit"; // get 32 or 64 bit
		result += $" (v.{Environment.OSVersion.Version})"; // get version

		return result;
	}

	public static double GetRamSize()
	{
		double result = 0;
		ManagementObjectSearcher searcher = new("SELECT Capacity FROM Win32_PhysicalMemory");
		foreach (ManagementObject ram in searcher.Get().Cast<ManagementObject>())
		{
			result += Convert.ToDouble(ram["Capacity"]);
		}

		return result;
	}

	public static double GetTotalStorageSize()
	{
		double result = 0;
		ManagementObjectSearcher searcher = new("SELECT Size FROM Win32_DiskDrive");
		foreach (ManagementObject storage in searcher.Get().Cast<ManagementObject>())
		{
			result += Convert.ToDouble(storage["Size"]);
		}

		return result;
	}

	public static string GetGpuName()
	{
		string result = string.Empty;
		ManagementObjectSearcher searcher = new("SELECT * FROM Win32_VideoController");
		foreach (ManagementObject mo in searcher.Get().Cast<ManagementObject>())
		{
			PropertyData currentBitsPerPixel = mo.Properties["CurrentBitsPerPixel"];
			PropertyData description = mo.Properties["Description"];
			if (currentBitsPerPixel != null && description != null)
			{
				if (currentBitsPerPixel.Value != null)
					result = $"{description.Value}";
			}
		}

		return result;
	}

	public static string GetMotherboardName()
	{
		string result = string.Empty;
		ManagementObjectSearcher searcher = new("SELECT * FROM Win32_BaseBoard");
		foreach (ManagementObject mo in searcher.Get().Cast<ManagementObject>())
		{
			PropertyData manufacturer = mo.Properties["Manufacturer"];
			PropertyData product = mo.Properties["Product"];
			if (manufacturer != null && product != null)
			{
				if (manufacturer.Value != null && product.Value != null)
					result = $"{product.Value}";
			}
		}
		return result;
	}

	[GeneratedRegex(@"(?:\d{1,}th Gen)?(?:Genuine)? ?(?'vendor'AMD|Intel)(?:\(R\)|\(r\))? (?'family'[a-zA-Z]*)(?:\(TM\)|\(tm\))?(?'model'.*?)( [a-zA-Z]{1,}-Core| with| CPU @| @|$)")]
	private static partial Regex CpuNameRegex();
}
