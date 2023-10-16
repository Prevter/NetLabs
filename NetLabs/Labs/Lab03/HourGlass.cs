namespace NetLabs.Labs.Lab03;

public sealed class HourGlass
{
	private static int _instanceCount = 0;
	public static int InstanceCount => _instanceCount;

	public double Width { get; set; }
	public double Height { get; set; }

	// For displaying
	public double X { get; set; }
	public double Y { get; set; }

	public HourGlass(double width, double height)
	{
		Width = width;
		Height = height;
		_instanceCount++;
	}
}
