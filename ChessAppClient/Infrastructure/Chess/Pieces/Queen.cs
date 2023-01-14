using System.Collections.Generic;

namespace ChessAppClient.Infrastructure.Chess.Pieces;

public class Queen: Piece
{
    public Queen(int row, int col, Color color) : base(row, col, color)
    {
    }

    public override string GetName()
    {
        return "queen";
    }

    public override List<Move> GetMoves()
    {
        throw new System.NotImplementedException();
    }
}