using System;

namespace NetLabs.Labs.Lab01;

public sealed class QueenFigure : ChessFigure
{
	public QueenFigure() : base() { }
	public QueenFigure(string position, bool isWhite) : base(position, isWhite) { }
	public QueenFigure(QueenFigure queen) : base(queen) { }

	public override bool CanMove(string from, string to)
	{
		if (!IsValidCell(from) || !IsValidCell(to)) return false;

		// Queen can move like a rook or a bishop
		var rook = new RookFigure(from, IsWhite);
		var bishop = new BishopFigure(from, IsWhite);
		return rook.CanMove(from, to) || bishop.CanMove(from, to);
	}

	public override bool Equals(object? obj)
	{
		if (obj is QueenFigure queen)
			return Position == queen.Position && IsWhite == queen.IsWhite;

		return false;
	}

	public override int GetHashCode()
	{
		return HashCode.Combine(Position, IsWhite, "Queen");
	}
}