using System.Collections.Generic;

namespace ChessApp.Chess.Pieces;

public class Bishop: Piece
{
    public Bishop(int row, int col, Color color) : base(row, col, color)
    {
    }

    public override string GetName()
    {
        return "bishop";
    }

    public override List<Move> GetMoves(Piece?[,] board)
    {
        List<Move> moves = new List<Move>();
        
        var tempRow = row + 1;
        var tempCol = col + 1;

        while (tempRow < 8 && tempCol < 8)
        {
            if (board[tempRow, tempCol] == null)
            {
                moves.Add(new Move(tempCol, tempRow));
            }
            else
            {
                if (board[tempRow, tempCol].color != color)
                {
                    moves.Add(new Move(tempCol, tempRow));
                }

                break;
            }

            tempCol++;
            tempRow++;
        }

        tempRow = row - 1;
        tempCol = col + 1;
        while (tempRow >= 0 && tempCol < 8)
        {
            if (board[tempRow, tempCol] == null)
            {
                moves.Add(new Move(tempCol, tempRow));
            }
            else
            {
                if (board[tempRow, tempCol].color != color)
                {
                    moves.Add(new Move(tempCol, tempRow));
                }

                break;
            }

            tempCol++;
            tempRow--;
        }

        tempRow = row - 1;
        tempCol = col - 1;
        while (tempRow >= 0 && tempCol >= 0)
        {
            if (board[tempRow, tempCol] == null)
            {
                moves.Add(new Move(tempCol, tempRow));
            }
            else
            {
                if (board[tempRow, tempCol].color != color)
                {
                    moves.Add(new Move(tempCol, tempRow));
                }

                break;
            }

            tempCol--;
            tempRow--;
        }
        
        tempRow = row + 1;
        tempCol = col - 1;
        while (tempRow < 8 && tempCol >= 0)
        {
            if (board[tempRow, tempCol] == null)
            {
                moves.Add(new Move(tempCol, tempRow));
            }
            else
            {
                if (board[tempRow, tempCol].color != color)
                {
                    moves.Add(new Move(tempCol, tempRow));
                }

                break;
            }

            tempCol--;
            tempRow++;
        }
        
        return moves;
    }
}