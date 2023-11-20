using System;
using System.Collections.Generic;
using System.Collections.Specialized;

namespace AlgLabs.Labs.Lab05;

public sealed class BinaryTree<T> : IEnumerable<Node<T>>, INotifyCollectionChanged where T : IComparable<T>
{
    private Node<T>? root;

    public BinaryTree()
    {
        root = null;
    }

    public BinaryTree(IEnumerable<T> values)
    {
        foreach (var value in values)
        {
            Insert(value);
        }
    }

    public void Insert(T value)
    {
        if (root == null)
        {
            root = new Node<T>(value);
        }
        else
        {
            Insert(value, root);
        }
        OnCollectionChanged();
    }

    private void Insert(T value, Node<T> node)
    {
        if (value.CompareTo(node.Value) < 0)
        {
            if (node.Left == null)
            {
                node.Left = new Node<T>(value);
            }
            else
            {
                Insert(value, node.Left);
            }
        }
        else
        {
            if (node.Right == null)
            {
                node.Right = new Node<T>(value);
            }
            else
            {
                Insert(value, node.Right);
            }
        }
    }

    public void Remove(T value)
    {
        root = Remove(value, root);
        OnCollectionChanged();
    }

    private Node<T>? Remove(T value, Node<T>? node)
    {
        if (node == null)
        {
            return null;
        }

        if (value.CompareTo(node.Value) < 0)
        {
            node.Left = Remove(value, node.Left);
        }
        else if (value.CompareTo(node.Value) > 0)
        {
            node.Right = Remove(value, node.Right);
        }
        else
        {
            if (node.Left == null)
            {
                return node.Right;
            }
            else if (node.Right == null)
            {
                return node.Left;
            }
            else
            {
                node.Value = Min(node.Right).Value;
                node.Right = Remove(node.Value, node.Right);
            }
        }

        return node;
    }

    public void Clear()
    {
        root = null;
        OnCollectionChanged();
    }

    public bool Contains(T value)
    {
        return Contains(value, root);
    }

    private bool Contains(T value, Node<T>? node)
    {
        if (node == null)
        {
            return false;
        }

        if (value.CompareTo(node.Value) < 0)
        {
            return Contains(value, node.Left);
        }
        else if (value.CompareTo(node.Value) > 0)
        {
            return Contains(value, node.Right);
        }
        else
        {
            return true;
        }
    }

    public Node<T>? Min()
    {
        return Min(root);
    }

    private Node<T>? Min(Node<T>? node)
    {
        if (node == null)
        {
            return null;
        }

        if (node.Left == null)
        {
            return node;
        }

        return Min(node.Left);
    }

    public Node<T>? Max()
    {
        return Max(root);
    }

    private Node<T>? Max(Node<T>? node)
    {
        if (node == null)
        {
            return null;
        }

        if (node.Right == null)
        {
            return node;
        }

        return Max(node.Right);
    }

    public event NotifyCollectionChangedEventHandler? CollectionChanged;
    public void OnCollectionChanged()
    {
        CollectionChanged?.Invoke(this, new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Reset));
    }

    public IEnumerator<Node<T>> GetEnumerator()
    {
        if (root == null)
            yield break;
        yield return root;
    }

    System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()
    {
        return GetEnumerator();
    }

    public int Count(Func<T, bool> predicate)
    {
        return Count(root, predicate);
    }

    private int Count(Node<T>? node, Func<T, bool> predicate)
    {
        if (node == null)
        {
            return 0;
        }

        return (predicate(node.Value) ? 1 : 0) + Count(node.Left, predicate) + Count(node.Right, predicate);
    }

    public int CountNodes(Func<Node<T>, bool> predicate)
    {
        return CountNodes(root, predicate);
    }

    private int CountNodes(Node<T>? node, Func<Node<T>, bool> predicate)
    {
        if (node == null)
        {
            return 0;
        }

        return (predicate(node) ? 1 : 0) + CountNodes(node.Left, predicate) + CountNodes(node.Right, predicate);
    }

    public int NodeCount => Count(x => true);

    public int LeafCount => CountNodes(x => x.Left == null && x.Right == null);

    public T MinMax(Func<T, T, int> comparer)
    {
        return MinMax(root, comparer);
    }

    private T MinMax(Node<T>? node, Func<T, T, int> comparer)
    {
        if (node == null)
        {
            throw new InvalidOperationException("The tree is empty.");
        }

        var minMax = node.Value;
        MinMax(node, ref minMax, comparer);
        return minMax;
    }

    private void MinMax(Node<T>? node, ref T minMax, Func<T, T, int> comparer)
    {
        if (node == null)
        {
            return;
        }

        if (comparer(node.Value, minMax) > 0)
        {
            minMax = node.Value;
        }

        MinMax(node.Left, ref minMax, comparer);
        MinMax(node.Right, ref minMax, comparer);
    }

    public int Height => GetHeight(root);

    private int GetHeight(Node<T>? node)
    {
        if (node == null)
        {
            return 0;
        }

        return 1 + Math.Max(GetHeight(node.Left), GetHeight(node.Right));
    }

    public int Sum(Func<T, int> selector)
    {
        return Sum(root, selector);
    }

    private int Sum(Node<T>? node, Func<T, int> selector)
    {
        if (node == null)
        {
            return 0;
        }

        return selector(node.Value) + Sum(node.Left, selector) + Sum(node.Right, selector);
    }

    public void ForEach(Action<Node<T>> action)
    {
        ForEach(root, action);
    }

    private void ForEach(Node<T>? node, Action<Node<T>> action)
    {
        if (node == null)
        {
            return;
        }

        action(node);
        ForEach(node.Left, action);
        ForEach(node.Right, action);
    }
}
