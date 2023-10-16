using AlgLabs.Labs.Bonus;
using FloxelLib.Common;
using FloxelLib.MVVM;
using Microsoft.Win32;
using System;
using System.IO;

namespace AlgLabs.MVVM;

public sealed partial class BonusViewModel : BaseViewModel
{
	private byte[]? _input;
	private byte[]? _output;

	private bool _useRLE;
	public bool UseRLE { get => _useRLE; set => SetField(ref _useRLE, value); }

	public int OriginalSize => _input?.Length ?? 0;
	public int EncodedSize => _output?.Length ?? 0;
	public double CompressionRatio
	{
		get
		{
			if (_input == null || _output == null)
				return 0;
			return (double)_output.Length / _input.Length * 100;
		}
	}

	[RelayCommand]
	private void SelectFile()
	{
		OpenFileDialog openFileDialog = new();
		if (openFileDialog.ShowDialog() == true)
		{
			try
			{
				_input = File.ReadAllBytes(openFileDialog.FileName);
			}
			catch (Exception e)
			{
				MessageBox.Show(e.Message, "Помилка", System.Windows.MessageBoxImage.Error);
				return;
			}
			OnPropertyChanged(nameof(OriginalSize));
			_output = null; OnPropertyChanged(nameof(EncodedSize));
		}
	}

	[RelayCommand]
	private void Encode()
	{
		if (_input == null) return;
		try
		{
			_output = BalancedTree.Encode(_input, UseRLE);
		}
		catch (Exception e)
		{
			MessageBox.Show(e.Message, "Помилка", System.Windows.MessageBoxImage.Error);
			return;
		}
		OnPropertyChanged(nameof(EncodedSize));
		OnPropertyChanged(nameof(CompressionRatio));
	}

	[RelayCommand]
	private void Decode()
	{
		if (_input == null) return;
		try
		{
			_output = BalancedTree.Decode(_input);
		}
		catch (Exception e)
		{
			MessageBox.Show(e.Message, "Помилка", System.Windows.MessageBoxImage.Error);
			return;
		}
		OnPropertyChanged(nameof(EncodedSize));
		OnPropertyChanged(nameof(CompressionRatio));
	}

	[RelayCommand]
	private void Save()
	{
		if (_output == null) return;
		SaveFileDialog saveFileDialog = new();
		if (saveFileDialog.ShowDialog() == true)
		{
			try
			{
				File.WriteAllBytes(saveFileDialog.FileName, _output);
			}
			catch (Exception e)
			{
				MessageBox.Show(e.Message, "Помилка", System.Windows.MessageBoxImage.Error);
				return;
			}
		}
	}

	[RelayCommand]
	private static void CopyTree()
	{
		if (BalancedTree.LastTree is null) return;
		System.Windows.Clipboard.SetText(BalancedTree.LastTree.ToString());
	}
}
