namespace ChessApp.Chess;

public class Move
{
    public int ColTo;
    public int RowTo;

    public Move(int colTo, int rowTo)
    {
        ColTo = colTo;
        RowTo = rowTo;
    }
}