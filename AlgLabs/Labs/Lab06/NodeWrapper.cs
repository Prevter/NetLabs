using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AlgLabs.Labs.Lab06;

public sealed class NodeWrapper<T> : INotifyPropertyChanged where T : class
{
    public T Value { get; set; }
    private bool _isSelected;

    public bool IsNodeSelected
    {
        get => _isSelected;
        set
        {
            _isSelected = value;
            OnPropertyChanged(nameof(IsNodeSelected));
        }
    }

    public NodeWrapper(T value)
    {
        Value = value;
    }

    public override string ToString()
    {
        return Value.ToString() ?? string.Empty;
    }

    public event PropertyChangedEventHandler? PropertyChanged;
    public void OnPropertyChanged(string propertyName)
    {
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }
}
