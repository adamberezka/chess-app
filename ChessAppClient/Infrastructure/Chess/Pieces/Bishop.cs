using System.Collections.Generic;

namespace ChessAppClient.Infrastructure.Chess.Pieces;

public class Bishop: Piece
{
    public Bishop(int row, int col, Color color) : base(row, col, color)
    {
    }

    public override string GetName()
    {
        return "bishop";
    }

    public override List<Move> GetMoves()
    {
        throw new System.NotImplementedException();
    }
}