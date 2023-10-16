using FloxelLib;
using FloxelLib.MVVM;
using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace AlgLabs.MVVM;

public sealed class SettingsViewModel : BaseViewModel
{
	private static readonly List<string> _themes = new()
	{
		Floxel.DarkTheme, Floxel.LightTheme
	};

	public ObservableCollection<string> Themes { get; } = new()
	{
		"Темна", "Світла"
	};

	private int _selectedTheme = _themes.IndexOf(App.Settings.Theme);

	public int SelectedTheme
	{
		get => _selectedTheme;
		set
		{
			SetField(ref _selectedTheme, value);
			App.Settings.Theme = _themes[value];
			Floxel.SetTheme(_themes[value]);
		}
	}
}
