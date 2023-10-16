namespace AlgLabs.Labs.Lab03;

public static class Sorting
{
	public static void QuickSort(ref int[] array)
	{
		QuickSort(ref array, 0, array.Length - 1);
	}

	private static void QuickSort(ref int[] array, int left, int right)
	{
		while (left < right)
		{
			int p = array[(left + right) / 2];
			int i = left, j = right;
			i--;
			j++;
			while (true)
			{
				while (array[++i] < p) ;
				while (array[--j] > p) ;
				if (i >= j)
					break;
				(array[j], array[i]) = (array[i], array[j]);
			}
			i = j++;
			if ((i - left) <= (right - j))
			{
				QuickSort(ref array, left, i);
				left = j;
			}
			else
			{
				QuickSort(ref array, j, right);
				right = i;
			}
		}
	}
	public static void MergeSort(ref int[] array)
	{
		MergeSort(ref array, 0, array.Length - 1);
	}

	private static void MergeSort(ref int[] array, int left, int right)
	{
		if (left >= right) return;
		int mid = (left + right) / 2;
		MergeSort(ref array, left, mid);
		MergeSort(ref array, mid + 1, right);
		Merge(ref array, left, mid, right);
	}

	private static void Merge(ref int[] array, int left, int mid, int right)
	{
		var temp = new int[right - left + 1];
		int i = left, j = mid + 1, k = 0;
		while (i <= mid && j <= right)
		{
			if (array[i] <= array[j])
			{
				temp[k] = array[i];
				i++;
			}
			else
			{
				temp[k] = array[j];
				j++;
			}
			k++;
		}
		while (i <= mid)
		{
			temp[k] = array[i];
			i++;
			k++;
		}
		while (j <= right)
		{
			temp[k] = array[j];
			j++;
			k++;
		}
		for (i = left; i <= right; i++)
		{
			array[i] = temp[i - left];
		}
	}

	public static void InsertSort(ref int[] array)
	{
		for (var i = 1; i < array.Length; i++)
		{
			int j = i;
			while (j > 0 && array[j - 1] > array[j])
			{
				(array[j - 1], array[j]) = (array[j], array[j - 1]);
				j--;
			}
		}
	}
}