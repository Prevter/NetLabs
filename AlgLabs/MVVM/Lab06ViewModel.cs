using AlgLabs.Labs.Lab04;
using AlgLabs.Labs.Lab06;
using FloxelLib.Common;
using FloxelLib.MVVM;
using System;
using System.Collections.Generic;
using System.Linq;

namespace AlgLabs.MVVM;

public sealed partial class Lab06ViewModel : BaseViewModel
{
    [UpdateProperty] private Graph<City> _cities = new();

    [UpdateProperty] public List<NodeWrapper<City>> _graphNodes = new();

    [UpdateProperty] private Microsoft.Msagl.Drawing.Graph _graph;

    [UpdateProperty] private string _name = "", _searchValue = "";

    [UpdateProperty] private int _year, _population;

    [UpdateProperty] private City? _foundCity;

    [RelayCommand]
    private void ResetView()
    {
        TryDo(() =>
        {
            var newList = new List<NodeWrapper<City>>();
            foreach (var node in _cities.Nodes)
                newList.Add(new(node.Value));
            GraphNodes = newList;
            Graph = _cities.GetGraph();
        });
    }

    [RelayCommand]
    private void Clear()
    {
        TryDo(_cities.Clear);
        ResetView();
    }

    [RelayCommand]
    private void Add()
    {
        TryDo(() => _cities.AddNode(new City(Name, Year, Population)));
        ResetView();
    }

    [RelayCommand]
    private void Remove(object arg)
    {
        var city = (NodeWrapper<City>)arg;
        TryDo(() => _cities.RemoveNode(city.Value));
        ResetView();
    }

    [RelayCommand]
    private void JoinNodes()
    {
        var selectedNodes = GraphNodes.Where(n => n.IsNodeSelected).ToList();
        if (selectedNodes.Count != 2)
        {
            MessageBox.Show("Оберіть 2 міста для з'єднання", "Помилка", MessageBoxImage.Error);
            return;
        }

        TryDo(() =>
        {
            var node1 = selectedNodes[0].Value;
            var node2 = selectedNodes[1].Value;

            // if the nodes are already connected, disconnect them
            if (_cities.AreConnected(node1, node2))
                _cities.RemoveEdge(node1, node2);
            // otherwise, connect them
            else _cities.AddEdge(node1, node2);
        });

        ResetView();
    }

    [RelayCommand]
    private void CheckConnection()
    {
        var selectedNodes = GraphNodes.Where(n => n.IsNodeSelected).ToList();
        if (selectedNodes.Count != 2)
        {
            MessageBox.Show("Оберіть 2 міста для перевірки", "Помилка", MessageBoxImage.Error);
            return;
        }
        TryDo(() =>
        {
            var node1 = selectedNodes[0].Value;
            var node2 = selectedNodes[1].Value;
            var path = _cities.FindPath(node1, node2);

            if (path.Count() <= 1)
                MessageBox.Show("Міста не з'єднані", "Результат", MessageBoxImage.Asterisk);
            else
            {
                var pathString = string.Join(" -> ", path.Select(n => n.Name));
                MessageBox.Show($"Міста з'єднані через: {pathString}", "Результат", MessageBoxImage.Asterisk);
            }
        });
    }

    [RelayCommand]
    private void Search(object obj)
    {
        var propertyName = (string)obj;
        bool isInt = int.TryParse(SearchValue, out int _);
        TryDo(() => FoundCity = Cities.Search((x) => propertyName switch
        {
            "Name" => x.Name == SearchValue,
            "Year" => isInt && x.Year == int.Parse(SearchValue),
            "Population" => isInt && x.Population == int.Parse(SearchValue),
            _ => throw new ArgumentException("Invalid property name.", nameof(obj))
        }));
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

    public Lab06ViewModel()
    {
        var cities = new City[]
        {
            new City("Київ", 482, 2_967_360),
            new City("Харків", 1654, 1_443_207),
            new City("Донецьк", 1869, 908_456),
            new City("Дніпро", 1635, 990_724),
            new City("Кривий Ріг", 1775, 619_278),
            new City("Миколаїв", -1250, 480_080),
            new City("Запоріжжя", 1770, 731_922),
            new City("Одеса", 1415, 1_017_699),
            new City("Львів", 1256, 724_314),
        };

        foreach (var city in cities)
            _cities.AddNode(city);

        _cities.AddEdge(cities[0], cities[1]);
        _cities.AddEdge(cities[1], cities[2]);
        _cities.AddEdge(cities[1], cities[3]);
        _cities.AddEdge(cities[3], cities[4]);
        _cities.AddEdge(cities[4], cities[5]);
        _cities.AddEdge(cities[4], cities[6]);
        _cities.AddEdge(cities[3], cities[6]);
        _cities.AddEdge(cities[5], cities[7]);
        _cities.AddEdge(cities[0], cities[8]);

        ResetView();
    }
}