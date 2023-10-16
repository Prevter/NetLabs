using AlgLabs.Labs.Lab02;
using FloxelLib;
using FloxelLib.MVVM;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AlgLabs.MVVM;

public sealed partial class Lab02ViewModel : BaseViewModel
{
	private SortResultGroup _insertion = new(), _merge = new();
	private bool _running = false;

	public SortResultGroup InsertionResult
	{
		get => _insertion;
		set => SetField(ref _insertion, value);
	}
	public SortResultGroup MergeResult
	{
		get => _merge;
		set => SetField(ref _merge, value);
	}

	public bool RunningTests
	{
		get => _running;
		set => SetField(ref _running, value);
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
		int n = 15;
		int size = 30 + 2 * n - (n / 2);

		// test best case
		{
			int[] array = new int[size];
			array.FillRange();

			InsertionResult.Best = Sorting.InsertionSort(ref array);
			MergeResult.Best = Sorting.MergeSort(ref array);
		}

		// test worse case
		{
			int[] array = new int[size];
			int[] copy = new int[size];
			array.FillRange(size - 1, -1);
			array.CopyTo(copy, 0);

			InsertionResult.Worst = Sorting.InsertionSort(ref array);
			MergeResult.Worst = Sorting.MergeSort(ref copy);
		}

		// test random cases
		{
			int[] array = new int[size];

			List<SortDetails> insertion = new();
			List<SortDetails> merge = new();

			for (int i = 0; i < 10000; i++)
			{
				array.FillRandom();
				insertion.Add(Sorting.InsertionSort(ref array));
			}

			InsertionResult.Experiment = new()
			{
				Memory = (uint)insertion.Average(x => x.Memory),
				Comparisons = (uint)insertion.Average(x => x.Comparisons),
				Swaps = (uint)insertion.Average(x => x.Swaps)
			};

			for (int i = 0; i < 10000; i++)
			{
				array.FillRandom();
				merge.Add(Sorting.MergeSort(ref array));
			}

			MergeResult.Experiment = new()
			{
				Memory = (uint)merge.Average(x => x.Memory),
				Comparisons = (uint)merge.Average(x => x.Comparisons),
				Swaps = (uint)merge.Average(x => x.Swaps)
			};
		}
	}

	public sealed class SortResultGroup : BaseViewModel
	{
		private SortDetails _best, _worst, _experiment;

		public SortDetails Best
		{
			get => _best;
			set => SetField(ref _best, value);
		}

		public SortDetails Worst
		{
			get => _worst;
			set => SetField(ref _worst, value);
		}

		public SortDetails Experiment
		{
			get => _experiment;
			set => SetField(ref _experiment, value);
		}
	}
}
