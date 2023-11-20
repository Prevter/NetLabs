using FloxelLib;
using NetLabs.Labs.Lab08;

namespace NetLabs.Common;

public sealed class Settings
{
	public string Theme { get; set; } = Floxel.CurrentTheme;
	public int SelectedTab { get; set; } = 0;
	public LabSettings Lab8 { get; set; } = new("HOME-PC\\SQLEXPRESS", "DotnetLab8");
}
