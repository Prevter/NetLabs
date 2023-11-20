using System.Collections.Generic;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Linq;

namespace AlgLabs.Labs.Lab05;

public sealed class Node<T> : INotifyPropertyChanged, INotifyCollectionChanged
{
    public T Value { get; set; }

    private Node<T>? left;
    public Node<T>? Left
    {
        get => left;
        set
        {
            left = value;
            OnCollectionChanged();
        }
    }

    private Node<T>? right;
    public Node<T>? Right
    {
        get => right;
        set
        {
            right = value;
            OnCollectionChanged();
        }
    }

    public bool IsExpanded { get; set; } = true;

    private bool _isHighlighted;
    public bool IsHighlighted
    {
        get => _isHighlighted;
        set
        {
            _isHighlighted = value;
            OnPropertyChanged(nameof(IsHighlighted));
        }
    }

    public IEnumerable<Node<T>> Children => new[] { Left, Right }.Where(x => x != null);

    public bool NoChildren => !Children.Any();

    public Node(T value)
    {
        Value = value;
    }

    public event NotifyCollectionChangedEventHandler? CollectionChanged;
    public void OnCollectionChanged()
    {
        CollectionChanged?.Invoke(this, new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Reset));
    }

    public event PropertyChangedEventHandler? PropertyChanged;
    public void OnPropertyChanged(string propertyName)
    {
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }
}
