using AlgLabs.Labs.Lab01;
using FloxelLib;
using FloxelLib.Common;
using FloxelLib.MVVM;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AlgLabs.MVVM;

public enum SortType
{
	Bubble,
	Selecting,
	Insertion
}

public sealed partial class Lab01ViewModel : BaseViewModel
{
	private string _arraySize = "1000";
	private SortType _sortType = SortType.Bubble;
	private bool _useDouble = false;

	private double _timeTaken;
	private bool _isWorking;

	private Dictionary<string, List<double>> _times = new()
	{
		{ "Bubble1000Int", new() },
		{ "Bubble1000Double", new() },
		{ "Bubble10000Int", new() },
		{ "Bubble10000Double", new() },
		{ "Bubble100000Int", new() },
		{ "Bubble100000Double", new() },
		{ "Selecting1000Int", new() },
		{ "Selecting1000Double", new() },
		{ "Selecting10000Int", new() },
		{ "Selecting10000Double", new() },
		{ "Selecting100000Int", new() },
		{ "Selecting100000Double", new() },
		{ "Insertion1000Int", new() },
		{ "Insertion1000Double", new() },
		{ "Insertion10000Int", new() },
		{ "Insertion10000Double", new() },
		{ "Insertion100000Int", new() },
		{ "Insertion100000Double", new() },
	};
	private Dictionary<string, double> _averageTimes = new()
	{
		{ "Bubble1000Int", 0 },
		{ "Bubble1000Double", 0 },
		{ "Bubble10000Int", 0 },
		{ "Bubble10000Double", 0 },
		{ "Bubble100000Int", 0 },
		{ "Bubble100000Double", 0 },
		{ "Selecting1000Int", 0 },
		{ "Selecting1000Double", 0 },
		{ "Selecting10000Int", 0 },
		{ "Selecting10000Double", 0 },
		{ "Selecting100000Int", 0 },
		{ "Selecting100000Double", 0 },
		{ "Insertion1000Int", 0 },
		{ "Insertion1000Double", 0 },
		{ "Insertion10000Int", 0 },
		{ "Insertion10000Double", 0 },
		{ "Insertion100000Int", 0 },
		{ "Insertion100000Double", 0 },
	};

	public string ArraySize
	{
		get => _arraySize;
		set => SetField(ref _arraySize, value);
	}

	public SortType SortTypeValue
	{
		get => _sortType;
		set => SetField(ref _sortType, value);
	}

	public bool UseDouble
	{
		get => _useDouble;
		set => SetField(ref _useDouble, value);
	}

	public double TimeTaken
	{
		get => _timeTaken;
		set => SetField(ref _timeTaken, value);
	}

	public bool IsWorking
	{
		get => _isWorking;
		set => SetField(ref _isWorking, value);
	}

	public Dictionary<string, double> AverageTimes
	{
		get => _averageTimes;
		set => SetField(ref _averageTimes, value);
	}

	[RelayCommand]
	private void ChangeSortType(object arg)
	{
		if (arg is SortType sortType)
		{
			SortTypeValue = sortType;
		}
	}

	private delegate void SortDouble(ref double[] array);
	private delegate void SortInt(ref int[] array);

	[RelayCommand]
	private void RunSort(object arg)
	{
		if (!ArraySize.Parse(out int arraySize)) return;

		long starttime;

		if (UseDouble)
		{
			double[] array = new double[arraySize];
			array.FillRandom(max: 1000);

			SortDouble action = SortTypeValue switch
			{
				SortType.Insertion => Sorting.InsertionSort,
				SortType.Selecting => Sorting.SelectionSort,
				_ => Sorting.BubbleSort,
			};

			string key = $"{SortTypeValue}{arraySize}Double";

			Task.Factory.StartNew(() =>
			{
				IsWorking = true;
				starttime = Stopwatch.GetTimestamp();
				action(ref array);
				var endtime = Stopwatch.GetTimePassed(starttime);
				TimeTaken = endtime.TotalMilliseconds;
				IsWorking = false;

				if (_times.ContainsKey(key))
				{
					_times[key].Add(endtime.TotalMilliseconds);
					var average = _times[key].Average();
					AverageTimes[key] = average;
					OnPropertyChanged(nameof(AverageTimes));
				}
			});
		}
		else
		{
			int[] array = new int[arraySize];
			array.FillRandom(max: 1000);

			SortInt action = SortTypeValue switch
			{
				SortType.Insertion => Sorting.InsertionSort,
				SortType.Selecting => Sorting.SelectionSort,
				_ => Sorting.BubbleSort,
			};

			string key = $"{SortTypeValue}{arraySize}Int";

			Task.Factory.StartNew(() =>
			{
				IsWorking = true;
				starttime = Stopwatch.GetTimestamp();
				action(ref array);
				var endtime = Stopwatch.GetTimePassed(starttime);
				TimeTaken = endtime.TotalMilliseconds;
				IsWorking = false;

				if (_times.ContainsKey(key))
				{
					_times[key].Add(endtime.TotalMilliseconds);
					var average = _times[key].Average();
					AverageTimes[key] = average;
					OnPropertyChanged(nameof(AverageTimes));
				}
			});
		}
	}
}
