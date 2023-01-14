using ChessAppClient.Infrastructure.Chess.Pieces;

namespace ChessAppClient.Infrastructure.Chess;

public class ChessBoard
{
    public Piece?[,] squares = new Piece[8, 8];

    public ChessBoard()
    {
        for (int i = 0; i < 8; i++)
        {
            squares[1, i] = new Pawn(1, i, Piece.Color.WHITE);
            squares[6, i] = new Pawn(6, i, Piece.Color.BLACK);
        }

        squares[0, 0] = new Rook(0, 0, Piece.Color.WHITE);
        squares[0, 7] = new Rook(0, 7, Piece.Color.WHITE);
        squares[7, 0] = new Rook(7, 0, Piece.Color.BLACK);
        squares[7, 7] = new Rook(7, 7, Piece.Color.BLACK);

        squares[0, 1] = new Knight(0, 1, Piece.Color.WHITE);
        squares[0, 6] = new Knight(0, 6, Piece.Color.WHITE);
        squares[7, 1] = new Knight(7, 1, Piece.Color.BLACK);
        squares[7, 6] = new Knight(7, 6, Piece.Color.BLACK);

        squares[0, 2] = new Bishop(0, 2, Piece.Color.WHITE);
        squares[0, 5] = new Bishop(0, 5, Piece.Color.WHITE);
        squares[7, 2] = new Bishop(7, 2, Piece.Color.BLACK);
        squares[7, 5] = new Bishop(7, 5, Piece.Color.BLACK);

        squares[0, 3] = new Queen(0, 3, Piece.Color.WHITE);
        squares[0, 4] = new King(0, 4, Piece.Color.WHITE);
        squares[7, 3] = new Queen(7, 3, Piece.Color.BLACK);
        squares[7, 4] = new King(7, 4, Piece.Color.BLACK);
    }

    public bool MakeMove(Piece piece, int destCol, int destRow)
    {
        if (ValidateMove(piece, destCol, destRow))
        {
            squares[destRow, destCol] = piece;
            squares[piece.row, piece.col] = null;
            piece.col = destCol;
            piece.row = destRow;

            return true;
        }
        return false;
    }

    private bool ValidateMove(Piece piece, int destCol, int destRow)
    {
        return true;
    }
}