using FloxelLib;
using FloxelLib.MVVM;
using NetLabs.Labs.Lab06;

namespace NetLabs.MVVM;

public sealed partial class Lab06ViewModel : BaseViewModel
{
	[UpdateProperty("Update();")]
	private string _leg1 = "3", _leg2 = "4";

	[UpdateProperty]
	private double _hypotenuse = 0, _area = 0;

	private void Update()
	{
		if (!_leg1.Parse(out double l1) || !_leg2.Parse(out double l2)) return;

		Hypotenuse = Triangle.GetHypotenuse(l1, l2);
		Area = Triangle.GetArea(l1, l2);
	}

	public Lab06ViewModel() => Update();
}
