using System.Collections.Generic;

namespace ChessAppClient.Infrastructure.Chess.Pieces;

public class Pawn: Piece
{
    public Pawn(int row, int col, Color color) : base(row, col, color)
    {
    }

    public override string GetName()
    {
        return "pawn";
    }

    public override List<Move> GetMoves()
    {
        throw new System.NotImplementedException();
    }
}