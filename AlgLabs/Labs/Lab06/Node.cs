using System.Collections.Generic;

namespace AlgLabs.Labs.Lab06;

public sealed class Node<T>
{
    public T Value { get; set; }
    public List<Node<T>> Adjacent { get; set; }

    public Node(T value)
    {
        Value = value;
        Adjacent = new();
    }
}
