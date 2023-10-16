using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace NetLabs.Labs.Lab05;

public static class BinaryConvert
{
	public static byte[] Serialize<T>(T value)
	{
		// read all properties from value
		Type type = typeof(T);
		var properties = type.GetProperties();
		var fields = type.GetFields();

		List<byte> bytes = new();

		foreach (var property in properties)
		{
			if (Attribute.IsDefined(property, typeof(NonSerializedAttribute)))
				continue;

			object? propertyValue = property.GetValue(value);
			Type propertyType = property.PropertyType;

			byte[] propertyValueBytes = GetBytes(propertyValue, propertyType);

			bytes.AddRange(propertyValueBytes);
		}

		foreach (var field in fields)
		{
			if (Attribute.IsDefined(field, typeof(NonSerializedAttribute)))
				continue;

			object? fieldValue = field.GetValue(value);
			Type fieldType = field.FieldType;

			byte[] fieldValueBytes = GetBytes(fieldValue, fieldType);

			bytes.AddRange(fieldValueBytes);
		}

		return bytes.ToArray();
	}

	private static byte[] GetBytes(object value, Type type)
	{
		if (type == typeof(int))
		{
			return BitConverter.GetBytes((int)value);
		}
		else if (type == typeof(double))
		{
			return BitConverter.GetBytes((double)value);
		}
		else if (type == typeof(float))
		{
			return BitConverter.GetBytes((float)value);
		}
		else if (type == typeof(bool))
		{
			return BitConverter.GetBytes((bool)value);
		}
		else if (type == typeof(string))
		{
			string stringValue = (string)value;
			byte[] buffer = new byte[4 + stringValue.Length];
			byte[] lengthBuffer = BitConverter.GetBytes(stringValue.Length);
			byte[] stringBuffer = Encoding.UTF8.GetBytes(stringValue);
			lengthBuffer.CopyTo(buffer, 0);
			stringBuffer.CopyTo(buffer, 4);
			return buffer;
		}
		else if (!type.IsGenericType)
		{
			return Serialize(value);
		}
		else
		{
			throw new NotImplementedException($"GetBytes not implemented for {type}");
		}
	}

	public static T Deserialize<T>(byte[] bytes) where T : new()
	{
		using MemoryStream stream = new(bytes);
		using BinaryReader reader = new(stream);
		return Deserialize<T>(reader);
	}

	public static T Deserialize<T>(BinaryReader reader) where T : new()
	{
		Type type = typeof(T);
		var properties = type.GetProperties();
		var fields = type.GetFields();

		T deserialized = new();

		foreach (var property in properties)
		{
			if (Attribute.IsDefined(property, typeof(NonSerializedAttribute)))
				continue;

			Type propertyType = property.PropertyType;
			object propertyValue = GetValue(reader, propertyType);
			property.SetValue(deserialized, propertyValue);
		}

		foreach (var field in fields)
		{
			if (Attribute.IsDefined(field, typeof(NonSerializedAttribute)))
				continue;

			Type fieldType = field.FieldType;
			object fieldValue = GetValue(reader, fieldType);
			field.SetValue(deserialized, fieldValue);
		}

		return deserialized;
	}

	private static object GetValue(BinaryReader reader, Type type)
	{
		if (type == typeof(int))
		{
			return reader.ReadInt32();
		}
		else if (type == typeof(double))
		{
			return reader.ReadDouble();
		}
		else if (type == typeof(float))
		{
			return reader.ReadSingle();
		}
		else if (type == typeof(bool))
		{
			return reader.ReadBoolean();
		}
		else if (type == typeof(string))
		{
			var length = reader.ReadInt32();
			var bytes = reader.ReadBytes(length);
			return Encoding.UTF8.GetString(bytes);
		}
		else if (!type.IsGenericType)
		{
			return typeof(BinaryConvert)
				.GetMethod(nameof(Deserialize))!
				.MakeGenericMethod(type)
				.Invoke(null, new object[] { type })!;
		}
		else
		{
			throw new NotImplementedException($"GetValue not implemented for {type}");
		}
	}
}
