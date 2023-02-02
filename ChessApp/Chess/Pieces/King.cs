using System.Collections.Generic;

namespace ChessApp.Chess.Pieces;

public class King: Piece
{

    public bool HasMoved = false;
    
    public King(int row, int col, Color color) : base(row, col, color)
    {
    }

    public override string GetName()
    {
        return "king";
    }

    public override List<Move> GetMoves(Piece?[,] board)
    {
        List<Move> moves = new List<Move>();
        
        var tempRow = row + 1;
        var tempCol = col + 1;
        if (tempRow < 8 && tempCol < 8 && (board[tempRow, tempCol] == null || board[tempRow, tempCol].color != color))
        {
            moves.Add(new Move(tempCol, tempRow));
        }

        tempRow = row + 1;
        tempCol = col - 1;
        if (tempRow < 8 && tempCol >= 0 && (board[tempRow, tempCol] == null || board[tempRow, tempCol].color != color))
        {
            moves.Add(new Move(tempCol, tempRow));
        }
        
        tempRow = row - 1;
        tempCol = col - 1;
        if (tempRow >= 0 && tempCol >= 0 && (board[tempRow, tempCol] == null || board[tempRow, tempCol].color != color))
        {
            moves.Add(new Move(tempCol, tempRow));
        }
        
        tempRow = row - 1;
        tempCol = col + 1;
        if (tempRow >= 0 && tempCol < 8 && (board[tempRow, tempCol] == null || board[tempRow, tempCol].color != color))
        {
            moves.Add(new Move(tempCol, tempRow));
        }
        
        
        tempRow = row + 1;
        tempCol = col;
        if (tempRow < 8 && (board[tempRow, tempCol] == null || board[tempRow, tempCol].color != color))
        {
            moves.Add(new Move(tempCol, tempRow));
        }
        
        tempRow = row - 1;
        tempCol = col;
        if (tempRow >= 0 && (board[tempRow, tempCol] == null || board[tempRow, tempCol].color != color))
        {
            moves.Add(new Move(tempCol, tempRow));
        }
        
        tempRow = row;
        tempCol = col - 1;
        if (tempCol >= 0 && (board[tempRow, tempCol] == null || board[tempRow, tempCol].color != color))
        {
            moves.Add(new Move(tempCol, tempRow));
        }
        
        tempRow = row;
        tempCol = col + 1;
        if (tempCol < 8 && (board[tempRow, tempCol] == null || board[tempRow, tempCol].color != color))
        {
            moves.Add(new Move(tempCol, tempRow));
        }

        if (!HasMoved)
        {
            if (color == Color.WHITE)
            {
                if (board[0, 5] == null && board[0, 6] == null && board[0, 7] is Rook { HasMoved: false })
                {
                    moves.Add(new Move(6, 0));
                }

                if (board[0, 1] == null && board[0, 2] == null && board[0, 3] == null &&
                    board[0, 0] is Rook { HasMoved: false })
                {
                    moves.Add(new Move(2, 0));
                }
            } else if (color == Color.BLACK)
            {
                if (board[7, 5] == null && board[7, 6] == null && board[7, 7] is Rook { HasMoved: false })
                {
                    moves.Add(new Move(6, 7));
                }

                if (board[7, 1] == null && board[7, 2] == null && board[7, 3] == null &&
                    board[7, 0] is Rook { HasMoved: false })
                {
                    moves.Add(new Move(2, 7));
                }
            }
        }
        
        return moves;
    }
}