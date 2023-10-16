﻿namespace AlgLabs.Labs.Lab04;

public sealed class Node<T>
{
	public T Value { get; set; }
	public Node<T>? Next { get; set; }
	public Node<T>? Previous { get; set; }

	public Node(T value)
	{
		Value = value;
	}
}