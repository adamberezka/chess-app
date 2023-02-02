using ChessApp.Chess.Pieces;

namespace ChessApp.Chess;

public class ChessBoard
{

    public Piece.Color? wonBy;
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

    public bool MakeMove(Piece? piece, int destCol, int destRow)
    {
        if (ValidateMove(piece, destCol, destRow))
        {
            ClearCanBeEnPassanted(piece.color);
            if (piece is Pawn pawn)
            {
                pawn.HasMoved = true;
                if (pawn.color == Piece.Color.WHITE && destRow - pawn.row == 2)
                {
                    pawn.CanBeEnPassented = true;
                }
                else if (pawn.color == Piece.Color.BLACK && pawn.row - destRow == 2)
                {
                    pawn.CanBeEnPassented = true;
                }

                if (destCol != pawn.col && squares[destRow, destCol] == null)
                {
                    if (pawn.color == Piece.Color.WHITE)
                    {
                        squares[destRow - 1, destCol] = null;
                    }
                    else if (pawn.color == Piece.Color.BLACK)
                    {
                        squares[destRow + 1, destCol] = null;
                    }
                }
                
                if (pawn.color == Piece.Color.WHITE && destRow == 7)
                {
                    piece = new Queen(piece.row, piece.col, piece.color);
                }
                else if (pawn.color == Piece.Color.BLACK && destRow == 0)
                {
                    piece = new Queen(piece.row, piece.col, piece.color);
                }
            }

            if (piece is King king)
            {
                if (!king.HasMoved && destRow == king.row)
                {
                    if (destCol == 6)
                    {
                        Rook castleRook = (Rook)squares[king.row, 7]!;
                        squares[king.row, 5] = castleRook;
                        squares[king.row, 7] = null;
                        castleRook.col = 5;
                        castleRook.HasMoved = true;
                    }
                    else if (destCol == 2)
                    {
                        Rook castleRook = (Rook)squares[king.row, 0]!;
                        squares[king.row, 3] = castleRook;
                        squares[king.row, 0] = null;
                        castleRook.col = 3;
                        castleRook.HasMoved = true;
                    }
                }
                king.HasMoved = true;
            }

            if (piece is Rook rook)
            {
                rook.HasMoved = true;
            }

            squares[destRow, destCol] = piece;
            squares[piece.row, piece.col] = null;
            piece.col = destCol;
            piece.row = destRow;

            if (IsCheckMate(piece.color == Piece.Color.WHITE ? Piece.Color.BLACK : Piece.Color.WHITE))
            {
                wonBy = piece.color;
            }

            return true;
        }
        return false;
    }

    private bool ValidateMove(Piece? piece, int destCol, int destRow)
    {
        King king = null;
        for (int i = 0; i < 8; i++)
        {
            for (int j = 0; j < 8; j++)
            {
                if (squares[j, i] != null && squares[j, i] is King k && k.color == piece.color)
                {
                    king = k;
                }
            }
        }
        
        List<Piece> opponentPieces = new List<Piece>();
        for (int i = 0; i < 8; i++)
        {
            for (int j = 0; j < 8; j++)
            {
                if (squares[j, i] != null && squares[j, i].color != piece.color)
                {
                    opponentPieces.Add(squares[j, i]);
                }
            }
        }

        if (piece == null)
            return false;

        return piece.GetMoves(squares)
            .FindAll(move => !IsKingAttackedAfterMove(king, piece, move.ColTo, move.RowTo, opponentPieces))
            .Exists(move => 
                move.ColTo == destCol && move.RowTo == destRow);
    }

    public List<Move>? GetMoves(int row, int col)
    {
        Piece? piece = squares[row, col];

        King king = null;
        for (int i = 0; i < 8; i++)
        {
            for (int j = 0; j < 8; j++)
            {
                if (squares[j, i] != null && squares[j, i] is King k && k.color == piece.color)
                {
                    king = k;
                }
            }
        }
        
        List<Piece> opponentPieces = new List<Piece>();
        for (int i = 0; i < 8; i++)
        {
            for (int j = 0; j < 8; j++)
            {
                if (squares[j, i] != null && squares[j, i].color != piece.color)
                {
                    opponentPieces.Add(squares[j, i]);
                }
            }
        }
        
        return squares[row, col]?.GetMoves(squares)
            .FindAll(move => !IsKingAttackedAfterMove(king, piece, move.ColTo,move.RowTo, opponentPieces));
    }

