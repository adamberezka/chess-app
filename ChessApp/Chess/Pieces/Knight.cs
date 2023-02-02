using System.Collections.Generic;

namespace ChessApp.Chess.Pieces;

public class Knight: Piece
{
    public Knight(int row, int col, Color color) : base(row, col, color)
    {
    }

    public override string GetName()
    {
        return "knight";
    }

    public override List<Move> GetMoves(Piece?[,] board)
    {
        List<Move> moves = new List<Move>();

        var tempRow = row + 1;
        var tempCol = col + 2;
        if (tempRow < 8 && tempCol < 8 && (board[tempRow, tempCol] == null || board[tempRow, tempCol].color != color))
        {
            moves.Add(new Move(tempCol, tempRow));
        }

        tempRow = row + 1;
        tempCol = col - 2;
        if (tempRow < 8 && tempCol >= 0 && (board[tempRow, tempCol] == null || board[tempRow, tempCol].color != color))
        {
            moves.Add(new Move(tempCol, tempRow));
        }
        
        tempRow = row - 1;
        tempCol = col - 2;
        if (tempRow >= 0 && tempCol >= 0 && (board[tempRow, tempCol] == null || board[tempRow, tempCol].color != color))
        {
            moves.Add(new Move(tempCol, tempRow));
        }
        
        tempRow = row - 1;
        tempCol = col + 2;
        if (tempRow >= 0 && tempCol < 8 && (board[tempRow, tempCol] == null || board[tempRow, tempCol].color != color))
        {
            moves.Add(new Move(tempCol, tempRow));
        }
        
        tempRow = row + 2;
        tempCol = col + 1;
        if (tempRow < 8 && tempCol < 8 && (board[tempRow, tempCol] == null || board[tempRow, tempCol].color != color))
        {
            moves.Add(new Move(tempCol, tempRow));
        }
        
        tempRow = row - 2;
        tempCol = col + 1;
        if (tempRow >= 0 && tempCol < 8 && (board[tempRow, tempCol] == null || board[tempRow, tempCol].color != color))
        {
            moves.Add(new Move(tempCol, tempRow));
        }
        
        tempRow = row - 2;
        tempCol = col - 1;
        if (tempRow >= 0 && tempCol >= 0 && (board[tempRow, tempCol] == null || board[tempRow, tempCol].color != color))
        {
            moves.Add(new Move(tempCol, tempRow));
        }
        
        tempRow = row + 2;
        tempCol = col - 1;
        if (tempRow < 8 && tempCol >= 0 && (board[tempRow, tempCol] == null || board[tempRow, tempCol].color != color))
        {
            moves.Add(new Move(tempCol, tempRow));
        }

        return moves;
    }
}