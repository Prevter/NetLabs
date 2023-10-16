using System;

namespace NetLabs.Labs.Lab01;

public sealed class BishopFigure : ChessFigure
{
	public BishopFigure() : base() { }
	public BishopFigure(string position, bool isWhite) : base(position, isWhite) { }
	public BishopFigure(BishopFigure bishop) : base(bishop) { }

	public override bool CanMove(string from, string to)
	{
		if (!IsValidCell(from) || !IsValidCell(to)) return false;
		var (fromColumn, fromRow) = GetCell(from);
		var (toColumn, toRow) = GetCell(to);

		// Bishop can move only diagonally (absolute value of column and row difference must be equal)
		var columnDiff = Math.Abs(fromColumn - toColumn);
		var rowDiff = Math.Abs(fromRow - toRow);

		return columnDiff == rowDiff;
	}

	public override bool Equals(object? obj)
	{
		if (obj is BishopFigure bishop)
			return Position == bishop.Position && IsWhite == bishop.IsWhite;

		return false;
	}

	public override int GetHashCode()
	{
		return HashCode.Combine(Position, IsWhite, "Bishop");
	}
}
