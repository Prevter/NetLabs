using FloxelLib;
using FloxelLib.Common;
using FloxelLib.MVVM;
using Microsoft.Win32;
using NetLabs.Labs.Lab05;
using System.Globalization;
using System.IO;
using System.Reflection;
using System.Text;

namespace NetLabs.MVVM;

public sealed partial class Lab05ViewModel : BaseViewModel
{
	[UpdateProperty]
	private double _radiusValue = 100;

	[UpdateProperty("if (_radius.Parse(out double radius)) RadiusValue = radius;")]
	private string _radius = "100";

	[RelayCommand]
	private void SerializeXml()
	{
		Pentagon pentagon = new(_radiusValue);
		string xml = pentagon.XmlSerialize();
		SaveFileDialog dialog = new()
		{
			Filter = "XML files (*.xml)|*.xml|All files (*.*)|*.*",
			FileName = "Pentagon.xml"
		};
		if (dialog.ShowDialog() == true)
		{
			File.WriteAllText(dialog.FileName, xml);
		}
	}

	[RelayCommand]
	private void SerializeBin()
	{
		Pentagon pentagon = new(_radiusValue);
		byte[] bytes = pentagon.BinSerialize();
		SaveFileDialog dialog = new()
		{
			Filter = "Binary files (*.bin)|*.bin|All files (*.*)|*.*",
			FileName = "Pentagon.bin"
		};
		if (dialog.ShowDialog() == true)
		{
			File.WriteAllBytes(dialog.FileName, bytes);
		}
	}

	[RelayCommand]
	private void DeserializeXml()
	{
		OpenFileDialog dialog = new()
		{
			Filter = "XML files (*.xml)|*.xml|All files (*.*)|*.*",
		};
		if (dialog.ShowDialog() == true)
		{
			string xml = File.ReadAllText(dialog.FileName);
			Pentagon? pentagon = Pentagon.XmlDeserialize(xml);
			if (pentagon is not null)
			{
				Radius = pentagon.Radius.ToString(CultureInfo.InvariantCulture);
			}
		}
	}

	[RelayCommand]
	private void DeserializeBin()
	{
		OpenFileDialog dialog = new()
		{
			Filter = "Binary files (*.bin)|*.bin|All files (*.*)|*.*",
		};
		if (dialog.ShowDialog() == true)
		{
			byte[] bytes = File.ReadAllBytes(dialog.FileName);
			Pentagon pentagon = Pentagon.BinDeserialize(bytes);
			Radius = pentagon.Radius.ToString(CultureInfo.InvariantCulture);
		}
	}

	[RelayCommand]
	private static void ShowReflection()
	{
		// list all methods in Pentagon class
		StringBuilder builder = new();
		MethodInfo[] methods = typeof(Pentagon).GetMethods();
		foreach (MethodInfo method in methods)
		{
			builder.AppendLine(method.ToString());
		}

		MessageBox.Show(builder.ToString(), "Методи");
	}
}