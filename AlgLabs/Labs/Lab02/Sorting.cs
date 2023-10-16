using System;

namespace AlgLabs.Labs.Lab02;

public struct SortDetails
{
	public SortDetails() { }
	public uint Comparisons { get; set; }
	public uint Swaps { get; set; }
	public uint Memory { get; set; }
}

public static class Sorting
{
	public static SortDetails InsertionSort<T>(ref T[] array) where T : IComparable
	{
		SortDetails details = new()
		{
			Memory = (uint)(1 + array.Length)
		};
		for (int i = 1; i < array.Length; i++)
		{
			T key = array[i];
			int j = i - 1;

			while (j >= 0)
			{
				details.Comparisons++;
				if (array[j].CompareTo(key) > 0)
				{
					array[j + 1] = array[j];
					j--;
					details.Swaps++;
				}
				else break;
			}

			array[j + 1] = key;
			details.Swaps++;
		}

		return details;
	}

	public static SortDetails MergeSort<T>(ref T[] array) where T : IComparable
	{
		SortDetails details = new()
		{
			Memory = (uint)array.Length
		};
		MergeSort(ref array, 0, array.Length - 1, ref details);
		return details;
	}

	private static void MergeSort<T>(ref T[] array, int left, int right, ref SortDetails details) where T : IComparable
	{
		if (left < right)
		{
			int middle = (left + right) / 2;

			MergeSort(ref array, left, middle, ref details);
			MergeSort(ref array, middle + 1, right, ref details);

			Merge(ref array, left, middle, right, ref details);
		}
	}

	private static void Merge<T>(ref T[] array, int lowIndex, int middleIndex, int highIndex, ref SortDetails details) where T : IComparable
	{
		var left = lowIndex;
		var right = middleIndex + 1;
		var tempArray = new T[highIndex - lowIndex + 1];
		details.Memory += (uint)tempArray.Length;
		var index = 0;

		while ((left <= middleIndex) && (right <= highIndex))
		{
			details.Comparisons++;
			if (array[left].CompareTo(array[right]) <= 0)
			{
				tempArray[index] = array[left];
				left++;
			}
			else
			{
				tempArray[index] = array[right];
				right++;
			}

			index++;
		}

		for (var i = left; i <= middleIndex; i++)
		{
			tempArray[index] = array[i];
			index++;
		}

		for (var i = right; i <= highIndex; i++)
		{
			tempArray[index] = array[i];
			index++;
		}

		for (var i = 0; i < tempArray.Length; i++)
		{
			if (!tempArray[i].Equals(array[lowIndex + i]))
				details.Swaps++;
			array[lowIndex + i] = tempArray[i];
		}
	}
}