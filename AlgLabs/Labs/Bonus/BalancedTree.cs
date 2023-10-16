using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace AlgLabs.Labs.Bonus;

/// <summary>
/// Contains all methods for BalancedTree encoding.
/// </summary>
public static class BalancedTree
{
	/// <summary>
	/// Used only for debugging. Contains last created tree.
	/// </summary>
	private static BalancedTreeNode? lastTree = null;
	public static BalancedTreeNode? LastTree => lastTree;

	/// <summary>
	/// Encodes an input data using BalancedTree algorithm and returns byte array of a file, containing encoded data and header.
	/// </summary>
	public static byte[] Encode(byte[] input, bool useRLE = false)
	{
		if (input.Length == 0)
			throw new InvalidDataException("Input data is empty");

		if (useRLE) input = RunLengthEncode(input);

		// Build frequency table
		Dictionary<byte, int> dictionary = new();
		foreach (byte b in input)
		{
			if (dictionary.ContainsKey(b))
				dictionary[b]++;
			else
				dictionary.Add(b, 1);
		}

		// Sort dictionary by frequency
		dictionary = dictionary.OrderBy(x => x.Value).ToDictionary(x => x.Key, x => x.Value);

		// Build BalancedTree tree
		BalancedTreeNode root = BuildTree(dictionary);
		lastTree = root;

		// Build dictionary
		Dictionary<byte, string> codes = new();
		BuildDictionary(root, codes);

		// Encode data
		List<bool> bits = new();
		foreach (byte b in input)
		{
			string code = codes[b];
			foreach (char c in code)
			{
				bits.Add(c == '1');
			}
		}

		// Convert bits to bytes
		byte[] bytes = new byte[(bits.Count + 7) / 8];
		for (int i = 0; i < bits.Count; i++)
		{
			if (bits[i])
				bytes[i / 8] |= (byte)(1 << (7 - (i % 8)));
		}

		// Build header
		FileHeader header = new()
		{
			Dictionary = dictionary,
			UseRLE = useRLE,
			FileSize = input.Length
		};

		byte[] headerBytes = header.GetBytes();
		byte[] result = new byte[headerBytes.Length + bytes.Length];
		headerBytes.CopyTo(result, 0);
		bytes.CopyTo(result, headerBytes.Length);

		return result;
	}

	/// <summary>
	/// Converts BalancedTree tree to dictionary of codes (e.g. 'a' -> "101").
	/// </summary>
	private static void BuildDictionary(BalancedTreeNode node, Dictionary<byte, string> codes, string code = "")
	{
		if (node.Left == null && node.Right == null)
		{
			codes.Add(node.Value, code);
			return;
		}

		BuildDictionary(node.Left!, codes, code + "0");
		BuildDictionary(node.Right!, codes, code + "1");
	}

	/// <summary>
	/// Generates tree based on character frequencies.
	/// </summary>
	public static BalancedTreeNode BuildTree(Dictionary<byte, int> frequency)
	{
		PriorityQueue<BalancedTreeNode, int> trees = new();

		foreach (KeyValuePair<byte, int> pair in frequency)
		{
			trees.Enqueue(new BalancedTreeNode(pair.Key, pair.Value), pair.Value);
		}

		while (trees.Count > 1)
		{
			BalancedTreeNode left = trees.Dequeue();
			BalancedTreeNode right = trees.Dequeue();
			trees.Enqueue(new BalancedTreeNode(left, right), left.Frequency + right.Frequency);
		}

		return trees.Dequeue();
	}

	/// <summary>
	/// Decode file, encoded with BalancedTree algorithm.
	/// </summary>
	public static byte[] Decode(byte[] input)
	{
		if (input.Length == 0)
			throw new InvalidDataException("Input data is empty");

		FileHeader header = FileHeader.FromBytes(input);
		byte[] data = new byte[input.Length - header.Size];
		Array.Copy(input, header.Size, data, 0, data.Length);

		// Build BalancedTree tree
		BalancedTreeNode root = BuildTree(header.Dictionary);
		lastTree = root;

		// Decode data
		List<byte> output = new();
		BalancedTreeNode node = root;

		foreach (byte b in data)
		{
			for (int i = 0; i < 8; i++)
			{
				bool bit = (b & (1 << (7 - i))) != 0;
				if (bit)
					node = node.Right!;
				else
					node = node.Left!;

				if (node.Left == null && node.Right == null)
				{
					output.Add(node.Value);
					node = root;
				}
			}
		}

		var result = output.Take(header.FileSize).ToArray();

		if (result.Length != header.FileSize)
			throw new InvalidDataException("File size mismatch");

		if (header.UseRLE) result = RunLengthDecode(result);

		return result;
	}

