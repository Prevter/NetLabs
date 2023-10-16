using FloxelLib.MVVM;
using NetLabs.Labs.Lab03;
using System.Collections.ObjectModel;

namespace NetLabs.MVVM;

public sealed partial class Lab03ViewModel : BaseViewModel
{
	public ObservableCollection<HourGlass> Figures { get; } = new();

	[RelayCommand]
	private void AddFigure()
	{
		int instanceCount = HourGlass.InstanceCount;
		double x = instanceCount * 10;
		double y = instanceCount * 4;
		double width = instanceCount * 2 + 10;
		double height = instanceCount * 5 + 15;

		HourGlass figure = new(width, height)
		{
			X = x,
			Y = y
		};

		Figures.Add(figure);
	}

	[RelayCommand]
	private void Clear()
	{
		Figures.Clear();
	}
}