using System;

namespace AlgLabs.Labs.Lab01;

public static class Sorting
{
	#region IComparable

	public static void BubbleSort<T>(ref T[] array) where T : IComparable
	{
		for (int i = 0; i < array.Length - 1; i++)
		{
			for (int j = 0; j < array.Length - i - 1; j++)
			{
				if (array[j].CompareTo(array[j + 1]) > 0)
				{
					(array[j + 1], array[j]) = (array[j], array[j + 1]);
				}
			}
		}
	}

	public static void SelectionSort<T>(ref T[] array) where T : IComparable
	{
		for (int i = 0; i < array.Length - 1; i++)
		{
			int minIndex = i;

			for (int j = i + 1; j < array.Length; j++)
			{
				if (array[j].CompareTo(array[minIndex]) < 0)
					minIndex = j;
			}

			if (minIndex != i)
			{
				(array[minIndex], array[i]) = (array[i], array[minIndex]);
			}
		}
	}

	public static void InsertionSort<T>(ref T[] array) where T : IComparable
	{
		for (int i = 1; i < array.Length; i++)
		{
			T key = array[i];
			int j = i - 1;

			while (j >= 0 && array[j].CompareTo(key) > 0)
			{
				array[j + 1] = array[j];
				j--;
			}

			array[j + 1] = key;
		}
	}

	#endregion

	#region Int32

	public static void BubbleSort(ref int[] array)
	{
		for (int i = 0; i < array.Length - 1; i++)
		{
			for (int j = 0; j < array.Length - i - 1; j++)
			{
				if (array[j] > array[j + 1])
				{
					(array[j + 1], array[j]) = (array[j], array[j + 1]);
				}
			}
		}
	}

	public static void SelectionSort(ref int[] array)
	{
		for (int i = 0; i < array.Length - 1; i++)
		{
			int minIndex = i;

			for (int j = i + 1; j < array.Length; j++)
			{
				if (array[j] < array[minIndex])
					minIndex = j;
			}

			if (minIndex != i)
			{
				(array[minIndex], array[i]) = (array[i], array[minIndex]);
			}
		}
	}

	public static void InsertionSort(ref int[] array)
	{
		for (int i = 1; i < array.Length; i++)
		{
			int key = array[i];
			int j = i - 1;

			while (j >= 0 && array[j] > key)
			{
				array[j + 1] = array[j];
				j--;
			}

			array[j + 1] = key;
		}
	}

	#endregion

	#region Double

	public static void BubbleSort(ref double[] array)
	{
		for (int i = 0; i < array.Length - 1; i++)
		{
			for (int j = 0; j < array.Length - i - 1; j++)
			{
				if (array[j] > array[j + 1])
				{
					(array[j + 1], array[j]) = (array[j], array[j + 1]);
				}
			}
		}
	}

	public static void SelectionSort(ref double[] array)
	{
		for (int i = 0; i < array.Length - 1; i++)
		{
			int minIndex = i;

			for (int j = i + 1; j < array.Length; j++)
			{
				if (array[j] < array[minIndex])
					minIndex = j;
			}

			if (minIndex != i)
			{
				(array[minIndex], array[i]) = (array[i], array[minIndex]);
			}
		}
	}

	public static void InsertionSort(ref double[] array)
	{
		for (int i = 1; i < array.Length; i++)
		{
			double key = array[i];
			int j = i - 1;

			while (j >= 0 && array[j] > key)
			{
				array[j + 1] = array[j];
				j--;
			}

			array[j + 1] = key;
		}
	}

	#endregion
}
