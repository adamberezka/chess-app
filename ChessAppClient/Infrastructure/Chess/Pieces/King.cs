using System.Collections.Generic;

namespace ChessAppClient.Infrastructure.Chess.Pieces;

public class King: Piece
{
    public King(int row, int col, Color color) : base(row, col, color)
    {
    }

    public override string GetName()
    {
        return "king";
    }

    public override List<Move> GetMoves()
    {
        throw new System.NotImplementedException();
    }
}