using AlgLabs.Labs.Lab05;
using FloxelLib.Common;
using FloxelLib.MVVM;
using System;

namespace AlgLabs.MVVM;

public sealed partial class Lab05ViewModel : BaseViewModel
{
    [UpdateProperty]
    private BinaryTree<City> _cities = new(new City[]
    {
        new City("Донецьк", 1869, 908_456),
        new City("Львів", 1256, 724_314),
        new City("Одеса", 1415, 1_017_699),
        new City("Кривий Ріг", 1775, 619_278),
        new City("Запоріжжя", 1770, 731_922),
        new City("Дніпро", 1635, 990_724),
        new City("Харків", 1654, 1_443_207),
        new City("Миколаїв", -1250, 480_080),
        new City("Київ", 482, 2_967_360),
    });

    [UpdateProperty] private string _name = "";

    [UpdateProperty] private int _year, _population;

    [UpdateProperty] private int _height, _nodesCount, _leafCount, _greater100kCount, _totalPopulation;

    [UpdateProperty] private City? _foundCity;

    [UpdateProperty] private bool _searchMax;

    [UpdateProperty("UpdateStats();")] private bool _highlight100k;

    [RelayCommand]
    private void AddFirst()
    {
        TryDo(() => Cities.Insert(new City(Name, Year, Population)));
        UpdateStats();
    }

    [RelayCommand]
    private void Remove(object arg)
    {
        var city = (City)arg;
        TryDo(() => Cities.Remove(city));
        UpdateStats();
    }

    [RelayCommand]
    private void Clear()
    {
        TryDo(Cities.Clear);
        UpdateStats();
    }

    [RelayCommand]
    private void FindMinMax(object obj)
    {
        var propertyName = (string)obj;
        TryDo(() => FoundCity = Cities.MinMax((x, y) => propertyName switch
        {
            "Name" => SearchMax ? x.Name.CompareTo(y.Name) : y.Name.CompareTo(x.Name),
            "Year" => SearchMax ? x.Year.CompareTo(y.Year) : y.Year.CompareTo(x.Year),
            "Population" => SearchMax ? x.Population.CompareTo(y.Population) : y.Population.CompareTo(x.Population),
            _ => throw new ArgumentException("Invalid property name.", nameof(obj))
        }));
    }

    private void UpdateStats()
    {
        Height = Cities.Height;
        NodesCount = Cities.NodeCount;
        LeafCount = Cities.LeafCount;
        Greater100kCount = Cities.Count(x => x.Population > 100_000);
        TotalPopulation = Cities.Sum(x => x.Population);

        if (Highlight100k)
            Cities.ForEach(x => x.IsHighlighted = x.Value.Population > 100_000);
        else
            Cities.ForEach(x => x.IsHighlighted = false);
    }

    /// <summary>
    /// This method is used to catch exceptions and show them in a message box.
    /// </summary>
    /// <param name="action">Action to be tested</param>
    private static void TryDo(Action action)
    {
        try { action(); }
        catch (Exception ex)
        { MessageBox.Show("Помилка: " + ex.Message, "Помилка", MessageBoxImage.Error); }
    }

    public Lab05ViewModel()
    {
        UpdateStats();
    }
}