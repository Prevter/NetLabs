using AlgLabs.Labs.Lab03;
using FloxelLib;
using FloxelLib.Common;
using FloxelLib.MVVM;
using Microsoft.Win32;
using Newtonsoft.Json;
using OxyPlot;
using OxyPlot.Axes;
using OxyPlot.Legends;
using OxyPlot.Series;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Globalization;
using System.IO;
using System.Threading;
using System.Threading.Tasks;

namespace AlgLabs.MVVM;

public sealed partial class Lab03ViewModel : BaseViewModel
{
	private bool _runningTests = false;

	public bool RunningTests
	{
		get => _runningTests;
		set => SetField(ref _runningTests, value);
	}

	private int _threadCount = Environment.ProcessorCount / 2;
	public int ThreadCount
	{
		get => _threadCount;
		set => SetField(ref _threadCount, value);
	}

	public List<SortTab> Tabs { get; set; } = new()
	{
		new() { Name = "Найкращий" },
		new() { Name = "Випадковий", Mode = TestMode.Random },
		new() { Name = "Найгірший", Mode = TestMode.Worst },
	};

	private SortTab _selectedTab;

	public SortTab SelectedTab
	{
		get => _selectedTab;
		set => SetField(ref _selectedTab, value);
	}

	private PlotModel _plotModel = new()
	{
		TextColor = Floxel.IsDarkMode ? OxyColors.White : OxyColors.Black,
		PlotAreaBorderColor = Floxel.IsDarkMode ? OxyColors.White : OxyColors.Black,
		Legends = {
			new Legend {
				LegendPosition = LegendPosition.TopLeft,
				SeriesInvisibleTextColor = Floxel.IsDarkMode ? OxyColors.LightGray : OxyColors.Gray,
			}
		},
	};
	public PlotModel PlotModel
	{
		get => _plotModel;
		set => SetField(ref _plotModel, value);
	}


