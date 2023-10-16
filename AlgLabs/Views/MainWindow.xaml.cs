using FloxelLib;

namespace AlgLabs.Views;

/// <summary>
/// Interaction logic for MainWindow.xaml
/// </summary>
public partial class MainWindow
{
	public MainWindow()
	{
		Floxel.InitWindow(this);
		InitializeComponent();
	}
}
