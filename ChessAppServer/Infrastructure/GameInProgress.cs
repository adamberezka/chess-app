using System.Net.WebSockets;
using ChessApp.Chess;
using ChessApp.WebSockets;
using ChessAppServer.Persistence;
using ChessAppServer.Persistence.Entities;

namespace ChessAppServer.Infrastructure;

public class GameInProgress
{
    private readonly DataContext _dataContext;
    private ChessBoard _chessBoard = new ChessBoard();

    public Player Black;
    public Player White;

    private Player _playerToMove;

    public GameInProgress(Player black, Player white, DataContext dataContext)
    {
        Black = black;
        White = white;
        _dataContext = dataContext;
    }

    public async void Start()
    {
        try
        {
            var whiteGameStartedMessage = new WebSocketGameStartedMessage(0, Black.Username, Black.Rating);
            await White.WebSocketConnection.SendAsync(
                new ArraySegment<byte>(WebSocketMessageTranslator.ToBytes(whiteGameStartedMessage)),
                WebSocketMessageType.Text, true, CancellationToken.None);

            var blackGameStartedMessage = new WebSocketGameStartedMessage(1, White.Username, White.Rating);
            await Black.WebSocketConnection.SendAsync(
                new ArraySegment<byte>(WebSocketMessageTranslator.ToBytes(blackGameStartedMessage)),
                WebSocketMessageType.Text, true, CancellationToken.None);

            bool gameEnded = false;
            _playerToMove = White;

            byte[] buffer = new byte[256];

            while (!gameEnded)
            {
                for (int i = 0; i < 256; i++)
                    buffer[i] = 0;
                await White.WebSocketConnection.ReceiveAsync(buffer, CancellationToken.None);
                var whiteMessage = WebSocketMessageTranslator.FromBytes(buffer);
                if (whiteMessage != null)
                {
                    if (whiteMessage is WebSocketResignMessage)
                    {
                        int colorWon = 1;
                        var gameOverMessage = new WebSocketGameOverMessage(colorWon);
                        await White.WebSocketConnection.SendAsync(
                            new ArraySegment<byte>(WebSocketMessageTranslator.ToBytes(gameOverMessage)),
                            WebSocketMessageType.Text, true, CancellationToken.None
                        );

                        await Black.WebSocketConnection.SendAsync(
                            new ArraySegment<byte>(WebSocketMessageTranslator.ToBytes(gameOverMessage)),
                            WebSocketMessageType.Text, true, CancellationToken.None
                        );
                        gameEnded = true;
                        UpdateRatings(Piece.Color.BLACK);
                    }
                    else if (whiteMessage is WebSocketMoveMessage moveMessage && _playerToMove == White)
                    {
                        var pieceToMove = _chessBoard.squares[moveMessage.RowFrom, moveMessage.ColFrom];
                        var moveAllowed = _chessBoard.MakeMove(pieceToMove, moveMessage.ColTo, moveMessage.RowTo);
                        if (moveAllowed)
                        {
                            if (_chessBoard.wonBy != null)
                            {
                                await Black.WebSocketConnection.SendAsync(
                                    new ArraySegment<byte>(WebSocketMessageTranslator.ToBytes(moveMessage)),
                                    WebSocketMessageType.Text, true, CancellationToken.None
                                );
                                int colorWon = _chessBoard.wonBy == Piece.Color.WHITE ? 0 : 1;
                                var gameOverMessage = new WebSocketGameOverMessage(colorWon);
                                await White.WebSocketConnection.SendAsync(
                                    new ArraySegment<byte>(WebSocketMessageTranslator.ToBytes(gameOverMessage)),
                                    WebSocketMessageType.Text, true, CancellationToken.None
                                );

                                await Black.WebSocketConnection.SendAsync(
                                    new ArraySegment<byte>(WebSocketMessageTranslator.ToBytes(gameOverMessage)),
                                    WebSocketMessageType.Text, true, CancellationToken.None
                                );
                                gameEnded = true;
                                UpdateRatings(Piece.Color.WHITE);
                            }
                            else
                            {
                                await Black.WebSocketConnection.SendAsync(
                                    new ArraySegment<byte>(WebSocketMessageTranslator.ToBytes(moveMessage)),
                                    WebSocketMessageType.Text, true, CancellationToken.None
                                );
                                SwitchPlayerToMove();
                                Piece.Color playerColor =
                                    _playerToMove == White ? Piece.Color.WHITE : Piece.Color.BLACK;
                                if (_chessBoard.IsDraw(playerColor))
                                {
                                    var gameOverMessage = new WebSocketGameOverMessage(2);
                                    await Black.WebSocketConnection.SendAsync(
                                        new ArraySegment<byte>(WebSocketMessageTranslator.ToBytes(gameOverMessage)),
                                        WebSocketMessageType.Text, true, CancellationToken.None
                                    );
                                    gameEnded = true;
                                }
                            }
                        }
                    }
                }

                for (int i = 0; i < 256; i++)
                    buffer[i] = 0;
                await Black.WebSocketConnection.ReceiveAsync(buffer, CancellationToken.None);
                var blackMessage = WebSocketMessageTranslator.FromBytes(buffer);
                if (blackMessage != null)
                {
                    if (blackMessage is WebSocketResignMessage)
                    {
                        int colorWon = 0;
                        var gameOverMessage = new WebSocketGameOverMessage(colorWon);
                        await White.WebSocketConnection.SendAsync(
                            new ArraySegment<byte>(WebSocketMessageTranslator.ToBytes(gameOverMessage)),
                            WebSocketMessageType.Text, true, CancellationToken.None
                        );

                        await Black.WebSocketConnection.SendAsync(
                            new ArraySegment<byte>(WebSocketMessageTranslator.ToBytes(gameOverMessage)),
                            WebSocketMessageType.Text, true, CancellationToken.None
                        );
                        gameEnded = true;
                        UpdateRatings(Piece.Color.WHITE);
                    }
                    else if (blackMessage is WebSocketMoveMessage moveMessage && _playerToMove == Black)
                    {
                        var pieceToMove = _chessBoard.squares[moveMessage.RowFrom, moveMessage.ColFrom];
                        var moveAllowed = _chessBoard.MakeMove(pieceToMove, moveMessage.ColTo, moveMessage.RowTo);
                        if (moveAllowed)
                        {
                            if (_chessBoard.wonBy != null)
                            {
                                await White.WebSocketConnection.SendAsync(
                                    new ArraySegment<byte>(WebSocketMessageTranslator.ToBytes(moveMessage)),
                                    WebSocketMessageType.Text, true, CancellationToken.None
                                );
                                int colorWon = _chessBoard.wonBy == Piece.Color.WHITE ? 0 : 1;
                                var gameOverMessage = new WebSocketGameOverMessage(colorWon);
                                await White.WebSocketConnection.SendAsync(
                                    new ArraySegment<byte>(WebSocketMessageTranslator.ToBytes(gameOverMessage)),
                                    WebSocketMessageType.Text, true, CancellationToken.None
                                );

                                await Black.WebSocketConnection.SendAsync(
                                    new ArraySegment<byte>(WebSocketMessageTranslator.ToBytes(gameOverMessage)),
                                    WebSocketMessageType.Text, true, CancellationToken.None
                                );
                                gameEnded = true;
                                UpdateRatings(Piece.Color.BLACK);
                            }
                            else
                            {
                                await White.WebSocketConnection.SendAsync(
                                    new ArraySegment<byte>(WebSocketMessageTranslator.ToBytes(moveMessage)),
                                    WebSocketMessageType.Text, true, CancellationToken.None
                                );
                                SwitchPlayerToMove();
                                Piece.Color playerColor =
                                    _playerToMove == White ? Piece.Color.WHITE : Piece.Color.BLACK;
                                if (_chessBoard.IsDraw(playerColor))
                                {
                                    var gameOverMessage = new WebSocketGameOverMessage(2);
                                    await Black.WebSocketConnection.SendAsync(
                                        new ArraySegment<byte>(WebSocketMessageTranslator.ToBytes(gameOverMessage)),
                                        WebSocketMessageType.Text, true, CancellationToken.None
                                    );
                                    gameEnded = true;
                                }
                            }
                        }
                    }
                }

                Thread.Sleep(100);
            }

            await White.WebSocketConnection.CloseOutputAsync(WebSocketCloseStatus.NormalClosure, String.Empty,
                CancellationToken.None);
            await Black.WebSocketConnection.CloseOutputAsync(WebSocketCloseStatus.NormalClosure, String.Empty,
                CancellationToken.None);
        }
        catch (Exception e)
        {
        }
    }

    private void SwitchPlayerToMove()
    {
        _playerToMove = _playerToMove == White ? Black : White;
    }

    private void UpdateRatings(Piece.Color wonBy)
    {
        int whiteRatingChange;
        int blackRatingChange;

        if (wonBy == Piece.Color.WHITE)
        {
            whiteRatingChange = RatingChangeCalculator.ratingChange(White.Rating, Black.Rating, true);
            blackRatingChange = RatingChangeCalculator.ratingChange(Black.Rating, White.Rating, false);
        }
        else
        {
            whiteRatingChange = RatingChangeCalculator.ratingChange(White.Rating, Black.Rating, false);
            blackRatingChange = RatingChangeCalculator.ratingChange(Black.Rating, White.Rating, true);
        }

        White.Rating += whiteRatingChange;
        Black.Rating += blackRatingChange;

        using (var db = _dataContext)
        {
            User? whiteUser = db.User.SingleOrDefault(user => user.Id == White.Id);
            User? blackUser = db.User.SingleOrDefault(user => user.Id == Black.Id);
            if (whiteUser != null)
            {
                whiteUser.Rating = White.Rating;
            }
            if (blackUser != null)
            {
                blackUser.Rating = Black.Rating;
            }
            db.SaveChanges();
        }
    }
}