	public Lab03ViewModel()
	{
		Tabs[0].Reset();
		Tabs[1].Reset();
		Tabs[2].Reset();

		_selectedTab = Tabs[0];
		PlotModel.Axes.Add(new LinearAxis
		{
			Position = AxisPosition.Left,
			Title = "Час (мс)",
			AxislineColor = Floxel.IsDarkMode ? OxyColors.White : OxyColors.Black,
			TicklineColor = Floxel.IsDarkMode ? OxyColors.White : OxyColors.Black,
			ExtraGridlineColor = Floxel.IsDarkMode ? OxyColors.White : OxyColors.Black,
			MajorGridlineColor = Floxel.IsDarkMode ? OxyColors.White : OxyColors.Black,
		});
		PlotModel.Axes.Add(new LinearAxis
		{
			Position = AxisPosition.Bottom,
			Title = "Кількість елементів",
			AxislineColor = Floxel.IsDarkMode ? OxyColors.White : OxyColors.Black,
			TicklineColor = Floxel.IsDarkMode ? OxyColors.White : OxyColors.Black,
			ExtraGridlineColor = Floxel.IsDarkMode ? OxyColors.White : OxyColors.Black,
			MajorGridlineColor = Floxel.IsDarkMode ? OxyColors.White : OxyColors.Black,
			MinorTicklineColor = Floxel.IsDarkMode ? OxyColors.White : OxyColors.Black,
			MinorGridlineColor = Floxel.IsDarkMode ? OxyColors.White : OxyColors.Black,
			Minimum = 50000,
			Maximum = 1000000,
		});

		// add 9 series (3 for each sort) to the plot model (lines)
		PlotModel.Series.Add(new LineSeries
		{
			Title = "Quicksort (найкращий)",
			Color = OxyColors.Red,
			StrokeThickness = 2,
			MarkerSize = 3,
			MarkerStroke = OxyColors.Red,
			MarkerType = MarkerType.Circle,
		});
		PlotModel.Series.Add(new LineSeries
		{
			Title = "Quicksort (випадковий)",
			Color = OxyColors.Orange,
			StrokeThickness = 2,
			MarkerSize = 3,
			MarkerStroke = OxyColors.Orange,
			MarkerType = MarkerType.Circle,
		});
		PlotModel.Series.Add(new LineSeries
		{
			Title = "Quicksort (найгірший)",
			Color = OxyColors.Yellow,
			StrokeThickness = 2,
			MarkerSize = 3,
			MarkerStroke = OxyColors.Yellow,
			MarkerType = MarkerType.Circle,
		});

		PlotModel.Series.Add(new LineSeries
		{
			Title = "Mergesort (найкращий)",
			Color = OxyColors.Green,
			StrokeThickness = 2,
			MarkerSize = 3,
			MarkerStroke = OxyColors.Green,
			MarkerType = MarkerType.Circle,
		});
		PlotModel.Series.Add(new LineSeries
		{
			Title = "Mergesort (випадковий)",
			Color = OxyColors.Blue,
			StrokeThickness = 2,
			MarkerSize = 3,
			MarkerStroke = OxyColors.Blue,
			MarkerType = MarkerType.Circle,
		});
		PlotModel.Series.Add(new LineSeries
		{
			Title = "Mergesort (найгірший)",
			Color = OxyColors.Purple,
			StrokeThickness = 2,
			MarkerSize = 3,
			MarkerStroke = OxyColors.Purple,
			MarkerType = MarkerType.Circle,
		});

		PlotModel.Series.Add(new LineSeries
		{
			Title = "Insertsort (найкращий)",
			Color = OxyColors.Brown,
			StrokeThickness = 2,
			MarkerSize = 3,
			MarkerStroke = OxyColors.Brown,
			MarkerType = MarkerType.Circle,
		});
		PlotModel.Series.Add(new LineSeries
		{
			Title = "Insertsort (випадковий)",
			Color = OxyColors.Gray,
			StrokeThickness = 2,
			MarkerSize = 3,
			MarkerStroke = OxyColors.Gray,
			MarkerType = MarkerType.Circle,
		});
		PlotModel.Series.Add(new LineSeries
		{
			Title = "Insertsort (найгірший)",
			Color = Floxel.IsDarkMode ? OxyColors.White : OxyColors.Black,
			StrokeThickness = 2,
			MarkerSize = 3,
			MarkerStroke = Floxel.IsDarkMode ? OxyColors.White : OxyColors.Black,
			MarkerType = MarkerType.Circle,
		});
	}

	public class SortTab : BaseViewModel
	{
		public string Name { get; set; } = string.Empty;
		public TestMode Mode { get; set; } = TestMode.Best;

		public ObservableCollection<SortResult> Results { get; set; } = new();

		public void Reset()
		{
			Results.Clear();
			Results.AddRange(new List<SortResult>() {
				new(), new(), new(), new(),
				new(), new(), new(), new(),
				new(), new(), new(), new(),
				new(), new(), new(), new(),
				new(), new(), new(), new(),
			});
		}

		public void RenderGraph(PlotModel model)
		{
			var typeIndex = (int)Mode;
			var quickSeries = (LineSeries)model.Series[0 + typeIndex];
			var mergeSeries = (LineSeries)model.Series[3 + typeIndex];
			var insertSeries = (LineSeries)model.Series[6 + typeIndex];

			quickSeries.Points.Clear();
			mergeSeries.Points.Clear();
			insertSeries.Points.Clear();

			for (int i = 0; i < Results.Count; i++)
			{
				int count = 50000 * (i + 1);

				if (Results[i].Quick != "")
					quickSeries.Points.Add(new DataPoint(count, Results[i].QuickTime));
				if (Results[i].Merge != "")
					mergeSeries.Points.Add(new DataPoint(count, Results[i].MergeTime));
				if (Results[i].Insert != "")
					insertSeries.Points.Add(new DataPoint(count, Results[i].InsertTime));
			}

			model.InvalidatePlot(true);
		}
	}

	public sealed class SortResult : BaseViewModel
	{
		private string _quick = string.Empty, _merge = string.Empty, _insert = string.Empty;
		private double _quickTime, _mergeTime, _insertTime;

		public double QuickTime
		{
			get => _quickTime;
			set
			{
				SetField(ref _quickTime, value);
				Quick = value.ToString("0.####", CultureInfo.InvariantCulture);
			}
		}

