using System;

namespace NetLabs.Labs.Lab01;

public sealed class KnightFigure : ChessFigure
{
	public KnightFigure() : base() { }
	public KnightFigure(string position, bool isWhite) : base(position, isWhite) { }
	public KnightFigure(KnightFigure horse) : base(horse) { }

	public override bool CanMove(string from, string to)
	{
		if (!IsValidCell(from) || !IsValidCell(to)) return false;
		var (fromColumn, fromRow) = GetCell(from);
		var (toColumn, toRow) = GetCell(to);

		// Horse can move only 2 cells in one direction and 1 cell in another direction
		var columnDiff = Math.Abs(fromColumn - toColumn);
		var rowDiff = Math.Abs(fromRow - toRow);

		return (columnDiff == 2 && rowDiff == 1) || (columnDiff == 1 && rowDiff == 2);
	}

	public override bool Equals(object? obj)
	{
		if (obj is KnightFigure horse)
			return Position == horse.Position && IsWhite == horse.IsWhite;

		return false;
	}

	public override int GetHashCode()
	{
		return HashCode.Combine(Position, IsWhite, "Knight");
	}
}
