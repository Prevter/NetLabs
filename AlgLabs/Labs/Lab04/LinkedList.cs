using System;
using System.Collections;
using System.Collections.Generic;

namespace AlgLabs.Labs.Lab04;

public sealed class LinkedList<T> : IEnumerable<T>
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
	public LinkedList(IEnumerable<T> collection)
	{
		foreach (var item in collection)
		{
			Add(item);
		}
	}

	/// <summary>
	/// Index operator for the linked list.
	/// </summary>
	public T this[int index]
	{
		get => GetNode(index).Value;
		set => GetNode(index).Value = value;
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

		// Find the node.
		var current = _head;
		for (var i = 0; i < index; i++)
		{
			current = current!.Next;
		}

		return current!;
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
	}

	/// <summary>
	/// Inserts an element at the specified index.
	/// </summary>
	/// <exception cref="ArgumentOutOfRangeException"></exception>
	public void Insert(T value, int index)
	{
		// Check if the index is valid.
		if (index < 0 || index > Count)
		{
			throw new ArgumentOutOfRangeException(nameof(index));
		}

		if (index == 0)
		{
			var node = new Node<T>(value)
			{
				Next = _head
			};

			_head = node;
			_tail ??= node;
		}
		else if (index == Count)
		{
			var node = new Node<T>(value)
			{
				Previous = _tail
			};

			_tail!.Next = node;
			_tail = node;
		}
		else
		{
			var current = GetNode(index);
			var node = new Node<T>(value)
			{
				Next = current,
				Previous = current.Previous
			};

			current.Previous!.Next = node;
			current.Previous = node;
		}

		Count++;
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
			_head!.Previous = null;
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
	}

	/// <summary>
	/// Clears the linked list.
	/// </summary>
	public void Clear()
	{
		_head = null;
		_tail = null;
		Count = 0;
	}

	/// <summary>
	/// Removes the first occurrence of the specified value.
	/// </summary>
	/// <returns>true, if element was found and removed</returns>
	public bool Remove(T value)
	{
		var current = _head;
		while (current is not null)
		{
			if (current.Value!.Equals(value))
			{
				if (current == _head)
				{
					_head = _head.Next;
					_head!.Previous = null;
				}
				else if (current == _tail)
				{
					_tail = _tail.Previous;
					_tail!.Next = null;
				}
				else
				{
					current.Previous!.Next = current.Next;
					current.Next!.Previous = current.Previous;
				}

				Count--;
				return true;
			}

			current = current.Next;
		}

		return false;
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
	}
}