    public bool IsDraw(Piece.Color playerToMoveColor)
    {
        King king = null;
        for (int i = 0; i < 8; i++)
        {
            for (int j = 0; j < 8; j++)
            {
                if (squares[j, i] != null && squares[j, i] is King k && k.color == playerToMoveColor)
                {
                    king = k;
                }
            }
        }

        List<Piece> pieces = new List<Piece>();
        for (int i = 0; i < 8; i++)
        {
            for (int j = 0; j < 8; j++)
            {
                if (squares[j, i] != null && squares[j, i].color == playerToMoveColor)
                {
                    pieces.Add(squares[j, i]);
                }
            }
        }

        List<Move> moves = pieces.ConvertAll(piece => piece.GetMoves(squares))
            .SelectMany(i => i)
            .ToList();

        if (moves.Count != 0)
            return false;

        List<Piece> opponentPieces = new List<Piece>();
        for (int i = 0; i < 8; i++)
        {
            for (int j = 0; j < 8; j++)
            {
                if (squares[j, i] != null && squares[j, i].color != playerToMoveColor)
                {
                    opponentPieces.Add(squares[j, i]);
                }
            }
        }

        return !opponentPieces.Exists(piece => piece.GetMoves(squares)
            .Exists(move => move.ColTo == king.col && move.RowTo == king.row));
    }

    private bool IsCheckMate(Piece.Color attackedKingColor)
    {
        King king = null;
        for (int i = 0; i < 8; i++)
        {
            for (int j = 0; j < 8; j++)
            {
                if (squares[j, i] != null && squares[j, i] is King k && k.color == attackedKingColor)
                {
                    king = k;
                }
            }
        }
        
        List<Piece> pieces = new List<Piece>();
        for (int i = 0; i < 8; i++)
        {
            for (int j = 0; j < 8; j++)
            {
                if (squares[j, i] != null && squares[j, i].color == attackedKingColor)
                {
                    pieces.Add(squares[j, i]);
                }
            }
        }
        
        List<Piece> opponentPieces = new List<Piece>();
        for (int i = 0; i < 8; i++)
        {
            for (int j = 0; j < 8; j++)
            {
                if (squares[j, i] != null && squares[j, i].color != attackedKingColor)
                {
                    opponentPieces.Add(squares[j, i]);
                }
            }
        }

        return !pieces.Exists(piece =>
            piece.GetMoves(squares)
                .Exists(move => !IsKingAttackedAfterMove(king, piece, move.ColTo, move.RowTo, opponentPieces)));
    }

    private void ClearCanBeEnPassanted(Piece.Color pieceColor)
    {
        for (int i = 0; i < 8; i++)
        {
            for (int j = 0; j < 8; j++)
            {
                var piece = squares[j, i];

                if (piece != null && piece.color == pieceColor && piece is Pawn pawn)
                {
                    pawn.CanBeEnPassented = false;
                }
            }
        }
    }

    private bool IsKingAttackedAfterMove(King king, Piece piece, int destCol, int destRow, List<Piece> opponentPieces)
    {
        Piece?[,] clonedSquares = (Piece?[,])squares.Clone();
        clonedSquares[piece.row, piece.col] = null;
        clonedSquares[destRow, destCol] = piece;

        if (piece == king)
        {
            return opponentPieces.Exists(p =>
                p.GetMoves(clonedSquares).Exists(move => move.RowTo == destRow && move.ColTo == destCol)
            );
        }

        return opponentPieces.Exists(p =>
            p.GetMoves(clonedSquares).Exists(move => move.RowTo == king.row && move.ColTo == king.col)
        );
    }
}