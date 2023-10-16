using FloxelLib;

namespace NetLabs.Common;

public sealed class Settings
{
	public string Theme { get; set; } = Floxel.CurrentTheme;
	public int SelectedTab { get; set; } = 0;
}
