using System.Collections.Generic;

namespace ChessApp.Chess.Pieces;

public class Pawn : Piece
{
    public bool HasMoved = false;
    public bool CanBeEnPassented = false;

    public Pawn(int row, int col, Color color) : base(row, col, color)
    {
    }

    public override string GetName()
    {
        return "pawn";
    }

    public override List<Move> GetMoves(Piece?[,] board)
    {
        List<Move> moves = new List<Move>();

        if (color == Color.WHITE)
        {
            if (col + 1 < 8 && board[row + 1, col + 1] != null && board[row + 1, col + 1].color == Color.BLACK)
            {
                moves.Add(new Move(col + 1, row + 1));
            }

            if (col - 1 >= 0 && board[row + 1, col - 1] != null && board[row + 1, col - 1].color == Color.BLACK)
            {
                moves.Add(new Move(col - 1, row + 1));
            }

            if (board[row + 1, col] == null)
            {
                moves.Add(new Move(col, row + 1));
            }

            if (HasMoved == false && board[row + 2, col] == null)
            {
                moves.Add(new Move(col, row + 2));
            }

            if (col - 1 >= 0 && board[row, col - 1] is Pawn { CanBeEnPassented: true, color: Color.BLACK })
            {
                moves.Add(new Move(col - 1, row + 1));
            }

            if (col + 1 < 8 && board[row, col + 1] is Pawn { CanBeEnPassented: true, color: Color.BLACK })
            {
                moves.Add(new Move(col + 1, row + 1));
            }
        }
        else if (color == Color.BLACK)
        {
            if (col + 1 < 8 && board[row - 1, col + 1] != null && board[row - 1, col + 1].color == Color.WHITE)
            {
                moves.Add(new Move(col + 1, row - 1));
            }

            if (col - 1 >= 0 && board[row - 1, col - 1] != null && board[row - 1, col - 1].color == Color.WHITE)
            {
                moves.Add(new Move(col - 1, row - 1));
            }

            if (board[row - 1, col] == null)
            {
                moves.Add(new Move(col, row - 1));
            }

            if (HasMoved == false && board[row - 2, col] == null)
            {
                moves.Add(new Move(col, row - 2));
            }

            if (col - 1 >= 0 && board[row, col - 1] is Pawn { CanBeEnPassented: true, color: Color.WHITE})
            {
                moves.Add(new Move(col - 1, row - 1));
            }

            if (col + 1 < 8 && board[row, col + 1] is Pawn { CanBeEnPassented: true, color: Color.WHITE })
            {
                moves.Add(new Move(col + 1, row - 1));
            }
        }

        return moves;
    }
}