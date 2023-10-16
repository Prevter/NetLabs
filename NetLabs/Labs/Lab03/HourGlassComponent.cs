using System.Windows;
using System.Windows.Media;
using System.Windows.Shapes;

namespace NetLabs.Labs.Lab03;

public sealed class HourGlassComponent : Shape
{
	protected override Geometry DefiningGeometry
	{
		get
		{
			StreamGeometry geometry = new();

			using (StreamGeometryContext context = geometry.Open())
			{
				context.BeginFigure(new Point(0, 0), true, true);
				context.LineTo(new Point(Width, 0), true, false);
				context.LineTo(new Point(Width / 2, Height / 2), true, false);
				context.LineTo(new Point(0, 0), true, false);
				context.BeginFigure(new Point(0, Height), true, true);
				context.LineTo(new Point(Width, Height), true, false);
				context.LineTo(new Point(Width / 2, Height / 2), true, false);
				context.LineTo(new Point(0, Height), true, false);
			}

			geometry.Freeze();
			return geometry;
		}
	}
}