	/// <summary>
	/// Encodes data using Run-Length Encoding.
	/// </summary>
	public static byte[] RunLengthEncode(byte[] input)
	{
		List<byte> output = new(input.Length);

		byte current = input[0];
		byte count = 1;
		for (int i = 1; i < input.Length; i++)
		{
			if (input[i] == current && count < 255)
			{
				count++;
			}
			else
			{
				output.Add(current);
				output.Add(count);
				current = input[i];
				count = 1;
			}
		}

		output.Add(current);
		output.Add(count);
		return output.ToArray();
	}

	/// <summary>
	/// Decodes Run-Length Encoded data.
	/// </summary>
	public static byte[] RunLengthDecode(byte[] input)
	{
		List<byte> output = new(input.Length);

		for (int i = 0; i < input.Length; i += 2)
		{
			byte current = input[i];
			int count = input[i + 1];
			for (int j = 0; j < count; j++)
			{
				output.Add(current);
			}
		}

		return output.ToArray();
	}

}

public sealed class BalancedTreeNode
{
	public byte Value { get; set; }
	public int Frequency { get; set; }
	public BalancedTreeNode? Left { get; set; }
	public BalancedTreeNode? Right { get; set; }

	public BalancedTreeNode(byte value)
	{
		Value = value;
	}

	public BalancedTreeNode(byte value, int frequency)
	{
		Value = value;
		Frequency = frequency;
	}

	public BalancedTreeNode(BalancedTreeNode left, BalancedTreeNode right)
	{
		Left = left;
		Right = right;
		Frequency = left.Frequency + right.Frequency;
	}

	public override string ToString()
	{
		if (Left == null && Right == null)
			return $"{Value}";
		return $"N({Left},{Right})";
	}
}

/// <summary>
/// Encoded file header structure.
/// </summary>
public struct FileHeader
{
	public char[] Magic { get; set; } = { 'H', 'F' };
	public byte Version { get; set; } = 2;

	/// <summary>
	/// Applies Run-Length Encoding to the input stream before encoding.
	/// </summary>
	public bool UseRLE { get; set; }

	/// <summary>
	/// Array of characters in their tree traversal order.
	/// </summary>
	public Dictionary<byte, int> Dictionary { get; set; }

	/// <summary>
	/// Original file size in bytes.
	/// </summary>
	public int FileSize { get; set; }

	public int Size { get; set; }

	public FileHeader() { }

	public byte[] GetBytes()
	{
		if (Dictionary == null)
			throw new InvalidDataException("Dictionary is null");

		using MemoryStream ms = new();
		using BinaryWriter bw = new(ms);
		bw.Write(Magic);
		bw.Write(Version);
		bw.Write(UseRLE.ToByte());
		bw.Write((ushort)Dictionary.Count);
		foreach (KeyValuePair<byte, int> pair in Dictionary)
		{
			bw.Write(pair.Key);
			bw.WriteVarInt(pair.Value);
		}
		bw.WriteVarInt(FileSize);
		Size = (int)ms.Position;
		return ms.ToArray();
	}

	public static FileHeader FromBytes(byte[] bytes)
	{
		using MemoryStream ms = new(bytes);
		using BinaryReader br = new(ms);
		FileHeader header = new()
		{
			Magic = br.ReadChars(2),
			Version = br.ReadByte(),
		};

		if (header.Magic[0] != 'H' || header.Magic[1] != 'F')
			throw new InvalidDataException("Invalid file header");

		header.UseRLE = br.ReadByte() != 0;
		int dictLength = br.ReadUInt16();
		header.Dictionary = new Dictionary<byte, int>(dictLength);
		for (int i = 0; i < dictLength; i++)
		{
			byte key = br.ReadByte();
			int value = br.ReadVarInt();
			header.Dictionary.Add(key, value);
		}
		header.FileSize = br.ReadVarInt();
		header.Size = (int)ms.Position;
		return header;
	}
}

internal static class Extensions
{
	public static byte ToByte(this bool b) => b ? (byte)1 : (byte)0;

	public static int ReadVarInt(this BinaryReader br)
	{
		int result = 0;
		int shift = 0;
		byte b;
		do
		{
			b = br.ReadByte();
			result |= (b & 0x7F) << shift;
			shift += 7;
		} while ((b & 0x80) != 0);
		return result;
	}

	public static void WriteVarInt(this BinaryWriter bw, int value)
	{
		do
		{
			byte b = (byte)(value & 0x7F);
			value >>= 7;
			if (value != 0)
				b |= 0x80;
			bw.Write(b);
		} while (value != 0);
	}
}