		public double MergeTime
		{
			get => _mergeTime;
			set
			{
				SetField(ref _mergeTime, value);
				Merge = value.ToString("0.####", CultureInfo.InvariantCulture);
			}
		}

		public double InsertTime
		{
			get => _insertTime;
			set
			{
				SetField(ref _insertTime, value);
				Insert = value.ToString("0.####", CultureInfo.InvariantCulture);
			}
		}

		[JsonIgnore]
		public string Quick
		{
			get => _quick;
			set => SetField(ref _quick, value);
		}

		[JsonIgnore]
		public string Merge
		{
			get => _merge;
			set => SetField(ref _merge, value);
		}

		[JsonIgnore]
		public string Insert
		{
			get => _insert;
			set => SetField(ref _insert, value);
		}
	}

	[RelayCommand]
	private void Import()
	{
		OpenFileDialog dialog = new()
		{
			Filter = "JSON (*.json)|*.json",
			FileName = "lab03.json",
		};

		if (dialog.ShowDialog() == true)
		{
			var json = File.ReadAllText(dialog.FileName);
			var tabs = JsonConvert.DeserializeObject<List<SortTab>>(json);
			Tabs = tabs; OnPropertyChanged(nameof(Tabs));
			SelectedTab = Tabs[0];
			Tabs[0].RenderGraph(PlotModel);
			Tabs[1].RenderGraph(PlotModel);
			Tabs[2].RenderGraph(PlotModel);
		}
	}

	[RelayCommand]
	private void Export()
	{
		SaveFileDialog dialog = new()
		{
			Filter = "JSON (*.json)|*.json",
			FileName = "lab03.json",
		};

		if (dialog.ShowDialog() == true)
		{
			var json = JsonConvert.SerializeObject(Tabs, Formatting.Indented);
			File.WriteAllText(dialog.FileName, json);
		}
	}


	[RelayCommand]
	private void Run()
	{
		RunningTests = true;
		Task.Factory.StartNew(() =>
		{
			RunTests();
			RunningTests = false;
		});
	}

	private void RunTests()
	{
		var tab = SelectedTab;
		tab.Reset();

		List<Thread> threads = new();

		for (var i = 0; i < 20; i++)
		{
			int index = i;
			Thread thread = new(() =>
			{
				tab.Results[index] = new();

				tab.Results[index].QuickTime = RunSortTest(50000 * (index + 1), Sorting.QuickSort, tab.Mode);
				tab.RenderGraph(PlotModel);
				tab.Results[index].MergeTime = RunSortTest(50000 * (index + 1), Sorting.MergeSort, tab.Mode);
				tab.RenderGraph(PlotModel);
				tab.Results[index].InsertTime = RunSortTest(50000 * (index + 1), Sorting.InsertSort, tab.Mode);
				tab.RenderGraph(PlotModel);
			});

			threads.Add(thread);
		}

		int maxThreads = ThreadCount;
		for (var i = 0; i < maxThreads; i++)
		{
			threads[i].Start();
		}

		threads[0].Join();

		for (int i = maxThreads; i < threads.Count; i++)
		{
			threads[i].Start();
			threads[i - maxThreads].Join();
		}

		foreach (var thread in threads)
		{
			thread.Join();
		}
	}

	private delegate void SortTest(ref int[] array);

	private static double RunSortTest(int count, SortTest method, TestMode fillType)
	{
		var arr = new int[count];
		switch (fillType)
		{
			case TestMode.Best:
				arr.FillRange();
				break;
			case TestMode.Random:
				arr.FillRandom();
				break;
			case TestMode.Worst:
				arr.FillRange(arr.Length - 1, -1);
				break;
		}

		long start = Stopwatch.GetTimestamp();
		method(ref arr);

		var result = Stopwatch.GetTimePassed(start);

		// assert that the array is sorted
		for (int i = 0; i < arr.Length - 1; i++)
		{
			if (arr[i] > arr[i + 1])
				throw new Exception("Array is not sorted!");
		}

		return result.TotalMilliseconds;
	}

	public enum TestMode
	{
		Best,
		Random,
		Worst
	}
}