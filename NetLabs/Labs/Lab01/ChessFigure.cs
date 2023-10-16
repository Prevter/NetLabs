using System;

namespace NetLabs.Labs.Lab01;

public abstract class ChessFigure
{
	public string Position { get; set; }
	public bool IsWhite { get; set; }

	public ChessFigure()
	{
		Position = "A1";
		IsWhite = true;
	}

	public ChessFigure(string position, bool isWhite)
	{
		Position = position;
		IsWhite = isWhite;
	}

	public ChessFigure(ChessFigure figure)
	{
		Position = figure.Position;
		IsWhite = figure.IsWhite;
	}

	public bool Move(string to)
	{
		if (!IsValidCell(to)) return false;
		if (!CanMove(to)) return false;

		Position = to;
		return true;
	}

	public bool CanMove(string to) => CanMove(Position, to);
	public abstract bool CanMove(string from, string to);

	public static bool IsValidCell(string cell)
	{
		if (cell.Length != 2)
			return false;

		cell = cell.ToUpper();
		char Column = cell[0];
		char Row = cell[1];

		return Column >= 'A' && Column <= 'H' && Row >= '1' && Row <= '8';
	}

	public static (int column, int row) GetCell(string cell)
	{
		if (!IsValidCell(cell)) throw new ArgumentException("Cell is not valid");

		cell = cell.ToUpper();
		char Column = cell[0];
		char Row = cell[1];

		return (Column - 'A' + 1, Row - '1' + 1);
	}
}
