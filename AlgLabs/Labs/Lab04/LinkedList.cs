using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;

namespace AlgLabs.Labs.Lab04;

public sealed class LinkedList<T> : IEnumerable<T>, INotifyCollectionChanged where T : class
{
	private Node<T>? _head;
	private Node<T>? _tail;

	public int Count { get; private set; }

	/// <summary>
	/// Creates an empty linked list.
	/// </summary>
	public LinkedList()
	{
		Count = 0;
	}

	/// <summary>
	/// Creates a linked list with elements from the collection.
	/// </summary>
	public LinkedList(IEnumerable<T> collection) => AddRange(collection);

	/// <summary>
	/// Index operator for the linked list.
	/// </summary>
	public T this[int index]
	{
		get => GetNode(index).Value;
		set 
		{ 
			GetNode(index).Value = value;
			OnCollectionChanged();
		}
	}

	/// <summary>
	/// Returns a node at the specified index.
	/// </summary>
	/// <exception cref="ArgumentOutOfRangeException"></exception>
	private Node<T> GetNode(int index)
	{
		// Check if the index is valid.
		if (index < 0 || index >= Count)
		{
			throw new ArgumentOutOfRangeException(nameof(index));
		}

		// Find which side is closer.
		if (index < Count / 2)
		{
            // Start from the head.
            var current = _head;
            for (int i = 0; i < index; i++)
			{
                current = current!.Next;
            }
            return current!;
        }
        else
		{
            // Start from the tail.
            var current = _tail;
            for (int i = Count - 1; i > index; i--)
			{
                current = current!.Previous;
            }
            return current!;
        }
	}

	/// <summary>
	/// Pushes an element to the end of the list.
	/// </summary>
	public void Add(T value)
	{
		var node = new Node<T>(value);

		if (_head is null)
		{
			// First element, just set the head and tail.
			_head = node;
			_tail = node;
		}
		else
		{
			// Not the first element, set the tail's next node and the new node's previous node.
			_tail!.Next = node;
			node.Previous = _tail;
			_tail = node;
		}

		Count++;
		OnCollectionChanged();
	}

	/// <summary>
	/// Adds the elements from the collection to the end of the list.
	/// More optimal than adding each element individually.
	/// </summary>
	public void AddRange(IEnumerable<T> collection)
	{
		if (collection is null || !collection.Any()) return;

		var node = new Node<T>(collection.First());
		var root = node;
		foreach (var item in collection.Skip(1))
		{
            node.Next = new(item) { Previous = node };
            node = node.Next;
        }

		if (_head is null)
		{
			// First element, just set the head and tail.
			_head = root;
			_tail = node;
        }
		else
		{
            // Not the first element, set the tail's next node and the new node's previous node.
            _tail!.Next = root;
            root.Previous = _tail;
            _tail = node;
        }

		Count += collection.Count();
		OnCollectionChanged();
    }

	/// <summary>
	/// Inserts an element at the specified index.
	/// </summary>
	/// <exception cref="ArgumentOutOfRangeException"></exception>
	public void Insert(T value, int index)
	{
		// Check if the index is valid.
		if (index < 0 || (index > Count && Count > 0))
		{
			throw new ArgumentOutOfRangeException(nameof(index));
		}

		var node = new Node<T>(value);

		if (Count == 0 && index == 0)
		{
			_head = node;
			_tail = node;
		}
		else if (index == 0)
		{
			_head!.Previous = node;
			node.Next = _head;
			_head = node;
		}
		else if (index == Count)
		{
            _tail!.Next = node;
            node.Previous = _tail;
            _tail = node;
        }
        else
		{
            var current = GetNode(index);
            // Connect the previous and next nodes.
            current.Previous!.Next = node;
            node.Previous = current.Previous;
            current.Previous = node;
            node.Next = current;
        }

		Count++;
		OnCollectionChanged();
	}

	public IEnumerator<T> GetEnumerator()
	{
		var current = _head;
		while (current is not null)
		{
			yield return current.Value;
			current = current.Next;
		}
	}

	IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();

	/// <summary>
	/// Removes element at the specified index.
	/// </summary>
	/// <exception cref="ArgumentOutOfRangeException"></exception>
	public void RemoveAt(int index)
	{
		// Check if the index is valid.
		if (index < 0 || index >= Count)
		{
			throw new ArgumentOutOfRangeException(nameof(index));
		}

		if (index == 0)
		{
			_head = _head!.Next;
			if (_head is not null) _head.Previous = null;
			else _tail = null;
		}
		else if (index == Count - 1)
		{
			_tail = _tail!.Previous;
			_tail!.Next = null;
		}
		else
		{
			var current = GetNode(index);
			// Connect the previous and next nodes.
			current.Previous!.Next = current.Next;
			current.Next!.Previous = current.Previous;
		}

		Count--;
		OnCollectionChanged();
	}

	/// <summary>
	/// Clears the linked list.
	/// </summary>
	public void Clear()
	{
		_head = null;
		_tail = null;
		Count = 0;
		OnCollectionChanged();
	}

	/// <summary>
	/// Removes the first occurrence of the specified value.
	/// </summary>
	/// <returns>true, if element was found and removed</returns>
	public bool Remove(T value)
	{
		var index = IndexOf(value);
		if (index == -1) return false;
		RemoveAt(index);
		return true;
	}

	/// <summary>
	/// Sorts the linked list using the specified comparer.
	/// </summary>
	public void Sort(Func<T, T, int> comparer)
	{
		var current = _head;
		while (current is not null)
		{
			var next = current.Next;
			while (next is not null)
			{
				if (comparer(current.Value!, next.Value!) > 0)
				{
					(next.Value, current.Value) = (current.Value, next.Value);
				}

				next = next.Next;
			}

			current = current.Next;
		}
		OnCollectionChanged();
	}

	/// <summary>
	/// Returns the index of the first occurrence of the specified value.
	/// </summary>
	public int IndexOf(T value)
	{
		int index = 0;
		foreach (var item in this)
		{
			if (item == value) return index;
            index++;
		}
		return -1;
	}

	/// <summary>
	/// Returns the index of the first element that matches the specified predicate.
	/// </summary>
    public int FindIndex(Func<T, bool> value)
    {
		int index = 0;
		foreach (var item in this)
		{
			if (value(item)) return index;
            index++;
        }
        return -1;
    }

    public event NotifyCollectionChangedEventHandler? CollectionChanged;
    public void OnCollectionChanged()
    {
        CollectionChanged?.Invoke(this, new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Reset));
    }
}
