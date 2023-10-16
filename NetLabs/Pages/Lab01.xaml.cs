using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Shapes;

namespace NetLabs.Pages;

/// <summary>
/// Interaction logic for Lab01.xaml
/// </summary>
public partial class Lab01
{
	public Lab01()
	{
		InitializeComponent();
		InitChessBoard();
	}

	public void InitChessBoard()
	{
		for (int y = 0; y < 8; y++)
		{
			for (int x = 0; x < 8; x++)
			{
				Rectangle rect = new();
				var color = (x + y) % 2 == 0 ? Color.FromRgb(238, 238, 210) : Color.FromRgb(118, 150, 86);
				rect.Fill = new SolidColorBrush(color);
				Grid.SetColumn(rect, y + 1);
				Grid.SetRow(rect, x + 1);
				chessBoard.Children.Insert(0, rect);
			}

			TextBlock rowIndex = new()
			{
				Text = (8 - y).ToString(),
				HorizontalAlignment = System.Windows.HorizontalAlignment.Center,
				VerticalAlignment = System.Windows.VerticalAlignment.Center
			};
			Grid.SetRow(rowIndex, y + 1);
			chessBoard.Children.Insert(0, rowIndex);

			TextBlock columnIndex = new()
			{
				Text = ((char)(y + 'A')).ToString(),
				HorizontalAlignment = System.Windows.HorizontalAlignment.Center,
				VerticalAlignment = System.Windows.VerticalAlignment.Center
			};
			Grid.SetColumn(columnIndex, y + 1);
			chessBoard.Children.Insert(0, columnIndex);
		}
	}
}
