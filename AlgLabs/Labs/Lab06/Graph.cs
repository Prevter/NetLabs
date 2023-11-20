using FloxelLib;
using System;
using System.Collections.Generic;

namespace AlgLabs.Labs.Lab06;

public sealed class Graph<T> where T : class
{
    public List<Node<T>> Nodes { get; set; }

    public Graph()
    {
        Nodes = new();
    }

    public void AddNode(T value)
    {
        Nodes.Add(new Node<T>(value));
    }

    public void AddEdge(T from, T to)
    {
        var fromNode = Nodes.Find(n => n.Value.Equals(from));
        var toNode = Nodes.Find(n => n.Value.Equals(to));
        if (fromNode == null || toNode == null)
        {
            throw new ArgumentException("One or both nodes not found");
        }
        fromNode.Adjacent.Add(toNode);
        toNode.Adjacent.Add(fromNode);
    }

    public void RemoveNode(T value)
    {
        var node = Nodes.Find(n => n.Value.Equals(value)) ?? throw new ArgumentException("Node not found");
        Nodes.Remove(node);
        foreach (var n in Nodes)
        {
            n.Adjacent.Remove(node);
        }
    }

    public void RemoveEdge(T from, T to)
    {
        var fromNode = Nodes.Find(n => n.Value.Equals(from));
        var toNode = Nodes.Find(n => n.Value.Equals(to));
        if (fromNode == null || toNode == null)
        {
            throw new ArgumentException("One or both nodes not found");
        }
        fromNode.Adjacent.Remove(toNode);
        toNode.Adjacent.Remove(fromNode);
    }

    public bool AreConnected(T from, T to)
    {
        // check if nodes are connected directly
        var fromNode = Nodes.Find(n => n.Value.Equals(from));
        var toNode = Nodes.Find(n => n.Value.Equals(to));
        if (fromNode == null || toNode == null)
        {
            throw new ArgumentException("One or both nodes not found");
        }
        if (fromNode.Adjacent.Contains(toNode))
        {
            return true;
        }
        return false;
    }

    public void TraverseDepthFirst(T start, Action<T> action)
    {
        var visited = new HashSet<Node<T>>();
        var startNode = Nodes.Find(n => n.Value.Equals(start)) ?? throw new ArgumentException("Start node not found");
        TraverseDepthFirst(startNode, visited, action);
    }

    private void TraverseDepthFirst(Node<T> node, HashSet<Node<T>> visited, Action<T> action)
    {
        action(node.Value);
        visited.Add(node);
        foreach (var n in node.Adjacent)
        {
            if (!visited.Contains(n))
            {
                TraverseDepthFirst(n, visited, action);
            }
        }
    }

    public void TraverseBreadthFirst(T start, Action<T> action)
    {
        var visited = new HashSet<Node<T>>();
        var startNode = Nodes.Find(n => n.Value.Equals(start)) ?? throw new ArgumentException("Start node not found");
        var queue = new Queue<Node<T>>();
        queue.Enqueue(startNode);
        while (queue.Count > 0)
        {
            var node = queue.Dequeue();
            action(node.Value);
            visited.Add(node);
            foreach (var n in node.Adjacent)
            {
                if (!visited.Contains(n))
                {
                    queue.Enqueue(n);
                }
            }
        }
    }

    public void Clear()
    {
        Nodes.Clear();
    }

    public IEnumerable<T> FindPath(T from, T to)
    {
        // breadth-first search
        var visited = new HashSet<Node<T>>();
        var startNode = Nodes.Find(n => n.Value.Equals(from)) ?? throw new ArgumentException("Start node not found");
        var queue = new Queue<Node<T>>();
        queue.Enqueue(startNode);
        var path = new Dictionary<Node<T>, Node<T>>();
        while (queue.Count > 0)
        {
            var node = queue.Dequeue();
            visited.Add(node);
            foreach (var n in node.Adjacent)
            {
                if (!visited.Contains(n))
                {
                    queue.Enqueue(n);
                    path[n] = node;
                }
            }
        }
        var pathNodes = new List<T>();
        var currentNode = Nodes.Find(n => n.Value.Equals(to));
        while (currentNode != null)
        {
            pathNodes.Add(currentNode.Value);
            if (path.ContainsKey(currentNode))
            {
                currentNode = path[currentNode];
            }
            else
            {
                currentNode = null;
            }
        }
        pathNodes.Reverse();
        return pathNodes;
    }

    public T? Search(Func<T, bool> predicate)
    {
        foreach (var node in Nodes)
            if (predicate(node.Value))
                return node.Value;

        return null;
    }

    public Microsoft.Msagl.Drawing.Graph GetGraph()
    {
        return GetGraph(null);
    }

    public Microsoft.Msagl.Drawing.Graph GetGraph(Func<T, string>? nodeLabel)
    {
        var themeColor = Floxel.IsDarkMode ? Microsoft.Msagl.Drawing.Color.White : Microsoft.Msagl.Drawing.Color.Black;
        var graph = new Microsoft.Msagl.Drawing.Graph();
        graph.Attr.BackgroundColor = Microsoft.Msagl.Drawing.Color.Transparent;
        foreach (var node in Nodes)
        {
            var label = nodeLabel?.Invoke(node.Value) ?? node.Value.ToString();
            var n = graph.AddNode(label);
            n.Attr.Color = themeColor;
            n.Attr.FillColor = Microsoft.Msagl.Drawing.Color.White;
        }

        // add edges
        var edges = new HashSet<(T, T)>();

        foreach (var node in Nodes)
        {
            // add only one edge between two nodes
            foreach (var adjacent in node.Adjacent)
            {
                if (!edges.Contains((node.Value, adjacent.Value)) && !edges.Contains((adjacent.Value, node.Value)))
                {
                    var e = graph.AddEdge(node.Value.ToString(), adjacent.Value.ToString());
                    edges.Add((node.Value, adjacent.Value));

                    e.Attr.Color = themeColor;
                    e.Attr.ArrowheadAtTarget = Microsoft.Msagl.Drawing.ArrowStyle.None;
                }
            }
        }
        return graph;
    }
}
