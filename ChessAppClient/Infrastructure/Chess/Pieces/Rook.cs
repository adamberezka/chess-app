using System.Collections.Generic;

namespace ChessAppClient.Infrastructure.Chess.Pieces;

public class Rook: Piece
{
    public Rook(int row, int col, Color color) : base(row, col, color)
    {
    }

    public override string GetName()
    {
        return "rook";
    }

    public override List<Move> GetMoves()
    {
        throw new System.NotImplementedException();
    }
}