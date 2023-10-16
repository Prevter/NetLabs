using FloxelLib.Common;
using FloxelLib.MVVM;
using NetLabs.Common;
using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Reflection;
using System.Windows.Controls;

namespace NetLabs.MVVM;

public sealed class MainViewModel : BaseViewModel
{
	private ObservableCollection<LabTab> tabs = new();
	public ObservableCollection<LabTab> Tabs { get => tabs; set => SetField(ref tabs, value); }

	private int selectedTabIndex = 0;
	public int SelectedTabIndex
	{
		get => selectedTabIndex;
		set
		{
			SetField(ref selectedTabIndex, value);
			App.Settings.SelectedTab = value;
			AppSettings.Save(App.Settings);
		}
	}

	public MainViewModel()
	{
		string pagesNamespace = typeof(App).Namespace + ".Pages";

		var pageTypes = Assembly.GetExecutingAssembly().GetTypes()
			.Where(t => t.Namespace == pagesNamespace && t.IsSubclassOf(typeof(Page)));

		foreach (var pageType in pageTypes)
		{
			var page = (Page?)Activator.CreateInstance(pageType);

			if (page is null)
				continue;

			Tabs.Add(new LabTab { Name = page.Title, Content = $"../Pages/{pageType.Name}.xaml" });
		}

		// make Settings tab first
		var settingsTab = Tabs.FirstOrDefault(t => t.Name == "Налаштування");

		if (settingsTab is not null)
		{
			settingsTab.Icon = "Cog";
			settingsTab.HasIcon = true;
			settingsTab.Name = "";
			Tabs.Remove(settingsTab);
			Tabs.Insert(0, settingsTab);
		}

		SelectedTabIndex = App.Settings.SelectedTab;
	}
}
