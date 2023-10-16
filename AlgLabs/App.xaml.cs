using AlgLabs.Common;
using FloxelLib;
using FloxelLib.Common;
using System.Windows;
using System.Windows.Navigation;

namespace AlgLabs;

/// <summary>
/// Interaction logic for App.xaml
/// </summary>
public partial class App : Application
{
	public static Settings Settings { get; set; }

	public App()
	{
		Settings = AppSettings.Load<Settings>();
	}

	protected override void OnLoadCompleted(NavigationEventArgs e)
	{
		base.OnLoadCompleted(e);
		Floxel.SetTheme(Settings.Theme);
	}
}
