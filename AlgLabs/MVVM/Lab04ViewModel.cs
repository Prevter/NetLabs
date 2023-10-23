using AlgLabs.Labs.Lab04;
using FloxelLib.Common;
using FloxelLib.MVVM;
using System;

namespace AlgLabs.MVVM;

public sealed partial class Lab04ViewModel : BaseViewModel
{
    [UpdateProperty]
    private LinkedList<City> _cities = new(new City[]
    {
        new City("Київ", 482, 2_967_360),
        new City("Харків", 1654, 1_443_207),
        new City("Одеса", 1415, 1_017_699),
        new City("Дніпро", 1635, 990_724),
        new City("Донецьк", 1869, 908_456),
        new City("Запоріжжя", 1770, 731_922),
        new City("Львів", 1256, 724_314),
        new City("Кривий Ріг", 1775, 619_278),
        new City("Миколаїв", -1250, 480_080),
    });

    [UpdateProperty]
    private string _name = "";

    [UpdateProperty]
    private int _year, _population, _insertIndex;

    [UpdateProperty]
    private bool _sortAscending = true;

    [RelayCommand]
    private void AddLast()
    {
        TryDo(() => Cities.Add(new City(Name, Year, Population)));
    }

    [RelayCommand]
    private void AddFirst()
    {
        TryDo(() => Cities.Insert(new City(Name, Year, Population), 0));
    }

    [RelayCommand]
    private void AddAt()
    {
        TryDo(() => Cities.Insert(new City(Name, Year, Population), InsertIndex));
    }

    [RelayCommand]
    private void AddSorted()
    {
        TryDo(() =>
        {
            // Make sure the list is sorted in ascending order by it's year.
            Cities.Sort((x, y) => x.Year.CompareTo(y.Year));

            var city = new City(Name, Year, Population);

            // Find the first element with a greater year.
            var index = Cities.FindIndex(x => x.Year > city.Year);
            if (index == -1) Cities.Add(city); // Add as the last element.
            else Cities.Insert(city, index); // Insert at the found index.
        });
    }

    [RelayCommand]
    private void Remove(object arg)
    {
        var city = (City)arg;
        TryDo(() => Cities.Remove(city));
    }

    [RelayCommand]
    private void Sort(object? arg)
    {
        if (arg is not string propertyName) return;

        TryDo(() => Cities.Sort((x, y) => propertyName switch
        {
            "Name" => x.Name.CompareTo(y.Name),
            "Year" => x.Year.CompareTo(y.Year),
            "Population" => x.Population.CompareTo(y.Population),
            _ => throw new ArgumentException("Invalid property name.", nameof(arg))
        } * (_sortAscending ? 1 : -1)));
    }

    [RelayCommand]
    private void Clear()
    {
        TryDo(Cities.Clear);
    }

    [RelayCommand]
    private void Replace()
    {
        TryDo(() =>
        {
            int index = 0;
            foreach (var city in Cities)
            {
                if (city.Population < _population)
                    Cities[index] = new City(Name, Year, Population);
                index++;
            }
        });
    }

    [RelayCommand]
    private void InsertMany()
    {
        const int yearMin = 1200, yearMax = 1500;

        TryDo(() =>
        {
            // Insert before each element with year between yearMin and yearMax.
            System.Collections.Generic.List<int> indices = new();
            for (int i = 0; i < Cities.Count; i++)
            {
                if (Cities[i].Year >= yearMin && Cities[i].Year <= yearMax)
                    indices.Add(i);
            }

            // Insert at the found indices, compensating for the shift.
            for (int i = 0; i < indices.Count; i++)
            {
                Cities.Insert(new City(Name, Year, Population), indices[i] + i);
            }
        });
    }

    /// <summary>
    /// This method is used to catch exceptions and show them in a message box.
    /// </summary>
    /// <param name="action">Action to be tested</param>
    private static void TryDo(Action action)
    {
        try { action(); }
        catch (Exception ex)
        { MessageBox.Show("Помилка: " + ex.Message, "Помилка", System.Windows.MessageBoxImage.Error); }
    }
}