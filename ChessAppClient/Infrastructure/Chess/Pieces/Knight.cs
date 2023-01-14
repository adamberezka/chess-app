using System.Collections.Generic;

namespace ChessAppClient.Infrastructure.Chess.Pieces;

public class Knight: Piece
{
    public Knight(int row, int col, Color color) : base(row, col, color)
    {
    }

    public override string GetName()
    {
        return "knight";
    }

    public override List<Move> GetMoves()
    {
        throw new System.NotImplementedException();
    }
}