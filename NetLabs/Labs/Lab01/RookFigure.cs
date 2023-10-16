using System;

namespace NetLabs.Labs.Lab01;

public sealed class RookFigure : ChessFigure
{
	public RookFigure() : base() { }
	public RookFigure(string position, bool isWhite) : base(position, isWhite) { }
	public RookFigure(RookFigure rook) : base(rook) { }

	public override bool CanMove(string from, string to)
	{
		if (!IsValidCell(from) || !IsValidCell(to)) return false;
		var (fromColumn, fromRow) = GetCell(from);
		var (toColumn, toRow) = GetCell(to);

		// Rook can move only horizontally or vertically (one of the coordinates must be the same)
		return fromColumn == toColumn || fromRow == toRow;
	}

	public override bool Equals(object? obj)
	{
		if (obj is RookFigure rook)
			return Position == rook.Position && IsWhite == rook.IsWhite;

		return false;
	}

	public override int GetHashCode()
	{
		return HashCode.Combine(Position, IsWhite, "Rook");
	}
}