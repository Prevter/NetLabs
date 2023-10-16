using AlgLabs.Common;
using FloxelLib.MVVM;

namespace AlgLabs.MVVM;

public sealed class AboutViewModel : BaseViewModel
{
	public string CpuName { get; set; }
	public string OsName { get; set; }
	public string RamSize { get; set; }
	public string StorageSize { get; set; }
	public string GpuName { get; set; }
	public string MotherboardName { get; set; }
	public string DotnetVersion { get; set; }

	public AboutViewModel()
	{
		CpuName = Hardware.GetCpuName();
		OsName = Hardware.GetOsName();
		RamSize = $"{Hardware.GetRamSize() / 1024 / 1024 / 1024:0.00} GB";
		StorageSize = $"{Hardware.GetTotalStorageSize() / 1024 / 1024 / 1024:0.00} GB";
		GpuName = Hardware.GetGpuName();
		MotherboardName = Hardware.GetMotherboardName();
		DotnetVersion = Hardware.GetRuntimeVersion();
	}
}
