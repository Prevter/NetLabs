using FloxelLib.Common;
using FloxelLib.MVVM;
using NetLabs.Labs.Lab01;

namespace NetLabs.MVVM;

public enum EChessFigure
{
	Knight,
	Bishop,
	Rook,
	Queen
}

public sealed partial class Lab01ViewModel : BaseViewModel
{
	private string _figureKind = "ChessKnight", _movePositionValue = "C3", _setPositionValue = "B1";
	private int _figureRow = 1, _figureColumn = 2;

	private EChessFigure _currentFigure;

	private readonly ChessFigure[] _figures = new ChessFigure[4]
	{
		new KnightFigure("B1", false),
		new BishopFigure("F1", false),
		new RookFigure("H1", false),
		new QueenFigure("D1", false),
	};

	public string FigureKind
	{
		get => _figureKind;
		set => SetField(ref _figureKind, value);
	}

	public string MovePositionValue
	{
		get => _movePositionValue;
		set => SetField(ref _movePositionValue, value);
	}

	public string SetPositionValue
	{
		get => _setPositionValue;
		set => SetField(ref _setPositionValue, value);
	}

	public int FigureRow
	{
		get => 9 - _figureRow;
		set => SetField(ref _figureRow, value);
	}

	public int FigureColumn
	{
		get => _figureColumn;
		set => SetField(ref _figureColumn, value);
	}

	[RelayCommand]
	private void ChangeFigure(object obj)
	{
		if (obj is not EChessFigure chessFigure) return;
		FigureKind = $"Chess{chessFigure}";
		_currentFigure = chessFigure;

		var figure = _figures[(int)_currentFigure];
		(FigureColumn, FigureRow) = ChessFigure.GetCell(figure.Position);
	}

	[RelayCommand]
	private void SetPosition(object obj)
	{
		if (!ChessFigure.IsValidCell(SetPositionValue))
		{
			MessageBox.Show("Невірна позиція фігури!");
			return;
		}

		ChessFigure figure = _figures[(int)_currentFigure];
		figure.Position = SetPositionValue;

		(FigureColumn, FigureRow) = ChessFigure.GetCell(figure.Position);
	}

	[RelayCommand]
	private void MovePosition()
	{
		if (!ChessFigure.IsValidCell(SetPositionValue))
		{
			MessageBox.Show("Невірна позиція фігури!");
			return;
		}

		var figure = _figures[(int)_currentFigure];

		if (figure.Move(MovePositionValue))
		{
			(FigureColumn, FigureRow) = ChessFigure.GetCell(figure.Position);
		}
		else
		{
			MessageBox.Show("Неможливий хід!");
		}
	}
}
