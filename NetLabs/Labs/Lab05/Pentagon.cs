using System;
using System.IO;
using System.Xml.Serialization;

namespace NetLabs.Labs.Lab05;

[Serializable]
public sealed class Pentagon
{
	public double Radius { get; set; }

	public Pentagon() => Radius = 0;
	public Pentagon(double radius) => Radius = radius;

	public static Pentagon? XmlDeserialize(string xml)
	{
		XmlSerializer serializer = new(typeof(Pentagon));
		using StringReader reader = new(xml);
		return serializer.Deserialize(reader) as Pentagon;
	}

	public static Pentagon BinDeserialize(byte[] bytes)
	{
		return BinaryConvert.Deserialize<Pentagon>(bytes);
	}

	public string XmlSerialize()
	{
		XmlSerializer serializer = new(typeof(Pentagon));
		using StringWriter writer = new();
		serializer.Serialize(writer, this);
		return writer.ToString();
	}

	public byte[] BinSerialize()
	{
		return BinaryConvert.Serialize(this);
	}
}
