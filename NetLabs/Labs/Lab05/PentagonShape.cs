using System;
using System.Windows;
using System.Windows.Shapes;

namespace NetLabs.Labs.Lab05;

public sealed class PentagonShape : Shape
{
	protected override System.Windows.Media.Geometry DefiningGeometry
	{
		get
		{
			System.Windows.Media.StreamGeometry geometry = new();

			using (System.Windows.Media.StreamGeometryContext context = geometry.Open())
			{
				// Draw a pentagon
				var points = new Point[5];
				for (int i = 0; i < 5; i++)
				{
					var angle = 2 * Math.PI * i / 5;
					points[i] = new Point(
						Width / 2 * Math.Cos(angle) + (Width / 2),
						Width / 2 * Math.Sin(angle) + (Width / 2)
					);
				}

				context.BeginFigure(points[0], true, true);
				for (int i = 1; i < 5; i++)
				{
					context.LineTo(points[i], true, false);
				}
			}

			geometry.Freeze();
			return geometry;
		}
	}
}
