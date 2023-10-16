using FloxelLib.MVVM;
using NetLabs.Labs.Lab02;

namespace NetLabs.MVVM;

public sealed class Lab02ViewModel : BaseViewModel
{
	private string _value = "";
	public string Value
	{
		get => _value;
		set
		{
			SetField(ref _value, value);
			Evaluate();
		}
	}

	private int _spaceCount;
	private string _latinUpper;

	public int SpaceCount { get => _spaceCount; set => SetField(ref _spaceCount, value); }
	public string LatinUpper { get => _latinUpper; set => SetField(ref _latinUpper, value); }


	private void Evaluate()
	{
		SpaceCount = Wrapper.CountSpaces(Value);
		LatinUpper = Wrapper.LatinToUpper(Value);
	}

}
