using System.Collections.Generic;

namespace ChessApp.Chess.Pieces;

public class Queen: Piece
{
    public Queen(int row, int col, Color color) : base(row, col, color)
    {
    }

    public override string GetName()
    {
        return "queen";
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
        
        tempCol = col + 1;
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

        tempRow = row + 1;
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