namespace NetLabs.Common;

public sealed class LabTab
{
	public string? Name { get; set; }
	public string? Content { get; set; }
	public string Icon { get; set; } = "FileOutline";
	public bool HasIcon { get; set; }
}