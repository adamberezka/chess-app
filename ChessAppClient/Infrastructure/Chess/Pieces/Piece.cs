using System.Collections.Generic;

namespace ChessAppClient.Infrastructure.Chess;

public abstract class Piece
{
    public int row;
    public int col;
    public Color color;

    protected Piece(int row, int col, Color color)
    {
        this.row = row;
        this.col = col;
        this.color = color;
    }

    public string GetImageFileName()
    {
        const string imageSuffix = "_2x_ns.png";
        var colorChar = color == Color.WHITE ? "w_" : "b_";
        return colorChar + GetName() + imageSuffix;
    }

    public abstract string GetName();

    public abstract List<Move> GetMoves();

    public enum Color
    {
        WHITE,
        BLACK
    }
}