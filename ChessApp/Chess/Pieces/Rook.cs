using System.Collections.Generic;

namespace ChessApp.Chess.Pieces;

public class Rook: Piece
{

    public bool HasMoved = false;
    
    public Rook(int row, int col, Color color) : base(row, col, color)
    {
    }

    public override string GetName()
    {
        return "rook";
    }

    public override List<Move> GetMoves(Piece?[,] board)
    {
        List<Move> moves = new List<Move>();
        
        var tempCol = col + 1;
        while (tempCol < 8)
        {
            if (board[row, tempCol] == null)
            {
                moves.Add(new Move(tempCol, row));
            }
            else 
            {
                if (board[row, tempCol].color != color)
                {
                    moves.Add(new Move(tempCol, row));
                }
                break;
            }
            tempCol++;
        }

        tempCol = col - 1;
        while (tempCol >= 0)
        {
            if (board[row, tempCol] == null)
            {
                moves.Add(new Move(tempCol, row));
            }
            else 
            {
                if (board[row, tempCol].color != color)
                {
                    moves.Add(new Move(tempCol, row));
                }
                break;
            }
            tempCol--;
        }

        var tempRow = row + 1;
        while (tempRow < 8)
        {
            if (board[tempRow, col] == null)
            {
                moves.Add(new Move(col, tempRow));
            }
            else 
            {
                if (board[tempRow, col].color != color)
                {
                    moves.Add(new Move(col, tempRow));
                }
                break;
            }
            tempRow++;
        }

        tempRow = row - 1;
        while (tempRow >= 0)
        {
            if (board[tempRow, col] == null)
            {
                moves.Add(new Move(col, tempRow));
            }
            else 
            {
                if (board[tempRow, col].color != color)
                {
                    moves.Add(new Move(col, tempRow));
                }
                break;
            }
            tempRow--;
        }

        return moves;
    }
}